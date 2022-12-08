namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiEventObject
{
	[JsonPropertyName("api_m_flag")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMFlag { get; set; } = default!;

	[JsonPropertyName("api_m_flag2")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiMFlag2 { get; set; } = default!;
}
