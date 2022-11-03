using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Observer;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
public class EquipmentUpgradePlanManager
{
	public bool IsInitialized { get; private set; } = false;

	public ObservableCollection<EquipmentUpgradePlanItemViewModel> PlannedUpgrades { get; private set; } = new();

	public EquipmentUpgradePlanManager()
	{
		SubscribeToApi();
	}

	private void SubscribeToApi()
	{
		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += (_, __) => Load();
	}

	public void Load()
	{
		if (IsInitialized) return;
		if (!KCDatabase.Instance.MasterEquipments.Any()) return;

		PlannedUpgrades.Clear();

		using ElectronicObserverContext db = new();
		List<EquipmentUpgradePlanItemModel> models = db.EquipmentUpgradePlanItems.ToList();

		foreach (EquipmentUpgradePlanItemModel model in models)
		{
			LoadModel(model);
		}

		IsInitialized = true;
	}

	private void LoadModel(EquipmentUpgradePlanItemModel model)
	{
		if (!KCDatabase.Instance.MasterEquipments.ContainsKey((int)model.EquipmentId)) throw new Exception("Equipment not found");

		IEquipmentDataMaster masterEquipment = KCDatabase.Instance.MasterEquipments[(int)model.EquipmentId];

		// Try to load the owned equipment
		// not found => scrapped ? lost ? logged on another acc ? (what to do ?)
		IEquipmentData equipmentData;

		if (model.EquipmentMasterId is int _dropIdNotNull && KCDatabase.Instance.Equipments.ContainsKey(_dropIdNotNull))
		{
			equipmentData = KCDatabase.Instance.Equipments[_dropIdNotNull]!;
		}
		else
		{
			equipmentData = new EquipmentDataMock(masterEquipment);
		}

		// TODO : try to match to an owned equipment (if _equipment is null) => do a method that can be called on some api updates (craft/drop/reward/...)

		PlannedUpgrades.Add(new EquipmentUpgradePlanItemViewModel(equipmentData)
		{
			Id = model.Id,
			DesiredUpgradeLevel = model.DesiredUpgradeLevel,
			Finished = model.Finished,
			Priority = model.Priority,
		});
	}

	public void Save()
	{
		if (!IsInitialized) return;

		using ElectronicObserverContext db = new();

		foreach (EquipmentUpgradePlanItemViewModel viewModel in PlannedUpgrades)
		{
			EquipmentUpgradePlanItemModel model = db.EquipmentUpgradePlanItems.Find(viewModel.Id) ?? db.EquipmentUpgradePlanItems.Add(new()).Entity;

			model.EquipmentId = viewModel.Equipment.EquipmentId;
			model.EquipmentMasterId = viewModel.Equipment.MasterID;
			model.DesiredUpgradeLevel = viewModel.DesiredUpgradeLevel;
			model.Finished = viewModel.Finished;
			model.Priority = viewModel.Priority;
		}

		db.SaveChanges();
	}
}
