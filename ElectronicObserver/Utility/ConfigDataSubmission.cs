namespace ElectronicObserver.Utility;

public sealed class ConfigDataSubmission : Configuration.ConfigurationData.ConfigPartBase
{
	public bool SendDataToPoiPreview { get; set; } = true;

	public string BonodereUserId { get; set; } = "";
	public string BonodereToken { get; set; } = "";

	public bool SubmitDataToTsunDb { get; set; }
}
