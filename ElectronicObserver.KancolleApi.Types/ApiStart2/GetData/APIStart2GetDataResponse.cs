using ElectronicObserver.Core.Types;
using ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiStart2.GetData;

public class ApiStart2GetDataResponse
{
	[JsonPropertyName("api_mst_bgm")]
	public List<ApiMstBgm> ApiMstBgm { get; set; } = new();

	[JsonPropertyName("api_mst_const")]
	public ApiMstConst ApiMstConst { get; set; } = new();

	[JsonPropertyName("api_mst_equip_exslot")]
	public List<int> ApiMstEquipExslot { get; set; } = new();

	/// <summary>
	/// Key is <see cref="EquipmentId"/>.
	/// </summary>
	[JsonPropertyName("api_mst_equip_exslot_ship")]
	public Dictionary<string, ApiMstEquipExslotShip> ApiMstEquipExslotShip { get; set; } = new();

	[JsonPropertyName("api_mst_equip_ship")]
	public List<ApiMstEquipShip> ApiMstEquipShip { get; set; } = new();

	[JsonPropertyName("api_mst_furniture")]
	public List<ApiMstFurniture> ApiMstFurniture { get; set; } = new();

	[JsonPropertyName("api_mst_furnituregraph")]
	public List<ApiMstFurnituregraph> ApiMstFurnituregraph { get; set; } = new();

	[JsonPropertyName("api_mst_item_shop")]
	public ApiMstItemShop ApiMstItemShop { get; set; } = new();

	[JsonPropertyName("api_mst_maparea")]
	public List<ApiMstMaparea> ApiMstMaparea { get; set; } = new();

	[JsonPropertyName("api_mst_mapbgm")]
	public List<ApiMstMapbgm> ApiMstMapbgm { get; set; } = new();

	[JsonPropertyName("api_mst_mapinfo")]
	public List<ApiMstMapinfo> ApiMstMapinfo { get; set; } = new();

	[JsonPropertyName("api_mst_mission")]
	public List<ApiMstMission> ApiMstMission { get; set; } = new();

	[JsonPropertyName("api_mst_payitem")]
	public List<ApiMstPayitem> ApiMstPayitem { get; set; } = new();

	[JsonPropertyName("api_mst_ship")]
	public List<ApiMstShip> ApiMstShip { get; set; } = new();

	[JsonPropertyName("api_mst_shipgraph")]
	public List<ApiMstShipgraph> ApiMstShipgraph { get; set; } = new();

	[JsonPropertyName("api_mst_shipupgrade")]
	public List<ApiMstShipupgrade> ApiMstShipupgrade { get; set; } = new();

	[JsonPropertyName("api_mst_slotitem")]
	public List<ApiMstSlotitem> ApiMstSlotitem { get; set; } = new();

	[JsonPropertyName("api_mst_slotitem_equiptype")]
	public List<ApiMstSlotitemEquiptype> ApiMstSlotitemEquiptype { get; set; } = new();

	[JsonPropertyName("api_mst_stype")]
	public List<ApiMstStype> ApiMstStype { get; set; } = new();

	[JsonPropertyName("api_mst_useitem")]
	public List<ApiMstUseitem> ApiMstUseitem { get; set; } = new();
}
