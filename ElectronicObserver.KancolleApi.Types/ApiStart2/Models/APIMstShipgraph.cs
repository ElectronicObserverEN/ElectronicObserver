namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstShipgraph
{
	[JsonPropertyName("api_battle_d")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiBattleD { get; set; } = default!;

	[JsonPropertyName("api_battle_n")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiBattleN { get; set; } = default!;

	[JsonPropertyName("api_boko_d")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiBokoD { get; set; } = default!;

	[JsonPropertyName("api_boko_n")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiBokoN { get; set; } = default!;

	[JsonPropertyName("api_ensyue_n")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiEnsyueN { get; set; } = default!;

	[JsonPropertyName("api_ensyuf_d")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiEnsyufD { get; set; } = default!;

	[JsonPropertyName("api_ensyuf_n")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiEnsyufN { get; set; } = default!;

	[JsonPropertyName("api_filename")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiFilename { get; set; } = default!;

	[JsonPropertyName("api_id")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[JsonPropertyName("api_kaisyu_d")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiKaisyuD { get; set; } = default!;

	[JsonPropertyName("api_kaisyu_n")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiKaisyuN { get; set; } = default!;

	[JsonPropertyName("api_kaizo_d")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiKaizoD { get; set; } = default!;

	[JsonPropertyName("api_kaizo_n")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiKaizoN { get; set; } = default!;

	[JsonPropertyName("api_map_d")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiMapD { get; set; } = default!;

	[JsonPropertyName("api_map_n")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiMapN { get; set; } = default!;

	[JsonPropertyName("api_pa")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiPa { get; set; } = default!;

	[JsonPropertyName("api_sortno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int? ApiSortno { get; set; } = default!;

	[JsonPropertyName("api_version")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required]
	public List<string> ApiVersion { get; set; } = new();

	[JsonPropertyName("api_weda")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiWeda { get; set; } = default!;

	[JsonPropertyName("api_wedb")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public List<int>? ApiWedb { get; set; } = default!;
}
