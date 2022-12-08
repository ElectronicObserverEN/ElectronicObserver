namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiFlavorInfo
{
	[JsonPropertyName("api_boss_ship_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiBossShipId { get; set; } = default!;

	[JsonPropertyName("api_class_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiClassName { get; set; } = default!;

	[JsonPropertyName("api_data")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiData { get; set; } = default!;

	[JsonPropertyName("api_message")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiMessage { get; set; } = default!;

	[JsonPropertyName("api_pos_x")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiPosX { get; set; } = default!;

	[JsonPropertyName("api_pos_y")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiPosY { get; set; } = default!;

	[JsonPropertyName("api_ship_name")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiShipName { get; set; } = default!;

	[JsonPropertyName("api_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiType { get; set; } = default!;

	[JsonPropertyName("api_voice_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVoiceId { get; set; } = default!;
}
