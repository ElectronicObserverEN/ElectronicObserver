namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstSlotitemEquiptype
{
	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_name")]
	public string ApiName { get; set; } = "";

	[JsonPropertyName("api_show_flg")]
	public int ApiShowFlg { get; set; }
}
