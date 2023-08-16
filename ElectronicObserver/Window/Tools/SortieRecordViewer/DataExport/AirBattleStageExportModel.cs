namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public record AirBattleStageExportModel
{
	public int PlayerAircraftTotal { get; init; }
	public int PlayerAircraftLost { get; init; }
	public int EnemyAircraftTotal { get; init; }
	public int EnemyAircraftLost { get; init; }
}
