namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstSlotitemEquiptype
{
	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_name")]
	[Required(AllowEmptyStrings = true)]
	public string ApiName { get; set; } = default!;

	[JsonPropertyName("api_show_flg")]
	public int ApiShowFlg { get; set; } = default!;
}
