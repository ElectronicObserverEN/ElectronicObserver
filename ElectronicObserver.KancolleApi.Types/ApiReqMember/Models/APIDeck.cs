namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiDeck
{
	[JsonPropertyName("api_ships")]
	[Required]
	public List<ApiShip> ApiShips { get; set; } = new();
}
