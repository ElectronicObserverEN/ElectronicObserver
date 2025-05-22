using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Utility.Data;
using WanaKanaNet;

namespace ElectronicObserver.Services;

public class TransliterationService
{
	private static Dictionary<ShipId, string> RomajiCache { get; } = new();

	public string GetRomajiName(IShipDataMaster ship)
	{
		if (!RomajiCache.ContainsKey(ship.ShipId))
		{
			string name = ship.IsAbyssalShip switch
			{
				true => ship.NameWithClass.ToLower(),
				_ => ship.NameReading,
			};

			RomajiCache.Add(ship.ShipId, WanaKana.ToRomaji(name));
		}

		return RomajiCache[ship.ShipId];
	}

	public bool Matches(IShipDataMaster ship, string filter, string romajiFilter)
	{
		bool literalSearch = ship.NameWithClass.ToLower().Contains(filter.ToLower());
		if (!ship.IsAbyssalShip)
		{
			literalSearch |= ship.NameReading.Contains(filter.ToLower());
		}

		if (literalSearch)
		{
			return true;
		}

		// when using kanji to filter, only do a literal search
		// 神 matches 北上 if you use romaji compare
		if (filter.ToCharArray().Any(WanaKana.IsKanji)) return false;
		// abyssals should get matched with literal search only
		if (ship.IsAbyssalShip) return false;
		// when using English ship names, do literal searches only
		if (!Utility.Configuration.Config.UI.JapaneseShipName) return false;

		string romajiName = GetRomajiName(ship);

		bool result = romajiName.Contains(romajiFilter.ToLower());

		return result;
	}

	public bool Matches(IEquipmentDataMaster equip, string filter)
	{
		bool Search(string searchWord) => Calculator.ToHiragana(equip.NameEN.ToLower()).Contains(searchWord);

		return Search(Calculator.ToHiragana(filter.ToLower())) || Search(Calculator.RomaToHira(filter));
	}
}
