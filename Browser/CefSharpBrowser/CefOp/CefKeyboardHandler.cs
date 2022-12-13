using System.Globalization;
using System.Threading;
using System.Windows.Input;
using CefSharp;

namespace Browser.CefSharpBrowser.CefOp;

public class CefKeyboardHandler : IKeyboardHandler
{
	private CefSharpViewModel ViewModel { get; }

	public CefKeyboardHandler(CefSharpViewModel viewModel)
	{
		ViewModel = viewModel;
	}

	public bool OnKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
	{
		return false;
	}

	public bool OnPreKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
	{
		Key key = KeyInterop.KeyFromVirtualKey(windowsKeyCode);

		CultureInfo c = new(ViewModel.Culture);

		Thread.CurrentThread.CurrentCulture = c;
		Thread.CurrentThread.CurrentUICulture = c;

		switch (key)
		{
			case Key.F2:
				ViewModel.ScreenshotCommand.Execute(null);
				return true;
			case Key.F12:
				chromiumWebBrowser.GetBrowser().ShowDevTools();
				return true;
			case Key.F7:
				ViewModel.MuteCommand.Execute(null);
				return true;
			case Key.F5:
				if (modifiers == CefEventFlags.ControlDown)
				{
					ViewModel.HardRefreshCommand.Execute(null);
				}
				else
				{
					ViewModel.RefreshCommand.Execute(null);
				}
				return true;
		}

		return false;
	}
}
