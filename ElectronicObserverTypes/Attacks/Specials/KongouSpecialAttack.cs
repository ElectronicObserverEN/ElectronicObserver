using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserverTypes.Attacks.Specials;

public record KongouSpecialAttack : SpecialAttack
{
	public KongouSpecialAttack(IFleetData fleet) : base(fleet)
	{
	}

	public override string GetDisplay() => AttackResources.SpecialKongou;

	public override bool CanTrigger()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		if (!ships.Any()) return false;

		IShipData? flagship = ships.First();
		if (flagship is null) return false;

		if (!IsKongouClassThirdRemodel(flagship.MasterShip.ShipId)) return false;
		if (flagship.HPRate <= 0.5) return false;

		IShipData? helper = ships[1];
		if (helper is null) return false;
		if (helper.HPRate <= 0.5) return false;

		return IsValidPair(flagship.MasterShip.ShipId, helper.MasterShip.ShipId);
	}

	public override List<SpecialAttackHit> GetAttacks()
		=> new()
		{
			new()
			{
				ShipIndex = 0,
				AccuracyModifier = 1.25,
				PowerModifier = GetPowerModifier(0),
			},
			new()
			{
				ShipIndex = 1,
				AccuracyModifier = 1.25,
				PowerModifier = GetPowerModifier(1),
			},
		};

	public override bool CanTriggerOnDay => false;

	public override double GetEngagmentModifier(EngagementType engagement) => engagement switch
	{
		EngagementType.TAdvantage => 1.25,
		EngagementType.TDisadvantage => 0.8,
		_ => 1,
	};

	public override double GetTriggerRate()
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		IShipData? flagship = ships.First();
		if (flagship is null) return 0;

		IShipData? helper = ships[1];
		if (helper is null) return 0;

		// https://docs.google.com/spreadsheets/d/1xO0krWBDnevmp0zrqDs1MKek7YD-ly3B6av5R2GRpho/edit#gid=0
		double rate = 4 * Math.Sqrt(flagship.Level) + 4 * Math.Sqrt(helper.Level) + Math.Sqrt(flagship.LuckTotal) + Math.Sqrt(helper.LuckTotal) - 45;

		if (flagship.AllSlotInstance.Any(e => e?.MasterEquipment is { IsSurfaceRadar: true, LOS: >= 8 }))
		{
			rate += flagship.MasterShip.ShipId switch
			{
				ShipId.KongouKaiNiC or
				ShipId.HarunaKaiNiB or
				ShipId.HarunaKaiNiC => 31,
				ShipId.HieiKaiNiC => 10,
				_ => 0,
			};
		}

		if (flagship.AllSlotInstance.Any(e => e?.MasterEquipment?.EquipmentId is EquipmentId.SearchlightLarge_Type96150cmSearchlight))
		{
			rate += flagship.MasterShip.ShipId switch
			{
				ShipId.KongouKaiNiC or
				ShipId.HarunaKaiNiB or
				ShipId.HarunaKaiNiC => 10,
				ShipId.HieiKaiNiC => 31,
				_ => 0,
			};
		}

		return Math.Max(0, Math.Floor(rate)) / 100;
	}

	private double GetPowerModifier(int shipIndex)
	{
		List<IShipData?> ships = Fleet.MembersInstance.ToList();

		IShipData? ship = ships[shipIndex];
		if (ship is null) return 1;

		int bonusEquipments = ship.AllSlotInstance.Count(IsBonusGun);

		return bonusEquipments switch
		{
			0 => 2.4,
			1 => 2.66,
			_ => 2.76,
		};
	}

	private static bool IsBonusGun(IEquipmentData? eq) => eq?.EquipmentId is EquipmentId.MainGunLarge_35_6cmTwinGunMountKaiYon or EquipmentId.MainGunLarge_35_6cmTwinGunMountKaiSanC;

	private static bool IsKongouClassThirdRemodel(ShipId id) => id is
		ShipId.KongouKaiNiC or
		ShipId.HieiKaiNiC or
		ShipId.HarunaKaiNiB or
		ShipId.HarunaKaiNiC;

	private static bool IsValidPair(ShipId flagship, ShipId helper) => flagship switch
	{
		ShipId.KongouKaiNiC => helper is
			ShipId.HieiKaiNiC or
			ShipId.HarunaKaiNi or
			ShipId.HarunaKaiNiB or
			ShipId.HarunaKaiNiC or
			ShipId.Warspite or
			ShipId.WarspiteKai,

		ShipId.HieiKaiNiC => helper is
			ShipId.KirishimaKaiNi or
			ShipId.HarunaKaiNiB or
			ShipId.HarunaKaiNiC or
			ShipId.KongouKaiNiC,

		ShipId.HarunaKaiNiB or ShipId.HarunaKaiNiC => helper is
			ShipId.HieiKaiNiC or
			ShipId.KongouKaiNiC,

		_ => false,
	};
}
