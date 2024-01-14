using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ElectronicObserverTypes;

public interface IFleetData
{
	/// <summary>
	/// 艦隊ID
	/// </summary>
	int FleetID { get; }

	/// <summary>
	/// 艦隊名
	/// </summary>
	string Name { get; }

	FleetType FleetType { get; }

	/// <summary>
	/// 遠征状態
	/// 0=未出撃, 1=遠征中, 2=遠征帰投, 3=強制帰投中
	/// </summary>
	int ExpeditionState { get; }

	/// <summary>
	/// 遠征先ID
	/// </summary>
	int ExpeditionDestination { get; }

	/// <summary>
	/// 遠征帰投時間
	/// </summary>
	DateTime ExpeditionTime { get; }

	/// <summary>
	/// 艦隊メンバー(艦船ID)
	/// </summary>
	ReadOnlyCollection<int>? Members { get; }

	/// <summary>
	/// 艦隊メンバー(艦船データ)
	/// </summary>
	ReadOnlyCollection<IShipData?> MembersInstance { get; }

	/// <summary>
	/// 艦隊メンバー(艦船データ、退避艦を除く)
	/// </summary>
	ReadOnlyCollection<IShipData?>? MembersWithoutEscaped { get; }

	/// <summary>
	/// 退避艦のIDリスト
	/// </summary>
	ReadOnlyCollection<int> EscapedShipList { get; }

	/// <summary>
	/// 出撃中かどうか
	/// </summary>
	bool IsInSortie { get; }

	bool IsInPractice { get; }
	int ID { get; }

	/// <summary>
	/// 支援艦隊種別
	/// 0=不発, 1=空撃, 2=砲撃, 3=雷撃
	/// </summary>
	SupportType SupportType { get; }

	/// <summary>
	/// 旗艦が工作艦か
	/// </summary>
	bool IsFlagshipRepairShip { get; }

	/// <summary>
	/// 泊地修理が発動可能か
	/// </summary>
	bool CanAnchorageRepair { get; }

	/// <summary>
	/// 疲労が回復すると予測される日時 (疲労していない場合は null)
	/// </summary>
	DateTime? ConditionTime { get; }

	Dictionary<string, string> RequestData { get; }

	/// <summary>
	/// 生の受信データ(api_data)
	/// </summary>
	dynamic RawData { get; }

	/// <summary>
	/// 現在のデータが有効かを取得します。
	/// </summary>
	bool IsAvailable { get; }

	int this[int i] { get; }
	void LoadFromResponse(string apiname, dynamic data);
	void LoadFromRequest(string apiname, Dictionary<string, string> data);

	/// <summary>
	/// 護衛退避を実行します。
	/// </summary>
	/// <param name="index">対象艦の艦隊内でのインデックス。[0-6]</param>
	void Escape(int index);

	/// <summary>
	/// 制空戦力を取得します。
	/// </summary>
	/// <returns>制空戦力。</returns>
	int GetAirSuperiority();

	/// <summary>
	/// 現在の設定に応じて、制空戦力を表す文字列を取得します。
	/// </summary>
	/// <returns></returns>
	string GetAirSuperiorityString();

	/// <summary>
	/// 現在の設定に応じて、索敵能力を取得します。
	/// </summary>
	double GetSearchingAbility();

	/// <summary>
	/// 新判定式(33)索敵能力を表す文字列を取得します。
	/// </summary>
	/// <param name="branchWeight">分岐点係数。1 / 4 / 3</param>
	string GetSearchingAbilityString(int branchWeight = 1);

	/// <summary>
	/// 触接開始率を取得します。
	/// </summary>
	/// <returns></returns>
	double GetContactProbability();

	Dictionary<int, double> GetContactSelectionProbability();
	void UpdateConditionTime();
	string ToString();
}
