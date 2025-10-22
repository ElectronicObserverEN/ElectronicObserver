using System.Collections.Generic;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Window.Tools.EquipmentList;

public class DetailCounter : IIdentifiable
{

	public int level;
	public int aircraftLevel;
	public int countAll;
	public int countRemain;
	public int countRemainPrev;

	public List<string> equippedShips;

	public DetailCounter(int lv, int aircraftLv)
	{
		level = lv;
		aircraftLevel = aircraftLv;
		countAll = 0;
		countRemainPrev = 0;
		countRemain = 0;
		equippedShips = new List<string>();
	}

	public static int CalculateID(int level, int aircraftLevel)
	{
		return level + aircraftLevel * 100;
	}

	public static int CalculateID(IEquipmentData eq)
	{
		return CalculateID(eq.Level, eq.AircraftLevel);
	}

	public int ID => CalculateID(level, aircraftLevel);
}
