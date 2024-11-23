using System.Text.Json.Serialization;

namespace ElectronicObserver.Window.Wpf.Bonodere;

public class BonodereUserInfo
{
	[JsonPropertyName("name")]
	public required string Username { get; set; }
}
