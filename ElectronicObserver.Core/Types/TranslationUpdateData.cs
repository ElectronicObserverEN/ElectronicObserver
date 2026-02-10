using System.Text.Json.Serialization;

namespace ElectronicObserver.Core.Types;

public class TranslationUpdateData
{
	[JsonPropertyName("equipment")]
	public string Equipment { get; set; } = "";

	[JsonPropertyName("expedition")]
	public string Expedition { get; set; } = "";

	[JsonPropertyName("operation")]
	public string Operation { get; set; } = "";

	[JsonPropertyName("quest")]
	public string Quest { get; set; } = "";

	[JsonPropertyName("ship")]
	public string Ship { get; set; } = "";

	[JsonPropertyName("Locks")]
	public int LockTranslations { get; set; }
}
