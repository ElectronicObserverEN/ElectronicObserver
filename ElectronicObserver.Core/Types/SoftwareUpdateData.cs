using System;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types;

public class SoftwareUpdateData
{
	[JsonPropertyName("bld_date")]
	[JsonConverter(typeof(CsvDateConverter))]
	public DateTime BuildDate { get; set; }

	[JsonPropertyName("ver")]
	public string AppVersion { get; set; } = "0.0.0.0";

	[JsonPropertyName("url")]
	public string AppDownloadUrl { get; set; } = "";

	[JsonPropertyName("ApiServer")]
	public string AppApiServerUrl { get; set; } = "";

	[JsonPropertyName("nodes")]
	public int Destination { get; set; }

	[JsonPropertyName("QuestTrackers")]
	public int QuestTrackers { get; set; }

	[JsonPropertyName("QuestsMetadata")]
	public int QuestsMetadata { get; set; }

	[JsonPropertyName("Locks")]
	public int EventLocks { get; set; }

	[JsonPropertyName("FitBonuses")]
	public int FitBonuses { get; set; }

	[JsonPropertyName("EquipmentUpgrades")]
	public int EquipmentUpgrades { get; set; }

	[JsonPropertyName("MaintStart")]
	[JsonConverter(typeof(CsvDateConverter))]
	public DateTime MaintenanceStart { get; set; }

	[JsonPropertyName("MaintEnd")]
	[JsonConverter(typeof(CsvNullableDateConverter))]
	public DateTime? MaintenanceEnd { get; set; }

	[JsonPropertyName("MaintInfoLink")]
	public string MaintenanceInformationLink { get; set; } = "";

	[JsonPropertyName("MaintEventState")]
	public MaintenanceState EventState { get; set; }
}
