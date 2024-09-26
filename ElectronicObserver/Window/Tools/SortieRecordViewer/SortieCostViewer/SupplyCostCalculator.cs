using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Database;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;

public class SupplyCostCalculator(ElectronicObserverContext db, ToolService toolService, SortieRecordViewModel sortie)
{
	private ElectronicObserverContext Db { get; } = db;
	private ToolService ToolService { get; } = toolService;

	private SortieRecord Model { get; } = sortie.Model;

	private SortieDetailViewModel? SortieDetails { get; set; }

	public SortieCostModel SupplyCost(List<IFleetData?> fleetsBeforeSortie,
		List<IFleetData?>? fleetsAfterSortie, int sortieFleetId, bool isCombinedFleet)
	{
		if (fleetsAfterSortie is null)
		{
			return CalculateSupplyCost(Db, Model);
		}

		IEnumerable<IShipData?>? mainShipsBefore = fleetsBeforeSortie[sortieFleetId - 1]?.MembersWithoutEscaped;
		IEnumerable<IShipData?>? mainShipsAfter = fleetsAfterSortie[sortieFleetId - 1]?.MembersWithoutEscaped;

		if (mainShipsBefore is null) return SortieCostModel.Zero;
		if (mainShipsAfter is null) return SortieCostModel.Zero;

		SortieCostModel cost = SupplyCost(mainShipsBefore, mainShipsAfter);

		if (!isCombinedFleet) return cost;

		IEnumerable<IShipData?>? escortShipsBefore = fleetsBeforeSortie[1]?.MembersWithoutEscaped;
		IEnumerable<IShipData?>? escortShipsAfter = fleetsAfterSortie[1]?.MembersWithoutEscaped;

		if (escortShipsBefore is null) return cost;
		if (escortShipsAfter is null) return cost;

		return cost + SupplyCost(escortShipsBefore, escortShipsAfter);
	}

	private static SortieCostModel SupplyCost(IEnumerable<IShipData?> before, IEnumerable<IShipData?> after)
	{
		List<IShipData?> beforeList = before.ToList();
		List<IShipData?> afterList = after.ToList();

		if (beforeList.Count == afterList.Count)
		{
			return beforeList
				.Zip(afterList, SupplyCost)
				.Sum();
		}

		List<SortieCostModel> costs = [];
		int afterIndex = 0;

		foreach (IShipData shipBefore in beforeList.OfType<IShipData>())
		{
			if (afterIndex >= afterList.Count) break;

			IShipData? shipAfter = afterList[afterIndex];

			if (shipBefore.ShipID != shipAfter?.ShipID) continue;

			costs.Add(SupplyCost(shipBefore, shipAfter));

			afterIndex++;
		}

		return costs.Sum();
	}

	private static SortieCostModel SupplyCost(IShipData? before, IShipData? after) => (before, after) switch
	{
		(not null, not null) => new()
		{
			Fuel = SupplyCost(before, before.FuelMax, before.Fuel, after.Fuel),
			Ammo = SupplyCost(before, before.AmmoMax, before.Ammo, after.Ammo),
			Bauxite = (before.Aircraft.Sum() - after.Aircraft.Sum()) * 5,
		},

		_ => SortieCostModel.Zero,
	};

	private static int SupplyCost(IShipData ship, int max, int before, int after) => (before == max) switch
	{
		true => MarriageResupply(ship, before - after),
		_ => MarriageResupply(ship, max - after) - MarriageResupply(ship, max - before),
	};

	private static int MarriageResupply(IShipData ship, int resupply) => resupply switch
	{
		<= 0 => 0,
		_ when ship.IsMarried => Math.Max(1, (int)(resupply * 0.85)),
		_ => resupply,
	};

	public SortieCostModel NodeSupportSupplyCost(List<IFleetData?> fleetsBeforeSortie,
		List<IFleetData?>? fleetsAfterSortie, int sortieFleetId)
	{
		if (sortieFleetId <= 0) return SortieCostModel.Zero;

		if (Model.CalculatedSortieCost.NodeSupportSupplyCost is not null)
		{
			return Model.CalculatedSortieCost.NodeSupportSupplyCost;
		}

		SortieCostModel cost = SupportSupplyCost(fleetsBeforeSortie, fleetsAfterSortie, sortieFleetId);

		Model.CalculatedSortieCost.NodeSupportSupplyCost = cost;

		Db.Sorties.Update(Model);
		Db.SaveChanges();

		return Model.CalculatedSortieCost.NodeSupportSupplyCost;
	}

