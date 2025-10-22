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
}
