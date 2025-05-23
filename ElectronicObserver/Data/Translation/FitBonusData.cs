using System.Collections.Generic;
using ElectronicObserver.Core.Types.Serialization.FitBonus;

namespace ElectronicObserver.Data.Translation;

public class FitBonusData : TranslationBase
{
	private string DefaultFilePath = DataAndTranslationManager.DataFolder + @"\FitBonuses.json";

	public List<FitBonusPerEquipment> FitBonusList = new List<FitBonusPerEquipment>();

	public override void Initialize()
	{
		LoadDictionary(DefaultFilePath);
	}

	public FitBonusData()
	{
		Initialize();
	}

	private void LoadDictionary(string path)
	{
		FitBonusList.Clear();

		List<FitBonusPerEquipment>? json = Load<List<FitBonusPerEquipment>>(path);
		if (json != null) FitBonusList = json;
	}
}
