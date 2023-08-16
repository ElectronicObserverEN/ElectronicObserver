namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public record AntiAirCutInExportModel
{
	public int? Ship { get; init; }
	public int? Id { get; init; }
	public string? DisplayedEquipment1 { get; init; }
	public string? DisplayedEquipment2 { get; init; }
	public string? DisplayedEquipment3 { get; init; }
}
