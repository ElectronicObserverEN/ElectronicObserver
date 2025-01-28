using System.Text.Json.Serialization;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Replay;

public class ReplayAirBaseSquadron
{
	[JsonPropertyName("mst_id")]
	public EquipmentId EquipmentId { get; set; }

	[JsonPropertyName("count")]
	public int Count { get; set; }

	[JsonPropertyName("stars")]
	public int Stars { get; set; }

	[JsonPropertyName("ace")]
	public int Ace { get; set; }

	[JsonPropertyName("state")]
	public int State { get; set; }

	[JsonPropertyName("morale")]
	public AirBaseCondition Morale { get; set; }
}
