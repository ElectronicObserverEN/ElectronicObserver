using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ElectronicObserver.Database;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.KancolleApi.Types;
using ElectronicObserver.KancolleApi.Types.ApiGetMember.Mapinfo;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.KancolleApi.Types.Models;
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
	private DateTime Time { get; } = sortie.SortieStart.ToUniversalTime();
	private List<IBaseAirCorpsData> AirBases { get; } = sortie.Model.FleetData.AirBases
		.Select(a => a.MakeAirBase())
		.ToList();

	private SortieDetailViewModel? SortieDetails { get; set; }

	public SortieCostModel SupplyCost(List<IFleetData?> fleetsBeforeSortie,
		List<IFleetData?>? fleetsAfterSortie, int sortieFleetId) => fleetsAfterSortie switch
	{
		not null => SupplyCost(fleetsBeforeSortie[sortieFleetId - 1], fleetsAfterSortie[sortieFleetId - 1]),
		_ => CalculateSupplyCost(Db, Model),
	};

	private static SortieCostModel SupplyCost(IFleetData? before, IFleetData? after) => (before, after) switch
	{
		(not null, not null) => before.MembersInstance
			.Zip(after.MembersInstance, SupplyCost)
			.Aggregate(new SortieCostModel(), (a, b) => a + b),

		_ => new(),
	};

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

	public SortieCostModel SupportSupplyCost(List<IFleetData?> fleetsBeforeSortie,
		List<IFleetData?>? fleetsAfterSortie, int sortieFleetId)
	{
		if (sortieFleetId > 0 && fleetsAfterSortie is not null)
		{
			SortieCostModel cost = SupplyCost(fleetsBeforeSortie[sortieFleetId - 1], fleetsAfterSortie[sortieFleetId - 1]);

			// in come cases the support fleet data isn't recorded correctly
			// no idea why
			if (cost != new SortieCostModel())
			{
				return cost;
			}
		}

		SortieCostModel calculatedSupportCost = new();

		return calculatedSupportCost;
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

		FleetType? enemyFleetType = fleetsBefore.EnemyFleet?.FleetType;

		int bauxite = 0;

		if (fleetsAfter.Fleet.MembersWithoutEscaped is null) return new();

		foreach (SortieNode node in SortieDetails.Nodes)
		{
			(double fuelConsumptionModifier, double ammoConsumptionModifier) = node.Happening switch
			{
				ApiHappening h => ConsumptionModifier(h, fleetsAfter.Fleet.MembersInstance),
				_ => ConsumptionModifier(node),
			};

			List<DayAttack> specialAttacks = [];
			bool hasSecondBattle = false;

			if (node is BattleNode battleNode)
			{
				hasSecondBattle = battleNode.SecondBattle is not null;

				specialAttacks = battleNode.FirstBattle.Phases
					.OfType<PhaseShelling>()
					.SelectMany(s => s.AttackDisplays)
					.SelectMany(a => a.Attacks)
					.Where(a => a.AttackKind.IsSpecialAttack())
					.ToList();

				bauxite += battleNode.FirstBattle.Phases
					.OfType<PhaseAirBattleBase>()
					.Sum(b => b.Stage1FLostcount + b.Stage2FLostcount) * 5;
			}

			foreach (IShipData? ship in fleetsAfter.Fleet.MembersWithoutEscaped)
			{
				if (ship is not ShipDataMock s) continue;

				if (fuelConsumptionModifier > 0)
				{
					s.Fuel -= node.Happening switch
					{
						ApiHappening => (int)Math.Max(1, Math.Floor(s.Fuel * fuelConsumptionModifier)),
						_ => (int)Math.Max(1, Math.Floor(s.FuelMax * fuelConsumptionModifier)),
					};
				}

				if (ammoConsumptionModifier > 0)
				{
					int ammoConsumption = 0;

					DayAttackKind? attack = specialAttacks
						.FirstOrDefault(a => a.Attacker.ShipID == s.ShipID)?.AttackKind;

					double specialAttackBonus = attack switch
					{
						DayAttackKind.SpecialNagato or
						DayAttackKind.SpecialMutsu or
						DayAttackKind.SpecialColorado => (enemyFleetType, hasSecondBattle) switch
						{
							(FleetType.Single, _) => ammoConsumptionModifier / 2,
							(_, true) => 0,
							_ => ammoConsumptionModifier / 2,
						},

						DayAttackKind.SpecialYamato2Ships => 0.12,

						_ => 0,
					};

					Func<double, double> roundingFunction = hasSecondBattle switch
					{
						true => Math.Ceiling,
						_ => Math.Floor,
					};

					if (hasSecondBattle)
					{
						ammoConsumptionModifier *= 1.5;
					}

					ammoConsumption += node.Happening switch
					{
						ApiHappening => (int)Math.Max(1, Math.Floor(s.Ammo * ammoConsumptionModifier)),
						_ => attack switch
						{
							DayAttackKind.SpecialNagato or
							DayAttackKind.SpecialMutsu or
							DayAttackKind.SpecialColorado => enemyFleetType switch
							{
								FleetType.Single => (int)Math.Max(1, Math.Ceiling(s.AmmoMax * (ammoConsumptionModifier + specialAttackBonus))),
								_ => (int)Math.Max(1, Math.Floor(s.AmmoMax * ammoConsumptionModifier) + Math.Floor(s.AmmoMax * specialAttackBonus)),
							},

							DayAttackKind.SpecialKongo => (int)Math.Max(1, roundingFunction(s.AmmoMax * (ammoConsumptionModifier * 1.2))),

							_ => (int)Math.Max(1, roundingFunction(s.AmmoMax * (ammoConsumptionModifier + specialAttackBonus))),
						},
					};

					s.Ammo -= ammoConsumption;
				}
			}
		}

		SortieCostModel supplyCost = SupplyCost(fleetsBefore.Fleet, fleetsAfter.Fleet);
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
}
