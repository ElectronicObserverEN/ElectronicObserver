using CefSharp;

namespace Browser.CefSharpBrowser.CefOp;

/// <summary>
/// コンテキストメニューを無効化します。
/// </summary>
public class MenuHandler : IContextMenuHandler
{
	public void OnBeforeContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model)
	{
		model.Clear();
	}

	public bool OnContextMenuCommand(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, CefMenuCommand commandId, CefEventFlags eventFlags)
	{
		return false;
	}

	public void OnContextMenuDismissed(IWebBrowser browserControl, IBrowser browser, IFrame frame)
	{
		// nop
	}

	public bool RunContextMenu(IWebBrowser browserControl, IBrowser browser, IFrame frame, IContextMenuParams parameters, IMenuModel model, IRunContextMenuCallback callback)
	{
		return false;
	}
}
