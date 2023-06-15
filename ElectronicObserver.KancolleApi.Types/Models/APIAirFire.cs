namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiAirFire
{
	[JsonPropertyName("api_idx")]
	public int ApiIdx { get; set; } = default!;

	[JsonPropertyName("api_kind")]
	public int ApiKind { get; set; } = default!;

	[JsonPropertyName("api_use_items")]
	[Required]
	public List<int> ApiUseItems { get; set; } = new();
}
