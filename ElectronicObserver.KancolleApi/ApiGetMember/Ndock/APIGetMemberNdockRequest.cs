namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Ndock;

public class ApiGetMemberNdockRequest
{
	[System.Text.Json.Serialization.JsonPropertyName("api_verno")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
