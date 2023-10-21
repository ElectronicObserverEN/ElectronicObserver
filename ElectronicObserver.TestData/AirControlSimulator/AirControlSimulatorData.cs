using System.Text.Json.Serialization;

namespace ElectronicObserver.TestData.AirControlSimulator;

public class AirControlSimulatorData
{
	[JsonPropertyName("api_mst_equip_ship")]
	public List<AirControlSimulatorApiMstEquipShip> ApiMstEquipShip { get; set; } = new();

	[JsonPropertyName("api_mst_stype")]
	public List<AirControlSimulatorApiMstStype> ApiMstStype { get; set; } = new();

	[JsonPropertyName("worlds")]
	public List<AirControlSimulatorWorld> Worlds { get; set; } = new();

	[JsonPropertyName("maps")]
	public List<AirControlSimulatorMap> Maps { get; set; } = new();

	[JsonPropertyName("ships")]
	public List<AirControlSimulatorShip> Ships { get; set; } = new();

	[JsonPropertyName("items")]
	public List<AirControlSimulatorItem> Items { get; set; } = new();

	[JsonPropertyName("enemies")]
	public List<AirControlSimulatorEnemy> Enemies { get; set; } = new();

	[JsonPropertyName("area_count")]
	public int AreaCount { get; set; }
}
