namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiAirBaseDatumElement
{
	[JsonPropertyName("api_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiCount { get; set; }

	[JsonPropertyName("api_mst_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public int ApiMstId { get; set; }
}
