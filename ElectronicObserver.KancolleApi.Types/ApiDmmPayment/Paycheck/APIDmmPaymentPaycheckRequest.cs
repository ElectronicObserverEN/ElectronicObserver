namespace ElectronicObserver.KancolleApi.Types.ApiDmmPayment.Paycheck;

public class ApiDmmPaymentPaycheckRequest
{
	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
