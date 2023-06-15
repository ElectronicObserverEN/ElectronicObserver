﻿using ElectronicObserverTypes;

namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiShipDatum
{
	[JsonPropertyName("api_backs")]
	public int ApiBacks { get; set; } = default!;

	[JsonPropertyName("api_bull")]
	public int ApiBull { get; set; } = default!;

	[JsonPropertyName("api_cond")]
	public int ApiCond { get; set; } = default!;

	[JsonPropertyName("api_exp")]
	public List<int> ApiExp { get; set; } = new();

	[JsonPropertyName("api_fuel")]
	public int ApiFuel { get; set; } = default!;

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_kaihi")]
	public List<int> ApiKaihi { get; set; } = new();

	[JsonPropertyName("api_karyoku")]
	public List<int> ApiKaryoku { get; set; } = new();

	[JsonPropertyName("api_kyouka")]
	public List<int> ApiKyouka { get; set; } = new();

	[JsonPropertyName("api_leng")]
	public int ApiLeng { get; set; } = default!;

	[JsonPropertyName("api_locked")]
	public int ApiLocked { get; set; } = default!;

	[JsonPropertyName("api_locked_equip")]
	public int ApiLockedEquip { get; set; } = default!;

	[JsonPropertyName("api_lucky")]
	public List<int> ApiLucky { get; set; } = new();

	[JsonPropertyName("api_lv")]
	public int ApiLv { get; set; } = default!;

	[JsonPropertyName("api_maxhp")]
	public int ApiMaxhp { get; set; } = default!;

	[JsonPropertyName("api_ndock_item")]
	public List<int> ApiNdockItem { get; set; } = new();

	[JsonPropertyName("api_ndock_time")]
	public int ApiNdockTime { get; set; } = default!;

	[JsonPropertyName("api_nowhp")]
	public int ApiNowhp { get; set; } = default!;

	[JsonPropertyName("api_onslot")]
	public List<int> ApiOnslot { get; set; } = new();

	[JsonPropertyName("api_raisou")]
	public List<int> ApiRaisou { get; set; } = new();

	[JsonPropertyName("api_sakuteki")]
	public List<int> ApiSakuteki { get; set; } = new();

	[JsonPropertyName("api_sally_area")]
	public int? ApiSallyArea { get; set; } = default!;

	[JsonPropertyName("api_ship_id")]
	public ShipId ApiShipId { get; set; } = default!;

	[JsonPropertyName("api_slot")]
	public List<int> ApiSlot { get; set; } = new();

	[JsonPropertyName("api_slot_ex")]
	public int ApiSlotEx { get; set; } = default!;

	[JsonPropertyName("api_slotnum")]
	public int ApiSlotnum { get; set; } = default!;

	[JsonPropertyName("api_soku")]
	public int ApiSoku { get; set; } = default!;

	[JsonPropertyName("api_sortno")]
	public int ApiSortno { get; set; } = default!;

	[JsonPropertyName("api_soukou")]
	public List<int> ApiSoukou { get; set; } = new();

	[JsonPropertyName("api_srate")]
	public int ApiSrate { get; set; } = default!;

	[JsonPropertyName("api_taiku")]
	public List<int> ApiTaiku { get; set; } = new();

	[JsonPropertyName("api_taisen")]
	public List<int> ApiTaisen { get; set; } = new();
}
