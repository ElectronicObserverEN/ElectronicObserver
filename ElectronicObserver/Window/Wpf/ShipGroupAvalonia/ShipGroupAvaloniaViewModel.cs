using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Avalonia.Controls;
using Avalonia.Win32.Interoperability;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Avalonia.Behaviors.PersistentColumns;
using ElectronicObserver.Avalonia.ShipGroup;
using ElectronicObserver.Common.Datagrid;
using ElectronicObserver.Data;
using ElectronicObserver.Data.ShipGroup;
using ElectronicObserver.Dialogs.DataGridSettings;
using ElectronicObserver.Dialogs.TextInput;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog;
using ElectronicObserverTypes;
using HanumanInstitute.MvvmDialogs;
using ShipGroupResources = ElectronicObserver.Avalonia.ShipGroup.ShipGroupResources;

namespace ElectronicObserver.Window.Wpf.ShipGroupAvalonia;

public sealed class ShipGroupAvaloniaViewModel : AnchorableViewModel
{
	private IDialogService DialogService { get; }

	public WpfAvaloniaHost WpfAvaloniaHost { get; }
	private ShipGroupView ShipGroupView { get; }
	private ShipGroupViewModel ShipGroupViewModel { get; }

	private ShipGroupItem? SelectedGroup { get; set; }

	public ShipGroupAvaloniaViewModel() : base("Group", "ShipGroup", IconContent.FormShipGroup)
	{
		DialogService = Ioc.Default.GetRequiredService<IDialogService>();

		ShipGroupViewModel = new()
		{
			SelectGroupAction = SelectGroup,
			AddGroupAction = AddGroup,
			CopyGroupAction = CopyGroup,
			RenameGroupAction = RenameGroup,
			DeleteGroupAction = DeleteGroup,
			AddToGroupAction = AddToGroup,
			CreateGroupAction = CreateGroup,
			ExcludeFromGroupAction = ExcludeFromGroup,
			FilterGroupAction = FilterGroup,
			FilterColumnsAction = FilterColumns,
			ExportCsvAction = ExportCsv,
			OpenDataGridSettingsAction = OpenDataGridSettings,
		};
		ShipGroupView = new()
		{
			DataContext = ShipGroupViewModel,
		};
		WpfAvaloniaHost = new()
		{
			Content = ShipGroupView,
		};

		Title = ShipGroupResources.Title;
		ShipGroupViewModel.FormShipGroup.PropertyChanged += (_, _) => Title = ShipGroupResources.Title;

		Configuration.ConfigurationData config = Configuration.Config;

		ShipGroupViewModel.AutoUpdate = config.FormShipGroup.AutoUpdate;
		ShipGroupViewModel.ShowStatusBar = config.FormShipGroup.ShowStatusBar;
		ShipGroupViewModel.GroupHeight = new(config.FormShipGroup.GroupHeight);
		ShipGroupViewModel.DataGridSettings.ColumnHeaderHeight = config.FormShipGroup.ColumnHeaderHeight;
		ShipGroupViewModel.DataGridSettings.RowHeight = config.FormShipGroup.RowHeight;

		Loaded();

		ShipGroupViewModel.ColumnProperties.CollectionChanged += (s, e) =>
		{
			if (SelectedGroup?.Group is not ShipGroupData group) return;
			if (e.NewItems?.Cast<ColumnModel>() is not IEnumerable<ColumnModel> newColumns) return;

			foreach (ShipGroupData.ViewColumnData newData in newColumns.Select(MakeColumnData))
			{
				group.ViewColumns.Add(newData.Name, newData);
			}
		};

		SystemEvents.SystemShuttingDown += SystemShuttingDown;
	}

