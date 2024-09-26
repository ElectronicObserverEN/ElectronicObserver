using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserverTypes.Attacks.Specials;

public record NagatoSpecialAttack : SpecialAttack
{
	public NagatoSpecialAttack(IFleetData fleet) : base(fleet)
	{
	}

	public override string GetDisplay() => Fleet.MembersInstance.AsEnumerable().FirstOrDefault() switch
	{
		{ MasterShip.ShipId: ShipId.NagatoKaiNi } => AttackResources.SpecialNagato,
		{ MasterShip.ShipId: ShipId.MutsuKaiNi } => AttackResources.SpecialMutsu,
		_ => "???"
	};

	/// <summary>
	/// https://docs.google.com/spreadsheets/d/1kd3t0wjLdpJZJSea7heivd-WByXHFEe1xWoUE3Kmkrg
	/// </summary>
	/// <returns></returns>
	public override double GetTriggerRate()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		IShipData? flagship = ships.First();
		if (flagship is null) return 0;

		IShipData? helper = ships[1];
		if (helper is null) return 0;

		double rate =  Math.Sqrt(flagship.Level) + 1.5 * Math.Sqrt(flagship.LuckTotal) + Math.Sqrt(helper.Level) + 1.5 * Math.Sqrt(helper.LuckTotal);

		return (Math.Floor(rate) + 25) / 100;
	}

	public override bool CanTrigger()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		if (!ships.Any()) return false;

		IShipData? flagship = ships.First();
		if (flagship is null) return false;
		if (flagship.MasterShip.ShipId is not ShipId.NagatoKaiNi and not ShipId.MutsuKaiNi) return false;

		if (flagship.HPRate <= 0.5) return false;

		if (Fleet.NumberOfSurfaceShipNotRetreatedNotSunk() < 6) return false;

		IShipData? helper = ships[1];
		if (helper is null) return false;
		if (helper.MasterShip.ShipType is not ShipTypes.Battleship and not ShipTypes.Battlecruiser and not ShipTypes.AviationBattleship) return false;
		if (helper.HPRate <= 0.25) return false;

		return true;
	}

	public override List<SpecialAttackHit> GetAttacks()
		=> new()
		{
			new()
			{
				ShipIndex = 0,
				AccuracyModifier = 1,
				PowerModifier = GetFlagshipPowerModifier(),
			},
			new()
			{
				ShipIndex = 0,
				AccuracyModifier = 1,
				PowerModifier = GetFlagshipPowerModifier(),
			},
			new()
			{
				ShipIndex = 1,
				AccuracyModifier = 1,
				PowerModifier = GetHelperPowerModifier(),
			},
		};

	private double GetFlagshipPowerModifier()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		IShipData? flagship = ships.First();
		if (flagship is null) return 1;

		IShipData? helper = ships[1];
		if (helper is null) return 1;

		double mod = 1.4;

		if (helper.MasterShip.ShipId is ShipId.NagatoKaiNi or ShipId.MutsuKaiNi) mod *= 1.2;
		if (flagship.MasterShip.ShipId is ShipId.NagatoKaiNi && helper.MasterShip.ShipId is ShipId.MutsuKai) mod *= 1.15;
		if (flagship.MasterShip.ShipId is ShipId.NagatoKaiNi && helper.MasterShip.ShipId is ShipId.NelsonKai) mod *= 1.1;

		mod *= GetEquipmentPowerModifier(flagship);

		return mod;
	}

	private double GetHelperPowerModifier()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		IShipData? flagship = ships.First();
		if (flagship is null) return 1;

		IShipData? helper = ships[1];
		if (helper is null) return 1;

		double mod = 1.2;

		if (helper.MasterShip.ShipId is ShipId.NagatoKaiNi or ShipId.MutsuKaiNi) mod *= 1.4;
		if (flagship.MasterShip.ShipId is ShipId.NagatoKaiNi && helper.MasterShip.ShipId is ShipId.MutsuKai) mod *= 1.35;
		if (flagship.MasterShip.ShipId is ShipId.NagatoKaiNi && helper.MasterShip.ShipId is ShipId.NelsonKai) mod *= 1.25;

		mod *= GetEquipmentPowerModifier(helper);

		return mod;
	}

	private double GetEquipmentPowerModifier(IShipData ship)
	{
		double mod = 1;

		if (ship.HasApShell()) mod *= 1.35;
		if (ship.HasRadar()) mod *= 1.15;

		return mod;
	}
}
