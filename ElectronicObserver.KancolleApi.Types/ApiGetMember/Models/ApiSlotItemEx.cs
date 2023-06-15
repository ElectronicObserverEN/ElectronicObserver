using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Models;

public class ApiSlotItemEx
{
	[JsonPropertyName("api_id")]
	[Required]
	public int ApiId { get; set; }

	[JsonPropertyName("api_level")]
	[Required]
	public int ApiLevel { get; set; }
}
