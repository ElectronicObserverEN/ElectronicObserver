namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiSupportAiratack
{
	[JsonPropertyName("api_deck_id")]
	public int ApiDeckId { get; set; }

	[JsonPropertyName("api_plane_from")]
	public List<List<int>?> ApiPlaneFrom { get; set; } = new();

	[JsonPropertyName("api_ship_id")]
	public List<int> ApiShipId { get; set; } = new();

	[JsonPropertyName("api_stage1")]
	public ApiStage1 ApiStage1 { get; set; } = new();

	[JsonPropertyName("api_stage2")]
	public ApiStage2Support ApiStage2 { get; set; } = new();

	[JsonPropertyName("api_stage3")]
	public ApiStage3Jet ApiStage3 { get; set; } = new();

	[JsonPropertyName("api_stage_flag")]
	public List<int> ApiStageFlag { get; set; } = new();
}
