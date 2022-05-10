using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;

namespace ElectronicObserver.Data;

/// <summary>
/// Custom quest made for event gimmicks
/// </summary>
public class GimmickIQuestData : IQuestData
{
	/// <summary>
	/// ID of the area for this custom quest
	/// </summary>
	public int MapArea { get; set; }

	/// <summary>
	/// Number of the map for this custom quest
	/// </summary>
	public int MapNumber { get; set; }

	/// <summary>
	/// Gimmick type
	/// </summary>
	public GimmickType GimmickType { get; set; }

	/// <summary>
	/// If multiple gimmick of same type on a map, this is the gimmick number of it
	/// </summary>
	public int GimmickNumber { get; set; }

	/// <summary>
	/// Quest ID (need to be set manually)
	/// </summary>
	public int QuestID { get; set; }

	/// <summary>
	/// 任務カテゴリ
	/// </summary>
	public int Category => (int)QuestCategory.Sortie;

	/// <summary>
	/// 任務出現タイプ
	/// 1=デイリー, 2=ウィークリー, 3=マンスリー, 4=単発, 5=他
	/// </summary>
	public int Type => (int)QuestResetType.Never;

	/// <summary>
	/// Not used for custom quests (always Sortie type)
	/// </summary>
	public int LabelType => (int)QuestResetType.Unknown;

	/// <summary>
	/// 遂行状態
	/// 1=未受領, 2=遂行中, 3=達成
	/// </summary>
	public int State { get; set; }

	/// <summary>
	/// Quest code
	/// </summary>
	public string Code => $"G{MapArea}_{MapNumber}-{GimmickType.ToString()[0]}{GimmickNumber}";

	/// <summary>
	/// Name (TODO : translate)
	/// </summary>
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
	public string NameJP => Name;

	/// <summary>
	/// Description (TODO : translate)
	/// </summary>
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
	public string DescriptionJP => Description;

	//undone:api_bonus_flag

	/// <summary>
	/// True if quest is translated
	/// </summary>
	public bool Translated => true;

	/// <summary>
	/// Quest progress (0 = use EO progress ?)
	/// </summary>
	public int Progress => 0;



	public int ID => QuestID;
	public override string ToString() => $"[{QuestID}] {Name}";
}
