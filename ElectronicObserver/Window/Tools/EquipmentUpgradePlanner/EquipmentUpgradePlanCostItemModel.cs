namespace ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;

public class EquipmentUpgradePlanCostItemModel
{
	/// <summary>
	/// Id of the item
	/// </summary>
	public int Id { get; set; }

	/// <summary>
	/// Number of this equipment required
	/// </summary>
	public int Required { get; set; }

	public override bool Equals(object? other)
	{
		if (other is not EquipmentUpgradePlanCostItemModel otherCost) return false;

		if (Id != otherCost.Id) return false;
		if (Required != otherCost.Required) return false;

		return true;
	}

	public override int GetHashCode() => base.GetHashCode();
}
