namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Destroyitem2;

public class ApiReqKousyouDestroyitem2Response
{
	[JsonPropertyName("api_get_material")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiGetMaterial { get; set; } = new();
}
