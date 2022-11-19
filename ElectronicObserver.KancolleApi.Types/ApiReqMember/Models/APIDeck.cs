namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiDeck
{
	[JsonPropertyName("api_ships")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiShip> ApiShips { get; set; } = new();
}
