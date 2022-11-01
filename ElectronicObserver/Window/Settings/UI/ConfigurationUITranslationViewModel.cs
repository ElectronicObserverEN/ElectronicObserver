using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;

namespace ElectronicObserver.Window.Settings.UI;

public class ConfigurationUITranslationViewModel : TranslationBaseViewModel
{
	public string MainFont => ConfigRes.Mainfont;
	public string SubFont => ConfigRes.Subfont;

	public string UI_BarColorMorphing => ConfigRes.UI_BarColorMorphing;
	public string UI_BarColorMorphingToolTip => Properties.Window.Dialog.DialogConfiguration.UI_BarColorMorphingToolTip;
	public string UI_IsLayoutFixed => Properties.Window.Dialog.DialogConfiguration.UI_IsLayoutFixed;
	public string UI_IsLayoutFixedToolTip => Properties.Window.Dialog.DialogConfiguration.UI_IsLayoutFixedToolTip;
	public string UI_RenderingTestToolTip => Properties.Window.Dialog.DialogConfiguration.UI_RenderingTestToolTip;


	public string UI_JapaneseShipTypes => Properties.Window.Dialog.DialogConfiguration.UI_JapaneseShipTypes;
	public string UI_JapaneseShipNames => Properties.Window.Dialog.DialogConfiguration.UI_JapaneseShipNames;
	public string UI_NodeNumbering => Properties.Window.Dialog.DialogConfiguration.UseLetterForNodes;
	public string UI_JapaneseEquipmentTypes => Properties.Window.Dialog.DialogConfiguration.UI_JapaneseEquipmentTypes;
	public string UI_JapaneseEquipmentNames => Properties.Window.Dialog.DialogConfiguration.UI_JapaneseEquipmentNames;
	public string UI_DisableOtherTranslations => Properties.Window.Dialog.DialogConfiguration.UI_DisableOtherTranslations;
	public string UI_DisableOtherTranslationsToolTip => Properties.Window.Dialog.DialogConfiguration.UI_DisableOtherTranslationsToolTip;

	public string Theme => Properties.Window.Dialog.DialogConfiguration.Theme;
	public string ThemeToolTip => Properties.Window.Dialog.DialogConfiguration.ThemeToolTip;
	public string Theme_Light => Properties.Window.Dialog.DialogConfiguration.Theme_Light;
	public string Theme_Dark => Properties.Window.Dialog.DialogConfiguration.Theme_Dark;
	public string Theme_Custom => Properties.Window.Dialog.DialogConfiguration.Theme_Custom;
	
	public string UI_LanguageLabel => Properties.Window.Dialog.DialogConfiguration.UI_LanguageLabel;
	public string Language_English => Properties.Window.Dialog.DialogConfiguration.Language_English;
	public string Language_Japanese => Properties.Window.Dialog.DialogConfiguration.Language_Japanese;

	public string UI_RestartHint => Properties.Window.Dialog.DialogConfiguration.UI_RestartHint;
}
