using System.Text.Json.Serialization;
using ElectronicObserverTypes;

namespace ElectronicObserver.Utility.ElectronicObserverApi.Models;

public class EquipmentModel
{
	[JsonPropertyName("equipmentId")] public EquipmentId EquipmentId { get; set; }

	[JsonPropertyName("level")] public UpgradeLevel Level { get; set; }
}
