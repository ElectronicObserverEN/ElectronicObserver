using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BrowserLibCore;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver;
using ElectronicObserver.Data;
using ElectronicObserver.Services;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window;
using ElectronicObserver.Window.Tools.AirControlSimulator;
using MagicOnion.Server.Hubs;
using AirControlSimulatorViewModel = ElectronicObserver.Window.Tools.AirControlSimulator.AirControlSimulatorViewModel;

namespace BrowserHost;

public class BrowserHostHub : StreamingHubBase<IBrowserHost, IBrowser>, IBrowserHost
{
	private IGroup Browsers { get; set; }
	public IBrowser Browser => Broadcast(Browsers);

	public Task<BrowserConfiguration> Configuration()
	{
		return Task.Run(() => FormBrowserHost.Instance.ConfigurationCore);
	}

	public async Task ConnectToBrowser(long handle)
	{
		Browsers = await Group.AddAsync("browser");
		await Task.Run(() => FormBrowserHost.Instance.Connect(this));
		FormBrowserHost.Instance.ConnectToBrowser((IntPtr)handle);
	}

	public async Task SendErrorReport(string exceptionName, string message)
	{
		await Task.Run(() => FormBrowserHost.Instance.SendErrorReport(exceptionName, message));
	}

	public async Task AddLog(int priority, string message)
	{
		await Task.Run(() => FormBrowserHost.Instance.AddLog(priority, message));
	}

	public async Task ConfigurationUpdated(BrowserConfiguration configuration)
	{
		await Task.Run(() => FormBrowserHost.Instance.ConfigurationUpdated(configuration));
	}

	public async Task SetProxyCompleted()
	{
		await Task.Run(() => FormBrowserHost.Instance.SetProxyCompleted());
	}

	public async Task RequestNavigation(string v)
	{
		await Task.Run(() => FormBrowserHost.Instance.RequestNavigation(v));
	}
	public Task<byte[][]> GetIconResource()
	{
		return Task.Run(() => FormBrowserHost.Instance.GetIconResource());
	}

	public Task<bool> IsServerAlive()
	{
		return Task.Run(() => true);
	}

	public Task<int> GetTheme()
	{
		return Task.Run(() => FormBrowserHost.Instance.GetTheme());
	}

	public Task<string?> GetFleetData()
	{
		Task<string?> a = App.Current.Dispatcher.Invoke(() =>
		{
			DataSerializationService serialization = Ioc.Default.GetService<DataSerializationService>()!;

			BaseAirCorpsSimulationContentDialog dialog = new(new()
			{
				DataSelectionVisible = false,
			});

			if (dialog.ShowDialog(App.Current.MainWindow) is true)
			{
				AirControlSimulatorViewModel result = dialog.Result!;

				List<BaseAirCorpsData> bases = KCDatabase.Instance.BaseAirCorps.Values
					.Where(b => b.MapAreaID == result.AirBaseArea?.AreaId)
					.ToList();

				return Task.Run(() => serialization.DeckBuilder
				(
					KCDatabase.Instance.Admiral.Level,
					result.Fleet1 ? KCDatabase.Instance.Fleet[1] : null,
					result.Fleet2 ? KCDatabase.Instance.Fleet[2] : null,
					result.Fleet3 ? KCDatabase.Instance.Fleet[3] : null,
					result.Fleet4 ? KCDatabase.Instance.Fleet[4] : null,
					bases.Skip(0).FirstOrDefault(),
					bases.Skip(1).FirstOrDefault(),
					bases.Skip(2).FirstOrDefault(),
					null,
					result.MaxAircraftLevelFleet,
					result.MaxAircraftLevelAirBase
				));
			}

			return Task.Run(() => (string?)null);
		});

		return a;
	}

	public Task<string> GetShipData(bool allShips)
	{
		DataSerializationService service = Ioc.Default.GetService<DataSerializationService>()!;

		return Task.Run(() => service.FleetAnalysisShips(allShips));
	}

	public Task<string> GetEquipmentData(bool allEquipment)
	{
		DataSerializationService service = Ioc.Default.GetService<DataSerializationService>()!;

		return Task.Run(() => service.FleetAnalysisEquipment(allEquipment));
	}
}
