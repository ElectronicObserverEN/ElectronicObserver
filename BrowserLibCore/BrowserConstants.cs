#if !DEBUG
using System;
using System.IO;
#endif

namespace BrowserLibCore;

public class BrowserConstants
{
#if DEBUG
	public static string WebView2CachePath => "BrowserCache";
#else
		public static string WebView2CachePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"ElectronicObserver\Webview2");
#endif

#if DEBUG
	public static string CefSharpCachePath => "BrowserCacheCEF";
#else
		public static string CefSharpCachePath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"ElectronicObserver\CEF");
#endif
}
