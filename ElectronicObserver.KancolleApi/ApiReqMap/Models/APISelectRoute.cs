namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiSelectRoute
{
	[System.Text.Json.Serialization.JsonPropertyName("api_select_cells")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiSelectCells { get; set; } = new();
}
