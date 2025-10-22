using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Database;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;

public class JetCostCalculator(ElectronicObserverContext db, ToolService toolService, SortieRecordViewModel sortie)
{
	private ElectronicObserverContext Db { get; } = db;
	private ToolService ToolService { get; } = toolService;

	private SortieRecord Model { get; } = sortie.Model;

	private SortieDetailViewModel? SortieDetails { get; set; }

	public SortieCostModel FleetJetCost() => FleetJetCost(Db, Model);
	
	public SortieCostModel AirBaseJetCost() => AirBaseJetCost(Db, Model);

	private SortieCostModel FleetJetCost(ElectronicObserverContext db, SortieRecord model)
	{
		if (model.CalculatedSortieCost.SortieFleetJetCost is not null)
		{
			return model.CalculatedSortieCost.SortieFleetJetCost;
		}

		SortieDetails ??= ToolService.GenerateSortieDetailViewModel(db, model);

		if (SortieDetails is null) return SortieCostModel.Zero;
		if (CalculateFleetJetCost(SortieDetails) is not SortieCostModel cost) return SortieCostModel.Zero;

		model.CalculatedSortieCost.SortieFleetJetCost = cost;

		db.Sorties.Update(model);
		db.SaveChanges();

		return model.CalculatedSortieCost.SortieFleetJetCost;
	}

	private static SortieCostModel? CalculateFleetJetCost(SortieDetailViewModel sortieDetails)
	{
		IEnumerable<BattleNode> jetPhaseBattles = sortieDetails.Nodes
			.OfType<BattleNode>()
			.Where(b => b.AllPhases.OfType<PhaseJetAirBattle>().Any());

		int steelCost = 0;

		foreach (BattleNode node in jetPhaseBattles)
		{
			List<int>? costs = node.FirstBattle.FleetsBeforeBattle.Fleet.MembersWithoutEscaped?
				.OfType<IShipData>()
				.SelectMany(s => s.AllSlotInstance.Zip(s.Aircraft, (e, a) => (Equipment: e, Aicraft: a)))
				.Select(s => JetSteelCost(s.Equipment?.MasterEquipment, s.Aicraft))
				.ToList();

			if (costs is null) return null;

			steelCost += costs.Sum();
		}

		return new() { Steel = steelCost };
	}

	private SortieCostModel AirBaseJetCost(ElectronicObserverContext db, SortieRecord model)
	{
		if (model.CalculatedSortieCost.AirBaseJetCost is not null)
		{
			return model.CalculatedSortieCost.AirBaseJetCost;
		}

		SortieDetails ??= ToolService.GenerateSortieDetailViewModel(db, model);

		if (SortieDetails is null) return SortieCostModel.Zero;

		model.CalculatedSortieCost.AirBaseJetCost = CalculateAirBaseJetCost(SortieDetails);

		db.Sorties.Update(model);
		db.SaveChanges();

		return model.CalculatedSortieCost.AirBaseJetCost;
	}

	private static SortieCostModel CalculateAirBaseJetCost(SortieDetailViewModel sortieDetails)
	{
		int steelCost = sortieDetails.Nodes
			.OfType<BattleNode>()
			.SelectMany(b => b.AllPhases.OfType<PhaseJetBaseAirAttack>())
			.SelectMany(p => p.Units)
			.SelectMany(u => u.Squadrons)
			.Select(s => JetSteelCost(s.Equipment, s.AircraftCount))
			.Sum();

		return new() { Steel = steelCost };
	}

	private static int JetSteelCost(IEquipmentDataMaster? equipment, int aircraft)
		=> (int)Math.Round(aircraft * JetCostMultiplier(equipment) * 0.2);

	private static double JetCostMultiplier(IEquipmentDataMaster? equipment) => equipment switch
	{
		{ CardType: EquipmentCardType.AllFlyingWingJetBomber } => equipment.AircraftCost * 1.2,
		{ CardType: EquipmentCardType.JetFightingBomber } => equipment.AircraftCost,

		_ => 0,
	};
}
