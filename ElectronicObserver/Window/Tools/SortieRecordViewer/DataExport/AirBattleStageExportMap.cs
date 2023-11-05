using CsvHelper.Configuration;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class AirBattleStageExportMap : ClassMap<AirBattleStageExportModel>
{
	public AirBattleStageExportMap()
	{
		Map(m => m.PlayerAircraftTotal).Name(CsvExportResources.PlayerAircraftTotal);
		Map(m => m.PlayerAircraftLost).Name(CsvExportResources.PlayerAircraftLost);
		Map(m => m.EnemyAircraftTotal).Name(CsvExportResources.EnemyAircraftTotal);
		Map(m => m.EnemyAircraftLost).Name(CsvExportResources.EnemyAircraftLost);
	}
}