	public override void Loaded()
	{
		base.Loaded();

		ShipGroupManager groups = KCDatabase.Instance.ShipGroup;

		// 空(≒初期状態)の時、おなじみ全所属艦を追加
		if (groups.ShipGroups.Count == 0)
		{
			Logger.Add(3, string.Format(ShipGroupResources.GroupNotFound, ShipGroupManager.DefaultFilePath));

			ShipGroupData group = KCDatabase.Instance.ShipGroup.Add();
			group.Name = GeneralRes.AllAssignedShips;

			// todo: make sure everything gets initialized correctly when there's no groups
		}

		foreach (ShipGroupItem group in groups.ShipGroups.Values.Select(MakeGroupItem))
		{
			ShipGroupViewModel.Groups.Add(group);
		}

		ConfigurationChanged();

		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += ApiUpdated;
		o.ApiGetMember_Ship2.ResponseReceived += ApiUpdated;
		o.ApiGetMember_ShipDeck.ResponseReceived += ApiUpdated;

		// added later - might affect performance
		o.ApiGetMember_NDock.ResponseReceived += ApiUpdated;
		o.ApiReqHensei_PresetSelect.ResponseReceived += ApiUpdated;

		Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
	}

	private void ConfigurationChanged()
	{
		ShipGroupViewModel.FormShipGroup.OnPropertyChanged("");
		ShipGroupViewModel.ConditionBorder = Configuration.Config.Control.ConditionBorder;
	}

	private void ApiUpdated(string apiName, dynamic data)
	{
		if (SelectedGroup is null) return;

		if (ShipGroupViewModel.AutoUpdate)
		{
			SelectGroup(SelectedGroup);
		}
	}

	private void SelectGroup(ShipGroupItem group)
	{
		if (group.Group is not ShipGroupData groupData) return;

		if (SelectedGroup is not null)
		{
			SelectedGroup.SortDescriptions = ShipGroupViewModel.CollectionView.SortDescriptions;

			if (SelectedGroup.Group is ShipGroupData previousGroup)
			{
				previousGroup.DataGridSortDescriptionCollection = ShipGroupViewModel.CollectionView.SortDescriptions;
				previousGroup.ViewColumns = ShipGroupViewModel.ColumnProperties
					.Select(MakeColumnData)
					.ToDictionary(c => c.Name, c => c);
			}

			SelectedGroup.IsSelected = false;
		}

		SelectedGroup = group;
		SelectedGroup.IsSelected = true;

		ShipGroupViewModel.ColumnProperties = group.Columns;
		ShipGroupViewModel.SortDescriptions = group.SortDescriptions;

		groupData.UpdateMembers();

		ShipGroupViewModel.Items = groupData.MembersInstance
			.OfType<IShipData>()
			.Select(s => new ShipGroupItemViewModel(s))
			.ToObservableCollection();

		ShipGroupViewModel.FrozenColumns = groupData.ScrollLockColumnCount;
	}

	private async Task AddGroup()
	{
		TextInputViewModel textInput = new()
		{
			Title = ShipGroupResources.DialogGroupAddTitle,
			Description = ShipGroupResources.DialogGroupAddDescription,
		};

		bool? result = await DialogService.ShowDialogAsync(App.MainViewModel, textInput);

		if (result is not true) return;

		ShipGroupData group = KCDatabase.Instance.ShipGroup.Add();
		group.Name = textInput.Text.Trim();

		if (SelectedGroup is not null)
		{
			foreach (ShipGroupData.ViewColumnData newData in SelectedGroup.Columns.Select(MakeColumnData))
			{
				group.ViewColumns.Add(newData.Name, newData);
			}
		}

		ShipGroupViewModel.Groups.Add(MakeGroupItem(group));
	}

	private async Task CopyGroup(ShipGroupItem group)
	{
		TextInputViewModel textInput = new()
		{
			Title = ShipGroupResources.DialogGroupCopyTitle,
			Description = ShipGroupResources.DialogGroupCopyDescription,
		};

		bool? result = await DialogService.ShowDialogAsync(App.MainViewModel, textInput);

		if (result is not true) return;

		ShipGroupData newGroup = (ShipGroupData)group.Group.Clone();

		newGroup.GroupID = KCDatabase.Instance.ShipGroup.GetUniqueID();
		newGroup.Name = textInput.Text.Trim();

		KCDatabase.Instance.ShipGroup.ShipGroups.Add(newGroup);

		ShipGroupViewModel.Groups.Add(MakeGroupItem(newGroup));
	}

