using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.PictureBook;

public class ApiGetMemberPictureBookResponse
{
	[JsonPropertyName("api_list")]
	[Required]
	public List<ApiPictureBookList> ApiList { get; set; } = new();
}
