using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Common.Datagrid;
using ElectronicObserver.Core.Types.Serialization.DeckBuilder;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Tools.FleetImageGenerator;
using ElectronicObserver.Window.Tools.SortieRecordViewer;
using Microsoft.EntityFrameworkCore;

namespace ElectronicObserver.Window.Tools.ExpeditionRecordViewer;

public partial class ExpeditionRecordViewerViewModel : WindowViewModelBase
{
	private ToolService ToolService { get; }

	private ElectronicObserverContext Db { get; } = new();

	public ExpeditionRecordViewerTranslationViewModel ExpeditionRecordViewer { get; }

	public List<object> Worlds { get; }
	public List<string> Missions { get; private set; } = new();

	private static string AllRecords { get; } = "*";
	public object World { get; set; } = AllRecords;
	public string Mission { get; set; } = AllRecords;

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

	public ObservableCollection<ExpeditionRecordViewModel> Expeditions { get; } = new();
	public ExpeditionRecordViewModel? SelectedExpedition { get; set; }
	public ObservableCollection<ExpeditionRecordViewModel> SelectedExpeditions { get; } = new();
	public ExpeditionSummary? ExpeditionSummary { get; private set; }

	public DataGridViewModel<ExpeditionRecordViewModel> DataGridViewModel { get; }

	private DateTime SearchStartTime { get; set; }
	public string? StatusBarText { get; private set; }

	public ExpeditionRecordViewerViewModel()
	{
		ToolService = Ioc.Default.GetRequiredService<ToolService>();
		ExpeditionRecordViewer = Ioc.Default.GetRequiredService<ExpeditionRecordViewerTranslationViewModel>();

		Db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

		DataGridViewModel = new(Expeditions);

		MinDate = Db.Expeditions
			.Include(s => s.ApiFiles)
			.OrderBy(s => s.Id)
			.FirstOrDefault()
			?.ApiFiles.Min(f => f.TimeStamp)
			.ToLocalTime() ?? DateTime.Now;

		MaxDate = DateTime.Now.AddDays(1);

		DateBegin = MinDate.Date;
		DateEnd = MaxDate.Date;

		Worlds = KCDatabase.Instance.Mission.Values
			.Select(w => w.MapAreaID)
			.Distinct()
			.Cast<object>()
			.Prepend(AllRecords)
			.ToList();

		PropertyChanged += (_, args) =>
		{
			if (args.PropertyName is not nameof(World)) return;

			Missions = KCDatabase.Instance.Mission.Values
				.Where(m => World as string == AllRecords || m.MapAreaID == World as int?)
				.OrderBy(m => m.SortID)
				.Select(m => m.DisplayID)
				.Distinct()
				.Prepend(AllRecords)
				.ToList();

			if (!Missions.Contains(Mission))
			{
				Mission = AllRecords;
			}
		};

		SelectedExpeditions.CollectionChanged += (_, _) =>
		{
			StatusBarText = string.Format(SortieRecordViewerResources.SelectedItems, SelectedExpeditions.Count, Expeditions.Count);
			ExpeditionSummary = SelectedExpeditions.Count switch
			{
				0 => null,
				_ => new(SelectedExpeditions),
			};
		};

		// generate missions
		OnPropertyChanged(nameof(World));
	}

	private static Func<ElectronicObserverContext, List<int>, DateTime, DateTime, IAsyncEnumerable<ExpeditionRecordViewModel>> ExpeditionQuery { get; }
		= EF.CompileAsyncQuery((ElectronicObserverContext db, List<int> expeditionIds, DateTime begin, DateTime end)
			=> db.Expeditions
				.Include(e => e.ApiFiles)
				.Where(e => expeditionIds.Contains(e.Expedition))
				.Select(s => new
				{
					Expedition = s,
					s.ApiFiles.OrderBy(f => f.TimeStamp).First().TimeStamp,
				})
				.Where(s => s.TimeStamp > begin)
				.Where(s => s.TimeStamp < end)
				.Where(s => s.Expedition.ApiFiles.Count > 0)
				.OrderByDescending(s => s.Expedition.Id)
				.Select(s => new ExpeditionRecordViewModel(s.Expedition, s.TimeStamp)));


	[RelayCommand(IncludeCancelCommand = true)]
	private async Task Search(CancellationToken ct)
	{
		SearchStartTime = DateTime.UtcNow;
		StatusBarText = EncycloRes.SearchingNow;

		try
		{
			List<int> expeditionIds = KCDatabase.Instance.Mission.Values
				.Where(m => Mission == AllRecords || m.DisplayID == Mission)
				.Where(m => World as string == AllRecords || m.MapAreaID == World as int?)
				.Select(m => m.ID)
				.ToList();

			DateTime begin = DateTimeBegin.ToUniversalTime();
			DateTime end = DateTimeEnd.ToUniversalTime();

			List<ExpeditionRecordViewModel> expeditions = await Task.Run(async () =>
				await ExpeditionQuery(Db, expeditionIds, begin, end).ToListAsync(ct), ct);

			Expeditions.Clear();

			foreach (ExpeditionRecordViewModel expedition in expeditions)
			{
				Expeditions.Add(expedition);
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
	private async Task OpenFleetImageGenerator()
	{
		if (SelectedExpedition is null) return;

		int hqLevel = await SelectedExpedition.Model.GetAdmiralLevel(Db) ?? KCDatabase.Instance.Admiral.Level;

		DeckBuilderData data = DataSerializationService.MakeDeckBuilderData(new()
		{
			HqLevel = hqLevel,
			Fleet1 = SelectedExpedition?.Model.Fleet.MakeFleet(0),
		});

		FleetImageGeneratorImageDataModel model = new()
		{
			Fleet1Visible = data.Fleet1 is not null,
			Fleet2Visible = data.Fleet2 is not null,
			Fleet3Visible = data.Fleet3 is not null,
			Fleet4Visible = data.Fleet4 is not null,
		};

		ToolService.FleetImageGenerator(model, data);
	}

	[RelayCommand]
	private static void SelectToday(Calendar? calendar)
	{
		if (calendar is null) return;

		calendar.SelectedDate = DateTime.Now.Date;
	}
}
