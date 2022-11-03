using System.Text.Json.Serialization;

namespace ElectronicObserver.KancolleApi.ApiPort.Port.Response;

public class ApiShip
{

	[JsonPropertyName("api_backs")]
	public int ApiBacks { get; set; }

	[JsonPropertyName("api_bull")]
	public int ApiBull { get; set; }

	[JsonPropertyName("api_cond")]
	public int ApiCond { get; set; }

	[JsonPropertyName("api_exp")]
	public IEnumerable<int>? ApiExp { get; set; }

	[JsonPropertyName("api_fuel")]
	public int ApiFuel { get; set; }

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_kaihi")]
	public IEnumerable<int>? ApiKaihi { get; set; }

	[JsonPropertyName("api_karyoku")]
	public IEnumerable<int>? ApiKaryoku { get; set; }

	[JsonPropertyName("api_kyouka")]
	public IEnumerable<int>? ApiKyouka { get; set; }

	[JsonPropertyName("api_leng")]
	public int ApiLeng { get; set; }

	[JsonPropertyName("api_locked")]
	public int ApiLocked { get; set; }

	[JsonPropertyName("api_locked_equip")]
	public int ApiLockedEquip { get; set; }

	[JsonPropertyName("api_lucky")]
	public IEnumerable<int>? ApiLucky { get; set; }

	[JsonPropertyName("api_lv")]
	public int ApiLv { get; set; }

	[JsonPropertyName("api_maxhp")]
	public int ApiMaxhp { get; set; }

	[JsonPropertyName("api_ndock_item")]
	public IEnumerable<int>? ApiNdockItem { get; set; }

	[JsonPropertyName("api_ndock_time")]
	public int ApiNdockTime { get; set; }

	[JsonPropertyName("api_nowhp")]
	public int ApiNowhp { get; set; }

	[JsonPropertyName("api_onslot")]
	public IEnumerable<int>? ApiOnslot { get; set; }

	[JsonPropertyName("api_raisou")]
	public IEnumerable<int>? ApiRaisou { get; set; }

	[JsonPropertyName("api_sakuteki")]
	public IEnumerable<int>? ApiSakuteki { get; set; }

	[JsonPropertyName("api_sally_area")]
	public int ApiSallyArea { get; set; }

	[JsonPropertyName("api_ship_id")]
	public int ApiShipId { get; set; }

	[JsonPropertyName("api_slot")]
	public IEnumerable<int>? ApiSlot { get; set; }

	[JsonPropertyName("api_slot_ex")]
	public int ApiSlotEx { get; set; }

	[JsonPropertyName("api_slotnum")]
	public int ApiSlotnum { get; set; }

	[JsonPropertyName("api_soku")]
	public int ApiSoku { get; set; }

	[JsonPropertyName("api_sortno")]
	public int ApiSortno { get; set; }

	[JsonPropertyName("api_soukou")]
	public IEnumerable<int>? ApiSoukou { get; set; }

	[JsonPropertyName("api_srate")]
	public int ApiSrate { get; set; }

	[JsonPropertyName("api_taiku")]
	public IEnumerable<int>? ApiTaiku { get; set; }

	[JsonPropertyName("api_taisen")]
	public IEnumerable<int>? ApiTaisen { get; set; }

}