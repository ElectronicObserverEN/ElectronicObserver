namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Mapinfo;

public class ApiGetMemberMapinfoRequest
{
	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
