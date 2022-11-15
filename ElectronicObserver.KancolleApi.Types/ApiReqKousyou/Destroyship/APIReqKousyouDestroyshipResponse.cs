namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Destroyship;

public class ApiReqKousyouDestroyshipResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_material")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiMaterial { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_unset_list")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public IDictionary<string, List<int>> ApiUnsetList { get; set; } = new Dictionary<string, List<int>>();
}
