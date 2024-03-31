using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ElectronicObserverTypes;

namespace ElectronicObserver.Data.Quest;

/// <summary>
/// 演習任務の進捗を管理します。
/// </summary>
[DataContract(Name = "ProgressPractice")]
public class ProgressPractice : ProgressData
{

	/// <summary>
	/// 勝利のみカウントする
	/// </summary>
	[DataMember]
	private bool WinOnly { get; set; }

	/// <summary>
	/// 条件を満たす最低ランク
	/// </summary>
	[DataMember]
	private int LowestRank { get; set; }

	public ProgressPractice(QuestData quest, int maxCount, bool winOnly)
		: base(quest, maxCount)
	{
		LowestRank = winOnly ? (int)Constants.GetWinRank("B") : (int)Constants.GetWinRank("");
		WinOnly = winOnly;
	}

	public ProgressPractice(QuestData quest, int maxCount, string lowestRank)
		: base(quest, maxCount)
	{
		LowestRank = (int)Constants.GetWinRank(lowestRank);
	}

	public void Increment(string rank)
	{
		if ((int)Constants.GetWinRank(rank) < LowestRank) return;

		if (!MeetsSpecialRequirements(QuestID)) return;

		Increment();
	}

	private bool MeetsSpecialRequirements(int questId)
	{
		FleetData fleet = KCDatabase.Instance.Fleet.Fleets.Values
			.FirstOrDefault(f => f.IsInPractice);

		if (fleet == null) return false;

		List<IShipData> ships = fleet.MembersInstance.Where(s => s != null).ToList();

		return questId switch
		{
			325 => ships.Any(s => s.MasterShip.ShipID == (int)ShipId.YuugumoKaiNi) &&
				   ships.Any(s => s.MasterShip.ShipID == (int)ShipId.NaganamiKaiNi),
			328 => ships.Any(s => s.MasterShip.ShipID == (int)ShipId.IsokazeBKai) &&
				   ships.Any(s => s.MasterShip.ShipID == (int)ShipId.HamakazeBKai) &&
				   ships.Any(s => s.MasterShip.ShipID == (int)ShipId.UrakazeDKai) &&
				   ships.Any(s => s.MasterShip.ShipID == (int)ShipId.TanikazeDKai),
			329 => ships.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer ||
									s.MasterShip.ShipType == ShipTypes.Escort) >= 2,
			330 => ships[0].MasterShip.IsAircraftCarrier &&
				   ships.Count(s => s.MasterShip.IsAircraftCarrier) >= 2 &&
				   ships.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer) >= 2,
			331 => ships[0].MasterShip.IsRegularCarrier &&
				   ships.Count(s => s.MasterShip.IsRegularCarrier) >= 2 &&
				   ships.Count(s => s.MasterShip.ShipType == ShipTypes.Destroyer) >= 2,
			336 => ships.Count(s => s.MasterShip.ShipType == ShipTypes.AmphibiousAssaultShip ||
									s.MasterShip.ShipType == ShipTypes.FleetOiler ||
									s.MasterShip.ShipType == ShipTypes.Escort) >= 2,
			337 => ships.Any(s => s.MasterShip.BaseShip().ShipID == (int)ShipId.Kagerou) &&
				   ships.Any(s => s.MasterShip.BaseShip().ShipID == (int)ShipId.Shiranui) &&
				   ships.Any(s => s.MasterShip.BaseShip().ShipID == (int)ShipId.Arare) &&
				   ships.Any(s => s.MasterShip.BaseShip().ShipID == (int)ShipId.Kasumi),
			339 => ships.Count(s => s.MasterShip.BaseShip().ShipId switch
			{
				ShipId.Isonami => true,
				ShipId.Uranami => true,
				ShipId.Ayanami => true,
				ShipId.Shikinami => true,

				_ => false
			}) >= 4,
			341 => ships.Count(s => s.MasterShip.BaseShip().ShipId switch
			{
				ShipId.Oboro => true,
				ShipId.Akebono => true,
				ShipId.Sazanami => true,
				ShipId.Ushio => true,

				_ => false
			}) >= 2,
			// DD+DE >= 3 and another DD/DE or CL
			342 => ships.Count(s => s.MasterShip.ShipType switch
				   {
					   ShipTypes.Destroyer => true,
					   ShipTypes.Escort => true,

					   _ => false
				   }) >= 3
				   && ships.Count(s => s.MasterShip.ShipType switch
				   {
					   ShipTypes.Destroyer => true,
					   ShipTypes.Escort => true,
					   ShipTypes.LightCruiser => true,

					   _ => false
				   }) >= 4,

			// C53
			348 => ships.Count(s => s.MasterShip.ShipType is
					   ShipTypes.LightCruiser or
					   ShipTypes.TrainingCruiser
				   ) >= 3 &&
				   ships[0].MasterShip.ShipType is ShipTypes.LightCruiser or ShipTypes.TrainingCruiser &&
				   ships.Count(s => s.MasterShip.ShipType is ShipTypes.Destroyer) >= 2,

			// 2102 LQ3
			349 =>
				ships.Count(s => s.MasterShip.ShipType is ShipTypes.Destroyer) >= 2 &&
				ships.Count(s => s.MasterShip.ShipType is ShipTypes.SeaplaneTender) >= 1 &&
				ships.Count(s => s.MasterShip.ShipType is
					ShipTypes.LightCruiser or
					ShipTypes.TrainingCruiser or
					ShipTypes.TorpedoCruiser
				) >= 1 &&
				ships.Count(s => s.MasterShip.ShipType is
					ShipTypes.HeavyCruiser or
					ShipTypes.AviationCruiser
				) >= 1,

			_ => true,
		};
	}

	public override string GetClearCondition()
	{
		return QuestTracking.Exercise + (WinOnly ? QuestTracking.ClearConditionVictories : "×") + ProgressMax;
	}
}
