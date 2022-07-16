using MessagePack;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Conditions;

[Union(0, typeof(GroupConditionModel))]
#pragma warning disable CS0618 // needed for backward compatibility
[Union(1, typeof(ShipConditionModel))]
#pragma warning restore CS0618
[Union(2, typeof(ShipTypeConditionModel))]
#pragma warning disable CS0618 // needed for backward compatibility
[Union(3, typeof(PartialShipConditionModel))]
#pragma warning restore CS0618
[Union(4, typeof(AllowedShipTypesConditionModel))]
[Union(5, typeof(ShipPositionConditionModel))]
[Union(6, typeof(ShipNationalityConditionModel))]
[Union(7, typeof(ShipConditionModelV2))]
[Union(8, typeof(PartialShipConditionModelV2))]
public interface ICondition
{
}
