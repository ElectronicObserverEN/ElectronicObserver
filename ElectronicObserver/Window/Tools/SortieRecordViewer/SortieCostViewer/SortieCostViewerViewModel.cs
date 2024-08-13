using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
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

	public SortieCostConfigurationViewModel Configuration { get; }
	public ObservableCollection<SortieCostViewModel> SortieCosts { get; } = [];

	public SortieCostModel? SortieCostSummary { get; private set; }
	public int NormalDamage => SortieCosts.Sum(s => s.NormalDamage);
	public int LightDamage => SortieCosts.Sum(s => s.LightDamage);
	public int MediumDamage => SortieCosts.Sum(s => s.MediumDamage);
	public int HeavyDamage => SortieCosts.Sum(s => s.HeavyDamage);
	public int Buckets => SortieCosts.Sum(s => s.Buckets);
	public List<ConsumableItem> ConsumedItems { get; private set; } = [];

	public string? Progress { get; set; }

	private CancellationTokenSource CancellationTokenSource { get; } = new();

	public SortieCostViewerViewModel(ElectronicObserverContext db, ToolService toolService,
		SortieRecordMigrationService sortieRecordMigrationService, ObservableCollection<SortieRecordViewModel> sorties,
		SortieCostConfigurationViewModel configuration)
	{
		Translation = Ioc.Default.GetRequiredService<SortieCostViewerTranslationViewModel>();

		Db = db;
		ToolService = toolService;
		SortieRecordMigrationService = sortieRecordMigrationService;
		Sorties = sorties;
		Configuration = configuration;

		Configuration.PropertyChanged += ConfigurationOnPropertyChanged;
	}

	private void ConfigurationOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		OnPropertyChanged(nameof(Buckets));
	}

	public override void Loaded()
	{
		base.Loaded();

		_ = CalculateCost(CancellationTokenSource.Token);
	}

	public override void Closed()
	{
		base.Closed();

		CancellationTokenSource.Cancel();
	}

	private async Task CalculateCost(CancellationToken cancellationToken)
	{
		int index = 0;
		int total = Sorties.Count;

		Progress = $"{index}/{total}";

		await Task.Run(() =>
		{
			foreach (SortieRecordViewModel sortie in Sorties)
			{
				if (cancellationToken.IsCancellationRequested)
				{
					break;
				}

				SortieCostViewModel sortieCost = new(Db, ToolService, SortieRecordMigrationService, sortie, Configuration);

				App.Current!.Dispatcher.Invoke(() =>
				{
					SortieCosts.Add(sortieCost);

					index++;
					Progress = $"{index}/{total}";

					SortieCostSummary = SortieCosts
						.Select(c => c.TotalCost)
						.Sum();

					ConsumedItems = SortieCosts
						.SelectMany(c => c.ConsumedItems)
						.GroupBy(c => c.Id)
						.Select(g => new ConsumableItem(g.First().Equipment, g.Sum(c => c.Count)))
						.ToList();

					OnPropertyChanged(nameof(NormalDamage));
					OnPropertyChanged(nameof(LightDamage));
					OnPropertyChanged(nameof(MediumDamage));
					OnPropertyChanged(nameof(HeavyDamage));
					OnPropertyChanged(nameof(Buckets));
				});
			}
		}, cancellationToken);

		Progress = null;
	}
}
