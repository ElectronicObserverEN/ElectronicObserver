namespace ElectronicObserver.KancolleApi.Types.ApiReqSortie.Models;

public class ApiStage3
{
	[System.Text.Json.Serialization.JsonPropertyName("api_ebak_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiEbakFlag { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_ecl_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiEclFlag { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_edam")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiEdam { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_erai_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiEraiFlag { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_fbak_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFbakFlag { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_fcl_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFclFlag { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_fdam")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFdam { get; set; } = new();

	[System.Text.Json.Serialization.JsonPropertyName("api_frai_flag")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	[System.ComponentModel.DataAnnotations.Required]
	public List<int> ApiFraiFlag { get; set; } = new();
}
