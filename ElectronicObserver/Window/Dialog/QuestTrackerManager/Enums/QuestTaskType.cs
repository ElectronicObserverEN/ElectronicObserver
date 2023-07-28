using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;

public enum QuestTaskType
{
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "QuestTaskType_BossKill")]
	BossKill,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "QuestTaskType_Expedition")]
	Expedition,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "QuestTaskType_BattleNodeId")]
	BattleNodeId,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "QuestTaskType_7_2_1")]
	World7Map2Boss1,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "QuestTaskType_7_2_2")]
	World7Map2Boss2,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "QuestTaskType_7_3_1")]
	World7Map3Boss1,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "QuestTaskType_7_3_2")]
	World7Map3Boss2,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "QuestTaskType_7_5_1")]
	World7Map5Boss1,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "QuestTaskType_7_5_2")]
	World7Map5Boss2,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "QuestTaskType_7_5_3")]
	World7Map5Boss3,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "QuestTaskType_EquipmentScrap")]
	EquipmentScrap,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "QuestTaskType_EquipmentCategoryScrap")]
	EquipmentCategoryScrap,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "QuestTaskType_EquipmentCardTypeScrap")]
	EquipmentCardTypeScrap,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "QuestTaskType_EquipmentIconTypeScrap")]
	EquipmentIconTypeScrap,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "QuestTaskType_NodeReach")]
	NodeReach,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "QuestTaskType_MapFirstClear")]
	MapFirstClear,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "QuestTaskType_Exercise")]
	Exercise,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "QuestTaskType_World1Map6ResourceNode")]
	World1Map6ResourceNode,
}
