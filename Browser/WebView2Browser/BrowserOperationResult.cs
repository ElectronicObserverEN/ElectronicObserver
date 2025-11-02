namespace Browser.WebView2Browser;

public class BrowserOperationResult
{
	public BrowserOperation Operation { get; init; } = BrowserOperation.Unknown;
	public object? Result { get; init; }
}
