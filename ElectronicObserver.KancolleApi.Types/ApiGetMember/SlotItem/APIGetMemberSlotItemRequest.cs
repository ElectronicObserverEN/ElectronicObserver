namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.SlotItem;

public class ApiGetMemberSlotItemRequest
{
	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
