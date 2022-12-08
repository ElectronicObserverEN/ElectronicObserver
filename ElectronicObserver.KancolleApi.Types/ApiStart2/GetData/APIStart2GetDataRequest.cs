namespace ElectronicObserver.KancolleApi.Types.ApiStart2.GetData;

public class ApiStart2GetDataRequest
{
	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
