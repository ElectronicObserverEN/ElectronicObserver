using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.PoiDbSubmission.PoiDbRouteSubmission;

public class LosValues
{
	[JsonPropertyName("sakuOne25")]
	public required double? SakuOne25 { get; set; }

	[JsonPropertyName("sakuOne25a")]
	public required double? SakuOne25a { get; set; }

	[JsonPropertyName("sakuOne33x1")]
	public required double SakuOne33x1 { get; set; }

	[JsonPropertyName("sakuOne33x2")]
	public required double SakuOne33x2 { get; set; }

	[JsonPropertyName("sakuOne33x3")]
	public required double SakuOne33x3 { get; set; }

	[JsonPropertyName("sakuOne33x4")]
	public required double SakuOne33x4 { get; set; }

	[JsonPropertyName("sakuTwo25")]
	public required double? SakuTwo25 { get; set; }

	[JsonPropertyName("sakuTwo25a")]
	public required double? SakuTwo25a { get; set; }

	[JsonPropertyName("sakuTwo33x1")]
	public required double? SakuTwo33x1 { get; set; }

	[JsonPropertyName("sakuTwo33x2")]
	public required double? SakuTwo33x2 { get; set; }

	[JsonPropertyName("sakuTwo33x3")]
	public required double? SakuTwo33x3 { get; set; }

	[JsonPropertyName("sakuTwo33x4")]
	public required double? SakuTwo33x4 { get; set; }
}
