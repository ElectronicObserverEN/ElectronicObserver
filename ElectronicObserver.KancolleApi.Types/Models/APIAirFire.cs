namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiAirFire
{
	[JsonPropertyName("api_idx")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiIdx { get; set; } = default!;

	[JsonPropertyName("api_kind")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiKind { get; set; } = default!;

	[JsonPropertyName("api_use_items")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiUseItems { get; set; } = new();
}
