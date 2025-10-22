using System.Collections.Generic;
using ElectronicObserver.Core.Types.Serialization.EquipmentUpgrade;

namespace ElectronicObserver.Data.Translation;

public class EquipmentUpgradeData : TranslationBase
{
	private string DefaultFilePath => DataAndTranslationManager.DataFolder + @"\EquipmentUpgrades.json";

	public List<EquipmentUpgradeDataModel> UpgradeList = new();

	public override void Initialize()
	{
		LoadDictionary(DefaultFilePath);
	}

	public EquipmentUpgradeData()
	{
		Initialize();
	}

	private void LoadDictionary(string path)
	{
		UpgradeList.Clear();

		List<EquipmentUpgradeDataModel>? json = Load<List<EquipmentUpgradeDataModel>>(path);
		if (json != null) UpgradeList = json;
	}
}
