namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.PresetSelect;

public class ApiReqHenseiPresetSelectResponse
{
	[JsonPropertyName("api_flagship")]
	[Required(AllowEmptyStrings = true)]
	public string ApiFlagship { get; set; } = default!;

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_member_id")]
	public int ApiMemberId { get; set; } = default!;

	[JsonPropertyName("api_mission")]
	[Required]
	public List<int> ApiMission { get; set; } = new();

	[JsonPropertyName("api_name")]
	[Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_name_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiNameId { get; set; } = default!;

	[JsonPropertyName("api_ship")]
	[Required]
	public List<int> ApiShip { get; set; } = new();
}
