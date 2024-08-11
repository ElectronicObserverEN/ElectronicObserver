using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Database;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Mocks;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;

public class RepairCostCalculator(ElectronicObserverContext db, ToolService toolService, SortieRecordViewModel sortie)
{
	private ElectronicObserverContext Db { get; } = db;
	private ToolService ToolService { get; } = toolService;

	private SortieRecord Model { get; } = sortie.Model;

	private SortieDetailViewModel? SortieDetails { get; set; }

	public SortieCostModel RepairCost(List<IFleetData?> fleetsBeforeSortie,
		List<IFleetData?>? fleetsAfterSortie, int sortieFleetId, bool isCombinedFleet)
	{
		if (fleetsAfterSortie is null)
		{
			return CalculateRepairCost(Db, Model);
		}

		IEnumerable<IShipData?>? mainShipsBefore = fleetsBeforeSortie[sortieFleetId - 1]?.MembersWithoutEscaped;
		IEnumerable<IShipData?>? mainShipsAfter = fleetsAfterSortie[sortieFleetId - 1]?.MembersWithoutEscaped;

		if (mainShipsBefore is null) return SortieCostModel.Zero;
		if (mainShipsAfter is null) return SortieCostModel.Zero;

		SortieCostModel cost = RepairCost(mainShipsBefore, mainShipsAfter);

		if (!isCombinedFleet) return cost;

		IEnumerable<IShipData?>? escortShipsBefore = fleetsBeforeSortie[1]?.MembersWithoutEscaped;
		IEnumerable<IShipData?>? escortShipsAfter = fleetsAfterSortie[1]?.MembersWithoutEscaped;

		if (escortShipsBefore is null) return cost;
		if (escortShipsAfter is null) return cost;

		return cost + RepairCost(escortShipsBefore, escortShipsAfter);
	}

	private static SortieCostModel RepairCost(IEnumerable<IShipData?> before, IEnumerable<IShipData?> after)
		=> before
			.Zip(after, RepairCost)
			.Sum();

	private static SortieCostModel RepairCost(IShipData? before, IShipData? after) => (before, after) switch
	{
		(not null, not null) => RepairCost(before, before.HPCurrent - after.HPCurrent),

		_ => SortieCostModel.Zero,
	};

	private static SortieCostModel RepairCost(IShipData ship, int damage) => new()
	{
		Fuel = (int)(ship.MasterShip.Fuel * 0.032 * damage),
		Steel = (int)(ship.MasterShip.Fuel * 0.06 * damage),
	};

	private SortieCostModel CalculateRepairCost(ElectronicObserverContext db, SortieRecord model)
	{
		if (model.CalculatedSortieCost.SortieFleetRepairCost is not null)
		{
			return model.CalculatedSortieCost.SortieFleetRepairCost;
		}

		SortieDetails ??= ToolService.GenerateSortieDetailViewModel(db, model);

		if (SortieDetails is null) return SortieCostModel.Zero;

		BattleFleets fleetsBefore = SortieDetails.FleetsBeforeSortie;
		BattleFleets fleetsAfter = fleetsBefore.Clone();

		List<IShipData?> shipsBefore = fleetsBefore.SortieShips();
		List<IShipData?>? shipsAfter = GetShipsAfter(SortieDetails, fleetsAfter);

		if (shipsAfter is null) return SortieCostModel.Zero;

		model.CalculatedSortieCost.SortieFleetRepairCost = RepairCost(shipsBefore, shipsAfter);

		db.Sorties.Update(model);
		db.SaveChanges();

		return model.CalculatedSortieCost.SortieFleetRepairCost;
	}

