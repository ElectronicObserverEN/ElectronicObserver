using System.Collections.Generic;
using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types.Serialization.FitBonus;

namespace ElectronicObserver.Utility.ElectronicObserverApi.Models;

public record FitBonusIssueModel : DataIssueModel
{
	[JsonPropertyName("expected")] public FitBonusValue ExpectedBonus { get; set; } = new();

	[JsonPropertyName("actual")] public FitBonusValue ActualBonus { get; set; } = new();

	[JsonPropertyName("ship")] public ShipModel Ship { get; set; } = new();

	[JsonPropertyName("equipments")] public List<EquipmentModel> Equipments { get; set; } = new();
}
