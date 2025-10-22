using System;
using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Utility.ElectronicObserverApi.Models;

public record EquipmentModel
{
	[JsonPropertyName("equipmentId")] public EquipmentId EquipmentId { get; set; }

	[JsonPropertyName("level")] public UpgradeLevel Level { get; set; }

	public virtual bool Equals(EquipmentModel? other)
	{
		if (other is null) return false;

		if (other.EquipmentId != EquipmentId) return false;
		if (other.Level != Level) return false;

		return true;
	}

	public override int GetHashCode()
	{
		return HashCode.Combine((int)EquipmentId, (int)Level);
	}
}
