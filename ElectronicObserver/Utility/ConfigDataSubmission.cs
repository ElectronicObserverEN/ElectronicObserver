namespace ElectronicObserver.Utility;

public sealed class ConfigDataSubmission : Configuration.ConfigurationData.ConfigPartBase
{
	public string BonodereLogin { get; set; } = "";
	public string BonoderePassword { get; set; } = "";
}
