namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstMaparea
{
	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiType { get; set; } = default!;
}
