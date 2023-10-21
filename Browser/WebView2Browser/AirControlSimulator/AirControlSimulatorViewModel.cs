using System;
using System.Threading.Tasks;
using BrowserLibCore;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

namespace Browser.WebView2Browser.AirControlSimulator;

public partial class AirControlSimulatorViewModel : ObservableObject
{
	public AirControlSimulatorTranslationViewModel AirControlSimulator { get; }

	private IBrowserHost BrowserHost { get; }

	public string Uri { get; set; }
	public Action<string>? ExecuteScriptAsync { get; set; }

	public AirControlSimulatorViewModel(IBrowserHost browserHost)
	{
		AirControlSimulator = Ioc.Default.GetService<AirControlSimulatorTranslationViewModel>()!;
		BrowserHost = browserHost;
	}

	[RelayCommand]
	private async Task UpdateFleet()
	{
		string? data = await BrowserHost.GetFleetData();

		if(data is null) return;

		ExecuteScriptAsync?.Invoke($"loadDeckBuilder('{data}')");
	}

	[RelayCommand]
	private async Task UpdateShips(bool? allShips)
	{
		if (allShips is not bool all) return;

		string data = await BrowserHost.GetShipData(all);

		ExecuteScriptAsync?.Invoke($"loadShipData('{data}')");
	}

	[RelayCommand]
	private async Task UpdateEquipment(bool? allEquipment)
	{
		if (allEquipment is not bool all) return;

		string data = await BrowserHost.GetEquipmentData(all);

		ExecuteScriptAsync?.Invoke($"loadItemData('{data}')");
	}
}
