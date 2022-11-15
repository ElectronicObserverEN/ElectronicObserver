using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.ShipDeck;

public class ApiGetMemberShipDeckResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_deck_data")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiDeckDatum> ApiDeckData { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_data")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiShipDatum> ApiShipData { get; set; } = new();
}
