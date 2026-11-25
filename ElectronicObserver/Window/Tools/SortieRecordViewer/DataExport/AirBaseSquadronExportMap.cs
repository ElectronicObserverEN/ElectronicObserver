namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class AirBaseSquadronExportMap : CsvExportMap<AirBaseSquadronExportModel>
{
	public AirBaseSquadronExportMap()
	{
		Map(m => m.Name).Name(() => CsvExportResources.Name);
		Map(m => m.Level).Name(() => CsvExportResources.EquipmentLevel);
		Map(m => m.AircraftLevel).Name(() => CsvExportResources.AircraftLevel);
		Map(m => m.Condition).Name(() => CsvExportResources.Condition);
		Map(m => m.Aircraft).Name(() => CsvExportResources.Aircraft2);
	}
}
