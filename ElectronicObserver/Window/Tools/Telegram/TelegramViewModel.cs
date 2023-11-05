using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;

namespace ElectronicObserver.Window.Tools.Telegram;

public partial class TelegramViewModel : WindowViewModelBase
{
	private HttpClient Client { get; } = new();
	public TelegramTranslationViewModel TelegramTranslation { get; }

	[ObservableProperty]
	private List<TweetViewModel> _tweets = new();

	[ObservableProperty]
	private string? _errorMessage;

	private List<string> TweetSources { get; } = new()
	{
		"https://rsshub.app/twitter/user/c2_staff/excludeReplies=1",
		"https://rsshub.app/twitter/user/KanColle_STAFF/excludeReplies=1",
	};

	private Timer Timer { get; } = new()
	{
		Interval = 1000,
		AutoReset = true,
		Enabled = true,
	};
	private static int Timeout => 10;

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(GetTweetsCommand))]
	private int _secondsTillNextFetch;

	[ObservableProperty]
	private string _searchButtonText = "";

	private bool CanSearch => SecondsTillNextFetch <= 0;

	public TelegramViewModel()
	{
		TelegramTranslation = Ioc.Default.GetRequiredService<TelegramTranslationViewModel>();

		SearchButtonText = TelegramTranslation.Search;

		Timer.Elapsed += (_, _) =>
		{
			if (SecondsTillNextFetch is 0) return;

			System.Windows.Application.Current.Dispatcher.BeginInvoke(() =>
			{
				SecondsTillNextFetch -= 1;

				SearchButtonText = SecondsTillNextFetch switch
				{
					> 0 => SecondsTillNextFetch.ToString(),
					_ => TelegramTranslation.Search,
				};
			});
		};

		Timer.Start();
	}

	[RelayCommand(CanExecute = nameof(CanSearch))]
	private async Task GetTweets()
	{
		if (SecondsTillNextFetch > 0) return;

		ErrorMessage = null;

		try
		{
			IEnumerable<TweetViewModel> tweets = Enumerable.Empty<TweetViewModel>();

			foreach (Task<List<TweetViewModel>> item in TweetSources.Select(GetTweets))
			{
				tweets = tweets.Concat(await item);
			}

			Tweets = tweets
				.OrderByDescending(t => t.PubDate)
				.ToList();

			SecondsTillNextFetch = Timeout;
		}
		catch (Exception ex)
		{
			Tweets = new();
			ErrorMessage = $"Failed to load tweets: {ex.Message} {ex.StackTrace}";
		}
	}

	private async Task<List<TweetViewModel>> GetTweets(string url)
	{
		XmlSerializer serializer = new(typeof(Rss));
		HttpResponseMessage response = await Client.GetAsync(url);
		Stream xmlStream = await response.Content.ReadAsStreamAsync();
		Rss data = (Rss)serializer.Deserialize(xmlStream)!;

		Stream imageStream = await GetImageStream(data.Channel.Image.Url);

		BitmapImage profileImage = MakeBitmap(imageStream);

		return data.Channel.Item.Select(i => ToTweetViewModel(i, profileImage)).ToList();
	}

	private BitmapImage MakeBitmap(Stream imageStream)
	{
		BitmapImage bitmap = new();

		bitmap.BeginInit();
		bitmap.StreamSource = imageStream;
		bitmap.CacheOption = BitmapCacheOption.OnLoad;
		bitmap.EndInit();
		bitmap.Freeze();

		return bitmap;
	}

	private async Task<Stream> GetImageStream(string url)
	{
		url = url.Replace("_normal", "_400x400");

		HttpResponseMessage imageResponse = await Client.GetAsync(url);
		return await imageResponse.Content.ReadAsStreamAsync();
	}

	private static TweetViewModel ToTweetViewModel(Item item, BitmapImage image) => new()
	{
		Image = image,
		Author = item.Author,
		Description = ParseDescription(item.Description),
		PubDate = DateTime.Parse(item.PubDate).ToLocalTime(),
		Link = item.Link
	};

	private static string ParseDescription(string description)
	{
		int start = description.IndexOf("<div class=\"rsshub-quote\">", StringComparison.InvariantCulture);
		int end = description.IndexOf("</div>", StringComparison.InvariantCulture);

		if (start > 0 && end > 0 && start < end)
		{
			description = description[..start];
		}

		description = description.Replace("<br>", "\n");

		return WebUtility.HtmlDecode(description);
	}
}
