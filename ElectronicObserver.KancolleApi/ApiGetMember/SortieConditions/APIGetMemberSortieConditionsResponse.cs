using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.SortieConditions;

public class ApiGetMemberSortieConditionsResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_war")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiWar ApiWar { get; set; } = new();
}
