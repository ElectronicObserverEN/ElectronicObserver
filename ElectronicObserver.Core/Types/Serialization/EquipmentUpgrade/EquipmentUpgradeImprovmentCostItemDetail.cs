using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;

public class EquipmentUpgradeImprovementCostItemDetail
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
}
