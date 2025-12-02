using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserver.Core.Types.Extensions;

public static class FleetDataExtensions
{
	public static int NumberOfSurfaceShipNotRetreatedNotSunk(this IFleetData fleet) =>
		fleet.MembersWithoutEscaped?.Count(ship => ship is { HPCurrent: > 0, MasterShip.IsSubmarine: false }) ?? 0;

	public static SupportType GetSupportType(this IFleetData fleet)
	{
		int destroyers = fleet.MembersInstance.Count(s => s?.MasterShip.ShipType is
			ShipTypes.Destroyer);

		if (destroyers < 2) return SupportType.None;

		if (IsAirSupport(fleet))
		{
			if (IsAntiSubmarineSupport(fleet))
			{
				return SupportType.AntiSubmarine;
			}

			return SupportType.Aerial;
		}

		if (IsShellingSupport(fleet))
		{
			return SupportType.Shelling;
		}

		return SupportType.Torpedo;
	}

	private static bool IsAirSupport(IFleetData fleet)
	{
		int carriers = fleet.MembersInstance.Count(s => s?.MasterShip.ShipType is
			ShipTypes.AircraftCarrier or
			ShipTypes.ArmoredAircraftCarrier or
			ShipTypes.LightAircraftCarrier);

		int carrierSupportA = fleet.MembersInstance.Count(s => s?.MasterShip.ShipType is
			ShipTypes.SeaplaneTender or
			ShipTypes.AmphibiousAssaultShip);

		int carrierSupportB = fleet.MembersInstance.Count(s => s?.MasterShip.ShipType is
			ShipTypes.AviationBattleship or
			ShipTypes.AviationCruiser or
			ShipTypes.FleetOiler);

		int gunboats = fleet.MembersInstance.Count(s => s?.MasterShip.ShipType is
			ShipTypes.Battleship or
			ShipTypes.Battlecruiser or
			ShipTypes.HeavyCruiser);

		if (gunboats is 0)
		{
			if (carriers >= 1) return true;
			if (carrierSupportA >= 2) return true;
			if (carrierSupportB >= 2) return true;
		}

		if (gunboats is 1)
		{
			return carriers + carrierSupportA >= 2;
		}

		return false;
	}

	/// <summary>
	/// This function doesn't check if it's a valid air support,
	/// so that check must be performed before calling this one.
	/// </summary>
	private static bool IsAntiSubmarineSupport(IFleetData fleet)
	{
		List<IShipData> antiSubmarineAircraftCarriers = fleet.MembersInstance
			.Where(s => s is not null)
			.Cast<IShipData>()
			.Where(s => s.HasAntiSubmarineAircraft())
			.ToList();

		int lightCarriers = antiSubmarineAircraftCarriers
			.Count(s => s.MasterShip.ShipType is ShipTypes.LightAircraftCarrier);

		int escorts = fleet.MembersInstance.Count(s => s?.MasterShip.ShipType is
			ShipTypes.Escort);

		return lightCarriers > 0 && (antiSubmarineAircraftCarriers.Count > 1 || escorts > 1);
	}

	private static bool IsShellingSupport(IFleetData fleet)
	{
		int battleships = fleet.MembersInstance.Count(s => s?.MasterShip.ShipType is
			ShipTypes.Battleship or
			ShipTypes.Battlecruiser or
			ShipTypes.AviationBattleship);

		if (battleships >= 2) return true;

		int heavyCruisers = fleet.MembersInstance.Count(s => s?.MasterShip.ShipType is
			ShipTypes.HeavyCruiser or
			ShipTypes.AviationCruiser);

		if (battleships > 0 && heavyCruisers > 2) return true;

		return heavyCruisers > 3;
	}

	/// <summary>
	/// https://x.com/yukicacoon/status/1739480992090632669
	/// </summary>
	/// <param name="fleet"></param>
	/// <returns></returns>
	public static List<SmokeGeneratorTriggerRate> GetSmokeTriggerRates(this IFleetData fleet) => GetSmokeTriggerRates([fleet]);

