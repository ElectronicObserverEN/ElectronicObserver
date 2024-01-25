using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
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
	public string? Progress { get; set; }

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

		_ = CalculateCost();
	}

	private async Task CalculateCost()
	{
		int index = 0;
		int total = Sorties.Count;

		Progress = $"{index}/{total}";

		await Task.Run(() =>
		{
			foreach (SortieRecordViewModel sortie in Sorties)
			{
				SortieCostViewModel sortieCost = new(Db, ToolService, SortieRecordMigrationService, sortie);

				App.Current!.Dispatcher.Invoke(() =>
				{
					SortieCosts.Add(sortieCost);

					index++;
					Progress = $"{index}/{total}";
				});
			}
		});

		SortieCostSummary = SortieCosts
			.Select(c => c.TotalCost)
			.Aggregate(new SortieCostModel(), (a, b) => a + b);

		Progress = null;
	}
}
