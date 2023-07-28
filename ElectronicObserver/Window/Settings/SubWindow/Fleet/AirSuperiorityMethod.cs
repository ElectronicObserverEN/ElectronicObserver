using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Settings.SubWindow.Fleet;

public enum AirSuperiorityMethod
{
	[Display(ResourceType = typeof(ConfigurationResources), Name = "FormFleet_AirSuperiorityMethod_Disabled")]
	Disabled,

	[Display(ResourceType = typeof(ConfigurationResources), Name = "FormFleet_AirSuperiorityMethod_Enabled")]
	Enabled,
}
