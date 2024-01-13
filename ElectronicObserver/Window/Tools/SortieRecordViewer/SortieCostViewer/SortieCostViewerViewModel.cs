using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Common;
using ElectronicObserver.Database;
using ElectronicObserver.Database.DataMigration;
using ElectronicObserver.Services;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;

public class SortieCostViewerViewModel : WindowViewModelBase
{
	public SortieCostViewerTranslationViewModel Translation { get; }

	private ElectronicObserverContext Db { get; }
	private ToolService ToolService { get; }
	private SortieRecordMigrationService SortieRecordMigrationService { get; }
	private ObservableCollection<SortieRecordViewModel> Sorties { get; }
	public ObservableCollection<SortieCostViewModel> SortieCosts { get; } = [];

	public SortieCostModel? SortieCostSummary { get; private set; }

	public SortieCostViewerViewModel(ElectronicObserverContext db, ToolService toolService,
		SortieRecordMigrationService sortieRecordMigrationService, ObservableCollection<SortieRecordViewModel> sorties)
	{
		Translation = Ioc.Default.GetRequiredService<SortieCostViewerTranslationViewModel>();

		Db = db;
		ToolService = toolService;
		SortieRecordMigrationService = sortieRecordMigrationService;
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
			SortieCosts.Add(new(Db, ToolService, SortieRecordMigrationService, sortie));
		}

		SortieCostSummary = SortieCosts
			.Select(c => c.TotalCost)
			.Aggregate(new SortieCostModel(), (a, b) => a + b);
	}
}
