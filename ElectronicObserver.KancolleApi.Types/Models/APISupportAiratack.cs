namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiSupportAiratack
{
	[JsonPropertyName("api_deck_id")]
	[Required]
	public int ApiDeckId { get; set; }

	[JsonPropertyName("api_plane_from")]
	[Required]
	public List<List<int>?> ApiPlaneFrom { get; set; } = new();

	[JsonPropertyName("api_ship_id")]
	[Required]
	public List<int> ApiShipId { get; set; } = new();

	[JsonPropertyName("api_stage1")]
	[Required]
	public ApiStage1 ApiStage1 { get; set; } = new();

	[JsonPropertyName("api_stage2")]
	[Required]
	public ApiStage2Support ApiStage2 { get; set; } = new();

	[JsonPropertyName("api_stage3")]
	[Required]
	public ApiStage3Jet ApiStage3 { get; set; } = new();

	[JsonPropertyName("api_stage_flag")]
	[Required]
	public List<int> ApiStageFlag { get; set; } = new();
}
