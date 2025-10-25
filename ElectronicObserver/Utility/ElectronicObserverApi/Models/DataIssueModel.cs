using System.Text.Json.Serialization;

namespace ElectronicObserver.Utility.ElectronicObserverApi.Models;

public abstract record DataIssueModel
{
	[JsonPropertyName("software_data_source")] public string SoftwareDataSource { get; set; } = "";

	[JsonPropertyName("software_version")] public string SoftwareVersion { get; set; } = "";

	[JsonPropertyName("data_version")] public int DataVersion { get; set; }
}
