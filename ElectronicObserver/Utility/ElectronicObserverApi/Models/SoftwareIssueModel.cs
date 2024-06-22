using System.Text.Json.Serialization;

namespace ElectronicObserver.Utility.ElectronicObserverApi.Models;

public class SoftwareIssueModel
{
	[JsonPropertyName("softwareVersion")] public string SoftwareVersion { get; set; } = "";

	[JsonPropertyName("exception")] public required SoftwareExceptionModel Exception { get; set; }
}
