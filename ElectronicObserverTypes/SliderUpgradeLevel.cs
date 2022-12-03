using System.ComponentModel.DataAnnotations;

namespace ElectronicObserverTypes;
public enum SliderUpgradeLevel
{
	/// <summary>Never</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "Never")]
	Never = 0,

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