	private async Task RenameGroup(ShipGroupItem group)
	{
		TextInputViewModel textInput = new()
		{
			Title = ShipGroupResources.DialogGroupRenameTitle,
			Description = ShipGroupResources.DialogGroupRenameDescription,
			Text = group.Name,
		};

		bool? result = await DialogService.ShowDialogAsync(App.MainViewModel, textInput);

		if (result is not true) return;

		group.Name = textInput.Text.Trim();
	}

	private void DeleteGroup(ShipGroupItem group)
	{
		if (MessageBox.Show(string.Format(ShipGroupResources.DialogGroupDeleteDescription, group.Name),
			ShipGroupResources.DialogGroupDeleteTitle,
			MessageBoxButtons.YesNo,
			MessageBoxIcon.Question,
			MessageBoxDefaultButton.Button2) != DialogResult.Yes)
		{
			return;
		}

		if (SelectedGroup == group)
		{
			SelectedGroup = null;
		}

		ShipGroupViewModel.Groups.Remove(group);
		KCDatabase.Instance.ShipGroup.ShipGroups.Remove((ShipGroupData)group.Group);
	}

	private List<int> GetSelectedShipIds() => ShipGroupViewModel.SelectedShips
		.Select(s => s.MasterId)
		.ToList();

	private void AddToGroup()
	{
		using DialogTextSelect dialog = new(ShipGroupResources.DialogGroupAddToGroupTitle,
			ShipGroupResources.DialogGroupAddToGroupDescription,
			KCDatabase.Instance.ShipGroup.ShipGroups.Values.ToArray());

		if (dialog.ShowDialog(App.Current!.MainWindow!) is not DialogResult.OK) return;

		ShipGroupData? group = (ShipGroupData?)dialog.SelectedItem;

		if (group is null) return;

		group.AddInclusionFilter(GetSelectedShipIds());

		if (group.ID == SelectedGroup?.Id)
		{
			SelectGroup(SelectedGroup);
		}
	}

	private async Task CreateGroup()
	{
		if (SelectedGroup is null) return;

		List<int> ships = GetSelectedShipIds();

		if (ships.Count is 0) return;

		TextInputViewModel textInput = new()
		{
			Title = ShipGroupResources.DialogGroupAddTitle,
			Description = ShipGroupResources.DialogGroupAddDescription,
		};

		bool? result = await DialogService.ShowDialogAsync(App.MainViewModel, textInput);

		if (result is not true) return;

		ShipGroupData group = KCDatabase.Instance.ShipGroup.Add();
		group.Name = textInput.Text.Trim();

		foreach (ShipGroupData.ViewColumnData newData in SelectedGroup.Columns.Select(MakeColumnData))
		{
			group.ViewColumns.Add(newData.Name, newData);
		}

		// 艦船ID == 0 を作成(空リストを作る)
		group.Expressions.Expressions.Add(new ExpressionList(false, true, false));
		group.Expressions.Expressions[0].Expressions.Add(new ExpressionData(".MasterID", ExpressionData.ExpressionOperator.Equal, 0));
		group.Expressions.Compile();

		group.AddInclusionFilter(ships);

		ShipGroupViewModel.Groups.Add(MakeGroupItem(group));
	}

	private void ExcludeFromGroup()
	{
		if (SelectedGroup?.Group is not ShipGroupData group) return;

		group.AddExclusionFilter(GetSelectedShipIds());

		SelectGroup(SelectedGroup);
	}

