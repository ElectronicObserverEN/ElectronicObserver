namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiMaterial
{
	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_member_id")]
	public int ApiMemberId { get; set; } = default!;

	[JsonPropertyName("api_value")]
	public int ApiValue { get; set; } = default!;
}
