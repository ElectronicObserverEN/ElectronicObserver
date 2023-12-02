namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed record EquipmentExportModel
{
	public required string? Name { get; init; }
	public required int? Level { get; init; }
	public required int? AircraftLevel { get; init; }
	public required int? Aircraft { get; init; }
	public required int? AircraftAfterBattle { get; init; }
}
