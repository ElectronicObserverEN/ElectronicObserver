namespace ElectronicObserver.KancolleApi.Types.ApiStart2.GetData;

public class ApiStart2GetDataRequest
{
	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
