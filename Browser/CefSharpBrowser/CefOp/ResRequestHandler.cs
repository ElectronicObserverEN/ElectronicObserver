using CefSharp;
using CefSharp.Handler;

namespace Browser.CefSharpBrowser.CefOp;

internal class ResRequestHandler : ResourceRequestHandler
{
	protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
	{
		// ログイン直後に勝手に遷移させられ、ブラウザがホワイトアウトすることがあるためブロックする
		if (request.Url.Contains(@"/rt.gsspat.jp/"))
		{
			return CefReturnValue.Cancel;
		}

		return base.OnBeforeResourceLoad(chromiumWebBrowser, browser, frame, request, callback);
	}

	protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
	{
		if (request.Url.Contains("/app_id=854854/"))
			return new AdFilter();

		return null;
	}

}
