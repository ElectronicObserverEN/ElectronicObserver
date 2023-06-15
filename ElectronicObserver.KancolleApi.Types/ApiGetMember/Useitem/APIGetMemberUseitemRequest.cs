namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Useitem;

public class ApiGetMemberUseitemRequest
{
	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
