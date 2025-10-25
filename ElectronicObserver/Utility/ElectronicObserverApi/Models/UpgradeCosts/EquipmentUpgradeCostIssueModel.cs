using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Utility.ElectronicObserverApi.Models.UpgradeCosts;

public record EquipmentUpgradeCostIssueModel : DataIssueModel
{
	[JsonPropertyName("expected")] public EquipmentUpgradeCostDetailIssueModel Expected { get; set; } = new();

	[JsonPropertyName("actual")] public EquipmentUpgradeCostDetailIssueModel Actual { get; set; } = new();

	[JsonPropertyName("helperId")] public ShipId HelperId { get; set; }

	[JsonPropertyName("equipmentId")] public EquipmentId EquipmentId { get; set; }

	[JsonPropertyName("upgradeLevel")] public UpgradeLevel UpgradeLevel { get; set; }
}
