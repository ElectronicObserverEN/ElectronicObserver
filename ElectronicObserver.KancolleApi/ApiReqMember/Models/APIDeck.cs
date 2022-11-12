namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiDeck
{
	[System.Text.Json.Serialization.JsonPropertyName("api_ships")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiShip> ApiShips { get; set; } = new();
}
