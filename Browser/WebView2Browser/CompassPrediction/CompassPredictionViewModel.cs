using System;
using System.Collections.Generic;
using System.Linq;
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
	private int? _selectedWorld;

	[ObservableProperty]
	private int? _selectedMap;

	private Dictionary<int, List<int>> MapList { get; set; } = new();

	public List<int> Worlds { get; set; } = new();

	[ObservableProperty]
	private List<int> _maps = new();

	public void Initialize()
	{
		MapList = BrowserHost.GetMapList().Result;

		Worlds = MapList.Keys.ToList();

		PropertyChanged += OnWorldChanged;
		PropertyChanged += OnMapChanged;

		UpdateFleet();
	}

	private void OnWorldChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(SelectedWorld)) return;

		if (SelectedWorld is null)
		{
			Maps = new List<int>();
		}

		Maps = SelectedWorld switch
		{
			{ } id => MapList[id],
			_ => Maps
		};

		UpdateDisplayedMap();
	}

	private void OnMapChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(SelectedMap)) return;

		UpdateDisplayedMap();
	}

	private void UpdateDisplayedMap()
	{
		if (SelectedWorld is null) return;
		if (SelectedMap is null) return;
		if (ExecuteScriptAsync is null) return;

		ExecuteScriptAsync($"document.querySelector(\".areas[value='{SelectedWorld}-{SelectedMap}']\").click();");
	}

	public void UpdateFleet()
	{
		if (ExecuteScriptAsync is null) return;

		string? fleetData = BrowserHost.GetFleetData().Result;

		if (fleetData is null) return;

		ExecuteScriptAsync($"document.querySelector(\"#fleet-import\").value='{fleetData}';document.querySelector(\"#fleet-import\").dispatchEvent(new Event(\"input\"))");
	}
}
