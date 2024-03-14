using System;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed record CommonDataExportModel
{
	public required int No { get; set; }
	public required DateTime Date { get; init; }
	public required string World { get; init; }
	public required string Square { get; init; }
	public required string Sortie { get; init; }
	public required string? Rank { get; init; }
	public required string? EnemyFleet { get; init; }
	public required int? AdmiralLevel { get; init; }
	public required string PlayerFormation { get; init; }
	public required string EnemyFormation { get; init; }
	public required string? PlayerSearch { get; init; }
	public required string? EnemySearch { get; init; }
	public required string? AirState { get; init; }
	public required string Engagement { get; init; }
	public required string? PlayerContact { get; init; }
	public required string? EnemyContact { get; init; }
	public required int? PlayerFlare { get; init; }
	public required int? EnemyFlare { get; init; }
}
