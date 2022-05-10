using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models;
using MessagePack;

namespace ElectronicObserver.Data;

/// <summary>
/// Custom quest made for event gimmicks
/// </summary>
[MessagePackObject]
public class GimmickQuestData : IQuestData
{
	/// <summary>
	/// ID of the area for this custom quest
	/// </summary>
	[Key("world")]
	public int MapArea { get; set; }

	/// <summary>
	/// Number of the map for this custom quest
	/// </summary>
	[Key("map")]
	public int MapNumber { get; set; }

	/// <summary>
	/// Gauge number for this requirement to be active
	/// </summary>
	[Key("gaugeNum")]
	public int GaugeNumber { get; set; }

	/// <summary>
	/// Gimmick type
	/// </summary>
	[Key("type")]
	public GimmickType GimmickType { get; set; }

	/// <summary>
	/// If multiple gimmick of same type on a map, this is the gimmick number of it
	/// </summary>
	[IgnoreMember]
	public int GimmickNumber { get; set; }

	/// <summary>
	/// If multiple gimmick of same type on a map, this is the gimmick number of it
	/// </summary>
	[IgnoreMember]
	public string DisplayGimmickNumber => GimmickNumber > 0 ? GimmickNumber.ToString() : string.Empty;

	/// <summary>
	/// Quest tracker
	/// </summary>
	[Key("tracker")]
	public TrackerModel? Tracker { get; set; }

	/// <summary>
	/// Quest ID (need to be set manually)
	/// </summary>
	[IgnoreMember]
	public int QuestID { get; set; }

	/// <summary>
	/// 任務カテゴリ
	/// </summary>
	[IgnoreMember]
	public int Category => (int)QuestCategory.Sortie;

	/// <summary>
	/// 任務出現タイプ
	/// 1=デイリー, 2=ウィークリー, 3=マンスリー, 4=単発, 5=他
	/// </summary>
	[IgnoreMember]
	public int Type => (int)QuestResetType.Never;

	/// <summary>
	/// Not used for custom quests (always Sortie type)
	/// </summary>
	[IgnoreMember]
	public int LabelType => (int)QuestResetType.Unknown;

	/// <summary>
	/// 遂行状態
	/// 1=未受領, 2=遂行中, 3=達成
	/// </summary>
	[IgnoreMember]
	public int State { get; set; }

	/// <summary>
	/// Quest code
	/// </summary>
	[IgnoreMember]
	public string Code => $"{MapArea}{MapNumber}{GimmickType.ToString()[0]}{DisplayGimmickNumber}";

	/// <summary>
	/// Name (TODO : translate)
	/// </summary>
	[IgnoreMember]
	public string Name => GimmickType switch
	{
		GimmickType.Other => "Gimmick",
		GimmickType.Unlock => "Unlock",
		GimmickType.Debuff => "Armor break",
		_ => ""
	} + $" - {(MapArea > 20 ? "E" : $"{MapArea}-")}{MapNumber}";

	/// <summary>
	/// Name in japanese (TODO : UNtranslate)
	/// </summary>
	[IgnoreMember]
	public string NameJP => Name;

	/// <summary>
	/// Description (TODO : translate)
	/// </summary>
	[IgnoreMember]
	public string Description => GimmickType switch
	{
		GimmickType.Other => "",
		GimmickType.Unlock => "Unlock a new path after completing the quest requirements",
		GimmickType.Debuff => "Break the enemy armor after completing the quest requirements",
		_ => ""
	};

	/// <summary>
	/// (TODO : UNtranslate)
	/// </summary>
	[IgnoreMember]
	public string DescriptionJP => Description;

	//undone:api_bonus_flag

	/// <summary>
	/// True if quest is translated
	/// </summary>
	[IgnoreMember]
	public bool Translated => true;

	/// <summary>
	/// Quest progress (0 = use EO progress ?)
	/// </summary>
	[IgnoreMember]
	public int Progress => 0;

	[IgnoreMember]
	public int ID => QuestID;

	public override string ToString() => $"[{QuestID}] {Name}";
}
