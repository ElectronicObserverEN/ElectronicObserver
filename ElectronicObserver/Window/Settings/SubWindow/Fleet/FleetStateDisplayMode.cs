using System.ComponentModel.DataAnnotations;
using ElectronicObserver.Properties.Window.Dialog;

namespace ElectronicObserver.Window.Settings.SubWindow.Fleet;

public enum FleetStateDisplayMode
{
	[Display(ResourceType = typeof(DialogConfiguration), Name = "FormFleet_FleetStateDisplayMode_SingleStatus")]
	SingleStatus,
	
	[Display(ResourceType = typeof(DialogConfiguration), Name = "FormFleet_FleetStateDisplayMode_CollapseAll")]
	CollapseAll,

	[Display(ResourceType = typeof(DialogConfiguration), Name = "FormFleet_FleetStateDisplayMode_CollapseMultiple")]
	CollapseMultiple,

	[Display(ResourceType = typeof(DialogConfiguration), Name = "FormFleet_FleetStateDisplayMode_ExpandAll")]
	ExpandAll,
}
