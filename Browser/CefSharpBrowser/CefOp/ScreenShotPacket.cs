using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;

namespace Browser.CefSharpBrowser.CefOp;

public class ScreenShotPacket
{
	public string ID { get; }
	private string? DataUrl { get; set; }

	public TaskCompletionSource<ScreenShotPacket> TaskSource { get; }

	public ScreenShotPacket() : this("ss_" + Guid.NewGuid().ToString("N")) { }

	public ScreenShotPacket(string id)
	{
		ID = id;
		TaskSource = new TaskCompletionSource<ScreenShotPacket>();
	}

	public void Complete(string dataurl)
	{
		DataUrl = dataurl;
		TaskSource.SetResult(this);
	}

	public Bitmap GetImage() => ConvertToImage(DataUrl);

	public static Bitmap? ConvertToImage(string? dataurl)
	{
		if (dataurl is null || !dataurl.StartsWith("data:image/png")) return null;

		string s = dataurl.Substring(dataurl.IndexOf(',') + 1);
		byte[] bytes = Convert.FromBase64String(s);

		using MemoryStream ms = new(bytes);
		Bitmap bitmap = new(ms);

		return bitmap;
	}
}
