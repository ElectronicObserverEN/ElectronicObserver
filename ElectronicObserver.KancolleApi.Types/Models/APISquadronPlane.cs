namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiSquadronPlane
{
	[JsonPropertyName("api_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCount { get; set; } = default!;

	[JsonPropertyName("api_mst_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMstId { get; set; } = default!;
}
