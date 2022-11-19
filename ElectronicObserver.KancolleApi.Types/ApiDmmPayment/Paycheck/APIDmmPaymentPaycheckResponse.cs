namespace ElectronicObserver.KancolleApi.Types.ApiDmmPayment.Paycheck;

public class ApiDmmPaymentPaycheckResponse
{
	[JsonPropertyName("api_check_value")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	public int ApiCheckValue { get; set; } = default!;
}