	/// <summary>
	/// https://x.com/Xe_UCH/status/1767407602554855730
	/// </summary>
	/// <param name="fleets"></param>
	/// <returns></returns>
	public static List<SmokeGeneratorTriggerRate> GetSmokeTriggerRates(this List<IFleetData> fleets)
	{
		if (fleets.Count is 0) return [];

		IShipData? flagship = fleets[0].MembersWithoutEscaped?.First();

		if (flagship is null) return [];

		List<IShipData> ships = fleets
			.SelectMany(fleet => fleet.MembersWithoutEscaped ?? new([]))
			.OfType<IShipData>()
			.ToList();

		List<IEquipmentData> smokeGenerators = ships
			.SelectMany(ship => ship.AllSlotInstance)
			.Where(e => e?.MasterEquipment.EquipmentId is EquipmentId.SurfaceShipEquipment_SmokeGenerator_SmokeScreen)
			.Cast<IEquipmentData>()
			.ToList();

		List<IEquipmentData> smokeGeneratorsKai = ships
			.SelectMany(ship => ship.AllSlotInstance)
			.Where(e => e?.MasterEquipment.EquipmentId is EquipmentId.SurfaceShipEquipment_SmokeGeneratorKai_SmokeScreen)
			.Cast<IEquipmentData>()
			.ToList();

		// https://twitter.com/yukicacoon/status/1739480992090632669
		int smokeGeneratorCount = smokeGenerators.Count + smokeGeneratorsKai.Count * 2;
		double upgradeModifier = 0.3 * smokeGenerators.Sum(eq => eq.Level) + 0.5 * smokeGeneratorsKai.Sum(eq => eq.Level);
		double modifier = Math.Ceiling(Math.Sqrt(flagship.LuckTotal) + upgradeModifier);
		double triggerRate = 1 - Math.Max(3.2 - 0.2 * modifier - smokeGeneratorCount, 0);

		if (smokeGeneratorCount >= 3)
		{
			double tripleTrigger = Math.Min(3 * Math.Ceiling(5 * smokeGeneratorCount + 1.5 * Math.Sqrt(flagship.LuckTotal) + upgradeModifier - 15) + 1, 100);
			double doubleTrigger = 30 - (tripleTrigger > 70 ? tripleTrigger - 70 : 0);
			double singleTrigger = Math.Max(100 - tripleTrigger - doubleTrigger, 0);

			return
			[
				new SmokeGeneratorTriggerRate
				{
					SmokeGeneratorCount = 3,
					ActivationRatePercentage = tripleTrigger * triggerRate,
				},
				new SmokeGeneratorTriggerRate
				{
					SmokeGeneratorCount = 2,
					ActivationRatePercentage = doubleTrigger * triggerRate,
				},
				new SmokeGeneratorTriggerRate
				{
					SmokeGeneratorCount = 1,
					ActivationRatePercentage = singleTrigger * triggerRate,
				},
			];
		}

		if (smokeGeneratorCount == 2)
		{
			double doubleTrigger = Math.Min(3 * Math.Ceiling(5 * smokeGeneratorCount + 1.5 * Math.Sqrt(flagship.LuckTotal) + upgradeModifier - 5) + 1, 100);
			double singleTrigger = Math.Max(100 - doubleTrigger, 0);

			return
			[
				new SmokeGeneratorTriggerRate
				{
					SmokeGeneratorCount = 2,
					ActivationRatePercentage = doubleTrigger * triggerRate,
				},
				new SmokeGeneratorTriggerRate
				{
					SmokeGeneratorCount = 1,
					ActivationRatePercentage = singleTrigger * triggerRate,
				},
			];
		}

		return triggerRate switch
		{
			> 0 =>
			[
				new()
				{
					SmokeGeneratorCount = 1,
					ActivationRatePercentage = triggerRate * 100,
				},
			],
			_ => [],
		};
	}

	/// <summary>
	/// Returns the maximum amount of ships that can be repaired, regardless if the ships in fleet can actually be repaired.
	/// </summary>
	public static int GetAnchorageRepairCount(this IFleetData fleet)
	{
		if (fleet.ExpeditionState is not ExpeditionState.NotDeployed) return 0;
		if (fleet.MembersInstance.FirstOrDefault() is not IShipData flagship) return 0;
		if (flagship.MasterShip.ShipType is not ShipTypes.RepairShip) return 0;
		if (flagship.DamageState <= DamageState.Medium) return 0;
		if (flagship.RepairingDockID is not -1) return 0;

		int repairCount = flagship.MasterShip.ShipId switch
		{
			ShipId.Akashi or ShipId.AkashiKai => 2,
			_ => 0,
		};

		List<IShipData> repairShips = fleet.MembersInstance
			.Take(2)
			.OfType<IShipData>()
			.Where(s => s.MasterShip.ShipType is ShipTypes.RepairShip)
			.Where(s => s.DamageState > DamageState.Medium)
			.ToList();

		if (repairShips.Count > 1)
		{
			repairCount++;
		}

		repairCount += repairShips
			.SelectMany(s => s.AllSlotInstance)
			.Count(e => e?.EquipmentId is EquipmentId.RepairFacility_ShipRepairFacility);

		return repairCount;
	}

	public static bool CanAnchorageRepair(this IFleetData fleet)
	{
		int count = fleet.GetAnchorageRepairCount();

		return fleet.MembersInstance
			.Take(count)
			.OfType<IShipData>()
			.Where(s => s.RepairingDockID == -1)
			.Any(s => s is { DamageState: > DamageState.Medium, HPRate: < 1 });
	}

