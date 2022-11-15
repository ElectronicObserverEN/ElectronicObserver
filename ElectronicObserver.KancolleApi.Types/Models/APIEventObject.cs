namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiEventObject
{
	[System.Text.Json.Serialization.JsonPropertyName("api_m_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMFlag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_m_flag2")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiMFlag2 { get; set; } = default!;
}
