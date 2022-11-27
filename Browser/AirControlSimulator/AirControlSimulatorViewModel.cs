using System;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;

namespace Browser.AirControlSimulator;

public partial class AirControlSimulatorViewModel : ObservableObject
{
	public AirControlSimulatorTranslationViewModel AirControlSimulator { get; }

	public string Uri { get; set; }
	public Action<string>? ExecuteScriptAsync { get; set; }

	public AirControlSimulatorViewModel()
	{
		AirControlSimulator = Ioc.Default.GetService<AirControlSimulatorTranslationViewModel>()!;
	}

	[RelayCommand]
	private async void UpdateFleet()
	{
		string? data = await BrowserViewModel.BrowserHost.GetFleetData();

		if(data is null) return;

		ExecuteScriptAsync?.Invoke($"loadDeckBuilder('{data}')");
	}

	[RelayCommand]
	private async void UpdateShips()
	{
		string data = await BrowserViewModel.BrowserHost.GetShipData();

		ExecuteScriptAsync?.Invoke($"loadShipData('{data}')");
	}

	[RelayCommand]
	private async void UpdateEquipment(bool? allEquipment)
	{
		if (allEquipment is not bool all) return;

		string data = await BrowserViewModel.BrowserHost.GetEquipmentData(all);

		ExecuteScriptAsync?.Invoke($"loadItemData('{data}')");
	}
}
