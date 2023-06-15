namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Destroyship;

public class ApiReqKousyouDestroyshipResponse
{
	[JsonPropertyName("api_material")]
	[Required]
	public List<int> ApiMaterial { get; set; } = new();

	[JsonPropertyName("api_unset_list")]
	[Required]
	public IDictionary<string, List<int>> ApiUnsetList { get; set; } = new Dictionary<string, List<int>>();
}
