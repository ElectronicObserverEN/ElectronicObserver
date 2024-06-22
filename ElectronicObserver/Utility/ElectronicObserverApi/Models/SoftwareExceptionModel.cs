using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Utility.ElectronicObserverApi.Models;

public record SoftwareExceptionModel
{
	[JsonPropertyName("type")] public string Type { get; set; } = "";

	[JsonPropertyName("message")] public string Message { get; set; } = "";

	[JsonPropertyName("stackTrace")] public string StackTrace { get; set; } = "";

	[JsonPropertyName("innerExceptions")] public List<SoftwareExceptionModel> InnerExceptions { get; set; } = [];
}
