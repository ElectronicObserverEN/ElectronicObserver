using System;
using System.ComponentModel;
using System.Threading.Tasks;
using BrowserLibCore;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jot;

namespace Browser.WebView2Browser.CompassPrediction;

public partial class CompassPredictionViewModel(IBrowserHost browserHost, CompassPredictionTranslationViewModel translations, Tracker tracker)
	: ObservableObject
{
	public CompassPredictionTranslationViewModel Translations { get; } = translations;

	private IBrowserHost BrowserHost { get; } = browserHost;

	public Action<string>? ExecuteScriptAsync { get; set; }

	public string Uri => "https://x-20a.github.io/compass/";

	[ObservableProperty]
	private bool _synchronizeMap;

	private Tracker Tracker { get; } = tracker;

	public async Task Initialize()
	{
		// TODO : fix map sync or remove its code
		// Tracker.Track(this);

		PropertyChanged += OnSynchronizeChanged;

		UpdateFleet();

		if (SynchronizeMap)
		{
			await SynchronizeMapWithPlayedOne();
		}
	}

	public void OnClose()
	{
		Tracker.StopTracking(this);
		Tracker.PersistAll();
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
		ExecuteScriptAsync?.Invoke($"""document.querySelector(".areas[value='{area}-{map}']").click();""");
	}

	[RelayCommand]
	public void UpdateFleet()
	{
		if (ExecuteScriptAsync is null) return;

		string fleetData = BrowserHost.GetFleetData().Result;
		
		ExecuteScriptAsync($$"""
		                   document.querySelector("#fleet-import").value='{{fleetData}}';
		                   document.querySelector("#fleet-import").dispatchEvent(new Event("input"));
		                   
		                   // Need to trigger fleet type change to update the map
		                   document.querySelector(".fleet-type[data-type='1']").click();
		                   """);
	}
}
