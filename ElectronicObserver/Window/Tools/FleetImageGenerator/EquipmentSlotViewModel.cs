using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;

namespace ElectronicObserver.Window.Tools.FleetImageGenerator;

public class EquipmentSlotViewModel : ObservableObject
{
	private KCDatabase Db { get; }

	private IEquipmentData? Model { get; }

	public int SlotSize { get; set; }
	public EquipmentId Id { get; set; }
	public EquipmentIconType IconType { get; set; }
	public string Name { get; set; }
	public int Level { get; set; }
	public int AircraftLevel { get; set; }
	public bool IsAircraft { get; set; }

	public EquipmentSlotViewModel(IEquipmentData? equipment, int slotSize)
	{
		Db = KCDatabase.Instance;

		Model = equipment;

		SlotSize = slotSize;
		Id = equipment?.EquipmentId ?? EquipmentId.Unknown;
		IconType = equipment?.MasterEquipment.IconTypeTyped ?? EquipmentIconType.Nothing;
		Name = equipment?.MasterEquipment.Name switch
		{
			string s => Db.Translation.Equipment.Name(s),
			_ => FleetImageGeneratorResources.Empty,
		};
		Level = equipment?.Level ?? 0;
		AircraftLevel = equipment?.AircraftLevel ?? 0;
		IsAircraft = equipment?.MasterEquipment.IsAircraft ?? false;
	}
}
