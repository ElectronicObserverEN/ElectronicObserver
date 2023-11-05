using CefSharp;
using CefSharp.DevTools;
using CefSharp.Handler;

namespace Browser.CefSharpBrowser.CefOp;

public class CustomRequestHandler : RequestHandler
{
	public delegate void RenderProcessTerminatedEventHandler(string message);
	public event RenderProcessTerminatedEventHandler? RenderProcessTerminated;

	private bool PixiSettingEnabled { get; }
	private bool UseGadgetRedirect { get; }


	public CustomRequestHandler(bool pixiSettingEnabled, bool useGadgetRedirect)
	{
		PixiSettingEnabled = pixiSettingEnabled;
		UseGadgetRedirect = useGadgetRedirect;
	}

	/// <summary>
	/// 戻る/進む操作をブロックします。
	/// </summary>
	protected override bool OnBeforeBrowse(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, bool userGesture, bool isRedirect)
	{
		if ((request.TransitionType & TransitionType.ForwardBack) != 0)
		{
			return true;
		}
		return base.OnBeforeBrowse(browserControl, browser, frame, request, userGesture, isRedirect);
	}

	/// <summary>
	/// 描画プロセスが何らかの理由で落ちた際の処理を行います。
	/// </summary>
	protected override void OnRenderProcessTerminated(IWebBrowser browserControl, IBrowser browser, CefTerminationStatus status)
	{
		// note: out of memory (例外コード: 0xe0000008) でクラッシュした場合、このイベントは呼ばれない

		string ret = Resources.RenderProcessTerminatedBy;
		ret += status switch
		{
			CefTerminationStatus.AbnormalTermination => Resources.RenderProcessAbnormalTermination,
			CefTerminationStatus.ProcessWasKilled => Resources.RenderProcessProcessWasKilled,
			CefTerminationStatus.ProcessCrashed => Resources.RenderProcessProcessCrashed,
			_ => Resources.RenderProcessUnexpectedTermination,
		};
		ret += Resources.RenderProcessReturnWhenReloaded;

		RenderProcessTerminated?.Invoke(ret);
	}

	protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool iNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
	{
		//NOTE: In most cases you examine the request.Url and only handle requests you are interested in
		if (request.Url.Contains(@"/kcs2/resources/bgm/"))
		{
			return new ResRequestHandler();
		}

		if (UseGadgetRedirect && request.Url.Contains("gadget_html5"))
		{
			return new GadgetUrlHandler();
		}

		if (request.Url.Contains(@"accounts.google.com"))
		{
			using DevToolsClient tools = chromiumWebBrowser.GetDevToolsClient();
			tools.Emulation.SetUserAgentOverrideAsync("Chrome");
		}

		return new CustomResourceRequestHandler(PixiSettingEnabled);
	}
}
