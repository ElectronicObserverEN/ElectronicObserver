namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiSquadronPlane
{
	[System.Text.Json.Serialization.JsonPropertyName("api_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMstId { get; set; } = default!;
}
