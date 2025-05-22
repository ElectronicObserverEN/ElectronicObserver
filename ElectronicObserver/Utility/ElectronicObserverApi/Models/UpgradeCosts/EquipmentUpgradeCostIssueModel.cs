using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Utility.ElectronicObserverApi.Models.UpgradeCosts;

public class EquipmentUpgradeCostIssueModel
{
	[JsonPropertyName("software_version")] public string SoftwareVersion { get; set; } = "";

	[JsonPropertyName("data_version")] public int DataVersion { get; set; }

	[JsonPropertyName("expected")] public EquipmentUpgradeCostDetailIssueModel Expected { get; set; } = new();

	[JsonPropertyName("actual")] public EquipmentUpgradeCostDetailIssueModel Actual { get; set; } = new();

	[JsonPropertyName("helperId")] public ShipId HelperId { get; set; }

	[JsonPropertyName("equipmentId")] public EquipmentId EquipmentId { get; set; }

	[JsonPropertyName("upgradeStage")] public UpgradeStage UpgradeStage { get; set; }
}
