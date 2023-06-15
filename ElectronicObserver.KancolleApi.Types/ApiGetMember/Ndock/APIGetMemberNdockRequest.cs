namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Ndock;

public class ApiGetMemberNdockRequest
{
	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
