using System;
using Browser.WebView2Browser.AirControlSimulator;
using BrowserLibCore;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

namespace Browser.CefSharpBrowser.AirControlSimulator;

public partial class AirControlSimulatorViewModel : ObservableObject
{
	public AirControlSimulatorTranslationViewModel AirControlSimulator { get; }

	public IBrowserHost BrowserHost { get; }

	public string Uri { get; set; }
	public Action<string>? ExecuteScriptAsync { get; set; }

	public AirControlSimulatorViewModel(IBrowserHost browserHost)
	{
		AirControlSimulator = Ioc.Default.GetService<AirControlSimulatorTranslationViewModel>()!;
		BrowserHost = browserHost;
	}

	[RelayCommand]
	private async void UpdateFleet()
	{
		string? data = await BrowserHost.GetFleetData();

		if(data is null) return;

		ExecuteScriptAsync?.Invoke($"loadDeckBuilder('{data}')");
	}

	[RelayCommand]
	private async void UpdateShips()
	{
		string data = await BrowserHost.GetShipData();

		ExecuteScriptAsync?.Invoke($"loadShipData('{data}')");
	}

	[RelayCommand]
	private async void UpdateEquipment(bool? allEquipment)
	{
		if (allEquipment is not bool all) return;

		string data = await BrowserHost.GetEquipmentData(all);

		ExecuteScriptAsync?.Invoke($"loadItemData('{data}')");
	}
}
