using ElectronicObserver.Core.Types;
using WanaKanaShaapu;

namespace ElectronicObserver.Avalonia.Services;

public class TransliterationService(IConfigurationUi configurationUi)
{
	private IConfigurationUi ConfigurationUi { get; } = configurationUi;

	private static Dictionary<ShipId, string> RomajiCache { get; } = new();

	public string GetRomajiName(IShipDataMaster ship)
	{
		if (RomajiCache.TryGetValue(ship.ShipId, out string? value))
		{
			return value;
		}

		string name = ship.IsAbyssalShip switch
		{
			true => ship.NameWithClass.ToLower(),
			_ => ship.NameReading,
		};
		value = WanaKana.ToRomaji(name);
		RomajiCache.Add(ship.ShipId, value);

		return value;
	}

	public bool Matches(IShipDataMaster ship, string filter)
	{
		bool literalSearch = ship.NameWithClass.Contains(filter, StringComparison.OrdinalIgnoreCase);

		if (!ship.IsAbyssalShip)
		{
			literalSearch |= ship.NameReading.Contains(filter, StringComparison.OrdinalIgnoreCase);
		}

		if (literalSearch)
		{
			return true;
		}

		// when using kanji to filter, only do a literal search
		// 神 matches 北上 if you use romaji compare
		if (WanaKana.IsKanji(filter)) return false;

		// abyssals should get matched with literal search only
		if (ship.IsAbyssalShip) return false;

		// when using English ship names, do literal searches only
		if (!ConfigurationUi.JapaneseShipName) return false;

		string romajiName = GetRomajiName(ship);

		bool result = romajiName.Contains(WanaKana.ToRomaji(filter), StringComparison.OrdinalIgnoreCase);

		return result;
	}

	public bool Matches(IEquipmentDataMaster equip, string filter) 
		=> Search(WanaKana.ToHiragana(filter.ToLower()), equip) || Search(WanaKana.ToHiragana(filter), equip);

	private static bool Search(string searchWord, IEquipmentDataMaster equip) 
		=> WanaKana.ToHiragana(equip.NameEN.ToLower()).Contains(searchWord);
}
