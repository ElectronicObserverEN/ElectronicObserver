namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Basic;

public class ApiGetMemberBasicRequest
{
	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
