namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstConst
{
	[System.Text.Json.Serialization.JsonPropertyName("api_boko_max_ships")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public IntOrString ApiBokoMaxShips { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_dpflag_quest")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public IntOrString ApiDpflagQuest { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_parallel_quest_max")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public IntOrString ApiParallelQuestMax { get; set; } = new();
}
