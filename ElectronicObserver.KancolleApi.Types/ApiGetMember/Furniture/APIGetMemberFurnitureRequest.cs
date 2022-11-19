namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Furniture;

public class ApiGetMemberFurnitureRequest
{
	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
