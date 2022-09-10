using System.Collections.Generic;
using System.Linq;
using System.Windows;
using ElectronicObserver.Data;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Tools.AirControlSimulator;
using ElectronicObserver.Window.Tools.ExpChecker;
using ElectronicObserver.Window.Tools.FleetImageGenerator;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Serialization.DeckBuilder;

namespace ElectronicObserver.Services;

public class ToolService
{
	private DataSerializationService DataSerializationService { get; }

	public ToolService(DataSerializationService dataSerializationService)
	{
		DataSerializationService = dataSerializationService;
	}


	public void AirControlSimulator(AirControlSimulatorViewModel? viewModel = null)
	{
		viewModel ??= new();

		BaseAirCorpsSimulationContentDialog dialog = new(viewModel);

		if (dialog.ShowDialog(App.Current.MainWindow) is not true) return;

		AirControlSimulatorViewModel result = dialog.Result!;

		List<BaseAirCorpsData> bases = KCDatabase.Instance.BaseAirCorps.Values
			.Where(b => b.MapAreaID == result.AirBaseArea?.AreaId)
			.ToList();

		IEnumerable<IEquipmentData> equipment = KCDatabase.Instance.Equipments.Values
			.Where(e => result.AllEquipment || e.IsLocked);

		string airControlSimulatorData = DataSerializationService.AirControlSimulator
		(
			KCDatabase.Instance.Admiral.Level,
			result.Fleet1 ? KCDatabase.Instance.Fleet.Fleets.Values.Skip(0).FirstOrDefault() : null,
			result.Fleet2 ? KCDatabase.Instance.Fleet.Fleets.Values.Skip(1).FirstOrDefault() : null,
			result.Fleet3 ? KCDatabase.Instance.Fleet.Fleets.Values.Skip(2).FirstOrDefault() : null,
			result.Fleet4 ? KCDatabase.Instance.Fleet.Fleets.Values.Skip(3).FirstOrDefault() : null,
			bases.Skip(0).FirstOrDefault(),
			bases.Skip(1).FirstOrDefault(),
			bases.Skip(2).FirstOrDefault(),
			result.ShipData ? KCDatabase.Instance.Ships.Values : null,
			result.EquipmentData ? equipment : null,
			result.MaxAircraftLevelFleet,
			result.MaxAircraftLevelAirBase
		);

		string url = @$"https://noro6.github.io/kc-web#import:{airControlSimulatorData}";

		Window.FormBrowserHost.Instance.Browser.OpenAirControlSimulator(url);

		// todo: this doesn't work and I don't feel like making a workaround right now
		// https://stackoverflow.com/a/3114737
		if (result.SystemBrowser)
		{
			/*
				ProcessStartInfo psi = new()
				{
					FileName = url,
					UseShellExecute = true
				};

				Process.Start(psi);
				*/
		}
	}

	public void FleetImageGenerator(FleetImageGeneratorImageDataModel? model = null)
	{
		if (!KCDatabase.Instance.Ships.Any())
		{
			MessageBox.Show
			(
				Properties.Window.Dialog.DialogExpChecker.NoShipsAvailable,
				Properties.Window.Dialog.DialogExpChecker.ShipsUnavailable,
				MessageBoxButton.OK,
				MessageBoxImage.Error
			);

			return;
		}

		DeckBuilderData data = DataSerializationService.MakeDeckBuilderData
		(
			KCDatabase.Instance.Admiral.Level,
			KCDatabase.Instance.Fleet.Fleets[1],
			KCDatabase.Instance.Fleet.Fleets[2],
			KCDatabase.Instance.Fleet.Fleets[3],
			KCDatabase.Instance.Fleet.Fleets[4]
		);

		model ??= new()
		{
			Fleet1Visible = true,
			Fleet2Visible = KCDatabase.Instance.Fleet.CombinedFlag > 0,
		};

		model.DeckBuilderData = data;

		new FleetImageGeneratorWindow(model).Show(App.Current.MainWindow);
	}

	/// <summary>
	/// Generates deck builder json of data that was selected for export.
	/// </summary>
	/// <returns>null if data selection is canceled, deck builder data otherwise</returns>
	public string? DeckBuilderFleetExport(AirControlSimulatorViewModel? viewModel = null)
	{
		viewModel ??= new();

		BaseAirCorpsSimulationContentDialog dialog = new(viewModel);

		if (dialog.ShowDialog(App.Current.MainWindow) is not true) return null;

		AirControlSimulatorViewModel result = dialog.Result!;

		List<BaseAirCorpsData> bases = KCDatabase.Instance.BaseAirCorps.Values
			.Where(b => b.MapAreaID == result.AirBaseArea?.AreaId)
			.ToList();

		return DataSerializationService.DeckBuilder
		(
			KCDatabase.Instance.Admiral.Level,
			result.Fleet1 ? KCDatabase.Instance.Fleet.Fleets.Values.Skip(0).FirstOrDefault() : null,
			result.Fleet2 ? KCDatabase.Instance.Fleet.Fleets.Values.Skip(1).FirstOrDefault() : null,
			result.Fleet3 ? KCDatabase.Instance.Fleet.Fleets.Values.Skip(2).FirstOrDefault() : null,
			result.Fleet4 ? KCDatabase.Instance.Fleet.Fleets.Values.Skip(3).FirstOrDefault() : null,
			bases.Skip(0).FirstOrDefault(),
			bases.Skip(1).FirstOrDefault(),
			bases.Skip(2).FirstOrDefault(),
			result.MaxAircraftLevelFleet,
			result.MaxAircraftLevelAirBase
		);
	}

	public void ExpChecker(ExpCheckerViewModel? viewModel = null)
	{
		if (!KCDatabase.Instance.Ships.Any())
		{
			MessageBox.Show
			(
				Properties.Window.Dialog.DialogExpChecker.NoShipsAvailable,
				Properties.Window.Dialog.DialogExpChecker.ShipsUnavailable,
				MessageBoxButton.OK,
				MessageBoxImage.Error
			);

			return;
		}

		viewModel ??= new();

		new ExpCheckerWindow(viewModel).Show(App.Current.MainWindow);
	}
}
