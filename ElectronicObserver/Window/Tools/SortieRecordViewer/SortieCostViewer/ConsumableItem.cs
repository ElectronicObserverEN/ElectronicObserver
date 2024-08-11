using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;

public class ConsumableItem(IEquipmentData equipment, int count)
{
	public IEquipmentData Equipment { get; } = equipment;
	public EquipmentId Id { get; } = equipment.EquipmentId;
	public int Count { get; } = count;
}
