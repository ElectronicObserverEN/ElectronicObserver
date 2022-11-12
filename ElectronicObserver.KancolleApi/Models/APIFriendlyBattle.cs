namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiFriendlyBattle
{
	[System.Text.Json.Serialization.JsonPropertyName("api_flare_pos")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFlarePos { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_hougeki")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiFriendlyBattleApiHougeki ApiHougeki { get; set; } = new();
}
