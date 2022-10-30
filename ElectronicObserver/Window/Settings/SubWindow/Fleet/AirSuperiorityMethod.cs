using System.ComponentModel.DataAnnotations;
using ElectronicObserver.Properties.Window.Dialog;

namespace ElectronicObserver.Window.Settings.SubWindow.Fleet;

public enum AirSuperiorityMethod
{
	[Display(ResourceType = typeof(DialogConfiguration), Name = "FormFleet_AirSuperiorityMethod_Disabled")]
	Disabled,

	[Display(ResourceType = typeof(DialogConfiguration), Name = "FormFleet_AirSuperiorityMethod_Enabled")]
	Enabled,
}
