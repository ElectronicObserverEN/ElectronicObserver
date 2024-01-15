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

		if (mainShipsBefore is null) return new();
		if (mainShipsAfter is null) return new();

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
			.Aggregate(new SortieCostModel(), (a, b) => a + b);

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

		List<IShipData?> shipsBefore = fleetsBefore.SortieShips();
		List<IShipData?> shipsAfter = fleetsAfter.SortieShips();

		List<IShipData?>? membersAfterFinalBattle = SortieDetails.Nodes
			.OfType<BattleNode>()
			.Select(n => n.LastBattle.FleetsAfterBattle)
			.LastOrDefault()
			?.SortieShips();

		if (membersAfterFinalBattle is null) return new();

		foreach ((IShipData? before, IShipData? after) in shipsAfter.Zip(membersAfterFinalBattle))
		{
			if (before is not ShipDataMock ship) continue;
			if (after is null) continue;

			ship.HPCurrent = after.HPCurrent;
		}

		model.CalculatedSortieCost.SortieFleetRepairCost = RepairCost(shipsBefore, shipsAfter);

		db.Sorties.Update(model);
		db.SaveChanges();

		return model.CalculatedSortieCost.SortieFleetRepairCost;
	}
}
