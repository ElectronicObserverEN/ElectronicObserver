using System.Text.Json.Serialization;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.TestData.AirControlSimulator;

public class AirControlSimulatorApiMstEquipShip
{
	/// <summary>
	/// Key is <see cref="EquipmentTypes"/>.
	/// </summary>
	[JsonPropertyName("api_equip_type")]
	public Dictionary<string, List<EquipmentId>?> ApiEquipType { get; set; } = [];
}
