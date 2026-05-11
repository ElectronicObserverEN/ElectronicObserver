using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Avalonia.Translation.Destination;

public class Destination : IIdentifiable
{
	public int ID { get; init; }
	public int MapAreaId { get; init; }
	public int MapInfoId { get; init; }
	public int CellId { get; init; }
	public required string PreviousCellDisplay { get; init; }
	public required string CellDisplay { get; init; }

	public string Display => $"{MapAreaId}-{MapInfoId}-{CellDisplay}";
}
