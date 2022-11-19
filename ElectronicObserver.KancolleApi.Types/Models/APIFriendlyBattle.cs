namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiFriendlyBattle
{
	[JsonPropertyName("api_flare_pos")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiFlarePos { get; set; } = new();

	[JsonPropertyName("api_hougeki")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiHougeki ApiHougeki { get; set; } = new();
}
