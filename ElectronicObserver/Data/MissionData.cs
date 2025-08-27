using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Data;

public enum ResetType
{
	Normal,
	Monthly
}

/// <summary>
/// 遠征データを保持します。
/// </summary>
public class MissionData : APIWrapper, IIdentifiable
{

	/// <summary>
	/// 遠征ID
	/// </summary>
	public int MissionID => (int)RawData.api_id;

	/// <summary>
	/// 表示される遠征ID
	/// </summary>
	public string DisplayID => RawData.api_disp_no;

	/// <summary>
	/// 海域カテゴリID
	/// </summary>
	public int MapAreaID => (int)RawData.api_maparea_id;

	/// <summary>
	/// Expedition IDs sorted by world ingame
	/// </summary>
	public int SortID => (int)RawData.api_maparea_id * 1000 + (int)RawData.api_id;

	/// <summary>
	/// 遠征名
	/// </summary>
	public string Name => RawData.api_name;

	public string NameEN => KCDatabase.Instance.Translation.Mission.Name(RawData.api_name);

	/// <summary>
	/// 説明文
	/// </summary>
	public string Detail => RawData.api_details;

	/// <summary>
	/// 進捗リセットタイミング
	/// </summary>
	public ResetType ResetType => (ResetType)RawData.api_reset_type;
	/// <summary>
	/// Expedition Damage Type: 0="Normal" 1="Type 1" 2="Type 2"
	/// </summary>
	private int ExpeditionDamageType => (int)RawData.api_damage_type;

	public ExpeditionType ExpeditionType => (ExpeditionType)ExpeditionDamageType;

	/// <summary>
	/// 遠征時間(分単位)
	/// </summary>
	public int Time => (int)RawData.api_time;

	/// <summary>
	/// 難易度
	/// </summary>
	public int Difficulty => (int)RawData.api_difficulty;

	/// <summary>
	/// 消費燃料割合
	/// </summary>
	public double Fuel => RawData.api_use_fuel;

	/// <summary>
	/// 消費弾薬割合
	/// </summary>
	public double Ammo => RawData.api_use_bull;

	//win_item<n>

	/// <summary>
	/// 遠征中断・強制帰投可能かどうか
	/// </summary>
	public bool Cancelable => (int)RawData.api_return_flag != 0;



	public int ID => MissionID;
	public override string ToString() => $"[{MissionID}] {Name} (Type: {ExpeditionType.Display()}";
}
