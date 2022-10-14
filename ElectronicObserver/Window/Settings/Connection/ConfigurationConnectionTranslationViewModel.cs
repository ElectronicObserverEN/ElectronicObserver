using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.Connection;

public class ConfigurationConnectionTranslationViewModel : TranslationBaseViewModel
{
	public string Port => ConfigRes.Port;
	public string ConnectionPort => ConfigRes.ConnectionPort;
	public string Connection_UseSystemProxy => Properties.Window.Dialog.DialogConfiguration.Connection_UseSystemProxy;
	public string UseSystemProxyTooltip => ConfigRes.UseSystemProxyTooltip;

	public string Connection_UseUpstreamProxy => Properties.Window.Dialog.DialogConfiguration.Connection_UseUpstreamProxy;
	public string Connection_UseUpstreamProxyToolTip => Properties.Window.Dialog.DialogConfiguration.Connection_UseUpstreamProxyToolTip;
	public string Connection_UpstreamProxyPortToolTip => Properties.Window.Dialog.DialogConfiguration.Connection_UpstreamProxyPortToolTip;
	public string UpstreamProxyToolTip => Properties.Window.Dialog.DialogConfiguration.UpstreamProxyToolTip;

	public string Connection_DownstreamProxyLabel => Properties.Window.Dialog.DialogConfiguration.Connection_DownstreamProxyLabel;
	public string Connection_DownstreamProxyToolTip => Properties.Window.Dialog.DialogConfiguration.Connection_DownstreamProxyToolTip;

	public string Connection_SaveReceivedData => Properties.Window.Dialog.DialogConfiguration.Connection_SaveReceivedData;
	public string MayIncreaseSize => ConfigRes.MayIncreaseSize;

	public string SaveLocation => ConfigRes.SaveLocation;

	public string SaveAPIRequests => ConfigRes.SaveAPIRequests;
	public string SaveAPIResponses => ConfigRes.SaveAPIResponses;
	public string SaveAllConnectionFiles => ConfigRes.SaveAllConnectionFiles;
	public string AddVersionToFile => ConfigRes.AddVersionToFile;

	public string OutputProxyScript => ConfigRes.OutputProxyScript;

	public string NetworkSettingsNote => Properties.Window.Dialog.DialogConfiguration.NetworkSettingsNote;

	public string Connection_RegisterAsSystemProxy => Properties.Window.Dialog.DialogConfiguration.Connection_RegisterAsSystemProxy;
	public string RegSystemProxyHint => ConfigRes.RegSystemProxyHint;
	public string Connection_SaveDataPathSearch => Properties.Window.Dialog.DialogConfiguration.Connection_SaveDataPathSearch;
}
