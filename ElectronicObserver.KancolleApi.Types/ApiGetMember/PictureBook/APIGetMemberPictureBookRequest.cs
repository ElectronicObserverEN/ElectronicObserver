namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.PictureBook;

public class ApiGetMemberPictureBookRequest
{
	[JsonPropertyName("api_no")]
	public string ApiNo { get; set; } = "";

	[JsonPropertyName("api_type")]
	public string ApiType { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";
}
