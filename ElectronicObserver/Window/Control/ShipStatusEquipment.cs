using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Control;

/// <summary>
/// 装備改修レベル・艦載機熟練度の表示フラグ
/// </summary>
public enum LevelVisibilityFlag
{

	/// <summary> 非表示 </summary>
	[Display(ResourceType = typeof(ConfigRes), Name = "EquipmentLevelVisibility_Hidden")]
	Invisible,

	/// <summary> 改修レベルのみ </summary>
	[Display(ResourceType = typeof(ConfigRes), Name = "EquipmentLevelVisibility_ImprovOnly")]
	LevelOnly,

	/// <summary> 艦載機熟練度のみ </summary>
	[Display(ResourceType = typeof(ConfigRes), Name = "EquipmentLevelVisibility_ProfOnly")]
	AircraftLevelOnly,

	/// <summary> 改修レベル優先 </summary>
	[Display(ResourceType = typeof(ConfigRes), Name = "EquipmentLevelVisibility_ImprovPrio")]
	LevelPriority,

	/// <summary> 艦載機熟練度優先 </summary>
	[Display(ResourceType = typeof(ConfigRes), Name = "EquipmentLevelVisibility_ProfPrio")]
	AircraftLevelPriority,

	/// <summary> 両方表示 </summary>
	[Display(ResourceType = typeof(ConfigRes), Name = "EquipmentLevelVisibility_Both")]
	Both,

	/// <summary> 両方表示(艦載機熟練度はアイコンにオーバーレイする) </summary>
	[Display(ResourceType = typeof(ConfigurationResources), Name = "EquipmentLevelVisibility_OverlayProficiency")]
	AircraftLevelOverlay,
}