	public SortieCostModel BossSupportSupplyCost(List<IFleetData?> fleetsBeforeSortie,
		List<IFleetData?>? fleetsAfterSortie, int sortieFleetId)
	{
		if (sortieFleetId <= 0) return SortieCostModel.Zero;

		if (Model.CalculatedSortieCost.BossSupportSupplyCost is not null)
		{
			return Model.CalculatedSortieCost.BossSupportSupplyCost;
		}

		SortieCostModel cost = SupportSupplyCost(fleetsBeforeSortie, fleetsAfterSortie, sortieFleetId);

		Model.CalculatedSortieCost.BossSupportSupplyCost = cost;

		Db.Sorties.Update(Model);
		Db.SaveChanges();

		return Model.CalculatedSortieCost.BossSupportSupplyCost;
	}

	private SortieCostModel SupportSupplyCost(List<IFleetData?> fleetsBeforeSortie,
		List<IFleetData?>? fleetsAfterSortie, int sortieFleetId)
	{
		if (fleetsBeforeSortie[sortieFleetId - 1] is not IFleetData fleet) return SortieCostModel.Zero;
		if (fleet.MembersWithoutEscaped is null) return SortieCostModel.Zero;

		if (fleetsAfterSortie is not null)
		{
			IEnumerable<IShipData?>? shipsBefore = fleet.MembersWithoutEscaped;
			IEnumerable<IShipData?>? shipsAfter = fleetsAfterSortie[sortieFleetId - 1]?.MembersWithoutEscaped;

			if (shipsAfter is null) return SortieCostModel.Zero;

			SortieCostModel cost = SupplyCost(shipsBefore, shipsAfter);

			// in come cases the support fleet data isn't recorded correctly
			// no idea why
			if (cost != SortieCostModel.Zero)
			{
				return cost;
			}
		}

		(double fuelConsumptionModifier, double ammoConsumptionModifier) = ConsumptionModifier(fleet.SupportType);

		int fuel = 0;
		int ammo = 0;
		int bauxite = 0;

		foreach (IShipData? ship in fleet.MembersWithoutEscaped)
		{
			if (ship is null) continue;

			fuel += MarriageResupply(ship, SupportResupply(ship.Fuel, ship.FuelMax, fuelConsumptionModifier));
			ammo += MarriageResupply(ship, SupportResupply(ship.Ammo, ship.AmmoMax, ammoConsumptionModifier));
		}

		if (fleet.SupportType is SupportType.Aerial or SupportType.AntiSubmarine)
		{
			SortieDetails ??= ToolService.GenerateSortieDetailViewModel(Db, Model);

			if (SortieDetails is not null)
			{
				bauxite = SortieDetails.Nodes
					.OfType<BattleNode>()
					.SelectMany(b => b.AllPhases)
					.OfType<PhaseSupport>()
					.Sum(s => (s.Stage1FLostcount ?? 0) + (s.Stage2FLostcount ?? 0)) * 5;
			}
		}

		SortieCostModel calculatedSupportCost = new()
		{
			Fuel = fuel,
			Ammo = ammo,
			Bauxite = bauxite,
		};

		return calculatedSupportCost;

		static int SupportResupply(int current, int max, double modifier) =>
			(int)Math.Min(current, Math.Ceiling(max * modifier));
	}

