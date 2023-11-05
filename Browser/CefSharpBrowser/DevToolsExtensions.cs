using System.Threading;
using System.Threading.Tasks;
using CefSharp;

namespace Browser.CefSharpBrowser;

public static class DevToolsExtensions
{
	private static int _lastMessageId = 600000;

	public static async Task ClearCache(this IWebBrowser chromiumWebBrowser)
	{
		int messageId = Interlocked.Increment(ref _lastMessageId);
		string methodName = "Network.clearBrowserCache";

		IBrowserHost host = chromiumWebBrowser.GetBrowserHost();

		int id;

		if (Cef.CurrentlyOnThread(CefThreadIds.TID_UI))
		{
			id = host.ExecuteDevToolsMethod(messageId, methodName);
		}
		else
		{
			id = await Cef.UIThreadTaskFactory
				.StartNew(() => host.ExecuteDevToolsMethod(messageId, methodName));
		}

		if (id != messageId)
		{
			// todo: log error?
		}
	}
}
