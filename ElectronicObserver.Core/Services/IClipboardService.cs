namespace ElectronicObserver.Core.Services;

public interface IClipboardService
{
	string? GetTextAndLogErrors();
	void SetTextAndLogErrors(string text);
}
