namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiSetShip
{
	[JsonPropertyName("api_backs")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiBacks { get; set; } = default!;

	[JsonPropertyName("api_bull")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiBull { get; set; } = default!;

	[JsonPropertyName("api_cond")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCond { get; set; } = default!;

	[JsonPropertyName("api_exp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiExp { get; set; } = new();

	[JsonPropertyName("api_fuel")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiFuel { get; set; } = default!;

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_kaihi")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiKaihi { get; set; } = new();

	[JsonPropertyName("api_karyoku")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiKaryoku { get; set; } = new();

	[JsonPropertyName("api_kyouka")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiKyouka { get; set; } = new();

	[JsonPropertyName("api_leng")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiLeng { get; set; } = default!;

	[JsonPropertyName("api_locked")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiLocked { get; set; } = default!;

	[JsonPropertyName("api_locked_equip")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiLockedEquip { get; set; } = default!;

	[JsonPropertyName("api_lucky")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiLucky { get; set; } = new();

	[JsonPropertyName("api_lv")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiLv { get; set; } = default!;

	[JsonPropertyName("api_maxhp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiMaxhp { get; set; } = default!;

	[JsonPropertyName("api_ndock_item")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiNdockItem { get; set; } = new();

	[JsonPropertyName("api_ndock_time")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiNdockTime { get; set; } = default!;

	[JsonPropertyName("api_nowhp")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiNowhp { get; set; } = default!;

	[JsonPropertyName("api_onslot")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiOnslot { get; set; } = new();

	[JsonPropertyName("api_raisou")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiRaisou { get; set; } = new();

	[JsonPropertyName("api_sakuteki")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiSakuteki { get; set; } = new();

	[JsonPropertyName("api_sally_area")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiSallyArea { get; set; } = default!;

	[JsonPropertyName("api_ship_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiShipId { get; set; } = default!;

	[JsonPropertyName("api_slot")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiSlot { get; set; } = new();

	[JsonPropertyName("api_slot_ex")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSlotEx { get; set; } = default!;

	[JsonPropertyName("api_slotnum")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSlotnum { get; set; } = default!;

	[JsonPropertyName("api_soku")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSoku { get; set; } = default!;

	[JsonPropertyName("api_sortno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSortno { get; set; } = default!;

	[JsonPropertyName("api_soukou")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiSoukou { get; set; } = new();

	[JsonPropertyName("api_srate")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiSrate { get; set; } = default!;

	[JsonPropertyName("api_taiku")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiTaiku { get; set; } = new();

	[JsonPropertyName("api_taisen")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiTaisen { get; set; } = new();
}
