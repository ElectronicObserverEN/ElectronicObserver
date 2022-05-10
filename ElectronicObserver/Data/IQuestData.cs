using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ElectronicObserver.Data.Translation;

namespace ElectronicObserver.Data;

/// <summary>
/// 任務のデータを保持します。
/// </summary>
public interface IQuestData : IIdentifiable
{
	public int QuestID { get; }

	/// <summary>
	/// 任務カテゴリ
	/// </summary>
	public int Category { get; }

	/// <summary>
	/// 任務出現タイプ
	/// 1=デイリー, 2=ウィークリー, 3=マンスリー, 4=単発, 5=他
	/// </summary>
	public int Type { get; }

	/// <summary>
	/// 周期アイコン種別
	/// 1=単発, 2=デイリー, 3=ウィークリー, 6=マンスリー, 7=他(輸送5と空母3,クォータリー), 100+x=イヤーリー(x月-)
	/// </summary>
	public int LabelType { get; }

	/// <summary>
	/// 遂行状態
	/// 1=未受領, 2=遂行中, 3=達成
	/// </summary>
	public int State { get; set; }

	public string Code { get; }

	/// <summary>
	/// Name (Translated)
	/// </summary>
	public string Name { get; }

	/// <summary>
	/// 任務名
	/// </summary>
	public string NameJP { get; }

	/// <summary>
	/// Description (Translated)
	/// </summary>
	public string Description { get; }

	/// <summary>
	/// 説明
	/// </summary>
	public string DescriptionJP { get; }

	//undone:api_bonus_flag

	/// <summary>
	/// True if quest is translated
	/// </summary>
	public bool Translated { get; }

	/// <summary>
	/// 進捗
	/// </summary>
	public int Progress { get; }

	/// <summary>
	/// The quest ID
	/// </summary>
	public int ID => QuestID;

	/// <summary>
	/// Implement override of ToString (needed ?)
	/// </summary>
	/// <returns></returns>
	public string ToString();
}
