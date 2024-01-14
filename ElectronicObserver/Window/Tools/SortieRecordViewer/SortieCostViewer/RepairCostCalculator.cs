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
		List<IFleetData?>? fleetsAfterSortie, int sortieFleetId) =>
		fleetsAfterSortie switch
		{
			not null => RepairCost(fleetsBeforeSortie[sortieFleetId - 1], fleetsAfterSortie[sortieFleetId - 1]),
			_ => CalculateRepairCost(Db, Model),
		};

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
			if (fleetsAfter.Fleet.MembersWithoutEscaped is null) continue;
			if (battleNode.LastBattle.FleetsAfterBattle.Fleet.MembersWithoutEscaped is null) continue;

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
}
