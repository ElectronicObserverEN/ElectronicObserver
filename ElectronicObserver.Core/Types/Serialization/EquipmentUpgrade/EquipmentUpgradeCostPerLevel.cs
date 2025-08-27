using System;
using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;

public sealed class EquipmentUpgradeCostPerLevel(UpgradeLevel level, EquipmentUpgradeImprovementCostDetail baseDetail) : IEquatable<EquipmentUpgradeCostPerLevel>
{
	public UpgradeLevel UpgradeLevel { get; set; } = level;

	public int DevmatCost { get; set; } = baseDetail.DevmatCost;

	public int SliderDevmatCost { get; set; } = baseDetail.SliderDevmatCost;

	public int ImproveMatCost { get; set; } = baseDetail.ImproveMatCost;

	public int SliderImproveMatCost { get; set; } = baseDetail.SliderImproveMatCost;

	public List<EquipmentUpgradeImprovementCostItemDetail> EquipmentDetail { get; set; } = baseDetail.EquipmentDetail.Select(item => new EquipmentUpgradeImprovementCostItemDetail()
	{
		Count = item.Count,
		Id = item.Id,
	}).ToList();

	public List<EquipmentUpgradeImprovementCostItemDetail> ConsumableDetail { get; set; } = baseDetail.ConsumableDetail.Select(item => new EquipmentUpgradeImprovementCostItemDetail()
	{
		Count = item.Count,
		Id = item.Id,
	}).ToList();

	/// <inheritdoc />
	public bool Equals(EquipmentUpgradeCostPerLevel? other)
	{
		if (other is null)
		{
			return false;
		}

		if (ReferenceEquals(this, other))
		{
			return true;
		}

		if (DevmatCost != other.DevmatCost) return false;
		if (SliderDevmatCost != other.SliderDevmatCost) return false;
		if (ImproveMatCost != other.ImproveMatCost) return false;
		if (SliderImproveMatCost != other.SliderImproveMatCost) return false;

		if (!EquipmentDetail.SequenceEqual(other.EquipmentDetail)) return false;
		if (!ConsumableDetail.SequenceEqual(other.ConsumableDetail)) return false;

		return true;
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

		return Equals((EquipmentUpgradeCostPerLevel)obj);
	}

	/// <inheritdoc />
	public override int GetHashCode()
	{
		return HashCode.Combine(DevmatCost, SliderDevmatCost, ImproveMatCost, SliderImproveMatCost, EquipmentDetail, ConsumableDetail);
	}
}
