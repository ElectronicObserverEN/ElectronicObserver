namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed record AirBaseAirDefenseShipExportModel
{
	public required int? Id { get; init; }
	public required string? Name { get; init; }
	public required int? Level { get; init; }
	public required string? Hp { get; init; }
	public required string? Equipment1Name { get; init; }
	public required string? Equipment2Name { get; init; }
	public required string? Equipment3Name { get; init; }
	public required string? Equipment4Name { get; init; }
	public required string? Equipment5Name { get; init; }
}
