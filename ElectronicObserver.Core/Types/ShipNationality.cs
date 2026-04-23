using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Core.Types;

public enum ShipNationality
{
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "Unknown")]
	Unknown = 0,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "Japanese")]
	Japanese = 1,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "German")]
	German = 2,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "Italian")]
	Italian = 3,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "American")]
	American = 4,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "British")]
	British = 5,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "French")]
	French = 6,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "Russian")]
	Russian = 7,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "Thai")]
	Thai = 8,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "Norwegian")]
	Norwegian = 9,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "Swedish")]
	Swedish = 10,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "Dutch")]
	Dutch = 11,
	[Display(ResourceType = typeof(Properties.ShipNationality), Name = "Australian")]
	Australian = 12,
}
