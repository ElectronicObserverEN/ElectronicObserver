using CsvHelper.Configuration;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class RedEquipmentExportMap : ClassMap<EquipmentExportModel>
{
	public RedEquipmentExportMap()
	{
		Map(m => m.Name).Name(CsvExportResources.Name);
		Map(m => m.Level).Name(CsvExportResources.EquipmentLevel);
		Map(m => m.AircraftLevel).Name(CsvExportResources.AircraftLevel);
		Map(m => m.Aircraft).Name(CsvExportResources.Aircraft);
		Map(m => m.AircraftAfterBattle).Name(CsvExportResources.AircraftAfterBattle);
	}
}
