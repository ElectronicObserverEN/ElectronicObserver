namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class AirBaseExportMap : CsvExportMap<AirBaseExportModel>
{
	public AirBaseExportMap()
	{
		Map(m => m.Hp).Name(() => CsvExportResources.Hp);
		References<AirBaseSquadronExportMap, AirBaseSquadronExportModel>(s => s.Squadron1, m => m.Prefix(CsvExportResources.PrefixSquadron1));
		References<AirBaseSquadronExportMap, AirBaseSquadronExportModel>(s => s.Squadron2, m => m.Prefix(CsvExportResources.PrefixSquadron2));
		References<AirBaseSquadronExportMap, AirBaseSquadronExportModel>(s => s.Squadron3, m => m.Prefix(CsvExportResources.PrefixSquadron3));
		References<AirBaseSquadronExportMap, AirBaseSquadronExportModel>(s => s.Squadron4, m => m.Prefix(CsvExportResources.PrefixSquadron4));
	}
}
