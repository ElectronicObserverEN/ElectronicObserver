using ElectronicObserver.KancolleApi.Types.ApiReqQuest.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqQuest.Clearitemget;

public class ApiReqQuestClearitemgetResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_bounus")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiBounus> ApiBounus { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_bounus_count")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiBounusCount { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_material")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiMaterial { get; set; } = new();
}
