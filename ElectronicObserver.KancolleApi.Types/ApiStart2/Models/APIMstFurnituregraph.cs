namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstFurnituregraph
{
	[JsonPropertyName("api_filename")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiFilename { get; set; } = default!;

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiNo { get; set; } = default!;

	[JsonPropertyName("api_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiType { get; set; } = default!;

	[JsonPropertyName("api_version")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVersion { get; set; } = default!;
}
