using System;
using System.ComponentModel;
using System.Threading.Tasks;
using BrowserLibCore;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Browser.WebView2Browser.CompassPrediction;

public partial class CompassPredictionViewModel(IBrowserHost browserHost, CompassPredictionTranslationViewModel translations)
	: ObservableObject
{
	public CompassPredictionTranslationViewModel Translations { get; } = translations;

	public IBrowserHost BrowserHost { get; } = browserHost;

	public Action<string>? ExecuteScriptAsync { get; set; }

	public string Uri => "https://x-20a.github.io/compass/";

	[ObservableProperty] 
	private bool _synchronizeMap = true;
	
	public void Initialize()
	{
		PropertyChanged += OnSynchronizeChanged;

		UpdateFleet();
	}

	private async void OnSynchronizeChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(SynchronizeMap)) return;

		await SynchronizeMapWithPlayedOne();
	}

	private async Task SynchronizeMapWithPlayedOne()
	{
		if (!SynchronizeMap) return;

		(int? currentArea, int? currentMap) = await BrowserHost.GetCurrentMap();

		if (currentArea is not {} currentAreaNotNull) return;
		if (currentMap is not {} currentMapNotNull) return;

		UpdateDisplayedMap(currentAreaNotNull, currentMapNotNull);
	}

	public void UpdateDisplayedMap(int area, int map)
	{
		ExecuteScriptAsync?.Invoke($"document.querySelector(\".areas[value='{area}-{map}']\").click();");
	}

	public void UpdateFleet()
	{
		if (ExecuteScriptAsync is null) return;

		string? fleetData = BrowserHost.GetFleetData().Result;

		if (fleetData is null) return;

		ExecuteScriptAsync($"document.querySelector(\"#fleet-import\").value='{fleetData}';document.querySelector(\"#fleet-import\").dispatchEvent(new Event(\"input\"))");
	}
}
