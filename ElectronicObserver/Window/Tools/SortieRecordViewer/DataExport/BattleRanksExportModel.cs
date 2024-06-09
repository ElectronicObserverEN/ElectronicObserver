using System;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed record BattleRanksExportModel
{
	public required DateTime Date { get; init; }
	public required string World { get; init; }
	public required string Square { get; init; }
	public required string ExpectedRank { get; init; }
	public required string ActualRank { get; init; }
}
