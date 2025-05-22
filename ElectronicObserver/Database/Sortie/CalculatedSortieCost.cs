using System.Collections.Generic;
using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;

namespace ElectronicObserver.Database.Sortie;

public class CalculatedSortieCost
{
	[JsonPropertyName("SortieFleetSupplyCost")]
	public SortieCostModel? SortieFleetSupplyCost { get; set; }

	[JsonPropertyName("SortieFleetRepairCost")]
	public SortieCostModel? SortieFleetRepairCost { get; set; }

	[JsonPropertyName("NodeSupportSupplyCost")]
	public SortieCostModel? NodeSupportSupplyCost { get; set; }

	[JsonPropertyName("BossSupportSupplyCost")]
	public SortieCostModel? BossSupportSupplyCost { get; set; }

	[JsonPropertyName("TotalAirBaseSortieCost")]
	public SortieCostModel? TotalAirBaseSortieCost { get; set; }

	[JsonPropertyName("TotalAirBaseSupplyCost")]
	public SortieCostModel? TotalAirBaseSupplyCost { get; set; }

	[JsonPropertyName("DamageStateCounts")]
	public Dictionary<DamageState, int>? DamageStateCounts { get; set; }

	[JsonPropertyName("ConsumedItems")]
	public List<ConsumableItem>? ConsumedItems { get; set; }
}
