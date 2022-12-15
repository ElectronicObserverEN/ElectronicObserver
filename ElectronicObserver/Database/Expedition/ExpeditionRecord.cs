using System.Collections.Generic;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.Database.Sortie;

namespace ElectronicObserver.Database.Expedition;

public class ExpeditionRecord
{
	public int Id { get; set; }
	public int Expedition { get; set; }
	public List<ApiFile> ApiFiles { get; set; } = new();
	public SortieFleet Fleet { get; set; } = new();
}
