namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiSupportInfo
{
	[JsonPropertyName("api_support_airatack")]
	public ApiSupportAiratack? ApiSupportAiratack { get; set; }

	[JsonPropertyName("api_support_hourai")]
	public ApiSupportHourai? ApiSupportHourai { get; set; } = new();
}
