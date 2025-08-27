using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.ExpChecker;

public class ASWEquipmentData
{
	public int ID { get; init; }
	public int ASW { get; init; }
	public string Name { get; init; } = "";
	public bool IsSonar { get; init; }
	public int Count { get; init; }
	public IEquipmentDataMaster? Equipment { get; init; }

	public override string ToString() => Name;
}