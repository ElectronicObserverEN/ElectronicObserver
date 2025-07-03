using System;
using System.Windows.Forms;
using ElectronicObserver.Core.Services;

namespace ElectronicObserver.Utility;

public class ClipboardService : IClipboardService
{
	public void SetTextAndLogErrors(string text)
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
