namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiAirBaseDatumElement
{
	[System.Text.Json.Serialization.JsonPropertyName("api_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiCount { get; set; }

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiMstId { get; set; }
}
