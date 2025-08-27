using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types.Serialization.DeckBuilder;

namespace ElectronicObserver.Window.Tools.FleetImageGenerator;

public class FleetImageGeneratorImageDataModel
{
	[JsonPropertyName("Title")]
	public string? Title { get; set; }

	[JsonPropertyName("Comment")]
	public string? Comment { get; set; }

	[JsonPropertyName("Fleet1Visible")]
	public bool Fleet1Visible { get; set; }

	[JsonPropertyName("Fleet2Visible")]
	public bool Fleet2Visible { get; set; }

	[JsonPropertyName("Fleet3Visible")]
	public bool Fleet3Visible { get; set; }

	[JsonPropertyName("Fleet4Visible")]
	public bool Fleet4Visible { get; set; }

	[JsonPropertyName("DeckBuilderData")]
	public DeckBuilderData DeckBuilderData { get; set; } = new();
}