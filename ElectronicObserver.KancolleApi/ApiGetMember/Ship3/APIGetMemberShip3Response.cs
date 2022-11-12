using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Ship3;

public class ApiGetMemberShip3Response
{
	[System.Text.Json.Serialization.JsonPropertyName("api_deck_data")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiDeckDatum> ApiDeckData { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_data")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiShipDatum> ApiShipData { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_slot_data")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public IDictionary<string, List<int>> ApiSlotData { get; set; } = new Dictionary<string, List<int>>();
}
