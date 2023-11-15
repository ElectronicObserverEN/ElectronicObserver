using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.ExpeditionRecordViewer;

public class ExpeditionItem(UseItemId id, int count)
{
	public UseItemId UseItemId { get; } = id;
	public int Count { get; set; } = count;
}
