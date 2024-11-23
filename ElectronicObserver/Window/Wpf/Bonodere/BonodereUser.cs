using System.Text.Json.Serialization;

namespace ElectronicObserver.Window.Wpf.Bonodere;

public class BonodereUser
{
	[JsonPropertyName("_id")]
	public required string Id { get; set; }

	[JsonPropertyName("info")]
	public required BonodereUserInfo UserInfo { get; set; }
}
