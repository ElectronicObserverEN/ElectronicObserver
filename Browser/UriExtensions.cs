namespace Browser;

public static class UriExtensions
{
	public static string CombineUrl(this string baseUrl, string relativeUrl)
	{
		if (baseUrl.Length == 0)
		{
			return relativeUrl;
		}

		if (relativeUrl.Length == 0)
		{
			return baseUrl;
		}

		baseUrl = baseUrl.TrimEnd('/').Trim();
		relativeUrl = relativeUrl.TrimStart('/');

		return $"{baseUrl}/{relativeUrl}";
	}
}
