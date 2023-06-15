namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiMaterial
{
	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_member_id")]
	public int ApiMemberId { get; set; }

	[JsonPropertyName("api_value")]
	public int ApiValue { get; set; }
}
