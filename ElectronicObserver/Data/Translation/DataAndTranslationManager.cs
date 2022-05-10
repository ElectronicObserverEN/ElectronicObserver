using System.IO;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Data.Translation;

public class DataAndTranslationManager
{
	public static string WorkingFolder => Path.Combine(SoftwareUpdater.AppDataFolder, "DataAndTranslations");

	public static string DataFolder => Path.Combine(WorkingFolder, "Data");
	public static string TranslationFolder => Path.Combine(WorkingFolder, "Translations", CurrentTranslationLanguage);

	public static string CurrentTranslationLanguage => "en-US";

	public DestinationData Destination { get; private set; }
	public QuestTranslationData Quest { get; private set; }
	public EquipmentTranslationData Equipment { get; private set; }
	public MissionTranslationData Mission { get; private set; }
	public ShipTranslationData Ship { get; private set; }
	public OperationData Operation { get; private set; }

	public DataAndTranslationManager()
	{
		Initialize();
	}

	public void Initialize()
	{
		Destination = new DestinationData();
		Equipment = new EquipmentTranslationData();
		Mission = new MissionTranslationData();
		Operation = new OperationData();
		Quest = new QuestTranslationData();
		Ship = new ShipTranslationData();
	}


	public enum TranslationFile
	{
		destination,
		equipment,
		expedition,
		operation,
		quest,
		ship
	}
}
