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

		EquipmentTypeGroup.Secondaries => new[]
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
			EquipmentTypes.Interceptor,
			EquipmentTypes.JetFighter,
		},

		EquipmentTypeGroup.Bomber => new[]
		{
			EquipmentTypes.CarrierBasedBomber,
			EquipmentTypes.LandBasedAttacker,
			EquipmentTypes.HeavyBomber,
			EquipmentTypes.JetBomber,
		},

		EquipmentTypeGroup.TorpedoBomber => new[]
		{
			EquipmentTypes.CarrierBasedTorpedo,
			EquipmentTypes.JetTorpedo,
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
		EquipmentTypes.MainGunSmall => EquipmentTypeGroup.MainGun,
		EquipmentTypes.MainGunMedium => EquipmentTypeGroup.MainGun,
		EquipmentTypes.MainGunLarge => EquipmentTypeGroup.MainGun,
		EquipmentTypes.MainGunLarge2 => EquipmentTypeGroup.MainGun,

		EquipmentTypes.SecondaryGun => EquipmentTypeGroup.Secondaries,
		EquipmentTypes.AAGun => EquipmentTypeGroup.Secondaries,

		EquipmentTypes.Torpedo => EquipmentTypeGroup.Torpedo,
		EquipmentTypes.MidgetSubmarine => EquipmentTypeGroup.Torpedo,
		EquipmentTypes.SubmarineTorpedo => EquipmentTypeGroup.Torpedo,

		EquipmentTypes.CarrierBasedFighter => EquipmentTypeGroup.Fighter,
		EquipmentTypes.Interceptor => EquipmentTypeGroup.Fighter,
		EquipmentTypes.JetFighter => EquipmentTypeGroup.Fighter,

		EquipmentTypes.CarrierBasedBomber => EquipmentTypeGroup.Bomber,
		EquipmentTypes.LandBasedAttacker => EquipmentTypeGroup.Bomber,
		EquipmentTypes.HeavyBomber => EquipmentTypeGroup.Bomber,
		EquipmentTypes.JetBomber => EquipmentTypeGroup.Bomber,

		EquipmentTypes.CarrierBasedTorpedo => EquipmentTypeGroup.TorpedoBomber,
		EquipmentTypes.JetTorpedo => EquipmentTypeGroup.TorpedoBomber,

		EquipmentTypes.CarrierBasedRecon => EquipmentTypeGroup.SeaplaneAndRecons,
		EquipmentTypes.SeaplaneRecon => EquipmentTypeGroup.SeaplaneAndRecons,
		EquipmentTypes.SeaplaneBomber => EquipmentTypeGroup.SeaplaneAndRecons,
		EquipmentTypes.Autogyro => EquipmentTypeGroup.SeaplaneAndRecons,
		EquipmentTypes.ASPatrol => EquipmentTypeGroup.SeaplaneAndRecons,
		EquipmentTypes.FlyingBoat => EquipmentTypeGroup.SeaplaneAndRecons,
		EquipmentTypes.SeaplaneFighter => EquipmentTypeGroup.SeaplaneAndRecons,
		EquipmentTypes.LandBasedRecon => EquipmentTypeGroup.SeaplaneAndRecons,
		EquipmentTypes.JetRecon => EquipmentTypeGroup.SeaplaneAndRecons,
		EquipmentTypes.CarrierBasedRecon2 => EquipmentTypeGroup.SeaplaneAndRecons,

		EquipmentTypes.RadarSmall => EquipmentTypeGroup.Radar,
		EquipmentTypes.RadarLarge => EquipmentTypeGroup.Radar,
		EquipmentTypes.RadarLarge2 => EquipmentTypeGroup.Radar,

		EquipmentTypes.Sonar => EquipmentTypeGroup.ASW,
		EquipmentTypes.DepthCharge => EquipmentTypeGroup.ASW,
		EquipmentTypes.SonarLarge => EquipmentTypeGroup.ASW,

		EquipmentTypes.ExtraArmor => EquipmentTypeGroup.Other,
		EquipmentTypes.Engine => EquipmentTypeGroup.Other,
		EquipmentTypes.AAShell => EquipmentTypeGroup.Other,
		EquipmentTypes.APShell => EquipmentTypeGroup.Other,
		EquipmentTypes.DamageControl => EquipmentTypeGroup.Other,
		EquipmentTypes.ExtraArmorMedium => EquipmentTypeGroup.Other,
		EquipmentTypes.ExtraArmorLarge => EquipmentTypeGroup.Other,
		EquipmentTypes.Searchlight => EquipmentTypeGroup.Other,
		EquipmentTypes.RepairFacility => EquipmentTypeGroup.Other,
		EquipmentTypes.StarShell => EquipmentTypeGroup.Other,
		EquipmentTypes.CommandFacility => EquipmentTypeGroup.Other,
		EquipmentTypes.AviationPersonnel => EquipmentTypeGroup.Other,
		EquipmentTypes.AADirector => EquipmentTypeGroup.Other,
		EquipmentTypes.Rocket => EquipmentTypeGroup.Other,
		EquipmentTypes.SurfaceShipPersonnel => EquipmentTypeGroup.Other,
		EquipmentTypes.SearchlightLarge => EquipmentTypeGroup.Other,
		EquipmentTypes.Ration => EquipmentTypeGroup.Other,
		EquipmentTypes.Supplies => EquipmentTypeGroup.Other,
		EquipmentTypes.TransportMaterial => EquipmentTypeGroup.Other,
		EquipmentTypes.SubmarineEquipment => EquipmentTypeGroup.Other,

		EquipmentTypes.LandingCraft => EquipmentTypeGroup.Transport,
		EquipmentTypes.TransportContainer => EquipmentTypeGroup.Transport,
		EquipmentTypes.SpecialAmphibiousTank => EquipmentTypeGroup.Transport,

		EquipmentTypes.VTFuse => throw new NotImplementedException(),
		_ => throw new NotImplementedException(),
	};
}
