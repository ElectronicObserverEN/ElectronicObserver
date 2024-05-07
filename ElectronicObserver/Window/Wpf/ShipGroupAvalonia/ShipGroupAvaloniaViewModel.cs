using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Avalonia.Controls;
using Avalonia.Win32.Interoperability;
using ElectronicObserver.Avalonia.Behaviors.PersistentColumns;
using ElectronicObserver.Avalonia.ShipGroup;
using ElectronicObserver.Common.Datagrid;
using ElectronicObserver.Data;
using ElectronicObserver.Data.ShipGroup;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog;
using ElectronicObserverTypes;
using KancolleProgress.Views;
using GeneralRes = ElectronicObserver.Avalonia.ShipGroup.GeneralRes;
using MessageBox = System.Windows.Forms.MessageBox;
using ShipGroupResources = ElectronicObserver.Avalonia.ShipGroup.ShipGroupResources;

namespace ElectronicObserver.Window.Wpf.ShipGroupAvalonia;

public sealed class ShipGroupAvaloniaViewModel : AnchorableViewModel
{
	public WpfAvaloniaHost WpfAvaloniaHost { get; }
	private ShipGroupView ShipGroupView { get; }
	private ShipGroupViewModel ShipGroupViewModel { get; }

	private ShipGroupItem? PreviousGroup { get; set; }
	public ShipGroupItem? SelectedGroup { get; set; }

	public ShipGroupAvaloniaViewModel() : base("Group", "ShipGroup", IconContent.FormShipGroup)
	{
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

		foreach (ShipGroupData g in groups.ShipGroups.Values)
		{
			ShipGroupViewModel.Groups.Add(new(g)
			{
				Columns = g.ViewColumns.Values
					.Select(c => new ColumnModel
					{
						Name = c.Name,
						DisplayIndex = c.DisplayIndex,
						IsVisible = c.Visible,
						SortDirection = null,
						Width = new(c.Width, c.AutoSize switch
						{
							true => DataGridLengthUnitType.Auto,
							_ => DataGridLengthUnitType.Pixel,
						}),
						SortMemberPath = "",
					})
					.ToObservableCollection(),
			});
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
		ShipGroupViewModel.Items = ((ShipGroupData)group.Group).MembersInstance
			.OfType<IShipData>()
			.Select(s => new ShipGroupItemViewModel(s))
			.ToObservableCollection();

		PreviousGroup = SelectedGroup;
		SelectedGroup = group;

		SelectedGroup.IsSelected = true;

		if (PreviousGroup is null) return;

		if (PreviousGroup == SelectedGroup)
		{
			// force DataGrid refresh when clicking the current group
			OnPropertyChanged(nameof(SelectedGroup));
			return;
		}

		PreviousGroup.IsSelected = false;
	}

	private void AddGroup()
	{
		using DialogTextInput dialog = new(ShipGroupResources.DialogGroupAddTitle,
			ShipGroupResources.DialogGroupAddDescription);

		if (dialog.ShowDialog(App.Current!.MainWindow!) != DialogResult.OK) return;

		ShipGroupData group = KCDatabase.Instance.ShipGroup.Add();
		group.Name = dialog.InputtedText.Trim();

		/*
		for (int i = 0; i < ShipGroup.DataGrid.Columns.Count; i++)
		{
			var newdata = new ShipGroupData.ViewColumnData(ShipGroup.DataGrid.Columns[i]);
			if (SelectedGroup is null)
			{
				newdata.Visible = true;     //初期状態では全行が非表示のため
			}
			@group.ViewColumns.Add(ShipGroup.DataGrid.Columns[i].Name, newdata);
		}
		*/
		ShipGroupViewModel.Groups.Add(new(group));
	}

	private void CopyGroup(ShipGroupItem group)
	{
		using DialogTextInput dialog = new(ShipGroupResources.DialogGroupCopyTitle,
			ShipGroupResources.DialogGroupCopyDescription);

		if (dialog.ShowDialog(App.Current!.MainWindow!) != DialogResult.OK) return;

		ShipGroupData newGroup = (ShipGroupData)group.Group.Clone();

		newGroup.GroupID = KCDatabase.Instance.ShipGroup.GetUniqueID();
		newGroup.Name = dialog.InputtedText.Trim();

		KCDatabase.Instance.ShipGroup.ShipGroups.Add(newGroup);

		ShipGroupViewModel.Groups.Add(new(newGroup));
	}

	private void RenameGroup(ShipGroupItem group)
	{
		using DialogTextInput dialog = new(ShipGroupResources.DialogGroupRenameTitle, ShipGroupResources.DialogGroupRenameDescription);
		dialog.InputtedText = group.Name;

		if (dialog.ShowDialog(App.Current!.MainWindow!) == DialogResult.OK)
		{
			group.Name = dialog.InputtedText.Trim();
		}
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
			// ShipGroup.DataGrid.Rows.Clear();
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
			// refresh datagrid
			// ChangeShipView(ViewModel.SelectedGroup, ViewModel.PreviousGroup);
		}
	}

