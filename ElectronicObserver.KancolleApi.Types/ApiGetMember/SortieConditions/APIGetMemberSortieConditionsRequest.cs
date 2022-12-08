namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.SortieConditions;

public class ApiGetMemberSortieConditionsRequest
{
	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;

}
