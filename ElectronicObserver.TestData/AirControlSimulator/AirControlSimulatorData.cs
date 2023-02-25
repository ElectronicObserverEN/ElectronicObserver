using System.Text.Json.Serialization;

namespace ElectronicObserver.TestData.AirControlSimulator;

public class AirControlSimulatorData
{
	[JsonPropertyName("api_mst_equip_exslot_ship")]
	public List<AirControlSimulatorApiMstEquipExslotShip> ApiMstEquipExslotShip { get; set; }

	[JsonPropertyName("api_mst_equip_ship")]
	public List<AirControlSimulatorApiMstEquipShip> ApiMstEquipShip { get; set; }

	[JsonPropertyName("api_mst_stype")]
	public List<AirControlSimulatorApiMstStype> ApiMstStype { get; set; }

	[JsonPropertyName("worlds")]
	public List<AirControlSimulatorWorld> Worlds { get; set; }

	[JsonPropertyName("maps")]
	public List<AirControlSimulatorMap> Maps { get; set; }

	[JsonPropertyName("ships")]
	public List<AirControlSimulatorShip> Ships { get; set; }

	[JsonPropertyName("items")]
	public List<AirControlSimulatorItem> Items { get; set; }

	[JsonPropertyName("enemies")]
	public List<AirControlSimulatorEnemy> Enemies { get; set; }

	[JsonPropertyName("area_count")]
	public int AreaCount { get; set; }
}