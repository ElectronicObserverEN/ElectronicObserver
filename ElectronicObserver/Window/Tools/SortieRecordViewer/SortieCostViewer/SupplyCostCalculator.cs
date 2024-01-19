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
using DayAttack = ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase.DayAttack;

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

		if (mainShipsBefore is null) return new();
		if (mainShipsAfter is null) return new();

		SortieCostModel cost = SupplyCost(mainShipsBefore, mainShipsAfter);

		if (!isCombinedFleet) return cost;

		IEnumerable<IShipData?>? escortShipsBefore = fleetsBeforeSortie[1]?.MembersWithoutEscaped;
		IEnumerable<IShipData?>? escortShipsAfter = fleetsAfterSortie[1]?.MembersWithoutEscaped;

		if (escortShipsBefore is null) return cost;
		if (escortShipsAfter is null) return cost;

		return cost + SupplyCost(escortShipsBefore, escortShipsAfter);
	}

	private static SortieCostModel SupplyCost(IEnumerable<IShipData?> before, IEnumerable<IShipData?> after)
		=> before
			.Zip(after, SupplyCost)
			.Aggregate(new SortieCostModel(), (a, b) => a + b);

	private static SortieCostModel SupplyCost(IShipData? before, IShipData? after) => (before, after) switch
	{
		(not null, not null) => new()
		{
			Fuel = SupplyCost(before, before.FuelMax, before.Fuel, after.Fuel),
			Ammo = SupplyCost(before, before.AmmoMax, before.Ammo, after.Ammo),
			Bauxite = (before.Aircraft.Sum() - after.Aircraft.Sum()) * 5,
		},

		_ => new(),
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
		if (sortieFleetId <= 0) return new();

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
		if (sortieFleetId <= 0) return new();

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
		if (fleetsBeforeSortie[sortieFleetId - 1] is not IFleetData fleet) return new();
		if (fleet.MembersWithoutEscaped is null) return new();

		if (fleetsAfterSortie is not null)
		{
			IEnumerable<IShipData?>? shipsBefore = fleet.MembersWithoutEscaped;
			IEnumerable<IShipData?>? shipsAfter = fleetsAfterSortie[sortieFleetId - 1]?.MembersWithoutEscaped;

			if (shipsAfter is null) return new();

			SortieCostModel cost = SupplyCost(shipsBefore, shipsAfter);

			// in come cases the support fleet data isn't recorded correctly
			// no idea why
			if (cost != new SortieCostModel())
			{
				return cost;
			}
		}

		(double fuelConsumptionModifier, double ammoConsumptionModifier) = ConsumptionModifier(fleet.SupportType);

		int fuel = 0;
		int ammo = 0;
		int baux = 0;

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
				baux = SortieDetails.Nodes
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
			Bauxite = baux,
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

		if (SortieDetails is null) return new();

		BattleFleets fleetsBefore = SortieDetails.FleetsBeforeSortie;
		BattleFleets fleetsAfter = fleetsBefore.Clone();

		FleetType playerFleetType = fleetsBefore.Fleet.FleetType;
		int bauxite = 0;

		List<IShipData?> shipsBefore = fleetsBefore.SortieShips();
		List<IShipData?> shipsAfter = fleetsAfter.SortieShips();

		foreach (SortieNode node in SortieDetails.Nodes)
		{
			(double fuelConsumptionModifier, double ammoConsumptionModifier) = node.Happening switch
			{
				ApiHappening h => ConsumptionModifier(h, shipsAfter),
				_ => ConsumptionModifier(node),
			};

			DayAttackKind? daySpecialAttack = null;
			List<int> daySpecialAttackIndexes = [];
			bool hasSecondBattle = false;
			FleetType? enemyFleetType = null;

			if (node is BattleNode battleNode)
			{
				bauxite += battleNode.FirstBattle.Phases
					.OfType<PhaseAirBattleBase>()
					.Sum(b => b.Stage1FLostcount + b.Stage2FLostcount) * 5;

				enemyFleetType = battleNode.FirstBattle.FleetsBeforeBattle.EnemyFleet?.FleetType;

				if (battleNode.BattleResult is null) continue;

				hasSecondBattle = battleNode.SecondBattle is not null;

				daySpecialAttack = battleNode.FirstBattle.Phases
					.OfType<PhaseShelling>()
					.SelectMany(s => s.AttackDisplays)
					.SelectMany(a => a.Attacks)
					.Where(a => a.AttackKind.IsSpecialAttack())
					.Select(a => a.AttackKind)
					.FirstOrDefault();

				daySpecialAttackIndexes = daySpecialAttack.SpecialAttackParticipationIndexes();
			}

			foreach (IShipData? ship in shipsAfter)
			{
				if (ship is not ShipDataMock s) continue;

				DayAttackKind? attack = daySpecialAttackIndexes.Contains(shipsAfter.IndexOf(ship)) switch
				{
					true => daySpecialAttack,
					_ => DayAttackKind.Unknown,
				};

				ApplyFuelConsumption(s, fuelConsumptionModifier, node);
				ApplyAmmoConsumption(s, ammoConsumptionModifier, node, playerFleetType, enemyFleetType, attack, hasSecondBattle);
			}
		}

		SortieCostModel supplyCost = SupplyCost(shipsBefore, shipsAfter);
		model.CalculatedSortieCost.SortieFleetSupplyCost = supplyCost with
		{
			Bauxite = bauxite,
		};

		db.Sorties.Update(model);
		db.SaveChanges();

		return model.CalculatedSortieCost.SortieFleetSupplyCost;
	}

	private static (double Fuel, double Ammo) ConsumptionModifier(SortieNode node) => node switch
	{
		// handled with api data
		{ ApiColorNo: CellType.Maelstrom } => (0, 0),

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
		SortieNode node)
	{
		if (fuelConsumptionModifier > 0)
		{
			int fuelConsumption = node.Happening switch
			{
				ApiHappening => (int)Math.Max(1, Math.Floor(ship.Fuel * fuelConsumptionModifier)),
				_ => (int)Math.Max(1, Math.Floor(ship.FuelMax * fuelConsumptionModifier)),
			};

			ship.Fuel = Math.Max(0, ship.Fuel - fuelConsumption);
		}
	}

	private static void ApplyAmmoConsumption(IShipData ship, double ammoConsumptionModifier,
		SortieNode node, FleetType playerFleetType, FleetType? enemyFleetType, DayAttackKind? attack,
		bool hasSecondBattle)
	{
		if (ammoConsumptionModifier > 0)
		{
			int ammoConsumption = CalculateAmmoConsumption(ship, attack, playerFleetType, enemyFleetType, hasSecondBattle,
				ammoConsumptionModifier, node);

			ship.Ammo = Math.Max(0, ship.Ammo - ammoConsumption);
		}
	}

	private static int CalculateAmmoConsumption(IShipData s, DayAttackKind? attack,
		FleetType playerFleetType, FleetType? enemyFleetType, bool hasSecondBattle,
		double ammoConsumptionModifier, SortieNode node)
	{
		double specialAttackBonus = attack switch
		{
			DayAttackKind.SpecialNagato or
			DayAttackKind.SpecialMutsu or
			DayAttackKind.SpecialColorado => (playerFleetType, enemyFleetType, hasSecondBattle) switch
			{
				(FleetType.Single, FleetType.Single, _) => ammoConsumptionModifier / 2,
				(_, _, true) => 0,
				_ => ammoConsumptionModifier / 2,
			},

			DayAttackKind.SpecialYamato2Ships => 0.12,
			DayAttackKind.SpecialYamato3Ships => 0.16,

			_ => 0,
		};

		int consumption = node.Happening switch
		{
			ApiHappening => (int)Math.Max(1, Math.Floor(s.Ammo * ammoConsumptionModifier)),
			_ => attack switch
			{
				DayAttackKind.SpecialNagato or
				DayAttackKind.SpecialMutsu or
				DayAttackKind.SpecialColorado
					=> (int)Math.Max(1, Math.Floor(s.AmmoMax * ammoConsumptionModifier) + Math.Floor(s.AmmoMax * specialAttackBonus)),

				DayAttackKind.SpecialKongo => (int)Math.Max(1, Math.Floor(s.AmmoMax * (ammoConsumptionModifier * 1.2))),

				_ => (int)Math.Max(1, Math.Floor(s.AmmoMax * (ammoConsumptionModifier + specialAttackBonus))),
			},
		};

		if (hasSecondBattle && enemyFleetType is FleetType.Single)
		{
			consumption += (int)Math.Max(1, Math.Ceiling(s.AmmoMax * (ammoConsumptionModifier / 2)));
		}

		return consumption;
	}
}
