using System.Text.Json.Serialization;

namespace ElectronicObserver.Window.Wpf.Bonodere;

public class BonodereUserDataResponse
{
	[JsonPropertyName("data")] 
	public required BonodereUser User { get; set; }
}
