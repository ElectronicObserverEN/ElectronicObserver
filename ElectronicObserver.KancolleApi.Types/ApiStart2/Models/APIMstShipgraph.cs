namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstShipgraph
{
	[JsonPropertyName("api_battle_d")]
	public List<int>? ApiBattleD { get; set; }

	[JsonPropertyName("api_battle_n")]
	public List<int>? ApiBattleN { get; set; }

	[JsonPropertyName("api_boko_d")]
	public List<int>? ApiBokoD { get; set; }

	[JsonPropertyName("api_boko_n")]
	public List<int>? ApiBokoN { get; set; }

	[JsonPropertyName("api_ensyue_n")]
	public List<int>? ApiEnsyueN { get; set; }

	[JsonPropertyName("api_ensyuf_d")]
	public List<int>? ApiEnsyufD { get; set; }

	[JsonPropertyName("api_ensyuf_n")]
	public List<int>? ApiEnsyufN { get; set; }

	[JsonPropertyName("api_filename")]
	public string ApiFilename { get; set; } = "";

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; }

	[JsonPropertyName("api_kaisyu_d")]
	public List<int>? ApiKaisyuD { get; set; }

	[JsonPropertyName("api_kaisyu_n")]
	public List<int>? ApiKaisyuN { get; set; }

	[JsonPropertyName("api_kaizo_d")]
	public List<int>? ApiKaizoD { get; set; }

	[JsonPropertyName("api_kaizo_n")]
	public List<int>? ApiKaizoN { get; set; }

	[JsonPropertyName("api_map_d")]
	public List<int>? ApiMapD { get; set; }

	[JsonPropertyName("api_map_n")]
	public List<int>? ApiMapN { get; set; }

	[JsonPropertyName("api_pa")]
	public List<int>? ApiPa { get; set; }

	[JsonPropertyName("api_sortno")]
	public int? ApiSortno { get; set; }

	[JsonPropertyName("api_version")]
	public List<string> ApiVersion { get; set; } = new();

	[JsonPropertyName("api_weda")]
	public List<int>? ApiWeda { get; set; }

	[JsonPropertyName("api_wedb")]
	public List<int>? ApiWedb { get; set; }
}
