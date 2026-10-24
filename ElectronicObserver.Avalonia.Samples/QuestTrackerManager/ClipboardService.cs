using Avalonia.Input.Platform;
using ElectronicObserver.Core.Services;

namespace ElectronicObserver.Avalonia.Samples.QuestTrackerManager;

public class ClipboardService : IClipboardService
{
	public IClipboard? Clipboard { get; set; }

	public string? GetTextAndLogErrors()
	{
		return Clipboard?.TryGetTextAsync().Result;
	}

	public void SetTextAndLogErrors(string text)
	{
		_ = Clipboard?.SetTextAsync(text);
	}
}
