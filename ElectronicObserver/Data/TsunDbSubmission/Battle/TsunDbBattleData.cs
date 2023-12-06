using System.Text.Json;
using System.Text.Json.Serialization;

namespace ElectronicObserver.Data.TsunDbSubmission.Battle;

public class TsunDbBattleData : TsunDbEntity
{
	protected override string Url => "eventbattle";

	protected override bool IsBetaAPI => false;

	private static KCDatabase Database => KCDatabase.Instance;

	#region Json properties
	/// <summary>
	/// Map name, e.g. 53-5
	/// </summary>
	[JsonPropertyName("map")]
	public string Map { get; }

	/// <summary>
	/// Node edge number
	/// </summary>
	[JsonPropertyName("node")]
	public int Node { get; }

	/// <summary>
	/// Chosen map difficulty
	/// </summary>
	[JsonPropertyName("difficulty")]
	public int Difficulty { get; }

	/// <summary>
	/// Checks if the node is debuffed
	/// </summary>
	[JsonPropertyName("debuffed")]
	public bool Debuffed { get; }

	/// <summary>
	/// Checks if the map is cleared
	/// </summary>
	[JsonPropertyName("cleared")]
	public bool MapCleared { get; }

	/// <summary>
	/// Contains information about the sortied fleet(s), LBAS and support
	/// </summary>
	[JsonPropertyName("fleet")]
	public TsunDbFleetsAndAirBaseData Fleet { get; }

	/// <summary>
	/// Checks if resupply has been used in the battle, obtained from request.params.api_supply_flag
	/// </summary>
	[JsonPropertyName("resupplyused")]
	public bool ResupplyUsed { get; }

	/// <summary>
	/// Formation ID chosen by the player, obtained from request.params.api_formation
	/// </summary>
	[JsonPropertyName("playerformation")]
	public int Formation { get; }

	/// <summary>
	/// Raw battle data
	/// </summary>
	[JsonPropertyName("rawapi")]
	public object? RawApi { get; }

	/// <summary>
	/// Amount of edges in the map
	/// </summary>
	[JsonPropertyName("amountofnodes")]
	public int AmountOfNodes { get; }

	/// <summary>
	/// API endpoint for the battle, e.g. api_req_sortie/battle
	/// </summary>
	[JsonPropertyName("apiname")]
	public string ApiName { get; }
	#endregion

	#region Constructor
	public TsunDbBattleData(string apiName, dynamic rawApi)
	{
		Map = string.Format("{0}-{1}", Database.Battle.Compass.MapAreaID, Database.Battle.Compass.MapInfoID);
		Node = Database.Battle.Compass.Destination;
		Difficulty = Database.Battle.Compass.MapInfo.EventDifficulty;

		// Start day or night, its always the first battle that should carry debuff info ?
		Debuffed = Database.Battle.FirstBattle.Initial.IsBossDamaged;
		MapCleared = Database.Battle.Compass.MapInfo.IsCleared;

		Fleet = new TsunDbFleetsAndAirBaseData();

		ResupplyUsed = Database.Battle.ResupplyUsed;
		Formation = Database.Battle.FirstBattle.Searching.FormationFriend;
		AmountOfNodes = TsunDbSubmissionManager.CurrentMapAmountOfNodes;

		RawApi = JsonSerializer.Deserialize<object?>(rawApi.ToString());

		ApiName = apiName;
	}
	#endregion
}
