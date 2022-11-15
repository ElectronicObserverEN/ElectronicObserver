namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiWar
{
	[System.Text.Json.Serialization.JsonPropertyName("api_lose")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiLose { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_rate")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiRate { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_win")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiWin { get; set; } = default!;
}
