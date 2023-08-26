using BrowserLibCore;
using CefSharp;
using CefSharp.Handler;
using IBrowser = CefSharp.IBrowser;

namespace Browser.CefSharpBrowser.CefOp;

internal class GadgetUrlHandler : ResourceRequestHandler
{
	private GadgetServerOptions GadgetBypassServer { get; }
	private string GadgetBypassServerCustom {get; }
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
				GadgetServerOptions.Wiki => new GadgetReplaceFilter("http://203.104.209.7/gadget_html5/", "https://kcwiki.github.io/cache/gadget_html5/"),
				GadgetServerOptions.EO => new GadgetReplaceFilter("http://203.104.209.7/gadget_html5/", "https://electronicobserveren.github.io/cache/gadget_html5/"),
				GadgetServerOptions.Custom => new GadgetReplaceFilter("http://203.104.209.7/gadget_html5/", GadgetBypassServerCustom + "/gadget_html5/"),
				_ => new GadgetReplaceFilter("http://203.104.209.7/gadget_html5/", "https://electronicobserveren.github.io/cache/gadget_html5/")
			};
		}

		return base.GetResourceResponseFilter(chromiumWebBrowser, browser, frame, request, response);
	}
}
