using System;
using System.Diagnostics;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Tools.Telegram;

public partial class TweetViewModel : ObservableObject
{
	public BitmapImage? Image { get; set; }

	public string? Title { get; set; }

	public string? Description { get; set; }

	public DateTime PubDate { get; set; }

	public Guid? Guid { get; set; }

	public string? Link { get; set; }

	public string? Author { get; set; }

	[RelayCommand]
	private void OpenTweet()
	{
		try
		{
			ProcessStartInfo psi = new ProcessStartInfo
			{
				FileName = Link,
				UseShellExecute = true
			};
			Process.Start(psi);
		}
		catch (Exception ex)
		{
			ErrorReporter.SendErrorReport(ex, Properties.Window.FormMain.FailedToOpenBrowser);
		}
	}
}
