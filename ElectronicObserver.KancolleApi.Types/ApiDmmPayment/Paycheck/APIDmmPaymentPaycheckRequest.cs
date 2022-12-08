namespace ElectronicObserver.KancolleApi.Types.ApiDmmPayment.Paycheck;

public class ApiDmmPaymentPaycheckRequest
{
	[JsonPropertyName("api_verno")]
	[JsonIgnore(Condition = JsonIgnoreCondition.Never)]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
