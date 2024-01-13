using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ElectronicObserver.Database;
using ElectronicObserver.Database.DataMigration;
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

public class SortieCostViewModel
{
	private ElectronicObserverContext Db { get; }
	private ToolService ToolService { get; }

	private SortieRecord Model { get; }
	private SortieDetailViewModel? SortieDetails { get; set; }

	public DateTime Time { get; }
	public int World { get; }
	public int Map { get; }

	private int SortieFleetId { get; }
	private bool IsCombinedFleet { get; }
	private int NodeSupportFleetId { get; }
	private int BossSupportFleetId { get; }

	private List<IFleetData?> FleetsBeforeSortie { get; }
	private List<IFleetData?>? FleetsAfterSortie { get; }
	private List<IBaseAirCorpsData> AirBases { get; }

	public SortieCostModel SortieFleetSupplyCost { get; }
	public SortieCostModel SortieFleetRepairCost { get; }
	public SortieCostModel NodeSupportSupplyCost { get; }
	public SortieCostModel BossSupportSupplyCost { get; }
	public SortieCostModel TotalSupplyCost { get; }
	public SortieCostModel TotalRepairCost { get; }
	public SortieCostModel TotalAirBaseSortieCost { get; }
	public SortieCostModel TotalAirBaseSupplyCost { get; }
	public SortieCostModel TotalCost { get; }

	public SortieCostViewModel(ElectronicObserverContext db, ToolService toolService,
		SortieRecordMigrationService sortieRecordMigrationService, SortieRecordViewModel sortie)
	{
		Db = db;
		ToolService = toolService;

		Model = sortie.Model;
		Time = sortie.SortieStart.ToUniversalTime();
		World = sortie.World;
		Map = sortie.Map;

		SortieFleetId = sortie.Model.FleetData.FleetId;
		IsCombinedFleet = sortie.Model.FleetData.CombinedFlag > 0;
		NodeSupportFleetId = sortie.Model.FleetData.NodeSupportFleetId;
		BossSupportFleetId = sortie.Model.FleetData.BossSupportFleetId;

		sortieRecordMigrationService.Migrate(db, sortie.Model).Wait();

		FleetsBeforeSortie = sortie.Model.FleetData.MakeFleets();
		FleetsAfterSortie = sortie.Model.FleetAfterSortieData.MakeFleets();
		AirBases = sortie.Model.FleetData.AirBases
			.Select(a => a.MakeAirBase())
			.ToList();

		if (FleetsAfterSortie is not null)
		{
			SortieFleetSupplyCost = SupplyCost(FleetsBeforeSortie[SortieFleetId - 1], FleetsAfterSortie[SortieFleetId - 1]);
			SortieFleetRepairCost = RepairCost(FleetsBeforeSortie[SortieFleetId - 1], FleetsAfterSortie[SortieFleetId - 1]);

			if (IsCombinedFleet)
			{
				SortieFleetSupplyCost += SupplyCost(FleetsBeforeSortie[1], FleetsAfterSortie[1]);
				SortieFleetRepairCost += RepairCost(FleetsBeforeSortie[1], FleetsAfterSortie[1]);
			}

			NodeSupportSupplyCost = NodeSupportFleetId switch
			{
				<= 0 => new(),
				int id => SupplyCost(FleetsBeforeSortie[id - 1], FleetsAfterSortie[id - 1]),
			};

			BossSupportSupplyCost = BossSupportFleetId switch
			{
				<= 0 => new(),
				int id => SupplyCost(FleetsBeforeSortie[id - 1], FleetsAfterSortie[id - 1]),
			};
		}
		else
		{
			SortieFleetSupplyCost = CalculateSupplyCost(Db, Model);
			SortieFleetRepairCost = CalculateRepairCost(Db, Model);
			NodeSupportSupplyCost ??= new();
			BossSupportSupplyCost ??= new();
		}

		TotalSupplyCost = SortieFleetSupplyCost + NodeSupportSupplyCost + BossSupportSupplyCost;
		TotalRepairCost = SortieFleetRepairCost;

		TotalAirBaseSortieCost = AirBases
			.Where(a => a.ActionKind is AirBaseActionKind.Mission)
			.Select(AirBaseSortieCost)
			.Aggregate(new SortieCostModel(), (a, b) => a + b);

		TotalAirBaseSupplyCost = AirBaseSupplyCost();

		TotalCost = TotalSupplyCost + TotalRepairCost + TotalAirBaseSortieCost + TotalAirBaseSupplyCost;
	}

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
					.Where(a => a.AttackKind is DayAttackKind.SpecialYamato2Ships)
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
					DayAttackKind? attack = specialAttacks
						.FirstOrDefault(a => a.Attacker.ShipID == s.ShipID)?.AttackKind;

