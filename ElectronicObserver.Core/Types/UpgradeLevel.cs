using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Core.Types;

public enum UpgradeLevel
{
	/// <summary>Unknown</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "Zero")]
	Zero = 0,

	/// <summary>1</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "One")]
	One = 1,

	/// <summary>2</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "Two")]
	Two = 2,

	/// <summary>3</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "Three")]
	Three = 3,

	/// <summary>4</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "Four")]
	Four = 4,

	/// <summary>5</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "Five")]
	Five = 5,

	/// <summary>6</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "Six")]
	Six = 6,

	/// <summary>7</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "Seven")]
	Seven = 7,

	/// <summary>8</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "Eight")]
	Eight = 8,

	/// <summary>9</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "Nine")]
	Nine = 9,

	/// <summary>Max</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "Max")]
	Max = 10,

	/// <summary>Equipment conversion</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "Conversion")]
	Conversion = 255
}
