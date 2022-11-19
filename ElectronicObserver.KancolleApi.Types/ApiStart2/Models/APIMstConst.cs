namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstConst
{
	[JsonPropertyName("api_boko_max_ships")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public IntOrString ApiBokoMaxShips { get; set; } = new();

	[JsonPropertyName("api_dpflag_quest")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public IntOrString ApiDpflagQuest { get; set; } = new();

	[JsonPropertyName("api_parallel_quest_max")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public IntOrString ApiParallelQuestMax { get; set; } = new();
}
