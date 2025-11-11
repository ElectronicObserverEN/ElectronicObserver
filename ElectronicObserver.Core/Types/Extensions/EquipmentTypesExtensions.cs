namespace ElectronicObserver.Core.Types.Extensions;

public static class EquipmentTypesExtensions
{
	public static EquipmentTypeGroup ToGroup(this EquipmentTypes type) => type switch
	{
		EquipmentTypes.MainGunSmall => EquipmentTypeGroup.MainGunSmall,

		EquipmentTypes.MainGunMedium => EquipmentTypeGroup.MainGunMedium,

		EquipmentTypes.MainGunLarge or
		EquipmentTypes.MainGunLarge2 => EquipmentTypeGroup.MainGunLarge,

		EquipmentTypes.SecondaryGun => EquipmentTypeGroup.Secondary,

		EquipmentTypes.AAGun => EquipmentTypeGroup.AntiAir,

		EquipmentTypes.Torpedo or
		EquipmentTypes.MidgetSubmarine or
		EquipmentTypes.SubmarineTorpedo => EquipmentTypeGroup.Torpedo,

		EquipmentTypes.CarrierBasedFighter or
		EquipmentTypes.JetFighter => EquipmentTypeGroup.Fighter,

		EquipmentTypes.CarrierBasedBomber or
		EquipmentTypes.JetBomber => EquipmentTypeGroup.Bomber,

		EquipmentTypes.CarrierBasedTorpedo or
		EquipmentTypes.JetTorpedo => EquipmentTypeGroup.TorpedoBomber,

		EquipmentTypes.Interceptor => EquipmentTypeGroup.LandBasedFighters,

		EquipmentTypes.LandBasedAttacker or
		EquipmentTypes.HeavyBomber => EquipmentTypeGroup.LandBasedBombers,

		EquipmentTypes.CarrierBasedRecon or
		EquipmentTypes.SeaplaneRecon or
		EquipmentTypes.SeaplaneBomber or
		EquipmentTypes.Autogyro or
		EquipmentTypes.ASPatrol or
		EquipmentTypes.FlyingBoat or
		EquipmentTypes.SeaplaneFighter or
		EquipmentTypes.LandBasedRecon or
		EquipmentTypes.JetRecon or
		EquipmentTypes.CarrierBasedRecon2 => EquipmentTypeGroup.SeaplaneAndRecons,

		EquipmentTypes.RadarSmall or
		EquipmentTypes.RadarLarge or
		EquipmentTypes.RadarLarge2 => EquipmentTypeGroup.Radar,

		EquipmentTypes.Sonar or
		EquipmentTypes.DepthCharge or
		EquipmentTypes.SonarLarge => EquipmentTypeGroup.ASW,

		EquipmentTypes.ExtraArmor or
		EquipmentTypes.Engine or
		EquipmentTypes.AAShell or
		EquipmentTypes.APShell or
		EquipmentTypes.DamageControl or
		EquipmentTypes.ExtraArmorMedium or
		EquipmentTypes.ExtraArmorLarge or
		EquipmentTypes.Searchlight or
		EquipmentTypes.RepairFacility or
		EquipmentTypes.StarShell or
		EquipmentTypes.CommandFacility or
		EquipmentTypes.AviationPersonnel or
		EquipmentTypes.AADirector or
		EquipmentTypes.Rocket or
		EquipmentTypes.SurfaceShipPersonnel or
		EquipmentTypes.SearchlightLarge or
		EquipmentTypes.Ration or
		EquipmentTypes.Supplies or
		EquipmentTypes.TransportMaterial or
		EquipmentTypes.SubmarineEquipment or
		EquipmentTypes.ArmyInfantry or
		EquipmentTypes.SurfaceShipEquipment => EquipmentTypeGroup.Other,

		EquipmentTypes.LandingCraft or
		EquipmentTypes.TransportContainer or
		EquipmentTypes.SpecialAmphibiousTank => EquipmentTypeGroup.Transport,

		_ => EquipmentTypeGroup.Other,
	};

