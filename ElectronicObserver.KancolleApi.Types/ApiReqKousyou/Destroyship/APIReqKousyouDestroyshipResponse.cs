namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Destroyship;

public class ApiReqKousyouDestroyshipResponse
{
	[JsonPropertyName("api_material")]
	public List<int> ApiMaterial { get; set; } = new();

	[JsonPropertyName("api_unset_list")]
	public IDictionary<string, List<int>> ApiUnsetList { get; set; } = new Dictionary<string, List<int>>();
}
