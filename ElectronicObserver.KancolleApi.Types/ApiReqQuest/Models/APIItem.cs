namespace ElectronicObserver.KancolleApi.Types.ApiReqQuest.Models;

public class ApiItem
{
	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiId { get; set; } = default!;

	[JsonPropertyName("api_id_from")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiIdFrom { get; set; } = default!;

	[JsonPropertyName("api_id_to")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiIdTo { get; set; } = default!;

	[JsonPropertyName("api_message")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiMessage { get; set; } = default!;

	[JsonPropertyName("api_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public string? ApiName { get; set; } = default!;
}
