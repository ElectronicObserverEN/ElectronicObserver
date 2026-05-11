namespace ElectronicObserver.Avalonia.Translation;

public static class DataConstants
{
	public static string AppDataFolder => Path.Join(
		Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
		"ElectronicObserver");

	public static string WorkingFolder => Path.Join(AppDataFolder, "DataAndTranslations");

	public static string DataFolder => Path.Join(WorkingFolder, "Data");

	public static string TranslationFolder(string currentLanguage) => Path.Join(
		WorkingFolder,
		"Translations",
		currentLanguage);
}
