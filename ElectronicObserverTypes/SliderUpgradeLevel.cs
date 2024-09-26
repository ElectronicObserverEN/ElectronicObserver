using System.ComponentModel.DataAnnotations;

namespace ElectronicObserverTypes;
public enum SliderUpgradeLevel
{
	/// <summary>Always</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "Always")]
	Always = 0,

	/// <summary>Starting from level 5</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "FromLevel5")]
	FromLevel5 = 5,

	/// <summary>Starting from level 6</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "FromLevel6")]
	FromLevel6 = 6,

	/// <summary>Starting from level 7</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "FromLevel7")]
	FromLevel7 = 7,

	/// <summary>Starting from level 8</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "FromLevel8")]
	FromLevel8 = 8,

	/// <summary>Starting from level 9</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "FromLevel9")]
	FromLevel9 = 9,

	/// <summary>Equipment conversion</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "Conversion")]
	ConversionOnly = 10,

	/// <summary>Never</summary>
	[Display(ResourceType = typeof(Properties.UpgradeLevel), Name = "Never")]
	Never = 256,
}