	private void CreateGroup()
	{
		if (SelectedGroup is null) return;

		List<int> ships = GetSelectedShipIds();

		if (!ships.Any()) return;

		using DialogTextInput dialog = new(ShipGroupResources.DialogGroupAddTitle, ShipGroupResources.DialogGroupAddDescription);

		if (dialog.ShowDialog(App.Current!.MainWindow!) != DialogResult.OK) return;

		ShipGroupData group = KCDatabase.Instance.ShipGroup.Add();

		group.Name = dialog.InputtedText.Trim();

		foreach (ShipGroupData.ViewColumnData newData in SelectedGroup.Columns.Select(MakeColumnData))
		{
			group.ViewColumns.Add(newData.Name, newData);
		}

		// 艦船ID == 0 を作成(空リストを作る)
		group.Expressions.Expressions.Add(new ExpressionList(false, true, false));
		group.Expressions.Expressions[0].Expressions.Add(new ExpressionData(".MasterID", ExpressionData.ExpressionOperator.Equal, 0));
		group.Expressions.Compile();

		group.AddInclusionFilter(ships);
		ShipGroupViewModel.Groups.Add(new(group));

		group.UpdateMembers();
	}

	private void ExcludeFromGroup()
	{
		if (SelectedGroup?.Group is not ShipGroupData group) return;

		group.AddExclusionFilter(GetSelectedShipIds());
		group.UpdateMembers();

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
			ShipGroupItem updatedGroup = new(group);
			ShipGroupViewModel.Groups.RemoveAt(groupIndex);
			ShipGroupViewModel.Groups.Insert(groupIndex, updatedGroup);

			group.UpdateMembers();

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

		ColumnSelectorView columnSelectionView = new(new(columns));

		if (columnSelectionView.ShowDialog() != true) return;

		foreach (ColumnViewModel column in columns)
		{
			column.SaveChanges();
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
						ship.IsLocked ? "●" : ship.IsLockedByEquipment ? "■" : "-",
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
						ship.IsLocked ? 1 : ship.IsLockedByEquipment ? 2 : 0,
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

	private static ShipGroupData.ViewColumnData MakeColumnData(ColumnModel column) => new(column.Name)
	{
		DisplayIndex = column.DisplayIndex,
		Visible = column.IsVisible,
		Width = (int)column.Width.DisplayValue,
		AutoSize = column.Width.IsAuto,
	};

	private void SystemShuttingDown()
	{
		Configuration.Config.FormShipGroup.AutoUpdate = ShipGroupViewModel.AutoUpdate;
		Configuration.Config.FormShipGroup.ShowStatusBar = ShipGroupViewModel.ShowStatusBar;
		Configuration.Config.FormShipGroup.GroupHeight = ShipGroupViewModel.GroupHeight.Value;

		// update group IDs to match their current order
		// the serializer saves groups ordered by ID to preserve user reordering
		foreach ((ShipGroupData group, int i) in ShipGroupViewModel.Groups.Select((g, i) => ((ShipGroupData)g.Group, i)))
		{
			group.GroupID = i + 1;
		}
	}
}
