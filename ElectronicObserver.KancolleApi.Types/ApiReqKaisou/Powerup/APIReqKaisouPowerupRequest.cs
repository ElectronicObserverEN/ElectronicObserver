namespace ElectronicObserver.KancolleApi.Types.ApiReqKaisou.Powerup;

public class ApiReqKaisouPowerupRequest
{
	[JsonPropertyName("api_id")]
	public string ApiId { get; set; } = "";

	[JsonPropertyName("api_id_items")]
	public string ApiIdItems { get; set; } = "";

	[JsonPropertyName("api_verno")]
	public string ApiVerno { get; set; } = "";

	/// <summary>
	/// Possible values are in the enum <see cref="Core.Types.LimitedFeedType"/>
	/// </summary>
	[JsonPropertyName("api_limited_feed_type")]
	public string? LimitedFeedType { get; set; }
}
