namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class AirBattleShipExportMap : CsvExportMap<AirBattleShipExportModel>
{
	public AirBattleShipExportMap()
	{
		Map(m => m.Id).Name(() => CsvExportResources.Id);
		Map(m => m.Name).Name(() => CsvExportResources.Name);
		Map(m => m.Level).Name(() => CsvExportResources.ShipLevel);
	}
}
