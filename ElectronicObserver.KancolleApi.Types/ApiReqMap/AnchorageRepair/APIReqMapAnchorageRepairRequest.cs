namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.AnchorageRepair;

public class ApiReqMapAnchorageRepairRequest
{
	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
