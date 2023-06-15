namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstShipgraph
{
	[JsonPropertyName("api_battle_d")]
	public List<int>? ApiBattleD { get; set; } = default!;

	[JsonPropertyName("api_battle_n")]
	public List<int>? ApiBattleN { get; set; } = default!;

	[JsonPropertyName("api_boko_d")]
	public List<int>? ApiBokoD { get; set; } = default!;

	[JsonPropertyName("api_boko_n")]
	public List<int>? ApiBokoN { get; set; } = default!;

	[JsonPropertyName("api_ensyue_n")]
	public List<int>? ApiEnsyueN { get; set; } = default!;

	[JsonPropertyName("api_ensyuf_d")]
	public List<int>? ApiEnsyufD { get; set; } = default!;

	[JsonPropertyName("api_ensyuf_n")]
	public List<int>? ApiEnsyufN { get; set; } = default!;

	[JsonPropertyName("api_filename")]
	public string ApiFilename { get; set; } = default!;

	[JsonPropertyName("api_id")]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_kaisyu_d")]
	public List<int>? ApiKaisyuD { get; set; } = default!;

	[JsonPropertyName("api_kaisyu_n")]
	public List<int>? ApiKaisyuN { get; set; } = default!;

	[JsonPropertyName("api_kaizo_d")]
	public List<int>? ApiKaizoD { get; set; } = default!;

	[JsonPropertyName("api_kaizo_n")]
	public List<int>? ApiKaizoN { get; set; } = default!;

	[JsonPropertyName("api_map_d")]
	public List<int>? ApiMapD { get; set; } = default!;

	[JsonPropertyName("api_map_n")]
	public List<int>? ApiMapN { get; set; } = default!;

	[JsonPropertyName("api_pa")]
	public List<int>? ApiPa { get; set; } = default!;

	[JsonPropertyName("api_sortno")]
	public int? ApiSortno { get; set; } = default!;

	[JsonPropertyName("api_version")]
	public List<string> ApiVersion { get; set; } = new();

	[JsonPropertyName("api_weda")]
	public List<int>? ApiWeda { get; set; } = default!;

	[JsonPropertyName("api_wedb")]
	public List<int>? ApiWedb { get; set; } = default!;
}
