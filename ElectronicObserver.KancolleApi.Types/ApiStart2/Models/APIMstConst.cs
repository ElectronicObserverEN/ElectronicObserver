namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstConst
{
	[JsonPropertyName("api_boko_max_ships")]
	public IntOrString ApiBokoMaxShips { get; set; } = new();

	[JsonPropertyName("api_dpflag_quest")]
	public IntOrString ApiDpflagQuest { get; set; } = new();

	[JsonPropertyName("api_parallel_quest_max")]
	public IntOrString ApiParallelQuestMax { get; set; } = new();
}
