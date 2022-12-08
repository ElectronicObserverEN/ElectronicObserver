using CefSharp;
using CefSharp.Handler;

namespace Browser.CefSharpBrowser.CefOp;

public class CustomResourceRequestHandler : ResourceRequestHandler
{

	bool pixiSettingEnabled;

	public CustomResourceRequestHandler(bool pixiSettingEnabled) : base()
	{
		this.pixiSettingEnabled = pixiSettingEnabled;
	}


	/// <summary>
	/// レスポンスの置換制御を行います。
	/// </summary>
	protected override IResponseFilter GetResourceResponseFilter(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IResponse response)
	{
		if (pixiSettingEnabled && request.Url.Contains(@"/kcs2/index.php"))
			return new ResponseFilterPixiSetting();

		return base.GetResourceResponseFilter(chromiumWebBrowser, browser, frame, request, response);
	}

	/// <summary>
	/// 特定の通信をブロックします。
	/// </summary>
	protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
	{
		// ログイン直後に勝手に遷移させられ、ブラウザがホワイトアウトすることがあるためブロックする
		if (request.Url.Contains(@"/rt.gsspat.jp/"))
		{
			return CefReturnValue.Cancel;
		}

		return base.OnBeforeResourceLoad(chromiumWebBrowser, browser, frame, request, callback);
	}

}
