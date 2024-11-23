using System.Text.Json.Serialization;

namespace ElectronicObserver.Window.Wpf.Bonodere;

public class BonodereLoginResponse
{
	[JsonPropertyName("token")]
	public required string Token { get; set; }

	[JsonPropertyName("user")] 
	public required BonodereUser User { get; set; }
}
