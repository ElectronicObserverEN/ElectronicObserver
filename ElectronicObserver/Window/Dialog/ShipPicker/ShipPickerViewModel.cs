﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Tools.DropRecordViewer;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Window.Dialog.ShipPicker;

public partial class ShipPickerViewModel : WindowViewModelBase
{
	public List<Filter> TypeFilters { get; }

	// todo adding drop record options like this feels really hacky
	// might wanna figure out a better way to handle this
	public List<DropRecordOption>? DropRecordOptions { get; set; }

	private List<IShipDataMaster> AllShips { get; }

	public ObservableCollection<ClassGroup> ShipClassGroups { get; set; } = new();

	public IShipDataMaster? PickedShip { get; private set; }
	public DropRecordOption? PickedOption { get; private set; }

	public ShipPickerViewModel(IEnumerable<IShipDataMaster> ships)
	{
		AllShips = ships.ToList();

		TypeFilters = Enum.GetValues<ShipTypeGroup>().Select(t => new Filter(t)).ToList();

		foreach (Filter filter in TypeFilters)
		{
			filter.PropertyChanged += Filter_PropertyChanged;
		}
	}

	public ShipPickerViewModel() : this(KCDatabase.Instance.MasterShips.Values.Where(s => !s.IsAbyssalShip).OrderBy(s => s.SortID))
	{ 

	}

	[RelayCommand]
	private void SelectShip(IShipDataMaster? ship)
	{
		PickedShip = ship;
	}


	[RelayCommand]
	private void SelectOption(DropRecordOption? option)
	{
		PickedOption = option;
	}

	private void Filter_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		List<ShipTypes> enabledFilters = TypeFilters
			.Where(f => f.IsChecked)
			.SelectMany(f => f.Value.ToTypes())
			.ToList();

		ShipClassGroups = new(AllShips
			.Where(s => enabledFilters.Contains(s.ShipType))
			.GroupBy(s => s.ShipClass)
			.Select(g => new ClassGroup
			{
				Id = g.Key,
				Name = g.Key switch
				{
					111 => $"{Constants.GetShipClass(g.Key, ShipId.Souya699)}・" +
						   $"{Constants.GetShipClass(g.Key, ShipId.Souya645)}・" +
						   $"{Constants.GetShipClass(g.Key, ShipId.Souya650)}",
					_ => Constants.GetShipClass(g.Key)
				},
				Ships = g.ToList(),
			}).ToList());
	}
}
