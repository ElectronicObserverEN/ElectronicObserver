using System.Text.Json.Serialization;

namespace ElectronicObserver.Window.Wpf.Bonodere;

public class BonodereError
{
	[JsonPropertyName("message")]
	public string Message { get; set; } = "";

	[JsonPropertyName("code")]
	public int Code;
}
