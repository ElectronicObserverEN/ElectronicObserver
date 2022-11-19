namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiSupportInfo
{
	[JsonPropertyName("api_support_airatack")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiSupportAiratack? ApiSupportAiratack { get; set; } = default!;

	[JsonPropertyName("api_support_hourai")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public ApiSupportHourai ApiSupportHourai { get; set; } = new();
}
