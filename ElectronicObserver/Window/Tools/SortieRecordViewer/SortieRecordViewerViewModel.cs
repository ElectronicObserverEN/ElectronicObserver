using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CsvHelper;
using CsvHelper.Configuration;
using ElectronicObserver.Common;
using ElectronicObserver.Common.ContentDialogs;
using ElectronicObserver.Common.ContentDialogs.ExportFilter;
using ElectronicObserver.Common.ContentDialogs.ExportProgress;
using ElectronicObserver.Common.Datagrid;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Database.DataMigration;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;
using Microsoft.EntityFrameworkCore;
using Calendar = System.Windows.Controls.Calendar;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer;

public partial class SortieRecordViewerViewModel : WindowViewModelBase
{
	private ToolService ToolService { get; }
	private FileService FileService { get; }
	private SortieRecordMigrationService SortieRecordMigrationService { get; }
	private ElectronicObserverContext Db { get; } = new();
	private DataExportHelper DataExportHelper { get; }

	public SortieRecordViewerTranslationViewModel SortieRecordViewer { get; }

	private static string AllRecords { get; } = "*";

	public List<object> Worlds { get; }
	public List<object> Maps { get; }

	public object World { get; set; } = AllRecords;
	public object Map { get; set; } = AllRecords;

	private DateTime DateTimeBegin =>
		new(DateBegin.Year, DateBegin.Month, DateBegin.Day, TimeBegin.Hour, TimeBegin.Minute, TimeBegin.Second);
	private DateTime DateTimeEnd =>
		new(DateEnd.Year, DateEnd.Month, DateEnd.Day, TimeEnd.Hour, TimeEnd.Minute, TimeEnd.Second);

	public DateTime DateBegin { get; set; }
	public DateTime TimeBegin { get; set; }
	public DateTime DateEnd { get; set; }
	public DateTime TimeEnd { get; set; }
	public DateTime MinDate { get; set; }
	public DateTime MaxDate { get; set; }

	public string Today => $"{DropRecordViewerResources.Today}: {DateTime.Now:yyyy/MM/dd}";

	public ObservableCollection<SortieRecordViewModel> Sorties { get; } = [];
	public SortieRecordViewModel? SelectedSortie { get; set; }
	public ObservableCollection<SortieRecordViewModel> SelectedSorties { get; set; } = [];

	public DataGridViewModel<SortieRecordViewModel> DataGridViewModel { get; }

	private DateTime SearchStartTime { get; set; }
	public string? StatusBarText { get; private set; }

	private ExportProgressViewModel? ExportProgress { get; set; }
	private ExportFilterViewModel ExportFilter { get; } = new();
	public ContentDialogService? ContentDialogService { get; set; }

	private SortieCostConfigurationViewModel SortieCostConfiguration { get; } = new();
	[ObservableProperty] private bool _showQuickExport;

	public bool IsDebug =>
#if DEBUG
		true;
#else
		false;
#endif

	public SortieRecordViewerViewModel()
	{
		ToolService = Ioc.Default.GetRequiredService<ToolService>();
		FileService = Ioc.Default.GetRequiredService<FileService>();
		SortieRecordMigrationService = Ioc.Default.GetRequiredService<SortieRecordMigrationService>();
		DataExportHelper = new(Db, ToolService);
		SortieRecordViewer = Ioc.Default.GetRequiredService<SortieRecordViewerTranslationViewModel>();

		Db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

		DataGridViewModel = new(Sorties);

		MinDate = Db.Sorties
			.Include(s => s.ApiFiles)
			.OrderBy(s => s.Id)
			.FirstOrDefault()
			?.ApiFiles.Min(f => f.TimeStamp)
			.ToLocalTime() ?? DateTime.Now;

		MaxDate = DateTime.Now.AddDays(1);

		DateBegin = MinDate.Date;
		DateEnd = MaxDate.Date;

		Worlds = Db.Worlds
			.Select(w => w.Id)
			.Distinct()
			.ToList()
			.Cast<object>()
			.Prepend(AllRecords)
			.ToList();

		Maps = Db.Maps
			.Select(m => m.MapId)
			.Distinct()
			.ToList()
			.Cast<object>()
			.Prepend(AllRecords)
			.ToList();

		SelectedSorties.CollectionChanged += OnSelectedSortiesOnCollectionChanged;
	}

