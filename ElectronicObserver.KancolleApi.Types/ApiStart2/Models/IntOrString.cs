namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class IntOrString
{
	[JsonPropertyName("api_int_value")]
	public int ApiIntValue { get; set; }

	[JsonPropertyName("api_string_value")]
	public string ApiStringValue { get; set; } = "";
}
