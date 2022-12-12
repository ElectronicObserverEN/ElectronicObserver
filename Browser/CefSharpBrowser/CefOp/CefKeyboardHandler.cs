using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CefSharp;
using CommunityToolkit.Mvvm.Input;

namespace Browser.CefSharpBrowser.CefOp;
public class CefKeyboardHandler : IKeyboardHandler
{
	private IRelayCommand Mute { get; }
	private IRelayCommand Refresh { get; }
	private IRelayCommand ScreenShot { get; }
	private IRelayCommand HardRefresh { get; }
	public CefKeyboardHandler(IRelayCommand mute, IRelayCommand refresh, IRelayCommand screenshot, IRelayCommand hardRefresh)
	{
		Mute = mute;
		Refresh = refresh;
		ScreenShot = screenshot;
		HardRefresh = hardRefresh;
	}
	public bool OnKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey)
	{
		return false;
	}

	public bool OnPreKeyEvent(IWebBrowser chromiumWebBrowser, IBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers, bool isSystemKey, ref bool isKeyboardShortcut)
	{
		var key = KeyInterop.KeyFromVirtualKey(windowsKeyCode);
		switch (key)
		{
			case Key.F2:
				ScreenShot.Execute(null);
				break;
			case Key.F12:
				chromiumWebBrowser.GetBrowser().ShowDevTools();
				break;
			case Key.F7:
				Mute.Execute(null);
				break;
			case Key.F5:
				if (modifiers == CefEventFlags.ControlDown)
				{
					HardRefresh.Execute(null);
				}
				else
				{
					Refresh.Execute(null);
				}
				break;
		}
		return true;
	}
}
