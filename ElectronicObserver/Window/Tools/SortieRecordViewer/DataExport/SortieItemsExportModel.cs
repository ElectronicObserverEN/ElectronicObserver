using System.Collections.Generic;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public record SortieItemsExportModel
{
	public string? SmokerFlag { get; init; }
	public int SmokerType { get; init; }
	public string? SupplyShip { get; init; }
	public string? GivenShip { get; init; }
	public int? UseNum { get; init;}
	public List<int>? ApiCombatRation { get; init; }
	public List<int>? ApiCombatRationCombined { get; init; }
}
