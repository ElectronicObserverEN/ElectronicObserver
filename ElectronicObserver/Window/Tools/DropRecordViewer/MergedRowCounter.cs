using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.DropRecordViewer;

public class MergedRowCounter
{
	public int Count { get; set; }
	public int CountS { get; set; }
	public int CountA { get; set; }
	public int CountB { get; set; }
	public ShipId ShipId { get; init; }
}