	private SortieCostModel CalculateSupplyCost(ElectronicObserverContext db, SortieRecord model)
	{
		if (model.CalculatedSortieCost.SortieFleetSupplyCost is not null)
		{
			return model.CalculatedSortieCost.SortieFleetSupplyCost;
		}

		SortieDetails ??= ToolService.GenerateSortieDetailViewModel(db, model);

		if (SortieDetails is null) return SortieCostModel.Zero;

		model.CalculatedSortieCost.SortieFleetSupplyCost = CalculateSupplyCost(SortieDetails);

		db.Sorties.Update(model);
		db.SaveChanges();

		return model.CalculatedSortieCost.SortieFleetSupplyCost;
	}

#pragma warning disable S3776
	private static SortieCostModel CalculateSupplyCost(SortieDetailViewModel details)
#pragma warning restore S3776
	{
		BattleFleets fleetsBefore = details.FleetsBeforeSortie;
		BattleFleets fleetsAfter = fleetsBefore.Clone();

		List<IShipData?> shipsBefore = fleetsBefore.SortieShips();
		List<IShipData?> shipsAfter = fleetsAfter.SortieShips();

		FleetType playerFleetType = fleetsBefore.Fleet.FleetType;
		int bauxite = 0;

		foreach (SortieNode node in details.Nodes)
		{
			DayAttackKind? daySpecialAttack = null;
			List<int> daySpecialAttackIndexes = [];
			NightAttackKind? nightSpecialAttack = null;
			List<int> nightSpecialAttackIndexes = [];
			bool hasSecondBattle = false;
			FleetType? enemyFleetType = null;
			ApiHappening? happening = node.Happening;

			(double fuelConsumptionModifier, double ammoConsumptionModifier) = happening switch
			{
				ApiHappening => ConsumptionModifier(happening, shipsAfter),
				_ => ConsumptionModifier(node),
			};

			if (node is BattleNode battleNode)
			{
				bauxite += battleNode.FirstBattle.Phases
					.OfType<PhaseAirBattleBase>()
					.Sum(b => b.AircraftLostStage1Friend + b.AircraftLostStage2Friend) * 5;

				if (battleNode.BattleResult is null) continue;

				enemyFleetType = battleNode.FirstBattle.FleetsBeforeBattle.EnemyFleet?.FleetType;
				hasSecondBattle = battleNode.SecondBattle is not null;

				daySpecialAttack = battleNode.AllPhases
					.OfType<PhaseShelling>()
					.SelectMany(s => s.AttackDisplays)
					.SelectMany(a => a.Attacks)
					.Where(a => a.AttackKind.IsSpecialAttack())
					.Select(a => a.AttackKind)
					.FirstOrDefault();

				daySpecialAttackIndexes = daySpecialAttack.SpecialAttackParticipationIndexes();

				nightSpecialAttack = battleNode.AllPhases
					.OfType<PhaseNightBattle>()
					.SelectMany(s => s.AttackDisplays)
					.SelectMany(a => a.Attacks)
					.Where(a => a.AttackKind.IsSpecialAttack())
					.Select(a => a.AttackKind)
					.FirstOrDefault();

				nightSpecialAttackIndexes = nightSpecialAttack.SpecialAttackParticipationIndexes();
			}

			foreach (IShipData? ship in shipsAfter)
			{
				if (ship is not ShipDataMock s) continue;

				Enum? attack = daySpecialAttackIndexes.Contains(shipsAfter.IndexOf(ship)) switch
				{
					true => daySpecialAttack,
					_ => null,
				};

				int combinedFleetOffset = playerFleetType switch
				{
					FleetType.Single => 0,
					_ => 6,
				};

				attack ??= nightSpecialAttackIndexes.Contains(shipsAfter.IndexOf(ship) - combinedFleetOffset) switch
				{
					true => nightSpecialAttack,
					_ => null,
				};

				ApplyFuelConsumption(s, fuelConsumptionModifier, happening);
				ApplyAmmoConsumption(s, ammoConsumptionModifier, happening, playerFleetType, enemyFleetType, attack, hasSecondBattle);
			}
		}

		List<ShipId> finalFleet = details.Fleets
			.SortieShips()
			.OfType<IShipData>()
			.Select(s => s.MasterShip.ShipId)
			.ToList();

		shipsBefore = shipsBefore.IntersectBy(finalFleet, s => s?.MasterShip.ShipId ?? ShipId.Unknown).ToList();
		shipsAfter = shipsAfter.IntersectBy(finalFleet, s => s?.MasterShip.ShipId ?? ShipId.Unknown).ToList();

		return SupplyCost(shipsBefore, shipsAfter) with
		{
			Bauxite = bauxite,
		};
	}

	private static (double Fuel, double Ammo) ConsumptionModifier(SortieNode node) => node switch
	{
		// handled with api data
		{ ApiColorNo: CellType.Maelstrom } => (0, 0),

		{ ApiColorNo: CellType.BossBattle } => (0.2, 0.2),

		{ ApiColorNo: CellType.SubAir } => (0.12, 0.06),
		{ ApiColorNo: CellType.NightBattle } => (0.1, 0.1),
		{ ApiColorNo: CellType.RadarFire } => (0.04, 0),

		BattleNode b => b.FirstBattle switch
		{
			DayFromNightBattleData => (0.2, 0.2),

			BattleAirBattle or
			BattleCombinedAirBattle => (0.2, 0.2),

			BattleAirRaid or
			BattleCombinedAirRaid => node.World switch
			{
				6 => (0.04, 0.08),
				_ => (0.06, 0.04),
			},

			_ when b.IsSubsOnly() => (0.08, 0),
			_ when b.IsPtOnly() => (0.04, 0.08),
			_ => (0.2, 0.2),
		},

		_ => (0, 0),
	};

	private static (double Fuel, double Ammo) ConsumptionModifier(ApiHappening happening, IEnumerable<IShipData?> ships)
		=> happening.ApiMstId switch
		{
			MaelstromType.Fuel => ((double)happening.ApiCount / ships.Max(s => s?.Fuel ?? 0), 0),
			MaelstromType.Ammo => (0, (double)happening.ApiCount / ships.Max(s => s?.Ammo ?? 0)),

			_ => (0, 0),
		};

