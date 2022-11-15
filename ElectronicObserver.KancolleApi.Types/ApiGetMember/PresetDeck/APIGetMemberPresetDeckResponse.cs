using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.PresetDeck;

public class ApiGetMemberPresetDeckResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_deck")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public IDictionary<string, ApiDeck> ApiDeck { get; set; } = new Dictionary<string, ApiDeck>();

	[System.Text.Json.Serialization.JsonPropertyName("api_max_num")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMaxNum { get; set; } = default!;
}
