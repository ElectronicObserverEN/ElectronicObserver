namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed record AirBaseExportModel
{
	public required string? Hp { get; init; }
	public required AirBaseSquadronExportModel Squadron1 { get; init; }
	public required AirBaseSquadronExportModel Squadron2 { get; init; }
	public required AirBaseSquadronExportModel Squadron3 { get; init; }
	public required AirBaseSquadronExportModel Squadron4 { get; init; }
}
