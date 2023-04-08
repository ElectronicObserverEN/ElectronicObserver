using System.Collections.Generic;
using System.Linq;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserverTypes.Attacks.Specials;

public record NagatoSpecialAttack : SpecialAttack
{
	public NagatoSpecialAttack(IFleetData fleet) : base(fleet)
	{
	}

	public override string GetDisplay() => Fleet.MembersInstance.ToList().FirstOrDefault() switch
	{
		{ MasterShip.ShipId: ShipId.NagatoKaiNi } => AttackResources.SpecialNagato,
		{ MasterShip.ShipId: ShipId.MutsuKaiNi } => AttackResources.SpecialMutsu,
		_ => "???"
	};

	protected override double GetTriggerRate()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		if (!ships.Any()) return -1;

		IShipData? flagship = ships.First();
		if (flagship is null) return -1;
		if (flagship.MasterShip.ShipId is not ShipId.NagatoKaiNi and not ShipId.MutsuKaiNi) return -1;

		if (flagship.HPRate <= 0.5) return -1;

		int availableShipCount = ships.Count(ship => ship?.HPCurrent > 0) - Fleet.EscapedShipList.Count;
		if (availableShipCount < 6) return -1;

		IShipData? helper = ships[1];
		if (helper is null) return -1;
		if (helper.MasterShip.ShipType is not ShipTypes.Battleship and not ShipTypes.Battlecruiser and not ShipTypes.AviationBattleship) return -1;
		if (helper.HPRate <= 0.25) return -1;

		// todo : find formula
		return 0;
	}

	protected override IEnumerable<SpecialAttackHit> GetAttacks()
		=> new List<SpecialAttackHit>()
		{
			new SpecialAttackHit()
			{
				ShipIndex = 0,
				AccuracyModifier = 1,
				PowerModifier = GetFlagshipPowerModifier(),
			},
			new SpecialAttackHit()
			{
				ShipIndex = 0,
				AccuracyModifier = 1,
				PowerModifier = GetFlagshipPowerModifier(),
			},
			new SpecialAttackHit()
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
