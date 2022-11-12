namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.AnchorageRepair;

public class ApiReqMapAnchorageRepairRequest
{
	[System.Text.Json.Serialization.JsonPropertyName("api_verno")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
