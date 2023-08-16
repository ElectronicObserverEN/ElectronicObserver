using CsvHelper.Configuration;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class EquipmentExportMap : ClassMap<EquipmentExportModel>
{
	public EquipmentExportMap()
	{
		Map(m => m.Name).Name(CsvExportResources.Name);
		Map(m => m.Level).Name(CsvExportResources.EquipmentLevel);
		Map(m => m.AircraftLevel).Name(CsvExportResources.AircraftLevel);
		Map(m => m.Aircraft).Name(CsvExportResources.Aircraft);
	}
}
