using ElectronicObserver.Core.Services;

namespace ElectronicObserver.Avalonia.Samples.QuestTrackerManager;

public class ConfigurationUi : IConfigurationUi
{
	public bool JapaneseShipName => true;

	/// <inheritdoc />
	public bool JapaneseShipType => true;

	/// <inheritdoc />
	public bool JapaneseEquipmentName => true;

	/// <inheritdoc />
	public bool JapaneseEquipmentType => true;

	/// <inheritdoc />
	public bool DisableOtherTranslations => true;

	/// <inheritdoc />
	public bool UseOriginalNodeId => false;

	/// <inheritdoc />
	public string Culture => "ja-JP";
}
