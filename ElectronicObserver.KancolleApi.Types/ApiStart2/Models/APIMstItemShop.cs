namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstItemShop
{
	[System.Text.Json.Serialization.JsonPropertyName("api_cabinet_1")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiCabinet1 { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_cabinet_2")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiCabinet2 { get; set; } = new();
}
