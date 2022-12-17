using System;
using System.Linq;
using ElectronicObserver.Database.Sortie;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer;

public class SortieRecordViewModel
{
	public SortieRecord Model { get; }

	public int World => Model.World;
	public int Map => Model.Map;
	public DateTime SortieStart { get; }

	public SortieRecordViewModel(SortieRecord sortie)
	{
		Model = sortie;
		SortieStart = sortie.ApiFiles.Min(f => f.TimeStamp).ToLocalTime();
	}
}
