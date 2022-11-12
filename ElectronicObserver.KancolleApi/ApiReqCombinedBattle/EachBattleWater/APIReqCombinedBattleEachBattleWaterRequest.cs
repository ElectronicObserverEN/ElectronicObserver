namespace ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.EachBattleWater;

public class ApiReqCombinedBattleEachBattleWaterRequest
{
	[System.Text.Json.Serialization.JsonPropertyName("api_formation")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiFormation { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_recovery_type")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiRecoveryType { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_start")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public string? ApiStart { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_verno")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
