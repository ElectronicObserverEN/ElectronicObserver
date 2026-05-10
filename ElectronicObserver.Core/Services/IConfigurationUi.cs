namespace ElectronicObserver.Core.Services;

public interface IConfigurationUi
{
	bool JapaneseShipName { get; }
	bool JapaneseShipType { get; }
	bool JapaneseEquipmentName { get; }
	bool JapaneseEquipmentType { get; }
	bool DisableOtherTranslations { get; }
	bool UseOriginalNodeId { get; }
	string Culture { get; }
}
