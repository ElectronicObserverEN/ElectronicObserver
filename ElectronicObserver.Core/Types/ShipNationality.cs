using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Core.Types;

public enum ShipNationality
{
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "Unknown")]
	Unknown,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "Japanese")]
	Japanese,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "German")]
	German,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "Italian")]
	Italian,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "American")]
	American,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "British")]
	British,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "French")]
	French,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "Russian")]
	Russian,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "Thai")]
	Thai,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "Norwegian")]
	Norwegian,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "Swedish")]
	Swedish,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "Dutch")]
	Dutch,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "Australian")]
	Australian,
}
