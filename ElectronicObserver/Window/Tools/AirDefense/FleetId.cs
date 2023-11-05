using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Tools.AirDefense;

public enum FleetId
{
	[Display(ResourceType = typeof(AntiAirDefenseResources), Name = "FleetID_First")]
	FirstFleet = 1,
	[Display(ResourceType = typeof(AntiAirDefenseResources), Name = "FleetID_Second")]
	SecondFleet = 2,
	[Display(ResourceType = typeof(AntiAirDefenseResources), Name = "FleetID_Third")]
	ThirdFleet = 3,
	[Display(ResourceType = typeof(AntiAirDefenseResources), Name = "FleetID_Fourth")]
	FourthFleet = 4,
	[Display(ResourceType = typeof(AntiAirDefenseResources), Name = "FleetID_CombinedFleet")]
	CombinedFleet = 5,
}