	private static (double Fuel, double Ammo) ConsumptionModifier(SupportType supportType)
		=> supportType switch
		{
			SupportType.Shelling or SupportType.Torpedo => (0.5, 0.8),

			_ => (0.5, 0.4),
		};

	private static void ApplyFuelConsumption(IShipData ship, double fuelConsumptionModifier,
		ApiHappening? happening)
	{
		if (fuelConsumptionModifier > 0)
		{
			int fuelConsumption = happening switch
			{
				ApiHappening => (int)Math.Max(1, Math.Floor(ship.Fuel * fuelConsumptionModifier)),
				_ => (int)Math.Max(1, Math.Floor(ship.FuelMax * fuelConsumptionModifier)),
			};

			ship.Fuel = Math.Max(0, ship.Fuel - fuelConsumption);
		}
	}

	private static void ApplyAmmoConsumption(IShipData ship, double ammoConsumptionModifier,
		ApiHappening? happening, FleetType playerFleetType, FleetType? enemyFleetType,
		Enum? attack, bool hasSecondBattle)
	{
		if (ammoConsumptionModifier > 0)
		{
			int ammoConsumption = CalculateAmmoConsumption(ship, attack, playerFleetType, enemyFleetType, hasSecondBattle,
				ammoConsumptionModifier, happening);

			ship.Ammo = Math.Max(0, ship.Ammo - ammoConsumption);
		}
	}

	private static int CalculateAmmoConsumption(IShipData s, Enum? attack,
		FleetType playerFleetType, FleetType? enemyFleetType, bool hasSecondBattle,
		double ammoConsumptionModifier, ApiHappening? happening)
	{
		double specialAttackBonus = SpecialAttackBonus(attack, playerFleetType, enemyFleetType,
			hasSecondBattle, ammoConsumptionModifier);

		int consumption = happening switch
		{
			ApiHappening => (int)Math.Max(1, Math.Floor(s.Ammo * ammoConsumptionModifier)),
			_ => attack switch
			{
				DayAttackKind.SpecialNagato or
				NightAttackKind.SpecialNagato or
				DayAttackKind.SpecialMutsu or
				NightAttackKind.SpecialMutsu or
				DayAttackKind.SpecialColorado or
				NightAttackKind.SpecialColorado => (hasSecondBattle, enemyFleetType) switch
				{
					(true, FleetType.Single) => (int)Math.Max(1, Math.Ceiling(s.AmmoMax * (ammoConsumptionModifier + specialAttackBonus))),
					_ => (int)Math.Max(1, Math.Floor(s.AmmoMax * ammoConsumptionModifier) + Math.Floor(s.AmmoMax * specialAttackBonus)),
				},

				NightAttackKind.SpecialKongou => (int)Math.Max(1, Math.Floor(s.AmmoMax * (ammoConsumptionModifier * 1.2))),

				_ => (int)Math.Max(1, Math.Floor(s.AmmoMax * (ammoConsumptionModifier + specialAttackBonus))),
			},
		};

		if (hasSecondBattle && enemyFleetType is FleetType.Single)
		{
			consumption += attack switch
			{
				NightAttackKind.SpecialKongou => (int)Math.Max(1, Math.Floor(s.AmmoMax * (ammoConsumptionModifier * 1.2 / 2))),
				_ => (int)Math.Max(1, Math.Ceiling(s.AmmoMax * ((ammoConsumptionModifier + specialAttackBonus) / 2))),
			};
		}

		return consumption;
	}

	private static double SpecialAttackBonus(Enum? attack, FleetType playerFleetType,
		FleetType? enemyFleetType, bool hasSecondBattle, double ammoConsumptionModifier) => attack switch
		{
			DayAttackKind.SpecialNagato or
			NightAttackKind.SpecialNagato or
			DayAttackKind.SpecialMutsu or
			NightAttackKind.SpecialMutsu or
			DayAttackKind.SpecialColorado or
			NightAttackKind.SpecialColorado => (playerFleetType, enemyFleetType, hasSecondBattle) switch
			{
				(FleetType.Single, FleetType.Single, _) => ammoConsumptionModifier / 2,
				(_, _, true) => 0,
				_ => ammoConsumptionModifier / 2,
			},

			DayAttackKind.SpecialYamato2Ships or
			NightAttackKind.SpecialYamato2Ships => 0.12,

			DayAttackKind.SpecialYamato3Ships or
			NightAttackKind.SpecialYamato3Ships => 0.16,

			_ => 0,
		};
}
