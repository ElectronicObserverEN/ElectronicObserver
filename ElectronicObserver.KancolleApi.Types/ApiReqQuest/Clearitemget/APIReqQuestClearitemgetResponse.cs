using ElectronicObserver.KancolleApi.Types.ApiReqQuest.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqQuest.Clearitemget;

public class ApiReqQuestClearitemgetResponse
{
	[JsonPropertyName("api_bounus")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiBounus> ApiBounus { get; set; } = new();

	[JsonPropertyName("api_bounus_count")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiBounusCount { get; set; } = default!;

	[JsonPropertyName("api_material")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiMaterial { get; set; } = new();
}
