using System;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;

public class EquipmentUpgradeImprovementCostItemDetail : IEquatable<EquipmentUpgradeImprovementCostItemDetail>
{
	/// <summary>
	/// Id of the item
	/// </summary>
	[JsonPropertyName("id")]
	public int Id { get; set; }

	/// <summary>
	/// Number of this equipment required
	/// </summary>
	[JsonPropertyName("eq_count")]
	public int Count { get; set; }

	/// <inheritdoc />
	public bool Equals(EquipmentUpgradeImprovementCostItemDetail? other)
	{
		if (other is null)
		{
			return false;
		}

		if (ReferenceEquals(this, other))
		{
			return true;
		}

		return Id == other.Id && Count == other.Count;
	}

	/// <inheritdoc />
	public override bool Equals(object? obj)
	{
		if (obj is null)
		{
			return false;
		}

		if (ReferenceEquals(this, obj))
		{
			return true;
		}

		if (obj.GetType() != GetType())
		{
			return false;
		}

		return Equals((EquipmentUpgradeImprovementCostItemDetail)obj);
	}

	/// <inheritdoc />
	public override int GetHashCode()
	{
		return HashCode.Combine(Id, Count);
	}
}
