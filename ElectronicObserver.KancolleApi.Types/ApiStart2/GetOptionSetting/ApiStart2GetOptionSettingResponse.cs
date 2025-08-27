using ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiStart2.GetOptionSetting;

public class ApiStart2GetOptionSettingResponse
{
	[JsonPropertyName("api_skin_id")]
	public int ApiSkinId { get; set; }

	[JsonPropertyName("api_volume_setting")]
	public ApiVolumeSetting ApiVolumeSetting { get; set; } = new();
}
