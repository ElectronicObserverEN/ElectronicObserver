using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.ElectronicObserverApi.Models;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Serialization.FitBonus;

namespace ElectronicObserver.Utility.ElectronicObserverApi.DataIssueLogs;

public class FitBonusIssueReporter(ElectronicObserverApiService api)
{
	private List<FitBonusIssueModel> AlreadySentIssues { get; } = new();

	public void ProcessShipDataChanged(string _, dynamic data)
	{
		if (!api.IsServerAvailable) return;

		foreach (dynamic elem in data.api_ship_data)
		{
			ReportIssueIfNeeded(elem);
		}
	}

	private void ReportIssueIfNeeded(dynamic elem)
	{
		int id = (int)elem.api_id;
		IShipData ship = KCDatabase.Instance.Ships[id];

		KCDatabase db = KCDatabase.Instance;

		// If there's equipment that doesn't exist in the db (remodel), we can't report the issue
		if (ship.Slot.Where(equipmentId => equipmentId > 0).Any(equipmentId => !db.Equipments.ContainsKey(equipmentId)))
		{
			return;
		}

		FitBonusValue theoricalBonus = ship.GetTheoricalFitBonus(KCDatabase.Instance.Translation.FitBonus.FitBonusList);

		if (!ship.MasterShip.ASW.IsDetermined)
		{
			theoricalBonus.ASW = 0;
		}

		if (!ship.MasterShip.Evasion.IsDetermined)
		{
			theoricalBonus.Evasion = 0;
		}

		if (!ship.MasterShip.LOS.IsDetermined)
		{
			theoricalBonus.LOS = 0;
		}

		// There's no way to get accuracy bonus from the API
		theoricalBonus.Accuracy = 0; 
		// Same for bombing
		theoricalBonus.Bombing = 0;

		FitBonusValue actualBonus = ship.GetFitBonus();

		if (!actualBonus.Equals(theoricalBonus))
		{
			ReportIssue(ship, theoricalBonus, actualBonus);
		}
	}

	private bool CheckIfIssueAlreadySent(FitBonusIssueModel issue) 
		=> AlreadySentIssues.Exists(item => issue.Ship.IsSameShip(item.Ship) && issue.Equipments.SequenceEqual(item.Equipments));

	private void ReportIssue(IShipData ship, FitBonusValue theoricalBonus, FitBonusValue actualBonus)
	{
		FitBonusIssueModel issue = new FitBonusIssueModel()
		{
			DataVersion = SoftwareUpdater.CurrentVersion.FitBonuses,
			SoftwareVersion = SoftwareInformation.VersionEnglish,

			ExpectedBonus = theoricalBonus,
			ActualBonus = actualBonus,

			Equipments = ship.AllSlotInstance
				.Where(eq => eq is not null)
				.Cast<IEquipmentData>()
				.Select(eq => new EquipmentModel()
				{
					EquipmentId = eq.EquipmentId,
					Level = eq.UpgradeLevel
				})
				.ToList(),

			Ship = new()
			{
				ShipId = ship.MasterShip.ShipId,
				Level = ship.Level,

				Firepower = ship.FirepowerTotal,
				Torpedo = ship.TorpedoTotal,
				AntiAir = ship.AATotal,
				Armor = ship.ArmorTotal,

				Evasion = ship.EvasionTotal,
				EvasionDetermined = ship.MasterShip.Evasion.IsDetermined,
				ASW = ship.ASWTotal,
				ASWDetermined = ship.MasterShip.ASW.IsDetermined,
				LOS = ship.LOSTotal,
				LOSDetermined = ship.MasterShip.LOS.IsDetermined,

				Accuracy = ship.AccuracyTotal,

				Range = ship.MasterShip.Range,
			},
		};

		if (CheckIfIssueAlreadySent(issue)) return;

		AlreadySentIssues.Add(issue);

#pragma warning disable CS4014
		api.PostJson("FitBonusIssues", issue);
#pragma warning restore CS4014
	}
}
