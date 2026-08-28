using System.Collections.Generic;
using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.Replay;

public class ReplayShip
{
	[JsonPropertyName("mst_id")]
	public ShipId ShipId { get; set; }

	[JsonPropertyName("level")]
	public int Level { get; set; }

	[JsonPropertyName("kyouka")]
	public List<int> Kyouka { get; set; }

	[JsonPropertyName("morale")]
	public int Morale { get; set; }

	[JsonPropertyName("equip")]
	public List<int> Equip { get; set; }

	/// <summary>
	/// Equipment improvement levels, same order as <see cref="Equip"/>.
	/// </summary>
	[JsonPropertyName("stars")]
	public List<int> Stars { get; set; } = new();

	/// <summary>
	/// Equipment proficiency levels, same order as <see cref="Equip"/>.
	/// </summary>
	[JsonPropertyName("ace")]
	public List<int> Ace { get; set; } = new();
}
