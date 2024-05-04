using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Avalonia.Controls;
using Avalonia.Win32.Interoperability;
using ElectronicObserver.Avalonia.Behaviors.PersistentColumns;
using ElectronicObserver.Avalonia.ShipGroup;
using ElectronicObserver.Data;
using ElectronicObserver.Data.ShipGroup;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Dialog;
using ElectronicObserverTypes;
using KancolleProgress.Views;
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

		SystemEvents.SystemShuttingDown += SystemShuttingDown;
	}

	public override void Loaded()
	{
		base.Loaded();

		ShipGroupManager groups = KCDatabase.Instance.ShipGroup;


		// 空(≒初期状態)の時、おなじみ全所属艦を追加
		if (groups.ShipGroups.Count == 0)
		{
			/*
			Utility.Logger.Add(3, string.Format(ShipGroupResources.GroupNotFound, ShipGroupManager.DefaultFilePath));

			ShipGroupData group = KCDatabase.Instance.ShipGroup.Add();
			group.Name = GeneralRes.AllAssignedShips;

			for (int i = 0; i < ShipView.Columns.Count; i++)
			{
				ShipGroupData.ViewColumnData newdata = new(ShipView.Columns[i]);
				if (SelectedGroup == null)
					newdata.Visible = true;     //初期状態では全行が非表示のため
				group.ViewColumns.Add(ShipView.Columns[i].Name, newdata);
			}
			*/
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
					.ToList(),
			});
		}

		ConfigurationChanged();


		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += APIUpdated;
		o.ApiGetMember_Ship2.ResponseReceived += APIUpdated;
		o.ApiGetMember_ShipDeck.ResponseReceived += APIUpdated;

		// added later - might affect performance
		o.ApiGetMember_NDock.ResponseReceived += APIUpdated;
		o.ApiReqHensei_PresetSelect.ResponseReceived += APIUpdated;

		Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
	}

	private void ConfigurationChanged()
	{
		ShipGroupViewModel.FormShipGroup.OnPropertyChanged("");
	}

	private void APIUpdated(string apiname, dynamic data)
	{
		if (ShipGroupViewModel.AutoUpdate)
		{
			// todo
			// ChangeShipView(ViewModel.SelectedGroup, ViewModel.PreviousGroup);
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

	private List<int> GetSelectedShipID() => ShipGroupViewModel.SelectedShips
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

		group.AddInclusionFilter(GetSelectedShipID());

		if (group.ID == SelectedGroup?.Id)
		{
			// refresh datagrid
			// ChangeShipView(ViewModel.SelectedGroup, ViewModel.PreviousGroup);
		}
	}

	private void CreateGroup()
	{
		if (SelectedGroup is null) return;

		List<int> ships = GetSelectedShipID();

		if (!ships.Any()) return;

		using DialogTextInput dialog = new(ShipGroupResources.DialogGroupAddTitle, ShipGroupResources.DialogGroupAddDescription);

		if (dialog.ShowDialog(App.Current!.MainWindow!) != DialogResult.OK) return;

		ShipGroupData group = KCDatabase.Instance.ShipGroup.Add();

		group.Name = dialog.InputtedText.Trim();

		foreach (ColumnModel column in SelectedGroup.Columns)
		{
			ShipGroupData.ViewColumnData newData = new(column.Name)
			{
				DisplayIndex = column.DisplayIndex,
				Visible = column.IsVisible,
				Width = (int)column.Width.DisplayValue,
				AutoSize = double.IsNaN(column.Width.Value),
			};

			if (SelectedGroup == null)
			{
				// 初期状態では全行が非表示のため
				newData.Visible = true;
			}

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

		group.AddExclusionFilter(GetSelectedShipID());
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
