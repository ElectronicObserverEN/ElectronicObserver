namespace ElectronicObserver.KancolleApi.Types.ApiReqPractice.Models;

public class ApiEnemyInfo
{
	[System.Text.Json.Serialization.JsonPropertyName("api_deck_name")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiDeckName { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_level")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiLevel { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_rank")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiRank { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_user_name")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiUserName { get; set; } = default!;
}
