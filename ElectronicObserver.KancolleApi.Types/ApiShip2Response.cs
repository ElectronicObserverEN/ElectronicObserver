using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types;

public class ApiShip2Response
{
	[JsonPropertyName("api_data")]
	public List<ApiShip> ApiData { get; set; } = [];

	[JsonPropertyName("api_data_deck")]
	public List<FleetDataDto> ApiDataDeck { get; set; } = [];

	[JsonPropertyName("api_result")]
	public int ApiResult { get; set; }

	[JsonPropertyName("api_result_msg")]
	public string ApiResultMessage { get; set; } = "";
}
