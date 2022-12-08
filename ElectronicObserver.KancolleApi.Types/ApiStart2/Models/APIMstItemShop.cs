namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstItemShop
{
	[JsonPropertyName("api_cabinet_1")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiCabinet1 { get; set; } = new();

	[JsonPropertyName("api_cabinet_2")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiCabinet2 { get; set; } = new();
}
