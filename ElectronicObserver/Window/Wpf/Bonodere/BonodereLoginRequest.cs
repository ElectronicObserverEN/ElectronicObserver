using System.Security;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Window.Wpf.Bonodere;

public class BonodereLoginRequest
{
	[JsonPropertyName("key")]
	public required string Key { get; set; }

	[JsonPropertyName("password")]
	public required SecureString Password { get; set; }

	[JsonPropertyName("duration")]
	public required int Duration { get; set; }
}
