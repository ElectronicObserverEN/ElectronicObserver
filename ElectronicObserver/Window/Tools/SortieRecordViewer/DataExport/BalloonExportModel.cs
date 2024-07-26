namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed record BalloonExportModel
{
	public required int? BalloonCell { get; init; }
	public required int BalloonCount { get; init; }
	public required int EnemyBalloonCount { get; init; }
}
