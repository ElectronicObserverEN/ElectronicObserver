namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.Combined;

public class ApiReqHenseiCombinedResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_combined")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCombined { get; set; } = default!;
}
