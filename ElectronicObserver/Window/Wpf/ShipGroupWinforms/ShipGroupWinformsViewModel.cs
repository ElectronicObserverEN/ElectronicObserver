﻿using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Forms;
using Avalonia.Win32.Interoperability;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.ShipGroup;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;
using MessageBox = System.Windows.Forms.MessageBox;

namespace ElectronicObserver.Window.Wpf.ShipGroupWinforms;

public class ShipGroupItem : ObservableObject
{
	public ShipGroupData Group { get; }

	public string Name { get; set; }
	public bool IsSelected { get; set; }

	public ShipGroupItem(ShipGroupData group)
	{
		Group = group;

		Name = group.Name;

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(Name)) return;

			Group.Name = Name;
		};
	}
}

public sealed partial class ShipGroupWinformsViewModel : AnchorableViewModel
{
	public WpfAvaloniaHost WpfAvaloniaHost { get; }
	public ShipGroupView ShipGroupView { get; }
	public ShipGroupViewModel ShipGroupViewModel { get; }
	public FormShipGroupTranslationViewModel FormShipGroup { get; }

	public bool AutoUpdate { get; set; }
	public bool ShowStatusBar { get; set; }

	public GridLength GroupHeight { get; set; }

	public ObservableCollection<ShipGroupItem> Groups { get; } = new();
	public ShipGroupItem? PreviousGroup { get; set; }
	public ShipGroupItem? SelectedGroup { get; set; }

	public ShipGroupWinformsViewModel() : base("Group", "ShipGroup", IconContent.FormShipGroup)
	{
		FormShipGroup = Ioc.Default.GetRequiredService<FormShipGroupTranslationViewModel>();

		Title = FormShipGroup.Title;
		FormShipGroup.PropertyChanged += (_, _) => Title = FormShipGroup.Title;

		ShipGroupViewModel = new();
		ShipGroupView = new()
		{
			DataContext = ShipGroupViewModel,
		};
		WpfAvaloniaHost = new()
		{
			Content = ShipGroupView,
		};

		Configuration.ConfigurationData config = Configuration.Config;

		AutoUpdate = config.FormShipGroup.AutoUpdate;
		ShowStatusBar = config.FormShipGroup.ShowStatusBar;
		GroupHeight = new(config.FormShipGroup.GroupHeight);

		Loaded();
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
			Groups.Add(new(g));
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
		Configuration.ConfigurationData config = Configuration.Config;
	}

	private void APIUpdated(string apiname, dynamic data)
	{
		if (AutoUpdate)
		{
			// todo
			// ChangeShipView(ViewModel.SelectedGroup, ViewModel.PreviousGroup);
		}
	}

	[RelayCommand]
	private void SelectGroup(ShipGroupItem group)
	{
		ShipGroupViewModel.Items = KCDatabase.Instance.Ships.Values
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

	[RelayCommand]
	private void AddGroup()
	{
		using DialogTextInput dialog = new(FormShipGroup.DialogGroupAddTitle, FormShipGroup.DialogGroupAddDescription);

		if (dialog.ShowDialog(App.Current.MainWindow) != DialogResult.OK) return;

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
		Groups.Add(new(group));
	}

	[RelayCommand]
	private void CopyGroup(ShipGroupItem group)
	{
		using var dialog = new DialogTextInput(FormShipGroup.DialogGroupCopyTitle, FormShipGroup.DialogGroupCopyDescription);

		if (dialog.ShowDialog(App.Current.MainWindow) != DialogResult.OK) return;

		var newGroup = group.Group.Clone();

		newGroup.GroupID = KCDatabase.Instance.ShipGroup.GetUniqueID();
		newGroup.Name = dialog.InputtedText.Trim();

		KCDatabase.Instance.ShipGroup.ShipGroups.Add(newGroup);

		Groups.Add(new(newGroup));
	}

	[RelayCommand]
	private void RenameGroup(ShipGroupItem group)
	{
		using var dialog = new DialogTextInput(FormShipGroup.DialogGroupRenameTitle, FormShipGroup.DialogGroupRenameDescription);
		dialog.InputtedText = group.Name;

		if (dialog.ShowDialog(App.Current.MainWindow) == DialogResult.OK)
		{
			group.Name = dialog.InputtedText.Trim();
		}
	}

	[RelayCommand]
	private void DeleteGroup(ShipGroupItem group)
	{
		if (MessageBox.Show(string.Format(FormShipGroup.DialogGroupDeleteDescription, group.Name),
			FormShipGroup.DialogGroupDeleteTitle,
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

		Groups.Remove(group);
		KCDatabase.Instance.ShipGroup.ShipGroups.Remove(group.Group);
	}
}
