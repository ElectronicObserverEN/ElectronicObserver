namespace ElectronicObserver.KancolleApi.Types.ApiReqRanking.Mxltvkpyuklh;

public class ApiReqRankingMxltvkpyuklhRequest
{
	[JsonPropertyName("api_token")]
	[Required(AllowEmptyStrings = true)]
	public string ApiToken { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;

	[JsonPropertyName("api_ranking")]
	[Required(AllowEmptyStrings = true)]
	public string ApiRanking { get; set; } = default!;
}
