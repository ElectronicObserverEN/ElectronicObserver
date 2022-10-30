using System.ComponentModel.DataAnnotations;
using ElectronicObserver.Window.Dialog;
using DialogConfiguration = ElectronicObserver.Properties.Window.Dialog.DialogConfiguration;

namespace ElectronicObserver.Window.Settings.SubWindow.Group;

public enum ShipNameSortMethod
{
	[Display(ResourceType = typeof(ConfigRes), Name = "ShipNameSortMethod_Number")]
	Library,

	[Display(ResourceType = typeof(ConfigRes), Name = "ShipNameSortMethod_Alphabet")]
	Alphabet,

	[Display(ResourceType = typeof(DialogConfiguration), Name = "ShipNameSortMethod_GameSort")]
	GameSort,
}
