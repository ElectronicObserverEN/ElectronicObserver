namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiAirFire
{
	[JsonPropertyName("api_idx")]
	public int ApiIdx { get; set; }

	[JsonPropertyName("api_kind")]
	public int ApiKind { get; set; }

	/// <summary>
	/// AACI displayed equipment ids.
	/// </summary>
	[JsonPropertyName("api_use_items")]
	public List<int> ApiUseItems { get; set; } = new();
}
