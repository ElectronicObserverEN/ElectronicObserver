namespace ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;

public record EquipmentExportModel
{
	public string? Name { get; init; }
	public int? Level { get; init; }
	public int? AircraftLevel { get; init; }
	public int? Aircraft { get; init; }
	public int? AircraftAfterBattle { get; init; }
}
