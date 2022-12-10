using System.Collections.Generic;

namespace ElectronicObserver.Database.Sortie;

public class SortieFleet
{
	public string Name { get; set; } = "";
	public List<SortieShip> Ships { get; set; } = new();
}
