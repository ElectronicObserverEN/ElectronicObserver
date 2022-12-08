using ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiStart2.GetData;

public class ApiStart2GetDataResponse
{
	[JsonPropertyName("api_mst_bgm")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiMstBgm> ApiMstBgm { get; set; } = new();

	[JsonPropertyName("api_mst_const")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiMstConst ApiMstConst { get; set; } = new();

	[JsonPropertyName("api_mst_equip_exslot")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<int> ApiMstEquipExslot { get; set; } = new();

	[JsonPropertyName("api_mst_equip_exslot_ship")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiMstEquipExslotShip> ApiMstEquipExslotShip { get; set; } = new();

	[JsonPropertyName("api_mst_equip_ship")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiMstEquipShip> ApiMstEquipShip { get; set; } = new();

	[JsonPropertyName("api_mst_furniture")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiMstFurniture> ApiMstFurniture { get; set; } = new();

	[JsonPropertyName("api_mst_furnituregraph")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiMstFurnituregraph> ApiMstFurnituregraph { get; set; } = new();

	[JsonPropertyName("api_mst_item_shop")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public ApiMstItemShop ApiMstItemShop { get; set; } = new();

	[JsonPropertyName("api_mst_maparea")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiMstMaparea> ApiMstMaparea { get; set; } = new();

	[JsonPropertyName("api_mst_mapbgm")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiMstMapbgm> ApiMstMapbgm { get; set; } = new();

	[JsonPropertyName("api_mst_mapinfo")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiMstMapinfo> ApiMstMapinfo { get; set; } = new();

	[JsonPropertyName("api_mst_mission")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiMstMission> ApiMstMission { get; set; } = new();

	[JsonPropertyName("api_mst_payitem")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiMstPayitem> ApiMstPayitem { get; set; } = new();

	[JsonPropertyName("api_mst_ship")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiMstShip> ApiMstShip { get; set; } = new();

	[JsonPropertyName("api_mst_shipgraph")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiMstShipgraph> ApiMstShipgraph { get; set; } = new();

	[JsonPropertyName("api_mst_shipupgrade")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiMstShipupgrade> ApiMstShipupgrade { get; set; } = new();

	[JsonPropertyName("api_mst_slotitem")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiMstSlotitem> ApiMstSlotitem { get; set; } = new();

	[JsonPropertyName("api_mst_slotitem_equiptype")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiMstSlotitemEquiptype> ApiMstSlotitemEquiptype { get; set; } = new();

	[JsonPropertyName("api_mst_stype")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiMstStype> ApiMstStype { get; set; } = new();

	[JsonPropertyName("api_mst_useitem")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<ApiMstUseitem> ApiMstUseitem { get; set; } = new();
}
