using ElectronicObserver.Core.Types;

namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstEquipExslotShip
{
	/// <summary>
	/// Key is <see cref="ShipId"/>.
	/// </summary>
	[JsonPropertyName("api_ship_ids")]
	public Dictionary<string, int>? ApiShipIds { get; set; }

	/// <summary>
	/// Key is <see cref="ShipTypes"/>.
	/// </summary>
	[JsonPropertyName("api_stypes")]
	public Dictionary<string, int>? ApiStypes { get; set; }

	/// <summary>
	/// Key is <see cref="ShipClass"/>.
	/// </summary>
	[JsonPropertyName("api_ctypes")]
	public Dictionary<string, int>? ApiCtypes { get; set; }
}
