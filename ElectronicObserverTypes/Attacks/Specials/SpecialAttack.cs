using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserverTypes.Attacks.Specials;

public abstract record SpecialAttack
{
	protected IFleetData Fleet { get; set; }

	public virtual bool CanTriggerOnDay => true;
	public virtual bool CanTriggerOnNight => true;

	protected SpecialAttack(IFleetData fleet)
	{
		Fleet = fleet;
	}

	public virtual double GetTriggerRate() => 0;

	public abstract bool CanTrigger();

	public abstract IEnumerable<SpecialAttackHit> GetAttacks();

	public List<SpecialAttackHit> GetHitsPerShip(int shipIndex)
	{
		return GetAttacks().Where(hit => hit.ShipIndex == shipIndex).ToList();
	}

	public virtual double GetEngagmentModifier(EngagementType engagement) => 1;

	public virtual string GetDisplay() => "???";

	// TODO : Have something to display activation conditions

	/* Note : fleet formation are not being taken into account */

	/*
	DayAttackKind

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
*/
}
