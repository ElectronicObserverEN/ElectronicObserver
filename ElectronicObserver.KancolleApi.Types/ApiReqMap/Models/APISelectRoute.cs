namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiSelectRoute
{
	[JsonPropertyName("api_select_cells")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiSelectCells { get; set; } = new();
}
