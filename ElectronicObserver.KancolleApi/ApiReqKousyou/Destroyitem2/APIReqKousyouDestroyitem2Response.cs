namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Destroyitem2;

public class ApiReqKousyouDestroyitem2Response
{
	[System.Text.Json.Serialization.JsonPropertyName("api_get_material")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiGetMaterial { get; set; } = new();
}
