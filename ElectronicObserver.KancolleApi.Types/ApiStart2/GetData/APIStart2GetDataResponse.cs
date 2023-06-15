using ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiStart2.GetData;

public class ApiStart2GetDataResponse
{
	[JsonPropertyName("api_mst_bgm")]
	[Required]
	public List<ApiMstBgm> ApiMstBgm { get; set; } = new();

	[JsonPropertyName("api_mst_const")]
	[Required]
	public ApiMstConst ApiMstConst { get; set; } = new();

	[JsonPropertyName("api_mst_equip_exslot")]
	[Required]
	public List<int> ApiMstEquipExslot { get; set; } = new();

	[JsonPropertyName("api_mst_equip_exslot_ship")]
	[Required]
	public List<ApiMstEquipExslotShip> ApiMstEquipExslotShip { get; set; } = new();

	[JsonPropertyName("api_mst_equip_ship")]
	[Required]
	public List<ApiMstEquipShip> ApiMstEquipShip { get; set; } = new();

	[JsonPropertyName("api_mst_furniture")]
	[Required]
	public List<ApiMstFurniture> ApiMstFurniture { get; set; } = new();

	[JsonPropertyName("api_mst_furnituregraph")]
	[Required]
	public List<ApiMstFurnituregraph> ApiMstFurnituregraph { get; set; } = new();

	[JsonPropertyName("api_mst_item_shop")]
	[Required]
	public ApiMstItemShop ApiMstItemShop { get; set; } = new();

	[JsonPropertyName("api_mst_maparea")]
	[Required]
	public List<ApiMstMaparea> ApiMstMaparea { get; set; } = new();

	[JsonPropertyName("api_mst_mapbgm")]
	[Required]
	public List<ApiMstMapbgm> ApiMstMapbgm { get; set; } = new();

	[JsonPropertyName("api_mst_mapinfo")]
	[Required]
	public List<ApiMstMapinfo> ApiMstMapinfo { get; set; } = new();

	[JsonPropertyName("api_mst_mission")]
	[Required]
	public List<ApiMstMission> ApiMstMission { get; set; } = new();

	[JsonPropertyName("api_mst_payitem")]
	[Required]
	public List<ApiMstPayitem> ApiMstPayitem { get; set; } = new();

	[JsonPropertyName("api_mst_ship")]
	[Required]
	public List<ApiMstShip> ApiMstShip { get; set; } = new();

	[JsonPropertyName("api_mst_shipgraph")]
	[Required]
	public List<ApiMstShipgraph> ApiMstShipgraph { get; set; } = new();

	[JsonPropertyName("api_mst_shipupgrade")]
	[Required]
	public List<ApiMstShipupgrade> ApiMstShipupgrade { get; set; } = new();

	[JsonPropertyName("api_mst_slotitem")]
	[Required]
	public List<ApiMstSlotitem> ApiMstSlotitem { get; set; } = new();

	[JsonPropertyName("api_mst_slotitem_equiptype")]
	[Required]
	public List<ApiMstSlotitemEquiptype> ApiMstSlotitemEquiptype { get; set; } = new();

	[JsonPropertyName("api_mst_stype")]
	[Required]
	public List<ApiMstStype> ApiMstStype { get; set; } = new();

	[JsonPropertyName("api_mst_useitem")]
	[Required]
	public List<ApiMstUseitem> ApiMstUseitem { get; set; } = new();
}
