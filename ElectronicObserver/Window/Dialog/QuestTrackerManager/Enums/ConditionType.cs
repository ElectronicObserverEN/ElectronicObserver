using System;
using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;

public enum ConditionType
{
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "ConditionType_Group")]
	Group,
	[Obsolete("Use ShipV2")]
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "ConditionType_Ship")]
	Ship,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "ConditionType_ShipType")]
	ShipType,
	[Obsolete("Use PartialShipV2")]
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "ConditionType_PartialShip")]
	PartialShip,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "ConditionType_AllowedShipTypes")]
	AllowedShipTypes,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "ConditionType_ShipPosition")]
	ShipPosition,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "ConditionType_ShipNationality")]
	ShipNationality,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "ConditionType_Ship")]
	ShipV2,
	[Display(ResourceType = typeof(QuestTrackerManagerResources), Name = "ConditionType_PartialShip")]
	PartialShipV2,
}
