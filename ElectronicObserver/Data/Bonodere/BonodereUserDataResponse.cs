using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.Bonodere;

public class BonodereUserDataResponse
{
	[JsonPropertyName("data")] 
	public required BonodereUser User { get; set; }
}
