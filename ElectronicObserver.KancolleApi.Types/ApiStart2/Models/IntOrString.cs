namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class IntOrString
{
	[JsonPropertyName("api_int_value")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiIntValue { get; set; } = default!;

	[JsonPropertyName("api_string_value")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiStringValue { get; set; } = default!;
}
