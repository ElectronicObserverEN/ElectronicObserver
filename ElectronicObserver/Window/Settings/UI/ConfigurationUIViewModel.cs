using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Converters;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Wpf;

namespace ElectronicObserver.Window.Settings.UI;

public partial class ConfigurationUIViewModel : ConfigurationViewModelBase
{
	public ConfigurationUITranslationViewModel Translation { get; }

	private Configuration.ConfigurationData.ConfigUI Config { get; }

	public List<FontFamily> AllFontFamilies { get; }

	public FontFamily MainFontFamily { get; set; }
	public int MainFontSize { get; set; }

	public FontFamily SubFontFamily { get; set; }
	public int SubFontSize { get; set; }

	public bool FontFamilyTextSearch { get; set; }

	public string Culture { get; set; }

	public bool JapaneseShipName { get; set; }

	public bool JapaneseShipType { get; set; }

	public bool JapaneseEquipmentName { get; set; }

	public bool JapaneseEquipmentType { get; set; }

	public bool DisableOtherTranslations { get; set; }

	public bool UseOriginalNodeId { get; set; }

	public ThemeMode ThemeMode { get; set; }

	public int ThemeID { get; set; }

	public int MaxAkashiPerHP { get; set; }

	public int DockingUnitTimeOffset { get; set; }

	public bool UseOldAircraftLevelIcons { get; set; }

	public bool TextWrapInLogWindow { get; set; }

	public bool CompactModeLogWindow { get; set; }

	public bool InvertedLogWindow { get; set; }

	public int HqResLowAlertFuel { get; set; }
	public int HqResLowAlertAmmo { get; set; }
	public int HqResLowAlertSteel { get; set; }
	public int HqResLowAlertBauxite { get; set; }
	public int HqResLowAlertBucket { get; set; }

	public bool AllowSortIndexing { get; set; }

	public bool BarColorMorphing { get; set; }

	public bool IsLayoutFixed { get; set; }

	public ConfigurationUIViewModel(Configuration.ConfigurationData.ConfigUI config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationUITranslationViewModel>();

		AllFontFamilies = Fonts.SystemFontFamilies.ToList();
		Config = config;
		Load(config);
	}

	private FontFamily FindFont(string name)
	{
		FontFamily? exactMatch = AllFontFamilies
			.FirstOrDefault(f => f.FamilyNames.Values?.Contains(name) == true);

		if (exactMatch is not null)
		{
			return exactMatch;
		}

		return AllFontFamilies.FirstOrDefault(f => f.FamilyNames.Values
			?.Any(name.StartsWith) == true) ?? new FontFamily("Meiryo UI");
	}

	private System.Drawing.Font? FindWinformsFont(FontFamily font, int size)
	{
		System.Drawing.Font NewFont(System.Drawing.FontFamily fontFamily, int size)
			=> new(fontFamily, size, System.Drawing.GraphicsUnit.Pixel);

		System.Drawing.FontFamily[] families = System.Drawing.FontFamily.Families;
		FontFamilyDisplayConverter converter = new();

		string fontFamilyName = (string)converter
			.Convert(font, typeof(string), null, null)!;


		System.Drawing.FontFamily? exactMatch = families
			.FirstOrDefault(f => f.Name == fontFamilyName);

		if (exactMatch is not null)
		{
			return NewFont(exactMatch, size);
		}

		System.Drawing.FontFamily? mainFontFamily = families
			.FirstOrDefault(f => f.Name.StartsWith(fontFamilyName));

		return mainFontFamily switch
		{
			null => null,
			_ => NewFont(mainFontFamily, size),
		};
	}

