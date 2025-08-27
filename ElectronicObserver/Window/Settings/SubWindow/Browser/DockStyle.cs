using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Settings.SubWindow.Browser;

public enum DockStyle
{
	[Display(ResourceType = typeof(ConfigRes), Name = "ToolMenuAlignment_Top")]
	Top,

	[Display(ResourceType = typeof(ConfigRes), Name = "ToolMenuAlignment_Bottom")]
	Bottom,

	[Display(ResourceType = typeof(ConfigRes), Name = "ToolMenuAlignment_Left")]
	Left,

	[Display(ResourceType = typeof(ConfigRes), Name = "ToolMenuAlignment_Right")]
	Right,

	[Display(ResourceType = typeof(ConfigRes), Name = "ToolMenuAlignment_Invisible")]
	Hidden,
}
