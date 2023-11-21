using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.ElectronicObserverApi.Models;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Serialization.FitBonus;

namespace ElectronicObserver.Utility.ElectronicObserverApi.DataIssueLogs;

public class FitBonusIssueReporter
{
	private List<FitBonusIssueModel> AlreadySentIssues { get; } = new();
	private ElectronicObserverApiService Api { get; }

	public FitBonusIssueReporter(ElectronicObserverApiService api)
	{
		Api = api;
		APIObserver.Instance.ApiGetMember_Ship3.ResponseReceived += ApiGetMember_Ship3OnResponseReceived;
	}

	private void ApiGetMember_Ship3OnResponseReceived(string apiname, dynamic data)
	{
		foreach (var elem in data.api_ship_data)
		{
			int id = (int)elem.api_id;
			IShipData ship = KCDatabase.Instance.Ships[id];

			FitBonusValue actualBonus = ship.GetFitBonus();
			FitBonusValue theoricalBonus = ship.GetTheoricalFitBonus(KCDatabase.Instance.Translation.FitBonus.FitBonusList);

			if (!actualBonus.Equals(theoricalBonus))
			{
				ReportIssue(ship, theoricalBonus, actualBonus);
			}
		}
	}

	private bool CheckIfIssueAlreadySent(FitBonusIssueModel issue) 
		=> AlreadySentIssues.Exists(item => item.Ship == issue.Ship && issue.Equipments.SequenceEqual(item.Equipments));

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
				ASW = ship.ASWTotal,
				LOS = ship.LOSTotal,

				Accuracy = ship.AccuracyTotal,

				Range = ship.MasterShip.Range,
			},
		};

		if (CheckIfIssueAlreadySent(issue)) return;

		AlreadySentIssues.Add(issue);

#pragma warning disable CS4014
		Api.PostJson("FitBonusIssues", issue);
#pragma warning restore CS4014
	}
}
