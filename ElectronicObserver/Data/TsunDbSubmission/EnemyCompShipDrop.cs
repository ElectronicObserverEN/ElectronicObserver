using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.TsunDbSubmission;

public class EnemyCompShipDrop(dynamic apidata) : EnemyComp
{
	[JsonPropertyName("mapName")]
	public string MapName => apidata.api_quest_name;

	[JsonPropertyName("compName")]
	public string CompName => apidata.api_enemy_info.api_deck_name;

	[JsonPropertyName("baseExp")]
	public int BaseExp => (int)apidata.api_get_base_exp;
}
