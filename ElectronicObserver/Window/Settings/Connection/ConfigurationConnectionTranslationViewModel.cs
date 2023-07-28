using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Settings.Connection;

public class ConfigurationConnectionTranslationViewModel : TranslationBaseViewModel
{
	public string Port => ConfigRes.Port;
	public string ConnectionPort => ConfigRes.ConnectionPort;
	public string Connection_UseSystemProxy => ConfigurationResources.Connection_UseSystemProxy;
	public string UseSystemProxyTooltip => ConfigRes.UseSystemProxyTooltip;

	public string Connection_UseUpstreamProxy => ConfigurationResources.Connection_UseUpstreamProxy;
	public string Connection_UseUpstreamProxyToolTip => ConfigurationResources.Connection_UseUpstreamProxyToolTip;
	public string Connection_UpstreamProxyPortToolTip => ConfigurationResources.Connection_UpstreamProxyPortToolTip;
	public string UpstreamProxyToolTip => ConfigurationResources.UpstreamProxyToolTip;

	public string Connection_DownstreamProxyLabel => ConfigurationResources.Connection_DownstreamProxyLabel;
	public string Connection_DownstreamProxyToolTip => ConfigurationResources.Connection_DownstreamProxyToolTip;

	public string Connection_SaveReceivedData => ConfigurationResources.Connection_SaveReceivedData;
	public string MayIncreaseSize => ConfigRes.MayIncreaseSize;

	public string SaveLocation => ConfigRes.SaveLocation;

	public string SaveAPIRequests => ConfigRes.SaveAPIRequests;
	public string SaveAPIResponses => ConfigRes.SaveAPIResponses;
	public string SaveAllConnectionFiles => ConfigRes.SaveAllConnectionFiles;
	public string AddVersionToFile => ConfigRes.AddVersionToFile;

	public string OutputProxyScript => ConfigRes.OutputProxyScript;

	public string NetworkSettingsNote => ConfigurationResources.NetworkSettingsNote;

	public string Connection_RegisterAsSystemProxy => ConfigurationResources.Connection_RegisterAsSystemProxy;
	public string RegSystemProxyHint => ConfigRes.RegSystemProxyHint;
	public string Connection_SaveDataPathSearch => ConfigurationResources.Connection_SaveDataPathSearch;
}
