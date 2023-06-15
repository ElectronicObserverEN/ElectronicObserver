namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiFlavorInfo
{
	[JsonPropertyName("api_boss_ship_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiBossShipId { get; set; } = default!;

	[JsonPropertyName("api_class_name")]
	[Required(AllowEmptyStrings = true)]
	public string ApiClassName { get; set; } = default!;

	[JsonPropertyName("api_data")]
	[Required(AllowEmptyStrings = true)]
	public string ApiData { get; set; } = default!;

	[JsonPropertyName("api_message")]
	[Required(AllowEmptyStrings = true)]
	public string ApiMessage { get; set; } = default!;

	[JsonPropertyName("api_pos_x")]
	[Required(AllowEmptyStrings = true)]
	public string ApiPosX { get; set; } = default!;

	[JsonPropertyName("api_pos_y")]
	[Required(AllowEmptyStrings = true)]
	public string ApiPosY { get; set; } = default!;

	[JsonPropertyName("api_ship_name")]
	[Required(AllowEmptyStrings = true)]
	public string ApiShipName { get; set; } = default!;

	[JsonPropertyName("api_type")]
	[Required(AllowEmptyStrings = true)]
	public string ApiType { get; set; } = default!;

	[JsonPropertyName("api_voice_id")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVoiceId { get; set; } = default!;
}
