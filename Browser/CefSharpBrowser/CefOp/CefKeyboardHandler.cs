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
		// when you press T, the key T gets passed here with type KeyType.RawKeyDown
		// right after that the key F5 gets passed with KeyType.Char
		// when you press F5, the F5 key gets passed here with type KeyType.RawKeyDown
		// I don't understand how this works but returning false on KeyType.Char seems like the correct behavior
		if (type is KeyType.Char) return false;

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

			case Key.M when modifiers is CefEventFlags.ControlDown:
				ViewModel.MuteCommand.Execute(null);
				return true;

			case Key.F5 when modifiers is CefEventFlags.ControlDown:
				ViewModel.HardRefreshCommand.Execute(null);
				return true;

			case Key.F5:
				ViewModel.RefreshCommand.Execute(null);
				return true;
		}

		return false;
	}
}
