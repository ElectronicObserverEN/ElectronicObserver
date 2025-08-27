using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Data.Quest;

/// <summary>
/// 装備廃棄任務の進捗を管理します。
/// </summary>
[DataContract(Name = "ProgressDiscard")]
public class ProgressDiscard : ProgressData
{

	/// <summary>
	/// 廃棄した個数をベースにカウントするか
	/// false = 回数、true = 個数
	/// </summary>
	[DataMember]
	private bool CountsAmount { get; set; }

	/// <summary>
	/// 対象となる装備カテゴリ
	/// null ならすべての装備を対象とする
	/// </summary>
	[DataMember]
	private HashSet<int> Categories { get; set; }

	/// <summary>
	/// Categories の扱い
	/// -1=装備ID, 1=図鑑分類, 2=通常の装備カテゴリ, 3=アイコン
	/// </summary>
	[DataMember]
	protected int CategoryIndex { get; set; }


	public ProgressDiscard(QuestData quest, int maxCount, bool countsAmount, int[] categories)
		: this(quest, maxCount, countsAmount, categories, 2) { }

	public ProgressDiscard(QuestData quest, int maxCount, bool countsAmount, int[] categories, int categoryIndex)
		: base(quest, maxCount)
	{
		CountsAmount = countsAmount;
		Categories = categories == null ? null : new HashSet<int>(categories);
		CategoryIndex = categoryIndex;
	}


	public void Increment(IEnumerable<int> equipments)
	{
		if (!CountsAmount)
		{
			Increment();
			return;
		}

		if (Categories == null)
		{
			foreach (var i in equipments)
				Increment();
			return;
		}

		if (!MeetsSpecialRequirements()) return;

		foreach (var i in equipments)
		{
			var eq = KCDatabase.Instance.Equipments[i];

			switch (CategoryIndex)
			{
				case -1:
					if (Categories.Contains(eq.EquipmentID))
						Increment();
					break;
				case 1:
					if (Categories.Contains(eq.MasterEquipment.CardType))
						Increment();
					break;
				case 2:
					if (Categories.Contains((int)eq.MasterEquipment.CategoryType))
						Increment();
					break;
				case 3:
					if (Categories.Contains(eq.MasterEquipment.IconType))
						Increment();
					break;
			}

		}
	}

	private bool MeetsSpecialRequirements()
	{
		// I hope the fleets are in order
		FleetData fleet = KCDatabase.Instance.Fleet.Fleets.Values.FirstOrDefault();

		if (fleet == null) return false;

		switch (QuestID)
		{
			case 621: // needs CL flag equipped with id 37 equip in first slot
				return fleet.MembersInstance[0].MasterShip.ShipType == ShipTypes.LightCruiser &&
					   fleet.MembersInstance[0].SlotInstance[0]?.EquipmentID == 37;

			case 685: // Fubuki-class FS w/ id 294 equip in 1st slot
				return (fleet.MembersInstance[0].MasterShip.BaseShip().ShipID == (int)ShipId.Fubuki ||
						fleet.MembersInstance[0].MasterShip.BaseShip().ShipID == (int)ShipId.Shirayuki ||
						fleet.MembersInstance[0].MasterShip.BaseShip().ShipID == (int)ShipId.Hatsuyuki ||
						fleet.MembersInstance[0].MasterShip.BaseShip().ShipID == (int)ShipId.Miyuki ||
						fleet.MembersInstance[0].MasterShip.BaseShip().ShipID == (int)ShipId.Murakumo ||
						fleet.MembersInstance[0].MasterShip.BaseShip().ShipID == (int)ShipId.Isonami ||
						fleet.MembersInstance[0].MasterShip.BaseShip().ShipID == (int)ShipId.Uranami) &&
					   fleet.MembersInstance[0].SlotInstance[0]?.EquipmentID == 294;
			case 695: // needs Ise k2 or Hyuuga k2 flag with id 57 in first slot
				return (fleet.MembersInstance[0].MasterShip.ShipID == (int)ShipId.IseKaiNi ||
						fleet.MembersInstance[0].MasterShip.ShipID == (int)ShipId.HyuugaKaiNi) &&
					   fleet.MembersInstance[0].SlotInstance[0]?.EquipmentID == 57;
		}

		return true;
	}

	public override string GetClearCondition()
	{
		return (Categories == null ? "" : string.Join("・", Categories.OrderBy(s => s).Select(s =>
		{
			switch (CategoryIndex)
			{
				case -1:
					return KCDatabase.Instance.MasterEquipments[s].NameEN;
				case 1:
					return $"{QuestTracking.Illust}[{s}]";
				case 2:
					return KCDatabase.Instance.EquipmentTypes[s].NameEN;
				case 3:
					return $"{QuestTracking.Icon}[{s}]";
				default:
					return $"???[{s}]";
			}
		}))) + QuestTracking.Discard + ProgressMax + (CountsAmount ? QuestTracking.NumberOfPieces : QuestTracking.NumberOfTimes);
	}



	/// <summary>
	/// 互換性維持：デフォルト値の設定
	/// </summary>
	[OnDeserializing]
	private void OnDeserializing(StreamingContext context)
	{
		CategoryIndex = 2;
	}

}
