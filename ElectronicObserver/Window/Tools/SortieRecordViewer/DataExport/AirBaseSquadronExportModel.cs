namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed record AirBaseSquadronExportModel
{
	public required string? Name { get; init; }
	public required int? Level { get; init; }
	public required int? AircraftLevel { get; init; }
	public required string? Condition { get; init; }
	public required int? Aircraft { get; init; }
}
