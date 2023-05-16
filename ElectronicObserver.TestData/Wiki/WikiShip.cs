using System.Text.Json;

namespace ElectronicObserver.TestData.Wiki;

public class WikiShip
{
	public int _aa { get; set; }
	public JsonElement _aa_max { get; set; }
	public JsonElement _aa_mod { get; set; }
	public int _ammo { get; set; }
	public int _api_id { get; set; }
	public int _armor { get; set; }
	public int _armor_max { get; set; }
	public JsonElement _armor_mod { get; set; }
	public string _artist { get; set; }
	public int? _asw { get; set; }
	public JsonElement _asw_max { get; set; }
	public JsonElement _availability { get; set; }
	public int _build_time { get; set; }
	public bool? _buildable { get; set; }
	public bool? _buildable_lsc { get; set; }
	public string _class { get; set; }
	public JsonElement _class_number { get; set; }
	public WikiShipSlot[] _equipment { get; set; }
	public int? _evasion { get; set; }
	public int? _evasion_max { get; set; }
	public int _firepower { get; set; }
	public int _firepower_max { get; set; }
	public JsonElement _firepower_mod { get; set; }
	public int _fuel { get; set; }
	public int _hp { get; set; }
	public int _hp_max { get; set; }
	public int _id { get; set; }
	public int[] _implementation_date { get; set; }
	public string _japanese_name { get; set; }
	public int? _los { get; set; }
	public int? _los_max { get; set; }
	public int _luck { get; set; }
	public int _luck_max { get; set; }
	public JsonElement _luck_mod { get; set; }
	public string _name { get; set; }
	public int _range { get; set; }
	public JsonElement _rarity { get; set; }
	public JsonElement _reading { get; set; }
	public JsonElement _remodel_from { get; set; }
	public JsonElement _remodel_level { get; set; }
	public JsonElement _remodel_to { get; set; }
	public JsonElement _scrap_ammo { get; set; }
	public JsonElement _scrap_baux { get; set; }
	public int _scrap_fuel { get; set; }
	public int _scrap_steel { get; set; }
	public int _speed { get; set; }
	public JsonElement _suffix { get; set; }
	public int _torpedo { get; set; }
	public JsonElement _torpedo_max { get; set; }
	public JsonElement _torpedo_mod { get; set; }
	public JsonElement _true_id { get; set; }
	public int _type { get; set; }
	public string _voice_actor { get; set; }
	public string _wikipedia { get; set; }
	public string _full_name { get; set; }
}
