using System.Collections.Generic;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public sealed record SortieItemsExportModel
{
	public required string? SmokerFlag { get; init; }
	public required int SmokerType { get; init; }
	public required string? SupplyShip { get; init; }
	public required string? GivenShip { get; init; }
	public required int? UseNum { get; init;}
	public required List<int>? ApiCombatRation { get; init; }
	public required List<int>? ApiCombatRationCombined { get; init; }
}
