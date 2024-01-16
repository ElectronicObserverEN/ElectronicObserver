using System;
using System.Collections.Generic;
using System.Linq;
using BrowserLibCore;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Browser.WebView2Browser.CompassPrediction;

public partial class CompassPredictionViewModel : ObservableObject
{
	public CompassPredictionTranslationViewModel Translations { get; }

	public IBrowserHost BrowserHost { get; }
	
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

	public CompassPredictionViewModel(IBrowserHost browserHost, CompassPredictionTranslationViewModel translations)
	{
		BrowserHost = browserHost;
		Translations = translations;

		Initialize();
	}

	public void Initialize()
	{
		MapList = BrowserHost.GetMapList().Result;

		Worlds = MapList.Keys.ToList();

		PropertyChanged += OnWorldChanged;
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
	}
}
