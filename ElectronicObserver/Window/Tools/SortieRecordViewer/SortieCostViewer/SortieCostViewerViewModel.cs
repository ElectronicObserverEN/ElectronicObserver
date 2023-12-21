using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Common;
using ElectronicObserver.Database;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;

public class SortieCostViewerViewModel : WindowViewModelBase
{
	public SortieCostViewerTranslationViewModel Translation { get; }
	public ToolService ToolService { get; }

	public ElectronicObserverContext Db { get; }
	public ObservableCollection<SortieRecordViewModel> Sorties { get; }
	public ObservableCollection<SortieCostViewModel> SortieCosts { get; } = new();

	public SortieCostModel? SortieCostSummary { get; private set; }

	public SortieCostViewerViewModel(ElectronicObserverContext db,
		ObservableCollection<SortieRecordViewModel> sorties)
	{
		Translation = Ioc.Default.GetRequiredService<SortieCostViewerTranslationViewModel>();
		ToolService = Ioc.Default.GetRequiredService<ToolService>();

		Db = db;
		Sorties = sorties;
	}

	public override void Loaded()
	{
		base.Loaded();

		CalculateCost();
	}

	private void CalculateCost()
	{
		foreach (SortieRecordViewModel sortie in Sorties)
		{
			sortie.Model.EnsureApiFilesLoaded(Db).Wait();
			SortieDetailViewModel? details = ToolService.GenerateSortieDetailViewModel(Db, sortie.Model);
			SortieCosts.Add(new(Db, sortie, details));
		}

		SortieCostSummary = SortieCosts
			.Select(c => c.TotalCost)
			.Aggregate(new SortieCostModel(), (a, b) => a + b);
	}
}
