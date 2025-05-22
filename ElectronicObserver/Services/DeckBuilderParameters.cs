using ElectronicObserver.Core.Types;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;

namespace ElectronicObserver.Services;

public record DeckBuilderParameters
{
	public int HqLevel { get; init; }
	public IFleetData? Fleet1 { get; init; }
	public IFleetData? Fleet2 { get; init; }
	public IFleetData? Fleet3 { get; init; }
	public IFleetData? Fleet4 { get; init; }
	public IBaseAirCorpsData? AirBase1 { get; init; }
	public IBaseAirCorpsData? AirBase2 { get; init; }
	public IBaseAirCorpsData? AirBase3 { get; init; }
	public SortieDetailViewModel? SortieDetails { get; init; }
	public bool MaxAircraftLevelFleet { get; init; }
	public bool MaxAircraftLevelAirBase { get; init; }
}
