using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.ApiPort.Port.Response;

public class ApiEventObject
{

	[JsonPropertyName("api_m_flag")]
	public int ApiMFlag { get; set; }

	[JsonPropertyName("api_m_flag2")]
	public int ApiMFlag2 { get; set; }

}