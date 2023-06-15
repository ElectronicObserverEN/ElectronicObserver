namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.PictureBook;

public class ApiGetMemberPictureBookRequest
{
	[JsonPropertyName("api_no")]
	public string ApiNo { get; set; } = default!;

	[JsonPropertyName("api_type")]
	public string ApiType { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = default!;
}
