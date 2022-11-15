namespace ElectronicObserver.KancolleApi.Types.Models;

public class ApiOpeningTaisen
{
	[System.Text.Json.Serialization.JsonPropertyName("api_at_eflag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiAtEflag { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_at_list")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiAtList { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_at_type")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiAtType { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_cl_list")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>> ApiClList { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_damage")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>> ApiDamage { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_df_list")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<int>> ApiDfList { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_si_list")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<List<string>> ApiSiList { get; set; } = new();
}
