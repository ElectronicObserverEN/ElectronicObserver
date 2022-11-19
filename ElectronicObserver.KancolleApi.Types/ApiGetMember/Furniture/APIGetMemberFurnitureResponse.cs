namespace ElectronicObserver.KancolleApi.Types.ApiGetMember.Furniture;

public class ApiGetMemberFurnitureResponse
{
	[JsonPropertyName("api_furniture_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFurnitureId { get; set; } = default!;

	[JsonPropertyName("api_furniture_no")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFurnitureNo { get; set; } = default!;

	[JsonPropertyName("api_furniture_type")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFurnitureType { get; set; } = default!;

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;
}
