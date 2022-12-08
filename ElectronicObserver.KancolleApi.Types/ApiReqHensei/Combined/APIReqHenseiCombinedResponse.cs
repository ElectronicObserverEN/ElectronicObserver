namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.Combined;

public class ApiReqHenseiCombinedResponse
{
	[JsonPropertyName("api_combined")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCombined { get; set; } = default!;
}
