namespace ElectronicObserver.Utility;

public sealed class ConfigDataSubmission : Configuration.ConfigurationData.ConfigPartBase
{
	public bool SendDataToPoiPreview { get; set; } = true;

    public string BonodereLogin { get; set; } = "";
    public string BonoderePassword { get; set; } = "";
}
