using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.SortieConditions;

public class ApiGetMemberSortieConditionsResponse
{
	[JsonPropertyName("api_war")]
	[Required]
	public ApiWar ApiWar { get; set; } = new();
}