	private void Load(Configuration.ConfigurationData.ConfigUI config)
	{
		MainFontFamily = FindFont(config.MainFont.FontData.FontFamily.Name);
		MainFontSize = (int)config.MainFont.FontData.ToSize();
		SubFontFamily = FindFont(config.SubFont.FontData.FontFamily.Name);
		SubFontSize = (int)config.SubFont.FontData.ToSize();
		Culture = config.Culture;
		JapaneseShipName = config.JapaneseShipName;
		JapaneseShipType = config.JapaneseShipType;
		JapaneseEquipmentName = config.JapaneseEquipmentName;
		JapaneseEquipmentType = config.JapaneseEquipmentType;
		DisableOtherTranslations = config.DisableOtherTranslations;
		UseOriginalNodeId = config.UseOriginalNodeId;
		ThemeMode = (ThemeMode)config.ThemeMode;
		ThemeID = config.ThemeID;
		MaxAkashiPerHP = config.MaxAkashiPerHP;
		DockingUnitTimeOffset = config.DockingUnitTimeOffset;
		UseOldAircraftLevelIcons = config.UseOldAircraftLevelIcons;
		TextWrapInLogWindow = config.TextWrapInLogWindow;
		CompactModeLogWindow = config.CompactModeLogWindow;
		InvertedLogWindow = config.InvertedLogWindow;
		HqResLowAlertFuel = config.HqResLowAlertFuel;
		HqResLowAlertAmmo = config.HqResLowAlertAmmo;
		HqResLowAlertSteel = config.HqResLowAlertSteel;
		HqResLowAlertBauxite = config.HqResLowAlertBauxite;
		HqResLowAlertBucket = config.HqResLowAlertBucket;
		AllowSortIndexing = config.AllowSortIndexing;
		BarColorMorphing = config.BarColorMorphing;
		IsLayoutFixed = config.IsLayoutFixed;
		FontFamilyTextSearch = config.FontFamilyTextSearch;
	}

	public override void Save()
	{
		Config.MainFont = FindWinformsFont(MainFontFamily, MainFontSize) switch
		{
			{} font => font,
			_ => Config.MainFont,
		};
		Config.SubFont = FindWinformsFont(SubFontFamily, SubFontSize) switch
		{
			{ } font => font,
			_ => Config.SubFont,
		};
		Config.Culture = Culture;
		Config.JapaneseShipName = JapaneseShipName;
		Config.JapaneseShipType = JapaneseShipType;
		Config.JapaneseEquipmentName = JapaneseEquipmentName;
		Config.JapaneseEquipmentType = JapaneseEquipmentType;
		Config.DisableOtherTranslations = DisableOtherTranslations;
		Config.UseOriginalNodeId = UseOriginalNodeId;
		Config.ThemeMode = (int)ThemeMode;
		Config.ThemeID = ThemeID;
		Config.MaxAkashiPerHP = MaxAkashiPerHP;
		Config.DockingUnitTimeOffset = DockingUnitTimeOffset;
		Config.UseOldAircraftLevelIcons = UseOldAircraftLevelIcons;
		Config.TextWrapInLogWindow = TextWrapInLogWindow;
		Config.CompactModeLogWindow = CompactModeLogWindow;
		Config.InvertedLogWindow = InvertedLogWindow;
		Config.HqResLowAlertFuel = HqResLowAlertFuel;
		Config.HqResLowAlertAmmo = HqResLowAlertAmmo;
		Config.HqResLowAlertSteel = HqResLowAlertSteel;
		Config.HqResLowAlertBauxite = HqResLowAlertBauxite;
		Config.HqResLowAlertBucket = HqResLowAlertBucket;
		Config.AllowSortIndexing = AllowSortIndexing;
		Config.BarColorMorphing = BarColorMorphing;
		Config.IsLayoutFixed = IsLayoutFixed;
		Config.FontFamilyTextSearch = FontFamilyTextSearch;
	}

	[ICommand]
	private void SetTheme(ThemeMode? themeMode)
	{
		if (themeMode is not { } mode) return;

		ThemeMode = mode;
	}

	[ICommand]
	private void SetLanguage(string? language)
	{
		if (language is null) return;

		Culture = language;
	}
}
