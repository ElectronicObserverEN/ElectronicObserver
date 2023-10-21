using CsvHelper.Configuration;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class AntiAirCutInExportMap : ClassMap<AntiAirCutInExportModel>
{
	public AntiAirCutInExportMap()
	{
		Map(m => m.Ship).Name(CsvExportResources.AntiAirCutInShip);
		Map(m => m.Id).Name(CsvExportResources.AntiAirCutInId);
		Map(m => m.DisplayedEquipment1).Name(CsvExportResources.DisplayedEquipment1);
		Map(m => m.DisplayedEquipment2).Name(CsvExportResources.DisplayedEquipment2);
		Map(m => m.DisplayedEquipment3).Name(CsvExportResources.DisplayedEquipment3);
	}
}
