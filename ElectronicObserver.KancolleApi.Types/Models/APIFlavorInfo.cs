namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiFlavorInfo
{
	[JsonPropertyName("api_boss_ship_id")]
	public string ApiBossShipId { get; set; } = default!;

	[JsonPropertyName("api_class_name")]
	public string ApiClassName { get; set; } = default!;

	[JsonPropertyName("api_data")]
	public string ApiData { get; set; } = default!;

	[JsonPropertyName("api_message")]
	public string ApiMessage { get; set; } = default!;

	[JsonPropertyName("api_pos_x")]
	public string ApiPosX { get; set; } = default!;

	[JsonPropertyName("api_pos_y")]
	public string ApiPosY { get; set; } = default!;

	[JsonPropertyName("api_ship_name")]
	public string ApiShipName { get; set; } = default!;

	[JsonPropertyName("api_type")]
	public string ApiType { get; set; } = default!;

	[JsonPropertyName("api_voice_id")]
	public string ApiVoiceId { get; set; } = default!;
}
