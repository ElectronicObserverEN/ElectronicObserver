namespace ElectronicObserver.Utility;

public sealed class ConfigDataSubmission : Configuration.ConfigurationData.ConfigPartBase
{
	public bool SendDataToPoi { get; set; }

	public bool BonodereIntegrationEnabled { get; set; }
	public string BonodereUserId { get; set; } = "";
	public string BonodereToken { get; set; } = "";

	public bool SubmitDataToTsunDb { get; set; }

	public bool SendDataToKancolleReplayDb { get; set; }
}
