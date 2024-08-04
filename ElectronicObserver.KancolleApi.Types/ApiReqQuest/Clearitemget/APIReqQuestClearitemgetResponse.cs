using ElectronicObserver.KancolleApi.Types.ApiReqQuest.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqQuest.Clearitemget;

public class ApiReqQuestClearitemgetResponse
{
	/// <summary>
	/// was null when devs messed up api responses
	/// </summary>
	[JsonPropertyName("api_bounus")]
	public List<ApiBounus>? ApiBounus { get; set; } = [];

	[JsonPropertyName("api_bounus_count")]
	public int ApiBounusCount { get; set; }

	[JsonPropertyName("api_material")]
	public List<int> ApiMaterial { get; set; } = [];
}
