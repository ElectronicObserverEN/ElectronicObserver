using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Dialog.ShipPicker;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using log4net.Core;

namespace ElectronicObserver.Window.Wpf.ShipTrainingPlanner;
public partial class ShipTrainingPlanViewModel : ObservableObject
{
	public ShipTrainingPlanModel Model { get; }

	public IShipData Ship { get; set; }

	public int TargetLevel { get; set; }

	public int TargetHP => Ship.HPMax + TargetHPBonus;

	public int TargetASW => Ship.ASWBase + TargetASWBonus;

	public event Action? OnSave;

	public bool ShipRemodelLevelReached => TargetRemodel is IShipDataMaster remodel && Ship.Level >= remodel.RemodelAfterLevel;

	/// <summary>
	/// From 0 to 2
	/// </summary>
	public int TargetHPBonus { get; set; }

	/// <summary>
	/// From 0 to 9
	/// </summary>
	public int TargetASWBonus { get; set; }

	public int MaximumHPMod => Ship.HpMaxModernizable();

	/// <summary>
	/// Targetted amount of luck 
	/// eg Yukikaze k2 max luck is 120, then i set this value to 120 to target max
	/// </summary>
	public int TargetLuck { get; set; }

	public IShipDataMaster? TargetRemodel { get; set; }

	public List<IShipDataMaster> PossibleRemodels { get; } = new();

	public ShipTrainingPlanViewModel(ShipTrainingPlanModel model)
	{
		Model = model;

		Ship = KCDatabase.Instance.Ships[model.ShipId];

		TargetLevel = model.TargetLevel;
		TargetLuck = model.TargetLuck;
		TargetHPBonus = model.TargetHPBonus;
		TargetASWBonus = model.TargetASWBonus;

		if (model.TargetRemodel is ShipId _shipId)
			TargetRemodel = KCDatabase.Instance.MasterShips[(int)_shipId];

		Update();

		PropertyChanged += OnStatPropertyChanged;
	}

	private void Update()
	{
		PossibleRemodels.Clear();

		IShipDataMaster? remodel = Ship.MasterShip;

		while (remodel is not null && !PossibleRemodels.Contains(remodel))
		{
			PossibleRemodels.Add(remodel);
			remodel = remodel.RemodelAfterShip;
		}
	}

	private void OnStatPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is nameof(TargetLevel) or nameof(TargetLuck) or nameof(TargetHPBonus) or nameof(TargetASWBonus) or nameof(TargetRemodel))
		{
			Save();
		}

		if (e.PropertyName is nameof(TargetHPBonus)) OnPropertyChanged(nameof(TargetHP));
		if (e.PropertyName is nameof(TargetASWBonus)) OnPropertyChanged(nameof(TargetASW));
	}

	private void Save()
	{
		Model.TargetLevel = TargetLevel;
		Model.TargetLuck = TargetLuck;
		Model.TargetHPBonus = TargetHPBonus;
		Model.TargetASWBonus = TargetASWBonus;
		Model.TargetRemodel = TargetRemodel?.ShipId;

		OnSave?.Invoke();
	}
}