	private void OnSelectedSortiesOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs args)
	{
		StatusBarText = string.Format(SortieRecordViewer.SelectedItems, SelectedSorties.Count, Sorties.Count);
	}

	public override void Closed()
	{
		base.Closed();

		DataGridViewModel.ItemsSource.Clear();
	}

	private static Func<ElectronicObserverContext, int?, int?, DateTime, DateTime, IAsyncEnumerable<SortieRecordViewModel>> SortieQuery { get; }
		= EF.CompileAsyncQuery((ElectronicObserverContext db, int? world, int? map, DateTime begin, DateTime end)
			=> db.Sorties
				.Where(s => world == null || s.World == world)
				.Where(s => map == null || s.Map == map)
				.OrderByDescending(s => s.Id)
				.Select(s => new { SortieRecord = s, s.ApiFiles.OrderBy(f => f.TimeStamp).First().TimeStamp, })
				.Where(s => s.TimeStamp > begin)
				.Where(s => s.TimeStamp < end)
				.Select(s => new SortieRecordViewModel(s.SortieRecord, s.TimeStamp)));

	[RelayCommand(IncludeCancelCommand = true)]
	private async Task Search(CancellationToken ct)
	{
		SearchStartTime = DateTime.UtcNow;
		StatusBarText = EncycloRes.SearchingNow;

		try
		{
			int? world = World as int?;
			int? map = Map as int?;
			DateTime begin = DateTimeBegin.ToUniversalTime();
			DateTime end = DateTimeEnd.ToUniversalTime();

			List<SortieRecordViewModel> sorties = await Task.Run(async () =>
				await SortieQuery(Db, world, map, begin, end).ToListAsync(ct), ct);

			Sorties.Clear();

			foreach (SortieRecordViewModel sortie in sorties)
			{
				Sorties.Add(sortie);
			}

			int searchTime = (int)(DateTime.UtcNow - SearchStartTime).TotalMilliseconds;

			StatusBarText = $"{EncycloRes.SearchComplete} ({searchTime} ms)";
		}
		catch (OperationCanceledException)
		{
			StatusBarText = EncycloRes.SearchCancelled;
		}
		catch (Exception e)
		{
			Logger.Add(2, $"Unknown error while loading data: {e.Message}{e.StackTrace}");
		}
	}

	[RelayCommand]
	private async Task CopyReplayLinkToClipboard()
	{
		if (SelectedSortie is null) return;

		await SelectedSortie.Model.EnsureApiFilesLoaded(Db);

		ToolService.CopyReplayLinkToClipboard(SelectedSortie);
	}

	[RelayCommand]
	private async Task CopyReplayDataToClipboard()
	{
		if (SelectedSortie is null) return;

		await SelectedSortie.Model.EnsureApiFilesLoaded(Db);

		ToolService.CopyReplayDataToClipboard(SelectedSortie);
	}

	[RelayCommand]
	private async Task OpenFleetImageGenerator()
	{
		if (SelectedSortie is null) return;

		int hqLevel = await SelectedSortie.Model.GetAdmiralLevel(Db) ?? KCDatabase.Instance.Admiral.Level;

		ToolService.FleetImageGenerator(SelectedSortie, hqLevel);
	}

	[RelayCommand]
	private static void SelectToday(Calendar? calendar)
	{
		if (calendar is null) return;

		calendar.SelectedDate = DateTime.Now.Date;
	}

	[RelayCommand]
	private async Task ShowSortieDetails()
	{
		if (SelectedSortie is null) return;

		await SelectedSortie.Model.EnsureApiFilesLoaded(Db);

		ToolService.OpenSortieDetail(Db, SelectedSortie.Model);
	}

	[RelayCommand]
	private async Task CopySmokerDataCsv()
	{
		foreach (SortieRecordViewModel sortieRecord in SelectedSorties)
		{
			await sortieRecord.Model.EnsureApiFilesLoaded(Db);
		}

		ToolService.CopySmokerDataCsv(Db, SelectedSorties);
	}

	[RelayCommand]
	private async Task CopySortieData()
	{
		await ToolService.CopySortieDataToClipboard(Db, SelectedSorties);
	}

	[RelayCommand]
	private void LoadSortieData()
	{
		ToolService.LoadSortieDataFromClipboard(Db);
	}

	[RelayCommand]
	private void CopyAirControlSimulatorLink()
	{
		if (SelectedSortie is null) return;

		ToolService.CopyAirControlSimulatorLink(SelectedSortie.Model);
	}

	[RelayCommand]
	private void OpenAirControlSimulator()
	{
		if (SelectedSortie is null) return;

		ToolService.AirControlSimulator(SelectedSortie.Model);
	}

	[RelayCommand]
	private void CopyOperationRoomLink()
	{
		if (SelectedSortie is null) return;

		ToolService.CopyOperationRoomLink(SelectedSortie.Model);
	}

	[RelayCommand]
	private void OpenOperationRoom()
	{
		if (SelectedSortie is null) return;

		ToolService.OperationRoom(SelectedSortie.Model);
	}

	[RelayCommand(IncludeCancelCommand = true)]
	private async Task ExportShellingBattle(CancellationToken cancellationToken)
	{
		await ExportCsv<ShellingBattleExportMap, ShellingBattleExportModel>(
			DataExportHelper.ShellingBattle,
			ExportShellingBattleCancelCommand,
			cancellationToken);
	}

	[RelayCommand(IncludeCancelCommand = true)]
	private async Task ExportNightBattle(CancellationToken cancellationToken)
	{
		await ExportCsv<NightBattleExportMap, NightBattleExportModel>(
			DataExportHelper.NightBattle,
			ExportNightBattleCancelCommand,
			cancellationToken);
	}

	[RelayCommand(IncludeCancelCommand = true)]
	private async Task ExportTorpedoBattle(CancellationToken cancellationToken)
	{
		await ExportCsv<TorpedoBattleExportMap, TorpedoBattleExportModel>(
			DataExportHelper.TorpedoBattle,
			ExportTorpedoBattleCancelCommand,
			cancellationToken);
	}

	[RelayCommand(IncludeCancelCommand = true)]
	private async Task ExportAirBattle(CancellationToken cancellationToken)
	{
		await ExportCsv<AirBattleExportMap, AirBattleExportModel>(
			DataExportHelper.AirBattle,
			ExportAirBattleCancelCommand,
			cancellationToken);
	}

	[RelayCommand(IncludeCancelCommand = true)]
	private async Task ExportAirBaseBattle(CancellationToken cancellationToken)
	{
		await ExportCsv<AirBaseBattleExportMap, AirBaseBattleExportModel>(
			DataExportHelper.AirBaseBattle,
			ExportAirBaseBattleCancelCommand,
			cancellationToken);
	}

	[RelayCommand(IncludeCancelCommand = true)]
	private async Task ExportRedShellingBattle(CancellationToken cancellationToken)
	{
		await ExportCsv<RedShellingBattleExportMap, ShellingBattleExportModel>(
			DataExportHelper.ShellingBattle,
			ExportRedShellingBattleCancelCommand,
			cancellationToken);
	}

	[RelayCommand(IncludeCancelCommand = true)]
	private async Task ExportRedNightBattle(CancellationToken cancellationToken)
	{
		await ExportCsv<RedNightBattleExportMap, NightBattleExportModel>(
			DataExportHelper.NightBattle,
			ExportRedNightBattleCancelCommand,
			cancellationToken);
	}

	[RelayCommand(IncludeCancelCommand = true)]
	private async Task ExportRedTorpedoBattle(CancellationToken cancellationToken)
	{
		await ExportCsv<RedTorpedoBattleExportMap, TorpedoBattleExportModel>(
			DataExportHelper.TorpedoBattle,
			ExportRedTorpedoBattleCancelCommand,
			cancellationToken);
	}

	[RelayCommand(IncludeCancelCommand = true)]
	private async Task ExportAirBaseAirDefense(CancellationToken cancellationToken)
	{
		await ExportCsv<AirBaseAirDefenseExportMap, AirBaseAirDefenseExportModel>(
			DataExportHelper.AirBaseAirDefense,
			ExportRedTorpedoBattleCancelCommand,
			cancellationToken);
	}

	[RelayCommand(IncludeCancelCommand = true)]
	private async Task ExportBattleRanks(CancellationToken cancellationToken)
	{
		await ExportCsv<BattleRanksExportMap, BattleRanksExportModel>(
			DataExportHelper.BattleRank,
			ExportRedTorpedoBattleCancelCommand,
			cancellationToken);
	}

	private async Task ExportCsv<TMap, TElement>(
		Func<ObservableCollection<SortieRecordViewModel>, ExportFilterViewModel?, ExportProgressViewModel, CancellationToken, Task<List<TElement>>> processData,
		ICommand cancellationCommand,
		CancellationToken cancellationToken = default
	) where TMap : ClassMap<TElement>
	{
		await App.Current!.Dispatcher.BeginInvoke(async () =>
		{
			ObservableCollection<SortieRecordViewModel> exportData = Sorties;

			if (exportData.Count <= 0) return;

			if (ContentDialogService is not null)
			{
				ExportFilter.Initialize(World, Map);

				_ = await ContentDialogService.ShowExportFilterAsync(ExportFilter);
			}

			string? path = FileService.ExportCsv(Configuration.Config.Life.CsvExportPath);

			if (string.IsNullOrEmpty(path)) return;

			Configuration.Config.Life.CsvExportPath = Path.GetDirectoryName(path);

			ExportProgress = new();

			Task<List<TElement>> processDataTask = Task.Run(async () => await
				processData(exportData, ExportFilter, ExportProgress, cancellationToken), cancellationToken);

			List<Task> tasks = [processDataTask];

			if (ContentDialogService is not null)
			{
				tasks.Add(ContentDialogService.ShowExportProgressAsync(ExportProgress));
			}

			await Task.WhenAny(tasks);

			try
			{
				if (processDataTask.IsCompleted)
				{
					List<TElement> dayShellingData = await processDataTask;
					await SaveCsvFile<TMap, TElement>(path, dayShellingData);
				}
				else
				{
					cancellationCommand.Execute(null);
				}

				ExportProgress = null;

				if (ContentDialogService is not null)
				{
					StatusBarText = CsvExportResources.CsvWasSavedSuccessfully;
				}
			}
			catch (Exception e)
			{
				if (ContentDialogService is not null)
				{
					await ContentDialogService.ShowNotificationAsync($"{CsvExportResources.FailedToSaveCsv}: {e.Message}{e.StackTrace}");
				}
			}
		});
	}

	private static async Task SaveCsvFile<TMap, TElement>(string path, IEnumerable<TElement> data)
		where TMap : ClassMap<TElement>
	{
		CsvConfiguration config = new(CultureInfo.CurrentCulture);

		// UTF8 BOM so excel can recognize it
		Encoding encoding = Encoding.UTF8;

		FileStreamOptions fileStreamOptions = new()
		{
			Access = FileAccess.Write,
			Mode = FileMode.Create,
		};

		await using StreamWriter writer = new(path, encoding, fileStreamOptions);
		await using CsvWriter csv = new(writer, config);

		csv.Context.RegisterClassMap<TMap>();
		await csv.WriteRecordsAsync(data);
	}

	[RelayCommand]
	private void OpenSortieCost()
	{
		if (SelectedSorties.Count <= 0) return;

		SortieCostViewerViewModel sortieCost = new(Db, ToolService, SortieRecordMigrationService, SelectedSorties, SortieCostConfiguration);

		new SortieCostViewerWindow(sortieCost).Show();
	}

	[RelayCommand]
	private async Task OpenLocalApiLoader()
	{
		if (!IsDebug) return;
		if (SelectedSortie is null) return;

		await SelectedSortie.Model.EnsureApiFilesLoaded(Db);

		new DialogLocalAPILoader2(SelectedSortie.Model).Show();
	}
}
