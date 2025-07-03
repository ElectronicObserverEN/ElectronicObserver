using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Core.Services;
using ElectronicObserver.Data;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;

namespace ElectronicObserver.Window.Tools.AirControlSimulator;

// todo: rename to FleetDataExportViewModel (rename will probably break jot, so need to handle that too)
public partial class AirControlSimulatorViewModel : WindowViewModelBase
{
	private DataSerializationService DataSerializationService { get; }
	public AirControlSimulatorTranslationViewModel AirControlSimulator { get; }
	private IClipboardService ClipboardService { get; }

	public bool FleetSelectionVisible { get; set; } = true;
	public bool Fleet1 { get; set; } = true;
	public bool Fleet2 { get; set; }
	public bool Fleet3 { get; set; }
	public bool Fleet4 { get; set; }
	public bool MaxAircraftLevelFleet { get; set; } = true;

	public bool AirBaseSelectionVisible { get; set; } = true;
	public ObservableCollection<AirBaseArea> AirBaseAreas { get; } = new();
	public AirBaseArea? AirBaseArea { get; set; }
	public bool MaxAircraftLevelAirBase { get; set; } = true;

	public bool DataSelectionVisible { get; set; } = true;
	public bool ShipData { get; set; } = true;
	public bool IncludeUnlockedShips { get; set; } = true;
	public bool EquipmentData { get; set; } = true;
	public bool IncludeUnlockedEquipment { get; set; } = true;

	public bool ElectronicObserverBrowser { get; set; } = true;
	public bool SystemBrowser { get; set; }

	public bool? DialogResult { get; set; }

	private Func<AirControlSimulatorViewModel, SortieDetailViewModel?, string> GenerateLink { get; }

	public AirControlSimulatorViewModel(Func<AirControlSimulatorViewModel, SortieDetailViewModel?, string>? generateLink = null)
	{
		AirControlSimulator = Ioc.Default.GetRequiredService<AirControlSimulatorTranslationViewModel>();
		DataSerializationService = Ioc.Default.GetRequiredService<DataSerializationService>();
		ClipboardService = Ioc.Default.GetRequiredService<IClipboardService>();

		GenerateLink = generateLink ?? DataSerializationService.AirControlSimulatorLink;

		AirBaseAreas.Add(new(0, AirControlSimulator.None));

		IEnumerable<MapAreaData> maps = KCDatabase.Instance.BaseAirCorps.Values
			.Select(b => b.MapAreaID)
			.Distinct()
			.OrderBy(i => i)
			.Select(i => KCDatabase.Instance.MapArea[i])
			.Where(m => m != null);

		foreach (MapAreaData map in maps)
		{
			int mapAreaID = map.MapAreaID;
			string name = map.NameEN;

			if (string.IsNullOrWhiteSpace(map.NameEN) || map.NameEN == "※")
			{
				name = BaseAirCorpsSimulationResources.EventMap;
			}

			AirBaseAreas.Add(new(mapAreaID, name));
		}

		AirBaseArea = AirBaseAreas.MaxBy(b => b.AreaId);
	}

	[RelayCommand]
	private void Confirm() => DialogResult = true;

	[RelayCommand]
	private void Cancel() => DialogResult = false;

	[RelayCommand]
	private void CopyLink()
	{
		string link = GenerateLink(this, null);

		ClipboardService.SetTextAndLogErrors(link);
	}
}
