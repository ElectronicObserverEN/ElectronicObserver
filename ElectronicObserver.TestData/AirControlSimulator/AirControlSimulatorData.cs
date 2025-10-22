using System.Text.Json.Serialization;

namespace ElectronicObserver.TestData.AirControlSimulator;

public class AirControlSimulatorData
{
	[JsonPropertyName("api_mst_equip_ship")]
	public required Dictionary<string, AirControlSimulatorApiMstEquipShip> ApiMstEquipShip { get; set; }

	[JsonPropertyName("api_mst_stype")]
	public required List<AirControlSimulatorApiMstStype> ApiMstStype { get; set; }

	[JsonPropertyName("worlds")]
	public required List<AirControlSimulatorWorld> Worlds { get; set; }

	[JsonPropertyName("maps")]
	public required List<AirControlSimulatorMap> Maps { get; set; }

	[JsonPropertyName("ships")]
	public required List<AirControlSimulatorShip> Ships { get; set; }

	[JsonPropertyName("items")]
	public required List<AirControlSimulatorItem> Items { get; set; }

	[JsonPropertyName("enemies")]
	public required List<AirControlSimulatorEnemy> Enemies { get; set; }

	[JsonPropertyName("area_count")]
	public int AreaCount { get; set; }
}
