using System;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types;

public class SoftwareUpdateData
{
	[JsonPropertyName("bld_date")]
	public string BuildDateRaw
	{
		set => BuildDate = DateTimeHelper.CSVStringToTime(value);
		get => throw new NotSupportedException();
	}

	public DateTime BuildDate { get; private set; }

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
	public string MaintenanceStartRaw
	{
		set => MaintenanceStart = DateTimeHelper.CSVStringToTime(value);
		get => throw new NotSupportedException();
	}

	public DateTime MaintenanceStart { get; set; }

	[JsonPropertyName("MaintEnd")]
	public string? MaintenanceEndRaw
	{
		set => MaintenanceEnd = value switch
		{
			not null => DateTimeHelper.CSVStringToTime(value),
			_ => null,
		};
		get => throw new NotSupportedException();
	}

	public DateTime? MaintenanceEnd { get; set; }

	[JsonPropertyName("MaintInfoLink")]
	public string MaintenanceInformationLink { get; set; } = "";

	/// <summary>
	/// 1=event start, 2=event end, 3=regular maintenance
	/// </summary>
	[JsonPropertyName("MaintEventState")]
	public MaintenanceState EventState { get; set; }
}
