using System.Collections.Generic;

namespace ElectronicObserverTypes;

public interface IBaseAirCorpsData
{
	/// <summary>
	/// 飛行場が存在する海域ID
	/// </summary>
	int MapAreaID { get; }

	/// <summary>
	/// 航空隊ID
	/// </summary>
	int AirCorpsID { get; }

	/// <summary>
	/// 航空隊名
	/// </summary>
	string Name { get; }

	/// <summary>
	/// 戦闘行動半径 base distance
	/// </summary>
	int Distance { get; }

	///<summary>
	///LBAS bonus distance
	///</summary>
	int Bonus_Distance { get; }

	///<summary>
	///LBAS base distance
	///</summary>
	int Base_Distance { get; }

	/// <summary>
	/// 行動指示
	/// 0=待機, 1=出撃, 2=防空, 3=退避, 4=休息
	/// </summary>
	AirBaseActionKind ActionKind { get; }

	/// <summary>
	/// List of points (edge ?) the LBAS will strike
	/// </summary>
	List<int> StrikePoints { get; }

	/// <summary>
	/// 航空中隊情報
	/// </summary>
	public IDictionary<int, IBaseAirCorpsSquadron> Squadrons { get; }

	int ID { get; }

	/// <summary>
	/// 現在のデータが有効かを取得します。
	/// </summary>
	bool IsAvailable { get; }

	int HPCurrent { get; }

	int HPMax { get; }
}
