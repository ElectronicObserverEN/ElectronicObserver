using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Core.Types.Attacks.Specials;

public record Yamato123SpecialAttack : SpecialAttack
{
	public Yamato123SpecialAttack(IFleetData fleet) : base(fleet)
	{
	}

	public override string GetDisplay() => AttackResources.SpecialYamato123;

	public override bool CanTrigger()
	{
		List<IShipData> ships = Fleet.MembersInstance.ToList();

		if (!ships.Any()) return false;

		IShipData? flagship = ships.First();
		if (flagship is null) return false;

		if (flagship.MasterShip.ShipId is not ShipId.YamatoKaiNiJuu and not ShipId.YamatoKaiNi) return false;
		if (flagship.HPRate <= 0.5) return false;

		if (Fleet.NumberOfSurfaceShipNotRetreatedNotSunk() < 6) return false;

		return
			MeetsHelperRequirement(ships[1]) &&
			MeetsHelperRequirement(ships[2]) &&
			IsYamatoHelperPair(ships[1]!.MasterShip.ShipId, ships[2]!.MasterShip.ShipId);
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
				ShipIndex = 1,
				AccuracyModifier = 1,
				PowerModifier = GetFirstHelperPowerModifier(),
			},
			new()
			{
				ShipIndex = 2,
				AccuracyModifier = 1,
				PowerModifier = GetSecondHelperPowerModifier(),
			},
		};

	private double GetFlagshipPowerModifier()
	{
		List<IShipData> ships = Fleet.MembersInstance.ToList();

		IShipData? flagship = ships.First();
		if (flagship is null) return 1;

		IShipData? firstHelper = ships[1];
		if (firstHelper is null) return 1;
		ShipId firstHelperId = firstHelper.MasterShip.ShipId;

		IShipData? secondHelper = ships[2];
		if (secondHelper is null) return 1;
		ShipId secondHelperId = secondHelper.MasterShip.ShipId;

		double mod = 1.5;

		if (IsMusashiAndNagatoClassPair(firstHelperId, secondHelperId)) mod *= 1.1;
		if (IsNagatoClassPair(firstHelperId, secondHelperId)) mod *= 1.1;
		if (IsIseClassPair(firstHelperId, secondHelperId)) mod *= 1.1;

		mod *= GetEquipmentPowerModifier(flagship, true);

		return mod;
	}

	private double GetFirstHelperPowerModifier()
	{
		List<IShipData> ships = Fleet.MembersInstance.ToList();

		IShipData? firstHelper = ships[1];
		if (firstHelper is null) return 1;
		ShipId firstHelperId = firstHelper.MasterShip.ShipId;

		IShipData? secondHelper = ships[2];
		if (secondHelper is null) return 1;
		ShipId secondHelperId = secondHelper.MasterShip.ShipId;

		double mod = 1.5;

		if (IsMusashiAndNagatoClassPair(firstHelperId, secondHelperId)) mod *= 1.2;
		if (IsNagatoClassPair(firstHelperId, secondHelperId)) mod *= 1.1;
		if (IsIseClassPair(firstHelperId, secondHelperId)) mod *= 1.05;

		mod *= GetEquipmentPowerModifier(firstHelper, true);

		return mod;
	}

	private double GetSecondHelperPowerModifier()
	{
		List<IShipData> ships = Fleet.MembersInstance.ToList();

		IShipData? secondHelper = ships[2];
		if (secondHelper is null) return 1;

		double mod = 1.65;

		mod *= GetEquipmentPowerModifier(secondHelper, false);

		return mod;
	}

	private double GetEquipmentPowerModifier(IShipData ship, bool applyYamatoRadarBonus)
	{
		double mod = 1;

		if (ship.HasApShell()) mod *= 1.35;
		if (ship.HasRadar()) mod *= 1.15;
		if (applyYamatoRadarBonus && ship.HasYamatoRadar()) mod *= 1.1;

		return mod;
	}

	private static bool IsMusashiAndNagatoClassPair(ShipId firstHelper, ShipId secondHelper) => firstHelper switch
	{
		ShipId.MusashiKaiNi => secondHelper is ShipId.MutsuKaiNi or ShipId.NagatoKaiNi,
		_ => false,
	};

	private static bool IsNagatoClassPair(ShipId firstHelper, ShipId secondHelper) => firstHelper switch
	{
		ShipId.NagatoKaiNi => secondHelper is ShipId.MutsuKaiNi,
		ShipId.MutsuKaiNi => secondHelper is ShipId.NagatoKaiNi,
		_ => false,
	};

	private static bool IsIseClassPair(ShipId firstHelper, ShipId secondHelper) => firstHelper switch
	{
		ShipId.IseKaiNi => secondHelper is ShipId.HyuugaKaiNi,
		ShipId.HyuugaKaiNi => secondHelper is ShipId.IseKaiNi,
		_ => false,
	};

	private static bool IsYamatoHelperPair(ShipId firstHelper, ShipId secondHelper) => firstHelper switch
	{
		ShipId.KongouKaiNiC => secondHelper is ShipId.HieiKaiNiC or ShipId.HarunaKaiNiB or ShipId.HarunaKaiNiC or ShipId.KirishimaKaiNiC,

		ShipId.HieiKaiNiC => secondHelper is ShipId.KongouKaiNiC or ShipId.KirishimaKaiNiC,

		ShipId.KirishimaKaiNiC => secondHelper is ShipId.HieiKaiNiC or ShipId.KongouKaiNiC,

		ShipId.HarunaKaiNiC or
		ShipId.HarunaKaiNiB => secondHelper is ShipId.KongouKaiNiC,

		ShipId.RomaKai => secondHelper is ShipId.Italia,
		ShipId.Italia => secondHelper is ShipId.RomaKai,

		ShipId.FusouKaiNi => secondHelper is ShipId.YamashiroKaiNi,
		ShipId.YamashiroKaiNi => secondHelper is ShipId.FusouKaiNi,

		ShipId.SouthDakotaKai => secondHelper is ShipId.WashingtonKai,
		ShipId.WashingtonKai => secondHelper is ShipId.SouthDakotaKai,

		ShipId.WarspiteKai => secondHelper is ShipId.ValiantKai or ShipId.NelsonKai,
		ShipId.NelsonKai => secondHelper is ShipId.WarspiteKai or ShipId.RodneyKai,
		ShipId.RodneyKai => secondHelper is ShipId.NelsonKai,
		ShipId.ValiantKai => secondHelper is ShipId.WarspiteKai,

		ShipId.ColoradoKai => secondHelper is ShipId.MarylandKai,
		ShipId.MarylandKai => secondHelper is ShipId.ColoradoKai,

		ShipId.RichelieuKai or ShipId.RichelieuDeux => secondHelper is ShipId.JeanBartKai,
		ShipId.JeanBartKai => secondHelper is ShipId.RichelieuKai or ShipId.RichelieuDeux,

		_ => IsMusashiAndNagatoClassPair(firstHelper, secondHelper) || IsNagatoClassPair(firstHelper, secondHelper) || IsIseClassPair(firstHelper, secondHelper),
	};

	private bool MeetsHelperRequirement(IShipData? helper)
	{
		if (helper is null) return false;
		if (helper.HPRate <= 0.5) return false;

		return true;
	}
}
