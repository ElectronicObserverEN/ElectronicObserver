﻿using System.Collections.Generic;
using System.Text.Json.Serialization;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;
using ElectronicObserverTypes;

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
}
