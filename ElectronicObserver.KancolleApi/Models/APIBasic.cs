namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiBasic
{
	[System.Text.Json.Serialization.JsonPropertyName("api_firstflag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiFirstflag { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_member_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMemberId { get; set; } = default!;
}
