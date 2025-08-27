using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Data;

/// <summary>
/// 任務のデータを保持します。
/// </summary>
public class QuestData : ResponseWrapper, IIdentifiable
{

	/// <summary>
	/// 任務ID
	/// </summary>
	public int QuestID => (int)RawData.api_no;

	/// <summary>
	/// 任務カテゴリ
	/// </summary>
	public int Category => (int)RawData.api_category;

	/// <summary>
	/// 任務出現タイプ
	/// 1=デイリー, 2=ウィークリー, 3=マンスリー, 4=単発, 5=他
	/// </summary>
	public int Type => (int)RawData.api_type;

	/// <summary>
	/// 周期アイコン種別
	/// 1=単発, 2=デイリー, 3=ウィークリー, 6=マンスリー, 7=他(輸送5と空母3,クォータリー), 100+x=イヤーリー(x月-)
	/// </summary>
	public int LabelType => (int)RawData.api_label_type;

	/// <summary>
	/// 遂行状態
	/// 1=未受領, 2=遂行中, 3=達成
	/// </summary>
	public int State
	{
		get { return (int)RawData.api_state; }
		set { RawData.api_state = value; }
	}

	public string Code => KCDatabase.Instance.Translation.Quest[QuestID]?.Code ?? "";

	/// <summary>
	/// Name (Translated)
	/// </summary>
	public string Name => KCDatabase.Instance.Translation.Quest.Name(QuestID, (string)RawData.api_title);

	/// <summary>
	/// {Code}: {Name} if code exists <br />
	/// Name if code doesn't exist
	/// </summary>
	public string NameWithCode => this switch
	{
		{ Code: "" } => Name,
		{ } => $"{Code}: {Name}",
		_ => "???"
	};

	/// <summary>
	/// 任務名
	/// </summary>
	public string NameJP => (string)RawData.api_title;

	/// <summary>
	/// Description (Translated)
	/// </summary>
	public string Description => KCDatabase.Instance.Translation.Quest.Description(QuestID, (string)RawData.api_detail);

	/// <summary>
	/// 説明
	/// </summary>
	public string DescriptionJP => (string)RawData.api_detail.Replace("<br>", "\r\n");

	//undone:api_bonus_flag

	/// <summary>
	/// True if quest is translated
	/// </summary>
	public bool Translated => (string)RawData.api_detail.Replace("<br>", "\r\n") != Description && (string)RawData.api_title != Name;

	/// <summary>
	/// 進捗
	/// </summary>
	public int Progress => (int)RawData.api_progress_flag;



	public int ID => QuestID;
	public override string ToString() => $"[{QuestID}] {Name}";
}
