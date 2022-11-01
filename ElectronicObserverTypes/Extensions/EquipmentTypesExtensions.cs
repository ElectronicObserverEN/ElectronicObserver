using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserverTypes.Extensions;

public static class EquipmentTypesExtensions
{
	public static IEnumerable<EquipmentTypes> ToTypes(this EquipmentTypeGroup group) => group switch
	{
		EquipmentTypeGroup.MainGun => new[]
		{
			EquipmentTypes.MainGunSmall,
			EquipmentTypes.MainGunMedium,
			EquipmentTypes.MainGunLarge,
			EquipmentTypes.MainGunLarge2,
		},

		EquipmentTypeGroup.Secondary => new[]
		{
			EquipmentTypes.SecondaryGun,
			EquipmentTypes.AAGun 
		},

		EquipmentTypeGroup.Torpedo => new[]
		{
			EquipmentTypes.Torpedo,
			EquipmentTypes.MidgetSubmarine,
			EquipmentTypes.SubmarineTorpedo,
		},

		EquipmentTypeGroup.Fighter => new[]
		{
			EquipmentTypes.CarrierBasedFighter,
			EquipmentTypes.JetFighter,
		},

		EquipmentTypeGroup.Bomber => new[]
		{
			EquipmentTypes.CarrierBasedBomber,
			EquipmentTypes.JetBomber,
		},

		EquipmentTypeGroup.TorpedoBomber => new[]
		{
			EquipmentTypes.CarrierBasedTorpedo,
			EquipmentTypes.JetTorpedo,
		},

		EquipmentTypeGroup.LandBasedFighters => new[]
		{
			EquipmentTypes.Interceptor,
		},

		EquipmentTypeGroup.LandBasedBombers => new[]
		{
			EquipmentTypes.LandBasedAttacker,
			EquipmentTypes.HeavyBomber,
		},

		EquipmentTypeGroup.SeaplaneAndRecons => new[]
		{
			EquipmentTypes.CarrierBasedRecon,
			EquipmentTypes.SeaplaneRecon,
			EquipmentTypes.SeaplaneBomber,
			EquipmentTypes.Autogyro,
			EquipmentTypes.ASPatrol,
			EquipmentTypes.FlyingBoat,
			EquipmentTypes.SeaplaneFighter,
			EquipmentTypes.LandBasedRecon,
			EquipmentTypes.JetRecon,
			EquipmentTypes.CarrierBasedRecon2,
		},

		EquipmentTypeGroup.Radar => new[]
		{
			EquipmentTypes.RadarSmall,
			EquipmentTypes.RadarLarge,
			EquipmentTypes.RadarLarge2,
		},

		EquipmentTypeGroup.ASW => new[]
		{
			EquipmentTypes.Sonar,
			EquipmentTypes.DepthCharge,
			EquipmentTypes.SonarLarge,
		},

		EquipmentTypeGroup.Other => new[]
		{
			EquipmentTypes.ExtraArmor,
			EquipmentTypes.Engine,
			EquipmentTypes.AAShell,
			EquipmentTypes.APShell,
			EquipmentTypes.DamageControl,
			EquipmentTypes.ExtraArmorMedium,
			EquipmentTypes.ExtraArmorLarge,
			EquipmentTypes.Searchlight,
			EquipmentTypes.RepairFacility,
			EquipmentTypes.StarShell,
			EquipmentTypes.CommandFacility,
			EquipmentTypes.AviationPersonnel,
			EquipmentTypes.AADirector,
			EquipmentTypes.Rocket,
			EquipmentTypes.SurfaceShipPersonnel,
			EquipmentTypes.SearchlightLarge,
			EquipmentTypes.Ration,
			EquipmentTypes.Supplies,
			EquipmentTypes.TransportMaterial,
			EquipmentTypes.SubmarineEquipment,
		},

		EquipmentTypeGroup.Transport => new[]
		{
			EquipmentTypes.LandingCraft,
			EquipmentTypes.TransportContainer,
			EquipmentTypes.SpecialAmphibiousTank,
		},

		_ => Enumerable.Empty<EquipmentTypes>()
	};

	public static EquipmentTypeGroup ToGroup(this EquipmentTypes type) => type switch
	{
		EquipmentTypes.MainGunSmall or
		EquipmentTypes.MainGunMedium or
		EquipmentTypes.MainGunLarge or
		EquipmentTypes.MainGunLarge2 => EquipmentTypeGroup.MainGun,

		EquipmentTypes.SecondaryGun or
		EquipmentTypes.AAGun => EquipmentTypeGroup.Secondary,

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
		EquipmentTypes.SubmarineEquipment => EquipmentTypeGroup.Other,

		EquipmentTypes.LandingCraft or
		EquipmentTypes.TransportContainer or
		EquipmentTypes.SpecialAmphibiousTank => EquipmentTypeGroup.Transport,

		EquipmentTypes.VTFuse or
		_ => EquipmentTypeGroup.Unknown,
	};
}
