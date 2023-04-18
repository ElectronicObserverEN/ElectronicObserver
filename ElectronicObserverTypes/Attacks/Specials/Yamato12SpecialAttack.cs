using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserverTypes.Attacks.Specials;

public record Yamato12SpecialAttack : SpecialAttack
{
	public Yamato12SpecialAttack(IFleetData fleet) : base(fleet)
	{
	}

	public override string GetDisplay() => AttackResources.SpecialYamato12;

	public override bool CanTrigger()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		if (!ships.Any()) return false;

		IShipData? flagship = ships.First();
		if (flagship is null) return false;

		if (flagship.MasterShip.ShipId is not ShipId.YamatoKaiNiJuu and not ShipId.YamatoKaiNi and not ShipId.MusashiKaiNi) return false;
		if (flagship.HPRate <= 0.5) return false;

		if (Fleet.NumberOfSurfaceShipNotRetreatedNotSunk() < 6) return false;

		IShipData? helper = ships[1];
		if (helper is null) return false;

		if (flagship.MasterShip.ShipId is ShipId.MusashiKaiNi && helper.MasterShip.ShipId is not ShipId.YamatoKaiNiJuu and not ShipId.YamatoKaiNi) return false;
		if (flagship.MasterShip.ShipId is ShipId.YamatoKaiNiJuu or ShipId.YamatoKaiNi && !IsYamatoHelper(helper.MasterShip.ShipId)) return false;

		if (helper.HPRate <= 0.5) return false;

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

	public override double GetTriggerRate()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		IShipData? flagship = ships.First();
		if (flagship is null) return 0;

		IShipData? helper = ships[1];
		if (helper is null) return 0;

		// https://twitter.com/chang1124414276/status/1634896670575198209?s=20
		double rate = Math.Sqrt(flagship.Level) + Math.Sqrt(helper.Level) + Math.Sqrt(flagship.LuckTotal) + Math.Sqrt(helper.LuckTotal) + 40;

		if (flagship.MasterShip.ShipId is ShipId.YamatoKaiNi or ShipId.YamatoKaiNiJuu) rate += 2;
		if (flagship.HasSurfaceRadar()) rate += 10;
		if (helper.HasSurfaceRadar()) rate += 10;

		return rate / 100;
	}

	private double GetFlagshipPowerModifier()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		IShipData? flagship = ships.First();
		if (flagship is null) return 1;

		IShipData? helper = ships[1];
		if (helper is null) return 1;

		double mod = 1.4;

		if (helper.MasterShip.ShipId is ShipId.YamatoKaiNiJuu or ShipId.YamatoKaiNi or ShipId.MusashiKaiNi) mod *= 1.1;

		mod *= GetEquipmentPowerModifier(flagship);

		return mod;
	}

	private double GetHelperPowerModifier()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		IShipData? helper = ships[1];
		if (helper is null) return 1;

		double mod = 1.55;

		if (helper.MasterShip.ShipId is ShipId.YamatoKaiNiJuu) mod *= 1.255;
		if (helper.MasterShip.ShipId is ShipId.YamatoKaiNi or ShipId.MusashiKaiNi) mod *= 1.2;

		mod *= GetEquipmentPowerModifier(helper);

		return mod;
	}

	private double GetEquipmentPowerModifier(IShipData ship)
	{
		double mod = 1;

		if (ship.HasApShell()) mod *= 1.35;
		if (ship.HasRadar()) mod *= 1.15;
		if (ship.HasYamatoRadar()) mod *= 1.1;

		return mod;
	}

	private static bool IsYamatoHelper(ShipId id) => id is
		ShipId.IowaKai or
		ShipId.BismarckDrei or
		ShipId.RichelieuKai or
		ShipId.MusashiKaiNi;
}
