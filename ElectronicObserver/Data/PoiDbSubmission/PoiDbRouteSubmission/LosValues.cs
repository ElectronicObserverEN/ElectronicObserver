using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;

public class LosValues
{
	[JsonPropertyName("sakuOne25")]
	public required string? SakuOne25 { get; set; }

	[JsonPropertyName("sakuOne25a")]
	public required string? SakuOne25a { get; set; }

	[JsonPropertyName("sakuOne33x1")]
	public required string SakuOne33x1 { get; set; }

	[JsonPropertyName("sakuOne33x2")]
	public required string SakuOne33x2 { get; set; }

	[JsonPropertyName("sakuOne33x3")]
	public required string SakuOne33x3 { get; set; }

	[JsonPropertyName("sakuOne33x4")]
	public required string SakuOne33x4 { get; set; }

	[JsonPropertyName("sakuTwo25")]
	public required string? SakuTwo25 { get; set; }

	[JsonPropertyName("sakuTwo25a")]
	public required string? SakuTwo25a { get; set; }

	[JsonPropertyName("sakuTwo33x1")]
	public required string? SakuTwo33x1 { get; set; }

	[JsonPropertyName("sakuTwo33x2")]
	public required string? SakuTwo33x2 { get; set; }

	[JsonPropertyName("sakuTwo33x3")]
	public required string? SakuTwo33x3 { get; set; }

	[JsonPropertyName("sakuTwo33x4")]
	public required string? SakuTwo33x4 { get; set; }
}
