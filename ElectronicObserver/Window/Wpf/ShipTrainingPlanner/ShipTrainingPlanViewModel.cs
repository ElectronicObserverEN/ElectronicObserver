﻿using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Window.Wpf.ShipTrainingPlanner;
public partial class ShipTrainingPlanViewModel : WindowViewModelBase
{
	public ShipTrainingPlanModel Model { get; }

	public IShipData Ship => KCDatabase.Instance.Ships[Model.ShipId] switch
	{
		IShipData => KCDatabase.Instance.Ships[Model.ShipId],
		_ => new ShipDataMock(new ShipDataMasterMock())
	};

	public int TargetLevel { get; set; }
	public int MaximumLevel => ExpTable.ShipMaximumLevel;

	public int TargetHP => Ship.HPMax + TargetHPBonus;

	public int TargetASW => Ship.ASWBase + TargetASWBonus;

	public ShipTrainingPlannerTranslationViewModel ShipTrainingPlanner { get; }

	public bool ShipRemodelLevelReached =>
		TargetRemodel is IShipDataMaster remodel
		&& Ship.MasterShip.ShipId != remodel.ShipId
		&& remodel.RemodelBeforeShip is IShipDataMaster shipBefore
		&& Ship.Level >= shipBefore.RemodelAfterLevel;

	public bool ShipAnyRemodelLevelReached => Ship.Level >= Ship.MasterShip.RemodelAfterLevel;

	public bool ShouldNotifyRemodelReady => NotifyOnAnyRemodelReady switch
	{
		true => ShipAnyRemodelLevelReached,
		_ => NotifyOnRemodelReady switch
		{
			true => ShipRemodelLevelReached,
			_ => false
		}
	};

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

	public bool NotifyOnRemodelReady { get; set; }

	public bool NotifyOnAnyRemodelReady { get; set; }

	public ShipTrainingPlanViewModel(ShipTrainingPlanModel model)
	{
		ShipTrainingPlanner = Ioc.Default.GetRequiredService<ShipTrainingPlannerTranslationViewModel>();
		Model = model;

		UpdateFromModel();

		SubscribeToApi();
		PropertyChanged += OnStatPropertyChanged;
	}

	private void SubscribeToApi()
	{
		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += (_, _) => OnPropertyChanged(nameof(Ship));

		o.ApiReqMission_Result.ResponseReceived += (_, _) => OnPropertyChanged(nameof(Ship));
		o.ApiReqPractice_BattleResult.ResponseReceived += (_, _) => OnPropertyChanged(nameof(Ship));
		o.ApiReqCombinedBattle_BattleResult.ResponseReceived += (_, _) => OnPropertyChanged(nameof(Ship));
		o.ApiReqSortie_BattleResult.ResponseReceived += (_, _) => OnPropertyChanged(nameof(Ship));

		o.ApiReqKaisou_Marriage.ResponseReceived += (_, _) => OnPropertyChanged(nameof(Ship));

		o.ApiReqKaisou_PowerUp.ResponseReceived += (_, _) => OnPropertyChanged(nameof(Ship));
		o.ApiReqKaisou_Remodeling.ResponseReceived += (_, _) => OnPropertyChanged(nameof(Ship));

		o.ApiReqKaisou_PowerUp.ResponseReceived += (_, _) => OnPropertyChanged(nameof(TargetASW));
		o.ApiReqKaisou_Remodeling.ResponseReceived += (_, _) => OnPropertyChanged(nameof(TargetASW));

		o.ApiReqKaisou_PowerUp.ResponseReceived += (_, _) => OnPropertyChanged(nameof(TargetHP));
		o.ApiReqKaisou_Remodeling.ResponseReceived += (_, _) => OnPropertyChanged(nameof(TargetHP));
		o.ApiReqKaisou_PowerUp.ResponseReceived += (_, _) => OnPropertyChanged(nameof(MaximumHPMod));
		o.ApiReqKaisou_Remodeling.ResponseReceived += (_, _) => OnPropertyChanged(nameof(MaximumHPMod));
	}

	public void UpdateFromModel()
	{
		TargetLevel = Model.TargetLevel;
		TargetLuck = Model.TargetLuck;
		TargetHPBonus = Model.TargetHPBonus;
		TargetASWBonus = Model.TargetASWBonus;

		NotifyOnRemodelReady = Model.NotifyOnRemodelReady;
		NotifyOnAnyRemodelReady = Model.NotifyOnAnyRemodelReady;

		if (Model.TargetRemodel is ShipId shipId)
			TargetRemodel = KCDatabase.Instance.MasterShips[(int)shipId];

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
		if (e.PropertyName is nameof(TargetHPBonus)) OnPropertyChanged(nameof(TargetHP));
		if (e.PropertyName is nameof(TargetASWBonus)) OnPropertyChanged(nameof(TargetASW));
	}

	public void Save()
	{
		Model.TargetLevel = TargetLevel;
		Model.TargetLuck = TargetLuck;
		Model.TargetHPBonus = TargetHPBonus;
		Model.TargetASWBonus = TargetASWBonus;
		Model.TargetRemodel = TargetRemodel?.ShipId;

		Model.NotifyOnRemodelReady = NotifyOnRemodelReady;
		Model.NotifyOnAnyRemodelReady = NotifyOnAnyRemodelReady;
	}
}
