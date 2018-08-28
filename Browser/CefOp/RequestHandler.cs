﻿using CefSharp;
using CefSharp.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Browser.CefOp
{
	/// <summary>
	/// レスポンスの置換制御を行います。
	/// </summary>
	public class RequestHandler : DefaultRequestHandler
	{
		bool pixiSettingEnabled;

		public RequestHandler(bool pixiSettingEnabled) : base() {
			this.pixiSettingEnabled = pixiSettingEnabled;
		}

		public override IResponseFilter GetResourceResponseFilter(IWebBrowser browserControl, IBrowser browser, IFrame frame, IRequest request, IResponse response)
		{
			if (pixiSettingEnabled && request.Url.Contains(@"/kcs2/index.php"))
				return new ResponseFilterPixiSetting();
			if (request.Url.Contains("/app_id=854854/"))
				return new AdFilter();

			return base.GetResourceResponseFilter(browserControl, browser, frame, request, response);
	    }

	    public override CefReturnValue OnBeforeResourceLoad(IWebBrowser browserControl, CefSharp.IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
	    {
	        return request.Url.Contains("rt.gsspat.jp") ? CefReturnValue.Cancel : CefReturnValue.Continue;
	    }
    }
}
