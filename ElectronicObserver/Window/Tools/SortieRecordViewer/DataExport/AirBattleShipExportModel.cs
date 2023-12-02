namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed record AirBattleShipExportModel
{
	public required int? Id { get; init; }
	public required string? Name { get; init; }
	public required int? Level { get; init; }
}
