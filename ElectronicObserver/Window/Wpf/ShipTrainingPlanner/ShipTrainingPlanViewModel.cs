using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
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
		not null => KCDatabase.Instance.Ships[Model.ShipId],
		_ => new ShipDataMock(new ShipDataMasterMock()),
	};

	[ObservableProperty] private bool _planFinished;

	public int Priority { get; set; }

	public ShipTrainingPlannerTranslationViewModel ShipTrainingPlanner { get; }

	public bool ShipRemodelLevelReached =>
		TargetRemodel?.Ship is { } remodel
		&& Ship.MasterShip.ShipId != remodel.ShipId
		&& remodel.RemodelBeforeShip is { } shipBefore
		&& Ship.Level >= shipBefore.RemodelAfterLevel;

	public bool ShipAnyRemodelLevelReached => Ship.Level >= Ship.MasterShip.RemodelAfterLevel && Ship.MasterShip.RemodelAfterLevel > 0;

	public bool ShouldNotifyRemodelReady => NotifyOnAnyRemodelReady switch
	{
		true => ShipAnyRemodelLevelReached,
		_ => ShipRemodelLevelReached
	};

	public int TargetLevel { get; set; }
	public int MaximumLevel => ExpTable.ShipMaximumLevel;
	public int RemainingExpToTarget => Math.Max(ExpTable.GetExpToLevelShip(Ship.ExpTotal, TargetLevel), 0);

	/// <summary>
	/// From 0 to 2
	/// </summary>
	public int TargetHPBonus { get; set; }
	public int TargetHP => Ship.HPMax + TargetHPBonus - Ship.HPMaxModernized;
	public int RemainingHP => TargetHP - Ship.HPMax;

	public int MaximumHPMod => Ship.HpMaxModernizable();

	/// <summary>
	/// From 0 to 9
	/// </summary>
	public int TargetASWBonus { get; set; }
	public int TargetASW => Ship.ASWBase + TargetASWBonus - Ship.ASWModernized;
	public int RemainingASW => TargetASW - Ship.ASWBase;


	/// <summary>
	/// Targetted amount of luck 
	/// e.g. Yukikaze k2 max luck is 120, then I set this value to 120 to target max
	/// </summary>
	public int TargetLuck { get; set; }
	public int RemainingLuck => TargetLuck - Ship.LuckBase;

	public ComboBoxShip? TargetRemodel { get; set; }

	public List<ComboBoxShip> PossibleRemodels { get; } = new();

	public bool NotifyOnAnyRemodelReady { get; set; }

	public ShipTrainingPlanViewModel(ShipTrainingPlanModel model)
	{
		ShipTrainingPlanner = Ioc.Default.GetRequiredService<ShipTrainingPlannerTranslationViewModel>();
		Model = model;

		UpdateFromModel();

		SubscribeToApi();

		PropertyChanged += OnStatPropertyChanged;
		PropertyChanged += OnTargetRemodelChanged;
	}

	private void SubscribeToApi()
	{
		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += UpdateShipData;
		o.ApiReqMap_Next.ResponseReceived += UpdateShipData;

		o.ApiReqKaisou_Marriage.ResponseReceived += UpdateShipData;

		o.ApiReqKaisou_PowerUp.ResponseReceived += UpdateShipData;
		o.ApiReqKaisou_Remodeling.ResponseReceived += UpdateShipData;
	}

	private void UpdateShipData(string apiname, object data)
	{
		OnPropertyChanged(nameof(Ship));
		OnPropertyChanged(nameof(RemainingExpToTarget));
		OnPropertyChanged(nameof(TargetASW));
		OnPropertyChanged(nameof(RemainingASW));
		OnPropertyChanged(nameof(TargetHP));
		OnPropertyChanged(nameof(RemainingHP));
		OnPropertyChanged(nameof(MaximumHPMod));
		OnPropertyChanged(nameof(RemainingLuck));

		UpdatePlanFinished();
	}

	private void UpdatePlanFinished()
	{
		PlanFinished =
			Ship.Level >= TargetLevel
			&& Ship.HPMax >= TargetHP
			&& Ship.ASWBase >= TargetASW
			&& Ship.LuckBase >= TargetLuck
			&& (TargetRemodel is null || Ship.MasterShip.ShipId == TargetRemodel?.Ship.ShipId);
	}

	public void UpdateFromModel()
	{
		TargetLevel = Model.TargetLevel;
		TargetLuck = Model.TargetLuck;
		TargetHPBonus = Model.TargetHPBonus;
		TargetASWBonus = Model.TargetASWBonus;

		Priority = Model.Priority;

		NotifyOnAnyRemodelReady = Model.NotifyOnAnyRemodelReady;

		if (Model.TargetRemodel is { } shipId)
		{
			TargetRemodel = new(KCDatabase.Instance.MasterShips[(int)shipId]);
		}

		PossibleRemodels.Clear();

		ComboBoxShip remodel = new(Ship.MasterShip);

		while (!PossibleRemodels.Contains(remodel))
		{
			PossibleRemodels.Add(remodel);
			if (remodel.Ship.RemodelAfterShip is null) break;
			remodel = new(remodel.Ship.RemodelAfterShip);
		}

		UpdatePlanFinished();
	}

	private void OnStatPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is nameof(TargetHPBonus))
		{
			OnPropertyChanged(nameof(TargetHP));
		}

		if (e.PropertyName is nameof(TargetASWBonus))
		{
			OnPropertyChanged(nameof(TargetASW));
		}
	}

	private void OnTargetRemodelChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(TargetRemodel)) return;

		TargetLevel = TargetRemodel?.Ship.RemodelBeforeShip switch
		{
			{ } ship => ship.RemodelAfterLevel,
			_ => TargetLevel,
		};
	}

	public void Save()
	{
		Model.TargetLevel = TargetLevel;
		Model.TargetLuck = TargetLuck;
		Model.TargetHPBonus = TargetHPBonus;
		Model.TargetASWBonus = TargetASWBonus;
		Model.TargetRemodel = TargetRemodel?.Ship.ShipId;

		Model.Priority = Priority;

		Model.NotifyOnAnyRemodelReady = NotifyOnAnyRemodelReady;
	}
}
