namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Material;

public class ApiGetMemberMaterialResponse
{
	[JsonPropertyName("api_id")]
	public ApiGetMemberMaterialId ApiId { get; set; }

	[JsonPropertyName("api_member_id")]
	public int ApiMemberId { get; set; }

	[JsonPropertyName("api_value")]
	public int ApiValue { get; set; }
}
