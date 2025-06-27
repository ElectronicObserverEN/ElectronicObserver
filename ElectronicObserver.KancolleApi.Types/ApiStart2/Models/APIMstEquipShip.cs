
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstEquipShip
{
	/// <summary>
	/// Key is <see cref="EquipmentTypes"/>.
	/// </summary>
	[JsonPropertyName("api_equip_type")]
	public Dictionary<string, List<int>> ApiEquipType { get; set; } = [];
}
