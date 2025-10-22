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
			return new GadgetReplaceFilter("http://w00g.kancolle-server.com/gadget_html5/", GadgetBypassServer.GetReplaceUrl(GadgetBypassServerCustom));
		}

		return base.GetResourceResponseFilter(chromiumWebBrowser, browser, frame, request, response);
	}
}