	public static EquipmentIconType ToIconType(this EquipmentTypes type) => type switch
	{
		EquipmentTypes.MainGunSmall => EquipmentIconType.MainGunSmall,
		EquipmentTypes.MainGunMedium => EquipmentIconType.MainGunMedium,
		
		EquipmentTypes.MainGunLarge or
		EquipmentTypes.MainGunLarge2 => EquipmentIconType.MainGunLarge,
		
		EquipmentTypes.SecondaryGun or
		EquipmentTypes.SecondaryGun2 => EquipmentIconType.SecondaryGun,
		
		EquipmentTypes.Torpedo or
		EquipmentTypes.MidgetSubmarine or
		EquipmentTypes.SubmarineTorpedo => EquipmentIconType.Torpedo,
		
		EquipmentTypes.CarrierBasedFighter => EquipmentIconType.CarrierBasedFighter,
		EquipmentTypes.CarrierBasedBomber => EquipmentIconType.CarrierBasedBomber,
		EquipmentTypes.CarrierBasedTorpedo => EquipmentIconType.CarrierBasedTorpedo,
		
		EquipmentTypes.CarrierBasedRecon or
		EquipmentTypes.CarrierBasedRecon2 or
		EquipmentTypes.LandBasedRecon => EquipmentIconType.CarrierBasedRecon,
		
		EquipmentTypes.SeaplaneRecon or
		EquipmentTypes.SeaplaneBomber => EquipmentIconType.Seaplane,
		
		EquipmentTypes.RadarSmall or
		EquipmentTypes.RadarLarge or
		EquipmentTypes.RadarLarge2 => EquipmentIconType.Radar,
		
		EquipmentTypes.Sonar or
		EquipmentTypes.SonarLarge => EquipmentIconType.Sonar,
		
		EquipmentTypes.DepthCharge => EquipmentIconType.DepthCharge,
		
		EquipmentTypes.ExtraArmor or
		EquipmentTypes.ExtraArmorMedium or
		EquipmentTypes.ExtraArmorLarge => EquipmentIconType.ExtraArmor,
		
		EquipmentTypes.Engine => EquipmentIconType.Engine,
		EquipmentTypes.AAShell => EquipmentIconType.AAShell,
		EquipmentTypes.APShell => EquipmentIconType.APShell,
		EquipmentTypes.DamageControl => EquipmentIconType.DamageControl,
		EquipmentTypes.AAGun => EquipmentIconType.AAGun,
		EquipmentTypes.LandingCraft => EquipmentIconType.LandingCraft,
		EquipmentTypes.Autogyro => EquipmentIconType.Autogyro,
		EquipmentTypes.ASPatrol => EquipmentIconType.ASPatrol,
		
		EquipmentTypes.Searchlight or
		EquipmentTypes.SearchlightLarge => EquipmentIconType.Searchlight,
		
		EquipmentTypes.TransportContainer => EquipmentIconType.TransportContainer,
		EquipmentTypes.RepairFacility => EquipmentIconType.RepairFacility,
		EquipmentTypes.StarShell => EquipmentIconType.StarShell,
		EquipmentTypes.CommandFacility => EquipmentIconType.CommandFacility,
		EquipmentTypes.AviationPersonnel => EquipmentIconType.AviationPersonnel,
		EquipmentTypes.AADirector => EquipmentIconType.AADirector,
		EquipmentTypes.Rocket => EquipmentIconType.Rocket,
		EquipmentTypes.SurfaceShipPersonnel => EquipmentIconType.SurfaceShipPersonnel,
		EquipmentTypes.FlyingBoat => EquipmentIconType.FlyingBoat,
		EquipmentTypes.Ration => EquipmentIconType.Ration,
		EquipmentTypes.Supplies => EquipmentIconType.Supplies,
		EquipmentTypes.SeaplaneFighter => EquipmentIconType.SeaplaneFighter,
		EquipmentTypes.SpecialAmphibiousTank => EquipmentIconType.SpecialAmphibiousTank,
		EquipmentTypes.LandBasedAttacker => EquipmentIconType.LandBasedAttacker,
		EquipmentTypes.Interceptor => EquipmentIconType.Interceptor,
		EquipmentTypes.TransportMaterial => EquipmentIconType.TransportMaterial,
		EquipmentTypes.SubmarineEquipment => EquipmentIconType.SubmarineEquipment,
		EquipmentTypes.ArmyInfantry => EquipmentIconType.ArmyInfantry,
		EquipmentTypes.HeavyBomber => EquipmentIconType.HeavyBomber,

		EquipmentTypes.SurfaceShipEquipment => EquipmentIconType.SmokeGenerator,
		
		EquipmentTypes.JetFighter => EquipmentIconType.Unknown,
		EquipmentTypes.JetBomber => EquipmentIconType.JetBomberKikka,
		EquipmentTypes.JetTorpedo => EquipmentIconType.Unknown,
		EquipmentTypes.JetRecon => EquipmentIconType.Unknown,
		
		EquipmentTypes.VTFuse => EquipmentIconType.Unknown,

		_ => EquipmentIconType.Unknown,
	};
}
