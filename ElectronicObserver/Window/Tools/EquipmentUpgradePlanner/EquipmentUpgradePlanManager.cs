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
		// not found => Set to null 
		// TODO : when upgrading something that isn't in the plan list look for an entry with the same equipment id and master id null and assign it at that moment 
		IEquipmentData equipmentData = model.EquipmentMasterId switch
		{
			int => KCDatabase.Instance.Equipments.ContainsKey((int)model.EquipmentMasterId) switch
			{
				true => KCDatabase.Instance.Equipments[(int)model.EquipmentMasterId]!,
				_ => new EquipmentDataMock(masterEquipment)
			},
			_ => new EquipmentDataMock(masterEquipment)
		};

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
			model.EquipmentMasterId = viewModel.Equipment.MasterID > 0 ? viewModel.Equipment.MasterID : null;
			model.DesiredUpgradeLevel = viewModel.DesiredUpgradeLevel;
			model.Finished = viewModel.Finished;
			model.Priority = viewModel.Priority;
		}

		db.SaveChanges();
	}
}
