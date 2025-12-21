using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Settings.UI;

public class ConfigurationUITranslationViewModel : TranslationBaseViewModel
{
	public string MainFont => ConfigRes.Mainfont;
	public string SubFont => ConfigRes.Subfont;
	public string BrowserFont => ConfigRes.BrowserFont;
	public string TextSearch => ConfigRes.TextSearch;
	public string TextSearchToolTip => ConfigRes.TextSearchToolTip;
	public string MatchMainFont => ConfigRes.MatchMainFont;

	public string UI_BarColorMorphing => ConfigRes.UI_BarColorMorphing;
	public string UI_BarColorMorphingToolTip => ConfigurationResources.UI_BarColorMorphingToolTip;
	public string UI_IsLayoutFixed => ConfigurationResources.UI_IsLayoutFixed;
	public string UI_IsLayoutFixedToolTip => ConfigurationResources.UI_IsLayoutFixedToolTip;
	public string UI_RenderingTestToolTip => ConfigurationResources.UI_RenderingTestToolTip;


	public string UI_JapaneseShipTypes => ConfigurationResources.UI_JapaneseShipTypes;
	public string UI_JapaneseShipNames => ConfigurationResources.UI_JapaneseShipNames;
	public string UI_NodeNumbering => ConfigurationResources.UseLetterForNodes;
	public string UI_JapaneseEquipmentTypes => ConfigurationResources.UI_JapaneseEquipmentTypes;
	public string UI_JapaneseEquipmentNames => ConfigurationResources.UI_JapaneseEquipmentNames;
	public string UI_DisableOtherTranslations => ConfigurationResources.UI_DisableOtherTranslations;
	public string UI_DisableOtherTranslationsToolTip => ConfigurationResources.UI_DisableOtherTranslationsToolTip;

	public string Theme => ConfigurationResources.Theme;
	public string ThemeToolTip => ConfigurationResources.ThemeToolTip;
	public string Theme_Light => ConfigurationResources.Theme_Light;
	public string Theme_Dark => ConfigurationResources.Theme_Dark;
	public string Theme_Custom => ConfigurationResources.Theme_Custom;
	
	public string UI_LanguageLabel => ConfigurationResources.UI_LanguageLabel;
	public string Language_English => ConfigurationResources.Language_English;
	public string Language_Japanese => ConfigurationResources.Language_Japanese;
	public string Language_Korean => ConfigurationResources.Language_Korean;
	public string Language_SimplifiedChinese => ConfigurationResources.Language_SimplifiedChinese;
	public string Language_TraditionalChinese => ConfigurationResources.Language_TraditionalChinese;
	public string Language_Spanish => ConfigurationResources.Language_Spanish;

	public string UI_RestartHint => ConfigurationResources.UI_RestartHint;
}
