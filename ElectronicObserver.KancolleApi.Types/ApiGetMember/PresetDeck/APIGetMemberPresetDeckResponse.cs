using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.PresetDeck;

public class ApiGetMemberPresetDeckResponse
{
	[JsonPropertyName("api_deck")]
	public IDictionary<string, ApiDeck> ApiDeck { get; set; } = new Dictionary<string, ApiDeck>();

	[JsonPropertyName("api_max_num")]
	public int ApiMaxNum { get; set; }
}
