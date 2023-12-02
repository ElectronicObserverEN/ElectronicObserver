namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed record AntiAirCutInExportModel
{
	public required int? Ship { get; init; }
	public required int? Id { get; init; }
	public required string? DisplayedEquipment1 { get; init; }
	public required string? DisplayedEquipment2 { get; init; }
	public required string? DisplayedEquipment3 { get; init; }
}