					double specialAttackBonus = (attack, enemyFleetType, hasSecondBattle) switch
					{
						(DayAttackKind.SpecialNagato, FleetType.Single, _) => ammoConsumptionModifier / 2,
						(DayAttackKind.SpecialNagato, _, true) => 0,
						(DayAttackKind.SpecialNagato, _, _) => ammoConsumptionModifier / 2,

						(DayAttackKind.SpecialYamato2Ships, _, _) => 0.12,

						_ => 0,
					};

					s.Ammo -= node.Happening switch
					{
						ApiHappening => (int)Math.Max(1, Math.Floor(s.Ammo * ammoConsumptionModifier)),
						_ => (attack, enemyFleetType) switch
						{
							(DayAttackKind.SpecialNagato, FleetType.Single) => (int)Math.Max(1, Math.Ceiling(s.AmmoMax * (ammoConsumptionModifier + specialAttackBonus))),
							(DayAttackKind.SpecialNagato, _) => (int)Math.Max(1, Math.Floor(s.AmmoMax * ammoConsumptionModifier) + Math.Floor(s.AmmoMax * specialAttackBonus)),
							_ => (int)Math.Max(1, Math.Floor(s.AmmoMax * (ammoConsumptionModifier + specialAttackBonus))),
						},
					};
				}

				if (hasSecondBattle)
				{
					s.Ammo -= (int)Math.Max(1, Math.Ceiling(s.AmmoMax * ammoConsumptionModifier / 2));
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

	private SortieCostModel CalculateRepairCost(ElectronicObserverContext db, SortieRecord model)
	{
		if (model.CalculatedSortieCost.SortieFleetRepairCost is not null)
		{
			return model.CalculatedSortieCost.SortieFleetRepairCost;
		}

		SortieDetails ??= ToolService.GenerateSortieDetailViewModel(db, model);

		if (SortieDetails is null) return new();

		BattleFleets fleetsBefore = SortieDetails.FleetsBeforeSortie;
		BattleFleets fleetsAfter = fleetsBefore.Clone();

		foreach (BattleNode battleNode in SortieDetails.Nodes.OfType<BattleNode>())
		{
			foreach ((IShipData? before, IShipData? after) in fleetsAfter.Fleet.MembersWithoutEscaped
				.Zip(battleNode.LastBattle.FleetsAfterBattle.Fleet.MembersWithoutEscaped))
			{
				if (before is not ShipDataMock ship) continue;
				if (after is null) continue;

				ship.HPCurrent = after.HPCurrent;
			}
		}

		model.CalculatedSortieCost.SortieFleetRepairCost = RepairCost(fleetsBefore.Fleet, fleetsAfter.Fleet);

		db.Sorties.Update(model);
		db.SaveChanges();

		return model.CalculatedSortieCost.SortieFleetRepairCost;
	}

	private static SortieCostModel RepairCost(IFleetData? before, IFleetData? after) => (before, after) switch
	{
		(not null, not null) => before.MembersInstance
			.Zip(after.MembersInstance, RepairCost)
			.Aggregate(new SortieCostModel(), (a, b) => a + b),

		_ => new(),
	};

	private static SortieCostModel RepairCost(IShipData? before, IShipData? after) => (before, after) switch
	{
		(not null, not null) => RepairCost(before, before.HPCurrent - after.HPCurrent),

		_ => new(),
	};

	private static SortieCostModel RepairCost(IShipData ship, int damage) => new()
	{
		Fuel = (int)(ship.MasterShip.Fuel * 0.032 * damage),
		Steel = (int)(ship.MasterShip.Fuel * 0.06 * damage),
	};

	private static SortieCostModel AirBaseSortieCost(IBaseAirCorpsData airBase)
		=> airBase.Squadrons.Values
			.Where(s => s.EquipmentInstance is not null)
			.Select(s => new SortieCostModel
			{
				Fuel = GetAirBasePlaneCostCategory(s.EquipmentInstance!) switch
				{
					AirBasePlaneCostCategory.AirBaseAttacker => (int)Math.Ceiling(1.5 * s.AircraftCurrent),
					AirBasePlaneCostCategory.LargePlane => 2 * s.AircraftCurrent,
					AirBasePlaneCostCategory.Other => s.AircraftCurrent,

					_ => throw new NotImplementedException(),
				},
				Ammo = GetAirBasePlaneCostCategory(s.EquipmentInstance!) switch
				{
					AirBasePlaneCostCategory.AirBaseAttacker => (int)(0.7 * s.AircraftCurrent),
					AirBasePlaneCostCategory.LargePlane => 2 * s.AircraftCurrent,
					AirBasePlaneCostCategory.Other => (int)Math.Ceiling(0.6 * s.AircraftCurrent),

					_ => throw new NotImplementedException(),
				},
			})
			.Aggregate(new SortieCostModel(), (a, b) => a + b);

	private static AirBasePlaneCostCategory GetAirBasePlaneCostCategory(IEquipmentData equip)
		=> equip.MasterEquipment.CategoryType switch
		{
			EquipmentTypes.LandBasedAttacker => AirBasePlaneCostCategory.AirBaseAttacker,
			EquipmentTypes.HeavyBomber => AirBasePlaneCostCategory.LargePlane,
			_ => AirBasePlaneCostCategory.Other,
		};

	private SortieCostModel AirBaseSupplyCost()
	{
		if (TryGetAirBaseState(Db, Time) is List<ApiAirBase> airBaseState)
		{
			if (TryGetCostFromState(AirBases, airBaseState) is SortieCostModel sortieCost)
			{
				return sortieCost;
			}
		}

		// todo: get plane shotdown from battle details
		return new();
	}

	private static List<ApiAirBase>? TryGetAirBaseState(ElectronicObserverContext db, DateTime sortieStart)
	{
		ApiFile? a = db.ApiFiles
			.Where(f => f.TimeStamp > sortieStart)
			.Where(f => f.ApiFileType == ApiFileType.Response)
			.Where(f => f.Name.Contains("api_get_member/mapinfo"))
			.OrderBy(f => f.Id)
			.FirstOrDefault();

		if (a is null) return null;

		try
		{
			ApiResponse<ApiGetMemberMapinfoResponse>? response = JsonSerializer
				.Deserialize<ApiResponse<ApiGetMemberMapinfoResponse>>(a.Content);

			return response?.ApiData.ApiAirBase;
		}
		catch
		{
			// todo: log?
		}

		return null;
	}

	private static SortieCostModel? TryGetCostFromState(List<IBaseAirCorpsData> airBases, List<ApiAirBase> airBaseStates)
	{
		if (!AreIdentical(airBases, airBaseStates)) return null;

		if (airBases.Count is 0) return new();

		int world = airBases[0].MapAreaID;

		int aircraftLost = airBases
			.Zip(airBaseStates.Where(s => s.ApiAreaId == world), (ab, s) => (AirBase: ab, State: s))
			.SelectMany(t => t.AirBase.Squadrons.Values.Zip(t.State.ApiPlaneInfo, (sq, s) => (Squadron: sq, State: s)))
			.Sum(t => t.State.ApiCount switch
			{
				// null when no plane was added in the ab slot
				int count => t.Squadron.AircraftCurrent - count,
				null => 0,
			});

		return new()
		{
			Fuel = 3 * aircraftLost,
			Ammo = 5 * aircraftLost,
		};
	}

	private static bool AreIdentical(List<IBaseAirCorpsData> airBases, List<ApiAirBase> airBaseStates)
	{
		if (airBases.Count is 0) return true;

		int world = airBases[0].MapAreaID;

		foreach ((IBaseAirCorpsData airBase, ApiAirBase state) in airBases
			.Zip(airBaseStates.Where(s => s.ApiAreaId == world)))
		{
			if (airBase.ActionKind != state.ApiActionKind) return false;

			foreach ((IBaseAirCorpsSquadron squadron, ApiPlaneInfo plane) in airBase.Squadrons.Values.Zip(state.ApiPlaneInfo))
			{
				if (squadron.EquipmentMasterID != plane.ApiSlotid)
				{
					// don't have that data currently
					// return false;
				}
			}
		}

		return true;
	}
}
