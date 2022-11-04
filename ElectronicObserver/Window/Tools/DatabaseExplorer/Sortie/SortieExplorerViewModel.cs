using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Database;
using ElectronicObserver.Database.Sortie;
using Microsoft.EntityFrameworkCore;

namespace ElectronicObserver.Window.Tools.DatabaseExplorer.Sortie;

public partial class SortieExplorerViewModel : ObservableObject
{
	public int Limit { get; set; } = 20;

	public ObservableCollection<SortieRecord> Sorties { get; } = new();

	private ElectronicObserverContext Db { get; } = new();

	[ICommand]
	private void Search()
	{
		Sorties.Clear();

		IQueryable<SortieRecord> sorties = Db.Sorties
			.Include(s => s.ApiFiles)
			.AsQueryable()
			.OrderByDescending(f => f.Id)
			.Take(Limit);

		foreach (SortieRecord file in sorties)
		{
			Sorties.Add(file);
		}
	}
}
