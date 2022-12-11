using System.Text.Json.Serialization;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Replay;

public class ReplayRange
{
	[JsonPropertyName("api_base")]
	public int ApiBase { get; set; }
	
	[JsonPropertyName("api_bonus")]
	public int ApiBonus { get; set; }
}
