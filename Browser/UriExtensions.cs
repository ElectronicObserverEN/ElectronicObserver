using System;

namespace Browser;

public static class UriExtensions
{
	public static string CombineUrl(this string baseUrl, string relativeUrl)
	{
		UriBuilder baseUri = new UriBuilder(baseUrl);
		Uri newUri;

		if (Uri.TryCreate(baseUri.Uri, relativeUrl, out newUri))
		{
			return newUri.ToString();
		}
		else
		{
			throw new ArgumentException("Unable to combine specified url values");
		}
	}
}
