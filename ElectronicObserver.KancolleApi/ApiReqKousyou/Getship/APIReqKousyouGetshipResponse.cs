using ElectronicObserver.KancolleApi.Types.ApiGetMember.Kdock;
using ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Models;
using ElectronicObserver.KancolleApi.Types.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiReqKousyou.Getship;

public class ApiReqKousyouGetshipResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_kdock")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiGetMemberKdockResponse> ApiKdock { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_ship")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiShip ApiShip { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiShipId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_slotitem")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiSlotitem> ApiSlotitem { get; set; } = new();
}
