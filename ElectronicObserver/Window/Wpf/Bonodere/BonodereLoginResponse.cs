using System.Text.Json.Serialization;

namespace ElectronicObserver.Window.Wpf.Bonodere;

public class BonodereLoginResponse
{
	[JsonPropertyName("token")] 
	public required string Token { get; set; } = "";

	[JsonPropertyName("userId")] 
	public required string UserId { get; set; } = "";

	[JsonPropertyName("username")] 
	public required string Username { get; set; } = "";
}
