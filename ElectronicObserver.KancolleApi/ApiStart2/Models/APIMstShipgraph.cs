namespace ElectronicObserver.KancolleApi.Types.ApiStart2.Models;

public class ApiMstShipgraph
{
	[System.Text.Json.Serialization.JsonPropertyName("api_battle_d")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiBattleD { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_battle_n")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiBattleN { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_boko_d")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiBokoD { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_boko_n")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiBokoN { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ensyue_n")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiEnsyueN { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ensyuf_d")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiEnsyufD { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_ensyuf_n")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiEnsyufN { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_filename")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required(AllowEmptyStrings = true)]
	public string ApiFilename { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_id")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiId { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_kaisyu_d")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiKaisyuD { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_kaisyu_n")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiKaisyuN { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_kaizo_d")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiKaizoD { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_kaizo_n")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiKaizoN { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_map_d")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiMapD { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_map_n")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiMapN { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_pa")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiPa { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_sortno")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public int? ApiSortno { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_version")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<string> ApiVersion { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_weda")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiWeda { get; set; } = default!;

	[System.Text.Json.Serialization.JsonPropertyName("api_wedb")]
	[System.Text.Json.Serialization.JsonIgnore(Condition =
		System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]
	public List<int>? ApiWedb { get; set; } = default!;
}
