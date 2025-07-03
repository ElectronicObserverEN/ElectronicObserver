using System;
using System.Windows.Forms;

namespace ElectronicObserver.Utility;

public static class ClipboardExtensions
{
	// TODO : use static extension on Cliboard class when dotnet 10 happens ?
	public static void SetTextAndLogErrors(string text)
	{
		try
		{
			Clipboard.SetText(text);
		}
		catch (Exception ex)
		{
			ErrorReporter.SendErrorReport(ex, LoggerRes.FailedToCopyToClipboard);
		}
	}
}
