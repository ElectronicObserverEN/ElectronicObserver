namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiFlavorInfo
{
	[JsonPropertyName("api_boss_ship_id")]
	public string ApiBossShipId { get; set; } = "";

	[JsonPropertyName("api_class_name")]
	public string ApiClassName { get; set; } = "";

	[JsonPropertyName("api_data")]
	public string ApiData { get; set; } = "";

	[JsonPropertyName("api_message")]
	public string ApiMessage { get; set; } = "";

	[JsonPropertyName("api_pos_x")]
	public string ApiPosX { get; set; } = "";

	[JsonPropertyName("api_pos_y")]
	public string ApiPosY { get; set; } = "";

	[JsonPropertyName("api_ship_name")]
	public string ApiShipName { get; set; } = "";

	[JsonPropertyName("api_type")]
	public string ApiType { get; set; } = "";

	[JsonPropertyName("api_voice_id")]
	public string ApiVoiceId { get; set; } = "";
}
