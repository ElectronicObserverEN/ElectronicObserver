namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Material;

public class ApiGetMemberMaterialRequest
{
	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
