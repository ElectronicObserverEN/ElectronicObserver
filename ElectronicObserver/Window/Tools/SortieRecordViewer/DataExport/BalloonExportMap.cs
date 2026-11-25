namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed class BalloonExportMap : CsvExportMap<BalloonExportModel>
{
	public BalloonExportMap()
	{
		Map(m => m.BalloonCell).Name(() => CsvExportResources.BalloonCell);
		Map(m => m.BalloonCount).Name(() => CsvExportResources.PlayerBalloons);
		Map(m => m.EnemyBalloonCount).Name(() => CsvExportResources.EnemyBalloons);
	}
}
