using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Settings.SubWindow.Fleet;

public enum FleetStateDisplayMode
{
	[Display(ResourceType = typeof(ConfigurationResources), Name = "FormFleet_FleetStateDisplayMode_SingleStatus")]
	SingleStatus,
	
	[Display(ResourceType = typeof(ConfigurationResources), Name = "FormFleet_FleetStateDisplayMode_CollapseAll")]
	CollapseAll,

	[Display(ResourceType = typeof(ConfigurationResources), Name = "FormFleet_FleetStateDisplayMode_CollapseMultiple")]
	CollapseMultiple,

	[Display(ResourceType = typeof(ConfigurationResources), Name = "FormFleet_FleetStateDisplayMode_ExpandAll")]
	ExpandAll,
}