	private void FilterGroup()
	{
		if (SelectedGroup?.Group is not ShipGroupData group)
		{
			MessageBox.Show(ShipGroupResources.DialogGroupCanNotBeModifiedDescription, ShipGroupResources.DialogErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
			return;
		}

		try
		{
			group.Expressions ??= new ExpressionManager();

			using DialogShipGroupFilter dialog = new(group);

			if (dialog.ShowDialog(App.Current!.MainWindow!) != DialogResult.OK) return;

			// replace
			int id = group.GroupID;
			group = dialog.ExportGroupData();
			group.GroupID = id;
			group.Expressions.Compile();

			KCDatabase.Instance.ShipGroup.ShipGroups.Remove(id);
			KCDatabase.Instance.ShipGroup.ShipGroups.Add(group);

			int groupIndex = ShipGroupViewModel.Groups.IndexOf(ShipGroupViewModel.Groups.First(g => g.Group.GroupID == id));
			ShipGroupItem updatedGroup = MakeGroupItem(group);
			ShipGroupViewModel.Groups.RemoveAt(groupIndex);
			ShipGroupViewModel.Groups.Insert(groupIndex, updatedGroup);

			SelectGroup(updatedGroup);
		}
		catch (Exception ex)
		{
			ErrorReporter.SendErrorReport(ex, ShipGroupResources.FilterDialogError);
		}
	}

	private void FilterColumns()
	{
		if (SelectedGroup is null) return;

		List<ColumnViewModel> columns = ShipGroupViewModel.ColumnProperties
			.Select(column => new ColumnViewModel(column))
			.ToList();

		ColumnSelectorView columnSelectionView = new(new()
		{
			Columns = columns,
			FrozenColumns = SelectedGroup.FrozenColumns,
		});

		if (columnSelectionView.ShowDialog() != true) return;

		foreach (ColumnViewModel column in columns)
		{
			column.SaveChanges();
		}

		if (columnSelectionView.ViewModel.FrozenColumns is int frozenColumns)
		{
			SelectedGroup.FrozenColumns = frozenColumns;
		}

		SelectedGroup.Columns = columns
			.Select(col => col.ColumnModel)
			.OfType<ColumnModel>()
			.ToObservableCollection();

		ShipGroupViewModel.SelectGroupCommand.Execute(SelectedGroup);

		foreach ((ShipGroupData.ViewColumnData data, ColumnModel model) in ((ShipGroupData)SelectedGroup.Group).ViewColumns.Values.Zip(SelectedGroup.Columns))
		{
			data.Visible = model.IsVisible;
		}
	}

	[System.Diagnostics.CodeAnalysis.SuppressMessage("Critical Code Smell", "S3776:Cognitive Complexity of methods should not be too high", Justification = "todo")]
	private void ExportCsv()
	{
		// todo: does this need translating?
		const string shipCsvHeaderUser = "固有ID,艦種,艦名,Lv,Exp,next,改装まで,耐久現在,耐久最大,Cond,燃料,弾薬,装備1,装備2,装備3,装備4,装備5,補強装備,入渠,火力,火力改修,火力合計,雷装,雷装改修,雷装合計,対空,対空改修,対空合計,装甲,装甲改修,装甲合計,対潜,対潜合計,回避,回避合計,索敵,索敵合計,運,運改修,運合計,射程,速力,ロック,出撃先,母港ソートID,航空威力,砲撃威力,空撃威力,対潜威力,雷撃威力,夜戦威力";
		const string shipCsvHeaderData = "固有ID,艦種,艦名,艦船ID,Lv,Exp,next,改装まで,耐久現在,耐久最大,Cond,燃料,弾薬,装備1,装備2,装備3,装備4,装備5,補強装備,装備ID1,装備ID2,装備ID3,装備ID4,装備ID5,補強装備ID,艦載機1,艦載機2,艦載機3,艦載機4,艦載機5,入渠,入渠燃料,入渠鋼材,火力,火力改修,火力合計,雷装,雷装改修,雷装合計,対空,対空改修,対空合計,装甲,装甲改修,装甲合計,対潜,対潜合計,回避,回避合計,索敵,索敵合計,運,運改修,運合計,射程,速力,ロック,出撃先,母港ソートID,航空威力,砲撃威力,空撃威力,対潜威力,雷撃威力,夜戦威力";

		IEnumerable<ShipData?> ships = SelectedGroup switch
		{
			null => KCDatabase.Instance.Ships.Values,
			_ => ShipGroupViewModel.SelectedShips
				.Select(s => KCDatabase.Instance.Ships[s.MasterId]),
		};

		using DialogShipGroupCSVOutput dialog = new();

		if (dialog.ShowDialog(App.Current!.MainWindow!) != DialogResult.OK) return;

		try
		{
			using StreamWriter sw = new(dialog.OutputPath, false, Configuration.Config.Log.FileEncoding);

			string header = dialog.OutputFormat switch
			{
				DialogShipGroupCSVOutput.OutputFormatConstants.User => shipCsvHeaderUser,
				_ => shipCsvHeaderData,
			};

			sw.WriteLine(header);

			foreach (ShipData ship in ships.OfType<ShipData>())
			{
				if (dialog.OutputFormat is DialogShipGroupCSVOutput.OutputFormatConstants.User)
				{
					sw.WriteLine(string.Join(",",
						ship.MasterID,
						ship.MasterShip.ShipTypeName,
						ship.MasterShip.NameWithClass,
						ship.Level,
						ship.ExpTotal,
						ship.ExpNext,
						ship.ExpNextRemodel,
						ship.HPCurrent,
						ship.HPMax,
						ship.Condition,
						ship.Fuel,
						ship.Ammo,
						GetEquipmentString(ship, 0),
						GetEquipmentString(ship, 1),
						GetEquipmentString(ship, 2),
						GetEquipmentString(ship, 3),
						GetEquipmentString(ship, 4),
						GetEquipmentString(ship, 5),
						DateTimeHelper.ToTimeRemainString(DateTimeHelper.FromAPITimeSpan(ship.RepairTime)),
						ship.FirepowerBase,
						ship.FirepowerRemain,
						ship.FirepowerTotal,
						ship.TorpedoBase,
						ship.TorpedoRemain,
						ship.TorpedoTotal,
						ship.AABase,
						ship.AARemain,
						ship.AATotal,
						ship.ArmorBase,
						ship.ArmorRemain,
						ship.ArmorTotal,
						ship.ASWBase,
						ship.ASWTotal,
						ship.EvasionBase,
						ship.EvasionTotal,
						ship.LOSBase,
						ship.LOSTotal,
						ship.LuckBase,
						ship.LuckRemain,
						ship.LuckTotal,
						Constants.GetRange(ship.Range),
						Constants.GetSpeed(ship.Speed),
						ship switch
						{
							{ IsLocked: true } => "●",
							{ IsLockedByEquipment: true } => "■",
							_ => "-",
						},
						ship.SallyArea,
						ship.MasterShip.SortID,
						ship.AirBattlePower,
						ship.ShellingPower,
						ship.AircraftPower,
						ship.AntiSubmarinePower,
						ship.TorpedoPower,
						ship.NightBattlePower
					));
				}
				else
				{
					sw.WriteLine(string.Join(",",
						ship.MasterID,
						(int)ship.MasterShip.ShipType,
						ship.MasterShip.NameWithClass,
						ship.ShipID,
						ship.Level,
						ship.ExpTotal,
						ship.ExpNext,
						ship.ExpNextRemodel,
						ship.HPCurrent,
						ship.HPMax,
						ship.Condition,
						ship.Fuel,
						ship.Ammo,
						GetEquipmentString(ship, 0),
						GetEquipmentString(ship, 1),
						GetEquipmentString(ship, 2),
						GetEquipmentString(ship, 3),
						GetEquipmentString(ship, 4),
						GetEquipmentString(ship, 5),
						ship.Slot[0],
						ship.Slot[1],
						ship.Slot[2],
						ship.Slot[3],
						ship.Slot[4],
						ship.ExpansionSlot,
						ship.Aircraft[0],
						ship.Aircraft[1],
						ship.Aircraft[2],
						ship.Aircraft[3],
						ship.Aircraft[4],
						ship.RepairTime,
						ship.RepairFuel,
						ship.RepairSteel,
						ship.FirepowerBase,
						ship.FirepowerRemain,
						ship.FirepowerTotal,
						ship.TorpedoBase,
						ship.TorpedoRemain,
						ship.TorpedoTotal,
						ship.AABase,
						ship.AARemain,
						ship.AATotal,
						ship.ArmorBase,
						ship.ArmorRemain,
						ship.ArmorTotal,
						ship.ASWBase,
						ship.ASWTotal,
						ship.EvasionBase,
						ship.EvasionTotal,
						ship.LOSBase,
						ship.LOSTotal,
						ship.LuckBase,
						ship.LuckRemain,
						ship.LuckTotal,
						ship.Range,
						ship.Speed,
						ship switch
						{
							{ IsLocked: true } => 1,
							{ IsLockedByEquipment: true } => 2,
							_ => 0,
						},
						ship.SallyArea,
						ship.MasterShip.SortID,
						ship.AirBattlePower,
						ship.ShellingPower,
						ship.AircraftPower,
						ship.AntiSubmarinePower,
						ship.TorpedoPower,
						ship.NightBattlePower
					));
				}
			}

			Logger.Add(2, string.Format(ShipGroupResources.ExportToCsvSuccess, dialog.OutputPath));
		}
		catch (Exception ex)
		{
			ErrorReporter.SendErrorReport(ex, ShipGroupResources.ExportToCsvFail);
			MessageBox.Show(ShipGroupResources.ExportToCsvFail + "\r\n" + ex.Message, ShipGroupResources.DialogErrorTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}

	private static string GetEquipmentString(ShipData ship, int index)
	{
		if (index < 5)
		{
			return (index >= ship.SlotSize && ship.Slot[index] == -1) switch
			{
				true => "",
				_ => ship.SlotInstance[index]?.NameWithLevel ?? ShipGroupResources.None,
			};
		}

		return ship.ExpansionSlot switch
		{
			0 => "",
			_ => ship.ExpansionSlotInstance?.NameWithLevel ?? ShipGroupResources.None,
		};
	}

	private async Task OpenDataGridSettings(DataGridSettingsModel settings)
	{
		DataGridSettingsViewModel viewModel = new(settings);

		await DialogService.ShowDialogAsync(App.MainViewModel, viewModel);
	}

	private static ShipGroupData.ViewColumnData MakeColumnData(ColumnModel column) => new(column.Name)
	{
		DisplayIndex = column.DisplayIndex,
		Visible = column.IsVisible,
		Width = (int)column.Width.DisplayValue,
		AutoSize = column.Width.IsAuto,
	};

	private static ShipGroupItem MakeGroupItem(ShipGroupData group) => new(group)
	{
		Columns = group.ViewColumns.Values
			.Select(c => new ColumnModel
			{
				Name = c.Name,
				DisplayIndex = c.DisplayIndex,
				IsVisible = c.Visible,
				Width = new(c.Width, c.AutoSize switch
				{
					true => DataGridLengthUnitType.Auto,
					_ => DataGridLengthUnitType.Pixel,
				}),
			})
			.ToObservableCollection(),
		SortDescriptions = group.DataGridSortDescriptionCollection,
	};

	private void SystemShuttingDown()
	{
		if (SelectedGroup is not null)
		{
			// hack: reload the group to save its state
			SelectGroup(SelectedGroup);
		}

		Configuration.Config.FormShipGroup.AutoUpdate = ShipGroupViewModel.AutoUpdate;
		Configuration.Config.FormShipGroup.ShowStatusBar = ShipGroupViewModel.ShowStatusBar;
		Configuration.Config.FormShipGroup.GroupHeight = ShipGroupViewModel.GroupHeight.Value;
		Configuration.Config.FormShipGroup.ColumnHeaderHeight = ShipGroupViewModel.DataGridSettings.ColumnHeaderHeight;
		Configuration.Config.FormShipGroup.RowHeight = ShipGroupViewModel.DataGridSettings.RowHeight;

		IEnumerable<ShipGroupData> shipGroups = ShipGroupViewModel.Groups
			.Select(g => g.Group)
			.OfType<ShipGroupData>()
			.Union(KCDatabase.Instance.ShipGroup.ShipGroups.Values);

		// update group IDs to match their current order
		// the serializer saves groups ordered by ID to preserve user reordering
		foreach ((ShipGroupData group, int i) in shipGroups.Select((g, i) => (g, i)))
		{
			group.GroupID = i + 1;
		}
	}
}
