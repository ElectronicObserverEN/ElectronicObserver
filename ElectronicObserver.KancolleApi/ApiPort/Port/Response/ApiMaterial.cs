using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.ApiPort.Port.Response;

public class ApiMaterial
{

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_member_id")]
	public int ApiMemberId { get; set; }

	[JsonPropertyName("api_value")]
	public int ApiValue { get; set; }

}