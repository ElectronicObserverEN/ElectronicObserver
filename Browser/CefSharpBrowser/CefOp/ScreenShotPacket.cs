using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace Browser.CefSharpBrowser.CefOp;

public class ScreenShotPacket(string id)
{
	public string ID { get; } = id;
	private string? DataUrl { get; set; }
	public static string SetScreenshotDataFunctionName
		=> System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(nameof(SetScreenshotData));

	public TaskCompletionSource<ScreenShotPacket> TaskSource { get; } = new();

	public ScreenShotPacket() : this("ss_" + Guid.NewGuid().ToString("N")) { }

	public void SetScreenshotData(string dataurl)
	{
		DataUrl = dataurl;
		TaskSource.SetResult(this);
	}

	public Bitmap? GetImage() => ConvertToImage(DataUrl);

	public static Bitmap? ConvertToImage(string? dataurl)
	{
		if (dataurl is null || !dataurl.StartsWith("data:image/png")) return null;

		string s = dataurl[(dataurl.IndexOf(',') + 1)..];
		byte[] bytes = Convert.FromBase64String(s);

		using MemoryStream ms = new(bytes);
		Bitmap bitmap = new(ms);

		return bitmap;
	}
}
