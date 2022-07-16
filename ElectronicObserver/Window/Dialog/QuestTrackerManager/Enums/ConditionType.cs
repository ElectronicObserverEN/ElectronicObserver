using System;
using System.ComponentModel.DataAnnotations;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;

public enum ConditionType
{
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "ConditionType_Group")]
	Group,
	[Obsolete("Use ShipV2")]
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "ConditionType_Ship")]
	Ship,
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "ConditionType_ShipType")]
	ShipType,
	[Obsolete("Use PartialShipV2")]
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "ConditionType_PartialShip")]
	PartialShip,
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "ConditionType_AllowedShipTypes")]
	AllowedShipTypes,
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "ConditionType_ShipPosition")]
	ShipPosition,
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "ConditionType_ShipNationality")]
	ShipNationality,
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "ConditionType_Ship")]
	ShipV2,
	[Display(ResourceType = typeof(Properties.Window.Dialog.QuestTrackerManager), Name = "ConditionType_PartialShip")]
	PartialShipV2,
}
