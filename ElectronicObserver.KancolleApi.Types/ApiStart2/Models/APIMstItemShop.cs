namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstItemShop
{
	[JsonPropertyName("api_cabinet_1")]
	[Required]
	public List<int> ApiCabinet1 { get; set; } = new();

	[JsonPropertyName("api_cabinet_2")]
	[Required]
	public List<int> ApiCabinet2 { get; set; } = new();
}
