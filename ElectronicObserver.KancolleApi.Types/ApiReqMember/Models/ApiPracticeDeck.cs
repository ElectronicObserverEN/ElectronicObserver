namespace ElectronicObserver.KancolleApi.Types.ApiReqMember.Models;

public class ApiPracticeDeck
{
	[JsonPropertyName("api_ships")]
	public List<ApiPracticeShip> ApiShips { get; set; } = new();
}
