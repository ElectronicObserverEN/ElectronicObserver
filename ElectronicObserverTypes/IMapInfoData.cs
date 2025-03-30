using System.Collections.Generic;
using ElectronicObserverTypes.Data;

namespace ElectronicObserverTypes;

public interface IMapInfoData : IIdentifiable
{
	/// <summary>
	/// 海域ID
	/// </summary>
	public int MapID { get; }

	/// <summary>
	/// 海域カテゴリID
	/// </summary>
	public int MapAreaID { get; }

	/// <summary>
	/// 海域カテゴリ内番号
	/// </summary>
	public int MapInfoID { get; }

	/// <summary>
	/// 海域名
	/// </summary>
	public string Name { get; }

	public string NameEN { get; }

	/// <summary>
	/// 難易度
	/// </summary>
	public int Difficulty { get; }

	/// <summary>
	/// 作戦名
	/// </summary>
	public string OperationName { get; }

	/// <summary>
	/// 作戦情報
	/// </summary>
	public string Information { get; }

	/// <summary>
	/// クリアに必要な撃破回数(主にEO海域)
	/// 存在しなければ -1
	/// </summary>
	public int RequiredDefeatedCount { get; }

	/// <summary>
	/// 攻略済みかどうか
	/// </summary>
	public bool IsCleared { get; }

	/// <summary>
	/// 現在の撃破回数
	/// </summary>
	public int CurrentDefeatedCount { get; }

	/// <summary>
	/// 現在の海域HP
	/// </summary>
	public int MapHPCurrent { get; }

	/// <summary>
	/// 海域HPの最大値
	/// </summary>
	public int MapHPMax { get; }

	/// <summary>
	/// 現在選択されている難易度(甲乙丙丁)
	/// </summary>
	public int EventDifficulty { get; }

	/// <summary>
	/// 海域ゲージの種別
	/// 2=HP制(デフォルト), 3=TP制
	/// </summary>
	public int GaugeType { get; }

	/// <summary>
	/// 現在のゲージ本数　未指定なら 0
	/// </summary>
	public int CurrentGaugeIndex { get; }

	public void LoadFromResponse(string apiname, dynamic data);

	public void LoadFromRequest(string apiname, Dictionary<string, string> data);

	public int ID => MapID;
}
