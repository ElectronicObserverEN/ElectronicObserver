namespace ElectronicObserver.Core.Types;

public enum CellType
{
	Unknown = -1,

	/// <summary>
	/// 初期位置
	/// </summary>
	Start = 0,

	/// <summary>
	/// 資源獲得
	/// </summary>
	Resource = 2,

	/// <summary>
	/// 渦潮
	/// </summary>
	Maelstrom = 3,

	/// <summary>
	/// 通常戦闘, 気のせいだった
	/// </summary>
	NormalBattleOrItWasJustMyImagination = 4,

	/// <summary>
	/// ボス戦闘
	/// </summary>
	BossBattle = 5,

	/// <summary>
	/// 揚陸地点
	/// </summary>
	LandingPoint = 6,

	/// <summary>
	/// 航空戦
	/// </summary>
	AirBattle = 7,

	/// <summary>
	/// 船団護衛成功
	/// </summary>
	SuccessfulConvoyEscort = 8,

	/// <summary>
	/// 航空偵察
	/// </summary>
	AerialReconnaissance = 9,

	/// <summary>
	/// 長距離空襲戦
	/// </summary>
	LongRangeAirBattle = 10,

	/// <summary>
	/// 開幕夜戦
	/// </summary>
	NightBattle = 11,

	/// <summary>
	/// レーダー射撃
	/// </summary>
	RadarFire = 13,

	/// <summary>
	/// 泊地
	/// </summary>
	Anchorage = 14,

	/// <summary>
	/// 対潜空襲
	/// </summary>
	SubAir = 15,
}
