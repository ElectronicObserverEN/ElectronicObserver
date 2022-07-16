using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Conditions;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

public class ConditionTemplateSelector : DataTemplateSelector
{
	public DataTemplate? Group { get; set; }
	public DataTemplate? ShipType { get; set; }
	public DataTemplate? Ship { get; set; }
	public DataTemplate? PartialShip { get; set; }
	public DataTemplate? AllowedShipTypes { get; set; }
	public DataTemplate? ShipPosition { get; set; }
	public DataTemplate? ShipNationality { get; set; }
	public DataTemplate? ShipV2 { get; set; }
	public DataTemplate? PartialShipV2 { get; set; }
	public DataTemplate? Unknown { get; set; }

	public override DataTemplate? SelectTemplate(object item, DependencyObject container) => item switch
	{
		GroupConditionViewModel => Group,
		ShipTypeConditionViewModel => ShipType,
#pragma warning disable CS0618 // needed for backward compatibility
		ShipConditionViewModel => Ship,
		PartialShipConditionViewModel => PartialShip,
#pragma warning restore CS0618
		AllowedShipTypesConditionViewModel => AllowedShipTypes,
		ShipPositionConditionViewModel => ShipPosition,
		ShipNationalityConditionViewModel => ShipNationality,
		ShipConditionViewModelV2 => ShipV2,
		PartialShipConditionViewModelV2 => PartialShipV2,

		_ => Unknown
	};
}
