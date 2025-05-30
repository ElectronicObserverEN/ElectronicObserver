﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;

namespace ElectronicObserver.Database.DataMigration;

public class SortieRecordMigrationService(ToolService toolService)
{
	private ToolService ToolService { get; } = toolService;

	public async Task Migrate(ElectronicObserverContext db, SortieRecord record)
	{
		if (record.Version is 0)
		{
			await MigrateToVersion1(db, record);
		}
	}

	private async Task MigrateToVersion1(ElectronicObserverContext db, SortieRecord record)
	{
		SortieDetailViewModel? sortieDetails = ToolService.GenerateSortieDetailViewModel(db, record);

		List<IFleetData?>? computedFleetsBeforeSortie = sortieDetails?.Nodes
			.OfType<BattleNode>()
			.FirstOrDefault()
			?.FirstBattle.FleetsBeforeBattle.Fleets;

		if (computedFleetsBeforeSortie is null) return;

		FixHp(record.FleetData, computedFleetsBeforeSortie);

		if (record.FleetAfterSortieData is not null)
		{
			if (sortieDetails!.Fleets.Fleets is null) return;

			FixHp(record.FleetAfterSortieData, sortieDetails.Fleets.Fleets);
		}

		record.Version = 1;

		// need to manually call update because change tracker is off in sortie records
		// and because the hp change can't be detected by ef (would need to create a new SortieFleetData instance)
		db.Sorties.Update(record);
		await db.SaveChangesAsync();
	}

	private static void FixHp(SortieFleetData fleetsToFix, IEnumerable<IFleetData?> computedFleets)
	{
		foreach ((SortieFleet? dbFleet, IFleetData? computedFleet) in fleetsToFix.Fleets.Zip(computedFleets))
		{
			if (dbFleet is null) continue;
			if (computedFleet is null) continue;

			foreach ((SortieShip dbShip, IShipData? computedShip) in dbFleet.Ships.Zip(computedFleet.MembersInstance))
			{
				if (computedShip is null) continue;

				dbShip.Hp = computedShip.HPCurrent;
			}
		}
	}
}