	public static double GetAnchorageRepairTimeMultiplier(this IFleetData fleet)
	{
		List<IShipData> repairShips = fleet.MembersInstance
			.Take(2)
			.OfType<IShipData>()
			.Where(s => s.MasterShip.ShipType is ShipTypes.RepairShip)
			.Where(s => s.DamageState > DamageState.Medium)
			.ToList();

		if (repairShips.Count < 2) return 1;
		if (!repairShips[1].AllSlotInstance.Any(e => e?.EquipmentId is EquipmentId.RepairFacility_ShipRepairFacility)) return 1;

		return 0.85;
	}

	public static Dictionary<DetectionType, double> GetDetectionProbabilities(this IFleetData fleet)
	{
		if (fleet.MembersWithoutEscaped is null) return [];

		int baseValue = 20; // this is just hardcoded in kancolle kai code
		int basePower = 0; // num1/BasePow in SakutekiInfo
		int attack = 0; // num2/Attack in SakutekiInfo
		int aircraftCarrierCount = 0; // num4 in SakutekiInfo

		foreach ((IShipData? ship, int index) in fleet.MembersInstance.Select((s, i) => (s, i)))
		{
			if (ship is null) continue;
			if (ship.IsEscaped(fleet)) continue;

			int positionMod = index switch
			{
				0 => 2,
				1 => 5,
				_ => 8,
			};

			basePower += (int)((double)ship.LOSTotal / positionMod);

			if (ship.IsAircraftCarrier())
			{
				aircraftCarrierCount++;
			}

			foreach ((IEquipmentData? equipment, int aircraftCount) in ship.AllSlotInstance.Zip(ship.Aircraft))
			{
				if (equipment is null) continue;
				if (!CountsForDetection(equipment)) continue;

				attack += aircraftCount + ProficiencyBonus(equipment);
			}
		}

		if (aircraftCarrierCount > 0)
		{
			attack += 20 + aircraftCarrierCount * 10;
		}

		int finalPower = basePower
			+ ShipCountBonus(fleet.MembersWithoutEscaped.OfType<IShipData>().Count())
			- baseValue
			+ (int)Math.Sqrt(attack * 10.0);

		return GetProbabilities(finalPower, attack);

		static bool CountsForDetection(IEquipmentData equipment) => equipment.MasterEquipment.CategoryType is
			EquipmentTypes.CarrierBasedRecon or
			EquipmentTypes.SeaplaneRecon or
			EquipmentTypes.SeaplaneBomber or
			EquipmentTypes.FlyingBoat;

		// this code doesn't really make any sense
		// the if statements look like they should be using slotExp rather than alvPlus
		// but the code is copied directly from kancolle kai source so...
		static int ProficiencyBonus(IEquipmentData equipment)
		{
			int slotExp = equipment.GetAircraftExp();
			double alvPlus = Math.Sqrt(slotExp * 0.2);

			if (alvPlus >= 25.0)
			{
				alvPlus += 5.0;
			}

			if (alvPlus >= 55.0)
			{
				alvPlus += 10.0;
			}

			if (alvPlus >= 100.0)
			{
				alvPlus += 15.0;
			}

			return (int)alvPlus;
		}

		static int ShipCountBonus(int count) => count switch
		{
			>= 6 => 4,
			5 => 3,
			4 => 2,
			3 => 1,
			_ => 0,
		};

		static Dictionary<DetectionType, double> GetProbabilities(int num1, int attack)
			=> attack switch
			{
				0 => GetProbabilitiesNoPlane(num1),
				_ => GetProbabilitiesPlane(num1),
			};

		static Dictionary<DetectionType, double> GetProbabilitiesNoPlane(int num1)
		{
			Dictionary<DetectionType, double> probabilities = [];

			if (num1 <= 0)
			{
				probabilities.Add(DetectionType.FailureNoPlane, 1.0);
				return probabilities;
			}

			double successRate = num1 / 20.0;

			probabilities.Add(DetectionType.SuccessNoPlane, successRate);

			if (successRate < 1)
			{
				probabilities.Add(DetectionType.FailureNoPlane, 1 - successRate);
			}

			return probabilities;
		}

		static Dictionary<DetectionType, double> GetProbabilitiesPlane(int num1)
		{
			Dictionary<DetectionType, double> probabilities = [];

			if (num1 <= 0)
			{
				// can be DetectionType.Failure based on enemy air power
				probabilities.Add(DetectionType.NoReturn, 1.0);
				return probabilities;
			}

			double successRate = num1 / 20.0;

			// can be DetectionType.SuccessNoReturn based on enemy air power
			probabilities.Add(DetectionType.Success, successRate);

			if (successRate < 1)
			{
				probabilities.Add(DetectionType.NoReturn, 1 - successRate);
			}

			return probabilities;
		}
	}
}
