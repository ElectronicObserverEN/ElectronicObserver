﻿namespace ElectronicObserver.KancolleApi.Types.ApiReqHensei.Combined;

public class ApiReqHenseiCombinedRequest
{
	[JsonPropertyName("api_combined_type")]
	[Required(AllowEmptyStrings = true)]
	public string ApiCombinedType { get; set; } = default!;

	[JsonPropertyName("api_verno")]
	[Required(AllowEmptyStrings = true)]
	public string ApiVerno { get; set; } = default!;
}
