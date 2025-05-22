using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserver.Core.Types.Extensions;

public static class ShipTypeExtensions
{
	public static IEnumerable<ShipTypes> ToTypes(this ShipTypeGroup group) => group switch
	{
		ShipTypeGroup.Battleships => new[]
		{
			ShipTypes.Battleship,
			ShipTypes.AviationBattleship,
			ShipTypes.Battlecruiser
		},

		ShipTypeGroup.Carriers => new[]
		{
			ShipTypes.AircraftCarrier,
			ShipTypes.ArmoredAircraftCarrier,
			ShipTypes.LightAircraftCarrier
		},

		ShipTypeGroup.HeavyCruisers => new[]
		{
			ShipTypes.HeavyCruiser,
			ShipTypes.AviationCruiser
		},

		ShipTypeGroup.LightCruisers => new[]
		{
			ShipTypes.LightCruiser,
			ShipTypes.TrainingCruiser,
			ShipTypes.TorpedoCruiser
		},

		ShipTypeGroup.Destroyers => new[]
		{
			ShipTypes.Destroyer
		},

		ShipTypeGroup.Escorts => new[]
		{
			ShipTypes.Escort
		},

		ShipTypeGroup.Submarines => new[]
		{
			ShipTypes.Submarine,
			ShipTypes.SubmarineAircraftCarrier
		},

		ShipTypeGroup.Auxiliaries => new[]
		{
			ShipTypes.SeaplaneTender,
			ShipTypes.FleetOiler,
			ShipTypes.RepairShip,
			ShipTypes.AmphibiousAssaultShip,
			ShipTypes.SubmarineTender
		},

		_ => Enumerable.Empty<ShipTypes>()
	};

	public static ShipTypeGroup ToGroup(this ShipTypes type) => type switch
	{
		ShipTypes.Battleship => ShipTypeGroup.Battleships,
		ShipTypes.AviationBattleship => ShipTypeGroup.Battleships,
		ShipTypes.Battlecruiser => ShipTypeGroup.Battleships,

		ShipTypes.AircraftCarrier => ShipTypeGroup.Carriers,
		ShipTypes.ArmoredAircraftCarrier => ShipTypeGroup.Carriers,
		ShipTypes.LightAircraftCarrier => ShipTypeGroup.Carriers,

		ShipTypes.HeavyCruiser => ShipTypeGroup.HeavyCruisers,
		ShipTypes.AviationCruiser => ShipTypeGroup.HeavyCruisers,

		ShipTypes.LightCruiser => ShipTypeGroup.LightCruisers,
		ShipTypes.TrainingCruiser => ShipTypeGroup.LightCruisers,
		ShipTypes.TorpedoCruiser => ShipTypeGroup.LightCruisers,

		ShipTypes.Destroyer => ShipTypeGroup.Destroyers,

		ShipTypes.Escort => ShipTypeGroup.Escorts,

		ShipTypes.Submarine => ShipTypeGroup.Submarines,
		ShipTypes.SubmarineAircraftCarrier => ShipTypeGroup.Submarines,

		ShipTypes.SeaplaneTender => ShipTypeGroup.Auxiliaries,
		ShipTypes.FleetOiler => ShipTypeGroup.Auxiliaries,
		ShipTypes.RepairShip => ShipTypeGroup.Auxiliaries,
		ShipTypes.AmphibiousAssaultShip => ShipTypeGroup.Auxiliaries,
		ShipTypes.SubmarineTender => ShipTypeGroup.Auxiliaries,

		ShipTypes.SuperDreadnoughts => throw new NotImplementedException(),
		ShipTypes.Transport => throw new NotImplementedException(),
		ShipTypes.Unknown => throw new NotImplementedException(),
		_ => throw new NotImplementedException()
	};
}
