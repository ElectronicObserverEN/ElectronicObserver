namespace ElectronicObserver.Avalonia;

public static class UrlHelpers
{
	public static string Join(params List<string> urlParts)
	{
		IEnumerable<string> cleanedParts = urlParts
			.Where(x => !string.IsNullOrWhiteSpace(x))
			.Select(x => x.Trim('/'));

		return string.Join('/', cleanedParts);
	}
}
