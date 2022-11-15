namespace ElectronicObserver.KancolleApi.Types.ApiDmmPayment.Paycheck;

public class ApiDmmPaymentPaycheckResponse
{
	[System.Text.Json.Serialization.JsonPropertyName("api_check_value")]
	[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.Never)]
	public int ApiCheckValue { get; set; } = default!;
}
