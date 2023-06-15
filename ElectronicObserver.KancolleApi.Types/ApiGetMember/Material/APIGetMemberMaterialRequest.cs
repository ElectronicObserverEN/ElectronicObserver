namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Material;

public class ApiGetMemberMaterialRequest
{
	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
