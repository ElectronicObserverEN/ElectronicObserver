namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class AirBaseAirDefenseShipExportMap : CsvExportMap<AirBaseAirDefenseShipExportModel>
{
	public AirBaseAirDefenseShipExportMap()
	{
		Map(m => m.Id).Name(() => CsvExportResources.Id);
		Map(m => m.Name).Name(() => CsvExportResources.Name);
		Map(m => m.Level).Name(() => CsvExportResources.ShipLevel);
		Map(m => m.Hp).Name(() => CsvExportResources.Hp);
		Map(m => m.Equipment1Name).Name(() => $"{CsvExportResources.PrefixEquipment1}{CsvExportResources.Name}");
		Map(m => m.Equipment2Name).Name(() => $"{CsvExportResources.PrefixEquipment2}{CsvExportResources.Name}");
		Map(m => m.Equipment3Name).Name(() => $"{CsvExportResources.PrefixEquipment3}{CsvExportResources.Name}");
		Map(m => m.Equipment4Name).Name(() => $"{CsvExportResources.PrefixEquipment4}{CsvExportResources.Name}");
		Map(m => m.Equipment5Name).Name(() => $"{CsvExportResources.PrefixEquipment5}{CsvExportResources.Name}");
	}
}
