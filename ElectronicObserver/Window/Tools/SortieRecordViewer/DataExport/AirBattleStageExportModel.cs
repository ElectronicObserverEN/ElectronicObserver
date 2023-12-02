namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed record AirBattleStageExportModel
{
	public required int PlayerAircraftTotal { get; init; }
	public required int PlayerAircraftLost { get; init; }
	public required int EnemyAircraftTotal { get; init; }
	public required int EnemyAircraftLost { get; init; }
}
