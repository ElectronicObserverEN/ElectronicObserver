using System.Collections.Generic;
using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Replay;

public class ReplayBattle
{
	[JsonPropertyName("sortie_id")]
	public int SortieId { get; set; }
	
	[JsonPropertyName("node")]
	public int Node { get; set; }
	
	[JsonPropertyName("data")]
	public object? FirstBattle { get; set; }
	
	[JsonPropertyName("yasen")]
	public object? SecondBattle { get; set; }
	
	[JsonPropertyName("rating")]
	public string? Rating { get; set; }
	
	[JsonPropertyName("drop")]
	public ShipId Drop { get; set; }
	
	[JsonPropertyName("time")]
	public double Time { get; set; }
	
	[JsonPropertyName("baseEXP")]
	public int BaseExp { get; set; }

	[JsonPropertyName("hqEXP")]
	public int HqExp { get; set; }

	[JsonPropertyName("mvp")]
	public List<int> Mvp { get; set; } = new();

	[JsonPropertyName("id")]
	public int Id { get; set; }
}
