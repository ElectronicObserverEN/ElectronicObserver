using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.ApiReqMap.Start.Response;

public class ApiSelectRoute
{

	[JsonPropertyName("api_select_cells")]
	public IEnumerable<int> ApiSelectCells { get; set; }

}