using CefSharp;
using CefSharp.Handler;

namespace Browser.CefSharpBrowser.CefOp;

internal class GadgetUrlHandler : ResourceRequestHandler
{
	protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
	{
		if (request.Url.Contains(@"gadget_html5"))
		{
			return new GadgetReplaceFilter("http://203.104.209.7/gadget_html5/", "https://kcwiki.github.io/cache/gadget_html5/");
		}

		return base.GetResourceResponseFilter(chromiumWebBrowser, browser, frame, request, response);
	}
}
