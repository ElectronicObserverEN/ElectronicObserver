using System;
using System.Windows.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Tools.Telegram;

public partial class TweetViewModel : ObservableObject
{
	public BitmapImage? Image { get; set; }

	public string? Title { get; set; }

	public string? Description { get; set; }

	public DateTime PubDate { get; set; }

	public Guid? Guid { get; set; }

	public string? Link { get; set; }

	public string? DisplayLink => Link?.Replace("_", "__");

	public string? Author { get; set; }
}
