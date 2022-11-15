using ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

namespace ElectronicObserver.KancolleApi.Types.ApiStart2.GetData;

public class ApiStart2GetDataResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_mst_bgm")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiMstBgm> ApiMstBgm { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_const")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiMstConst ApiMstConst { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_equip_exslot")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiMstEquipExslot { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_equip_exslot_ship")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiMstEquipExslotShip> ApiMstEquipExslotShip { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_equip_ship")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiMstEquipShip> ApiMstEquipShip { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_furniture")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiMstFurniture> ApiMstFurniture { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_furnituregraph")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiMstFurnituregraph> ApiMstFurnituregraph { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_item_shop")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public ApiMstItemShop ApiMstItemShop { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_maparea")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiMstMaparea> ApiMstMaparea { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_mapbgm")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiMstMapbgm> ApiMstMapbgm { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_mapinfo")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiMstMapinfo> ApiMstMapinfo { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_mission")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiMstMission> ApiMstMission { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_payitem")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiMstPayitem> ApiMstPayitem { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_ship")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiMstShip> ApiMstShip { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_shipgraph")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiMstShipgraph> ApiMstShipgraph { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_shipupgrade")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiMstShipupgrade> ApiMstShipupgrade { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_slotitem")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiMstSlotitem> ApiMstSlotitem { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_slotitem_equiptype")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiMstSlotitemEquiptype> ApiMstSlotitemEquiptype { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_stype")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiMstStype> ApiMstStype { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_mst_useitem")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<ApiMstUseitem> ApiMstUseitem { get; set; } = new();
}