	public Dictionary<DamageState, int> DamageStateCounts(List<IFleetData?> fleetsBeforeSortie,
		List<IFleetData?>? fleetsAfterSortie, int sortieFleetId, bool isCombinedFleet)
	{
		Dictionary<DamageState, int> damageStateCounts = new()
		{
			{ DamageState.Light, 0 },
			{ DamageState.Medium, 0 },
			{ DamageState.Heavy, 0 },
		};

		if (fleetsAfterSortie is null)
		{
			return CalculateDamageStateCounts(damageStateCounts, Db, Model);
		}

		IEnumerable<IShipData?>? mainShipsBefore = fleetsBeforeSortie[sortieFleetId - 1]?.MembersWithoutEscaped;
		IEnumerable<IShipData?>? mainShipsAfter = fleetsAfterSortie[sortieFleetId - 1]?.MembersWithoutEscaped;

		if (mainShipsBefore is null) return damageStateCounts;
		if (mainShipsAfter is null) return damageStateCounts;

		damageStateCounts = DamageStateCounts(damageStateCounts, mainShipsBefore, mainShipsAfter);

		if (!isCombinedFleet) return damageStateCounts;

		IEnumerable<IShipData?>? escortShipsBefore = fleetsBeforeSortie[1]?.MembersWithoutEscaped;
		IEnumerable<IShipData?>? escortShipsAfter = fleetsAfterSortie[1]?.MembersWithoutEscaped;

		if (escortShipsBefore is null) return damageStateCounts;
		if (escortShipsAfter is null) return damageStateCounts;

		return DamageStateCounts(damageStateCounts, escortShipsBefore, escortShipsAfter);
	}

	private Dictionary<DamageState, int> CalculateDamageStateCounts(Dictionary<DamageState, int> damageStateCounts,
		ElectronicObserverContext db, SortieRecord model)
	{
		if (model.CalculatedSortieCost.DamageStateCounts is not null)
		{
			return model.CalculatedSortieCost.DamageStateCounts;
		}

		SortieDetails ??= ToolService.GenerateSortieDetailViewModel(db, model);

		if (SortieDetails is null) return damageStateCounts;

		BattleFleets fleetsBefore = SortieDetails.FleetsBeforeSortie;
		BattleFleets fleetsAfter = fleetsBefore.Clone();

		List<IShipData?> shipsBefore = fleetsBefore.SortieShips();
		List<IShipData?>? shipsAfter = GetShipsAfter(SortieDetails, fleetsAfter);

		if (shipsAfter is null) return damageStateCounts;

		model.CalculatedSortieCost.DamageStateCounts = DamageStateCounts(damageStateCounts,
			shipsBefore, shipsAfter);

		db.Sorties.Update(model);
		db.SaveChanges();

		return model.CalculatedSortieCost.DamageStateCounts;
	}

	private static Dictionary<DamageState, int> DamageStateCounts(Dictionary<DamageState, int> damageStateCounts,
		IEnumerable<IShipData?> shipsBefore, IEnumerable<IShipData?> shipsAfter)
	{
		foreach ((IShipData? before, IShipData? after) in shipsBefore.Zip(shipsAfter))
		{
			if (before is null) continue;
			if (after is null) continue;

			switch (before, after)
			{
				case ({ DamageState: DamageState.Healthy }, { DamageState: < DamageState.Healthy }):
					damageStateCounts[after.DamageState]++;
					continue;

				case ({ DamageState: >= DamageState.Light }, { DamageState: < DamageState.Light }):
					damageStateCounts[after.DamageState]++;
					continue;

				case ({ DamageState: >= DamageState.Medium }, { DamageState: < DamageState.Medium }):
					damageStateCounts[after.DamageState]++;
					continue;
			}
		}

		return damageStateCounts;
	}

	private static List<IShipData?>? GetShipsAfter(SortieDetailViewModel sortieDetails, BattleFleets fleetsAfter)
	{
		List<IShipData?> shipsAfter = fleetsAfter.SortieShips();

		List<IShipData?>? membersAfterFinalBattle = sortieDetails.Nodes
			.OfType<BattleNode>()
			.Select(n => n.LastBattle.FleetsAfterBattle)
			.LastOrDefault()
			?.SortieShips();

		if (membersAfterFinalBattle is null) return null;

		foreach ((IShipData? before, IShipData? after) in shipsAfter.Zip(membersAfterFinalBattle))
		{
			if (before is not ShipDataMock ship) continue;
			if (after is null) continue;

			ship.HPCurrent = after.HPCurrent;
		}

		return shipsAfter;
	}
}
