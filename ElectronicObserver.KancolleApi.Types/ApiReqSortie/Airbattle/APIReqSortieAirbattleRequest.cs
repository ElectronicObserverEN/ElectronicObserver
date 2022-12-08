namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Airbattle;

public class ApiReqSortieAirbattleRequest
{
	[JsonPropertyName("api_formation")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiFormation { get; set; } = default!;

	[JsonPropertyName("api_recovery_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiRecoveryType { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
