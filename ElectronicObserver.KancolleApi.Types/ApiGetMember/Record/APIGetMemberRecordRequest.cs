namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Record;

public class ApiGetMemberRecordRequest
{
	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
