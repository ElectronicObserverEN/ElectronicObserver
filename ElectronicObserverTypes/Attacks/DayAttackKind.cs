namespace ElectronicObserverTypes.Attacks;

/// <summary>
/// 昼戦攻撃種別を表します。
/// </summary>
public enum DayAttackKind
{
	/// <summary> 不明 </summary>
	Unknown = -1,


	/// <summary> 通常攻撃 (API上でのみ使用されます) </summary>
	NormalAttack,

	/// <summary> レーザー攻撃 </summary>
	Laser,

	/// <summary> 連続射撃 </summary>
	DoubleShelling,

	/// <summary> カットイン(主砲/副砲) </summary>
	CutinMainSub,

	/// <summary> カットイン(主砲/電探) </summary>
	CutinMainRadar,

	/// <summary> カットイン(主砲/徹甲弾) </summary>
	CutinMainAP,

	/// <summary> カットイン(主砲/主砲) </summary>
	CutinMainMain,

	/// <summary> 空母カットイン </summary>
	CutinAirAttack,

	/// <summary> Nelson Touch </summary>
	SpecialNelson = 100,

	/// <summary> 一斉射かッ…胸が熱いな！ </summary>
	SpecialNagato = 101,

	/// <summary> 長門、いい？ いくわよ！ 主砲一斉射ッ！ </summary>
	SpecialMutsu = 102,

	/// <summary> Colorado Touch </summary>
	SpecialColorado = 103,

	/// <summary> 僚艦夜戦突撃 </summary>
	SpecialKongo = 104,

	/// <summary> Richelieuよ！圧倒しなさいっ！ </summary>
	SpecialRichelieu = 105,

	/// <summary> 瑞雲立体攻撃 </summary>
	ZuiunMultiAngle = 200,

	/// <summary> 海空立体攻撃 </summary>
	SeaAirMultiAngle = 201,


	/// <summary> 潜水艦隊攻撃 (参加潜水艦ポジション2・3) </summary>
	SpecialSubmarineTender23 = 300,

	/// <summary> 潜水艦隊攻撃 (参加潜水艦ポジション3・4) </summary>
	SpecialSubmarineTender34 = 301,

	/// <summary> 潜水艦隊攻撃 (参加潜水艦ポジション2・4) </summary>
	SpecialSubmarineTender24 = 302,


	/// <summary> 大和、突撃します！二番艦も続いてください！ </summary>
	SpecialYamato3Ships = 400,

	/// <summary> 第一戦隊、突撃！主砲、全力斉射ッ！ </summary>
	SpecialYamato2Ships = 401,


	/// <summary> 砲撃 </summary>
	Shelling = 1000,

	/// <summary> 空撃 </summary>
	AirAttack,

	/// <summary> 爆雷攻撃 </summary>
	DepthCharge,

	/// <summary> 雷撃 </summary>
	Torpedo,


	/// <summary> ロケット攻撃 </summary>
	Rocket = 2000,


	/// <summary> 揚陸攻撃(大発動艇) </summary>
	LandingDaihatsu = 3000,

	/// <summary> 揚陸攻撃(特大発動艇) </summary>
	LandingTokuDaihatsu,

	/// <summary> 揚陸攻撃(大発動艇(八九式中戦車&陸戦隊)) </summary>
	LandingDaihatsuTank,

	/// <summary> 揚陸攻撃(特二式内火艇) </summary>
	LandingAmphibious,

	/// <summary> 揚陸攻撃(特大発動艇+戦車第11連隊) </summary>
	LandingTokuDaihatsuTank,

}
