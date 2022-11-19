namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.SlotItem;

public class ApiGetMemberSlotItemRequest
{
	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
