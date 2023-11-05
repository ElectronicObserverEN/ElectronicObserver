namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiFurniture
{
	[JsonPropertyName("api_furniture_id")]
	public int ApiFurnitureId { get; set; }

	[JsonPropertyName("api_furniture_no")]
	public int ApiFurnitureNo { get; set; }

	[JsonPropertyName("api_furniture_type")]
	public int ApiFurnitureType { get; set; }

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }
}
