using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Models;

public class ApiSlotItemEx
{
	[JsonPropertyName("api_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiId { get; set; }

	[JsonPropertyName("api_level")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public int ApiLevel { get; set; }
}
