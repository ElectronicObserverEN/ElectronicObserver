namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class IntOrString
{
	[System.Text.Json.Serialization.JsonPropertyName("api_int_value")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiIntValue { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_string_value")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiStringValue { get; set; } = default!;
}
