using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Dialog.EquipmentPicker;

public class EquipmentPickerTranslationViewModel : TranslationBaseViewModel
{
	public string Level => Data.ShipGroup.ExpressionDataRes.Level;

	public string Name => EncycloRes.EquipName;

	public string AircraftLevel => EncycloRes.SkillLevel;
}
