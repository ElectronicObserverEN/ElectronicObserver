namespace ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;

public class ApiSelectRoute
{
	[JsonPropertyName("api_select_cells")]
	public List<int> ApiSelectCells { get; set; } = new();
}
