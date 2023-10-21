using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Data;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.FleetImageGenerator;

public class AirBaseViewModel : ObservableObject
{
	public IBaseAirCorpsData? Model { get; set; }

	public string Name { get; set; } = "";
	private AirBaseActionKind ActionKind { get; set; }
	public string ActionKindDisplay => Constants.GetBaseAirCorpsActionKind(ActionKind);

	public int AirPower { get; set; }
	public int HighAltitudeAirPower { get; set; }
	public bool ShowHighAltitude => ActionKind == AirBaseActionKind.AirDefense;
	public int Range { get; set; }

	public ObservableCollection<EquipmentSlotViewModel> Squadrons { get; private set; } = new();

	public AirBaseViewModel Initialize(IBaseAirCorpsData? ab)
	{
		Model = ab;

		if (ab is null)
		{
			return this;
		}

		Name = ab.Name;
		ActionKind = ab.ActionKind;

		AirPower = Calculator.GetAirSuperiority(ab);
		HighAltitudeAirPower = Calculator.GetAirSuperiority(ab, isHighAltitude: true);
		Range = ab.Distance;

		Squadrons = ab.Squadrons.Values
			.Select(s => new EquipmentSlotViewModel(s?.EquipmentInstance, s?.AircraftMax ?? 0))
			.ToObservableCollection();

		return this;
	}
}
