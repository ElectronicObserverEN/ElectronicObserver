namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiEventObject
{
	[JsonPropertyName("api_m_flag")]
	public int ApiMFlag { get; set; }

	[JsonPropertyName("api_m_flag2")]
	public int? ApiMFlag2 { get; set; }
}
