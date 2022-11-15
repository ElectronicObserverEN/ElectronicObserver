namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiShipDatum
{
	[System.Text.Json.Serialization.JsonPropertyName("api_backs")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiBacks { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_bull")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiBull { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_cond")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCond { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_exp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiExp { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_fuel")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiFuel { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_kaihi")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiKaihi { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_karyoku")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiKaryoku { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_kyouka")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiKyouka { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_leng")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiLeng { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_locked")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiLocked { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_locked_equip")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiLockedEquip { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_lucky")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiLucky { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_lv")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiLv { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_maxhp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiMaxhp { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ndock_item")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiNdockItem { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_ndock_time")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiNdockTime { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_nowhp")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiNowhp { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_onslot")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiOnslot { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_raisou")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiRaisou { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_sakuteki")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiSakuteki { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_sally_area")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiSallyArea { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ship_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiShipId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_slot")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiSlot { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_slot_ex")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSlotEx { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_slotnum")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSlotnum { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_soku")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSoku { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_sortno")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSortno { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_soukou")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiSoukou { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_srate")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiSrate { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_taiku")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiTaiku { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_taisen")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiTaisen { get; set; } = new();
}
