using System;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

// all required
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
public record CommonDataExportModel
{
	public int No { get; init; }
	public DateTime Date { get; init; }
	public string World { get; init; }
	public string Square { get; init; }
	public string Sortie { get; init; }
	public string? Rank { get; init; }
	public string? EnemyFleet { get; init; }
	public int? AdmiralLevel { get; init; }
	public string PlayerFormation { get; init; }
	public string EnemyFormation { get; init; }
	public string? PlayerSearch { get; init; }
	public string? EnemySearch { get; init; }
	public string? AirState { get; init; }
	public string Engagement { get; init; }
	public string? PlayerContact { get; init; }
	public string? EnemyContact { get; init; }
	public int? PlayerFlare { get; init; }
	public int? EnemyFlare { get; init; }
}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
