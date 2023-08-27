using BrowserLibCore;
using CefSharp;
using CefSharp.Handler;
using IBrowser = CefSharp.IBrowser;

namespace Browser.CefSharpBrowser.CefOp;

internal class GadgetUrlHandler : ResourceRequestHandler
{
	private GadgetServerOptions GadgetBypassServer { get; }
	private string GadgetBypassServerCustom { get; }

	public GadgetUrlHandler(GadgetServerOptions gadgetBypassServer, string gadgetBypassServerCustom)
	{
		GadgetBypassServer = gadgetBypassServer;
		GadgetBypassServerCustom = gadgetBypassServerCustom;
	}
	protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
	{
		if (request.Url.Contains(@"gadget_html5"))
		{
			return GadgetBypassServer switch
			{
				GadgetServerOptions.Wiki => new GadgetReplaceFilter("http://203.104.209.7/gadget_html5/", GadgetBypassServer.GetReplaceURL()),
				GadgetServerOptions.EO => new GadgetReplaceFilter("http://203.104.209.7/gadget_html5/", GadgetBypassServer.GetReplaceURL()),
				GadgetServerOptions.Custom => new GadgetReplaceFilter("http://203.104.209.7/gadget_html5/", GadgetBypassServer.GetReplaceURL(GadgetBypassServerCustom)),
				_ => new GadgetReplaceFilter("http://203.104.209.7/gadget_html5/", GadgetBypassServer.GetReplaceURL())
			};
		}

		return base.GetResourceResponseFilter(chromiumWebBrowser, browser, frame, request, response);
	}
}
