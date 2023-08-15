﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using CsvHelper;
using CsvHelper.Configuration;
using ElectronicObserver.Common;
using ElectronicObserver.Common.ContentDialogs;
using ElectronicObserver.Common.ContentDialogs.ExportProgress;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Tools.SortieRecordViewer.DataExport;
using Microsoft.EntityFrameworkCore;
using Calendar = System.Windows.Controls.Calendar;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer;

public partial class SortieRecordViewerViewModel : WindowViewModelBase
{
	private ToolService ToolService { get; }
	private FileService FileService { get; }
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

	public ObservableCollection<SortieRecordViewModel> Sorties { get; } = new();

	public SortieRecordViewModel? SelectedSortie { get; set; }

	public ObservableCollection<SortieRecordViewModel> SelectedSorties { get; set; } = new();

	private DateTime SearchStartTime { get; set; }
	public string? StatusBarText { get; private set; }

	public ExportProgressViewModel? ExportProgress { get; set; }
	public ContentDialogService? ContentDialogService { get; set; }

	public SortieRecordViewerViewModel()
	{
		ToolService = Ioc.Default.GetRequiredService<ToolService>();
		FileService = Ioc.Default.GetRequiredService<FileService>();
		DataExportHelper = new(Db, ToolService);
		SortieRecordViewer = Ioc.Default.GetRequiredService<SortieRecordViewerTranslationViewModel>();

		Db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

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

		SelectedSorties.CollectionChanged += (sender, args) =>
		{
			StatusBarText = string.Format(SortieRecordViewer.SelectedItems, SelectedSorties.Count, Sorties.Count);
		};
	}

	[RelayCommand(IncludeCancelCommand = true)]
	private async Task Search(CancellationToken ct)
	{
		SearchStartTime = DateTime.UtcNow;
		StatusBarText = EncycloRes.SearchingNow;

		try
		{
			List<SortieRecordViewModel> sorties = await Task.Run(async () => await Db.Sorties
				.Where(s => World as string == AllRecords || s.World == World as int?)
				.Where(s => Map as string == AllRecords || s.Map == Map as int?)
				.OrderByDescending(s => s.Id)
				.Select(s => new { SortieRecord = s, s.ApiFiles.OrderBy(f => f.TimeStamp).First().TimeStamp, })
				.Where(s => s.TimeStamp > DateTimeBegin.ToUniversalTime())
				.Where(s => s.TimeStamp < DateTimeEnd.ToUniversalTime())
				.Select(s => new SortieRecordViewModel(s.SortieRecord, s.TimeStamp))
				.ToListAsync(ct), ct);

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

		ToolService.OpenSortieDetail(SelectedSortie);
	}

	[RelayCommand]
	private async Task CopySmokerDataCsv()
	{
		foreach (SortieRecordViewModel sortieRecord in SelectedSorties)
		{
			await sortieRecord.Model.EnsureApiFilesLoaded(Db);
		}

		ToolService.CopySmokerDataCsv(SelectedSorties);
	}

	[RelayCommand]
	private async Task CopySortieData()
	{
		if (SelectedSortie is null) return;

		await SelectedSortie.Model.EnsureApiFilesLoaded(Db);

		SortieRecord sortie = new()
		{
			Id = SelectedSortie.Id,
			World = SelectedSortie.World,
			Map = SelectedSortie.Map,
			ApiFiles = SelectedSortie.Model.ApiFiles
				.Where(f => f.ApiFileType is ApiFileType.Response || f.Name is "api_req_map/start")
				.ToList(),
			FleetData = SelectedSortie.Model.FleetData,
			MapData = SelectedSortie.Model.MapData,
		};

		Clipboard.SetText(JsonSerializer.Serialize(sortie));
	}

	[RelayCommand]
	private void LoadSortieData()
	{
		try
		{
			SortieRecord? sortie = JsonSerializer
				.Deserialize<SortieRecord>(Clipboard.GetText());

			if (sortie is null) return;

			ToolService.OpenSortieDetail(new SortieRecordViewModel(sortie, DateTime.UtcNow));
		}
		catch (Exception e)
		{
			Logger.Add(2, $"Failed to load sortie details: {e.Message}{e.StackTrace}");
		}
	}

	[RelayCommand]
	private void CopyAirControlSimulatorLink()
	{
		if (SelectedSortie is null) return;

		ToolService.CopyAirControlSimulatorLink(SelectedSortie);
	}

	[RelayCommand]
	private void OpenAirControlSimulator()
	{
		if (SelectedSortie is null) return;

		ToolService.AirControlSimulator(SelectedSortie);
	}

	[RelayCommand(IncludeCancelCommand = true)]
	private async Task ExportDayShelling(CancellationToken cancellationToken)
	{
		await ExportCsv<DayShellingExportMap, DayShellingExportModel>(DataExportHelper.DayShelling, cancellationToken);
	}

	[RelayCommand(IncludeCancelCommand = true)]
	private async Task ExportNightShelling(CancellationToken cancellationToken)
	{
		await ExportCsv<NightShellingExportMap, NightShellingExportModel>(DataExportHelper.NightShelling, cancellationToken);
	}

	[RelayCommand(IncludeCancelCommand = true)]
	private async Task ExportTorpedo(CancellationToken cancellationToken)
	{
		await ExportCsv<TorpedoExportMap, TorpedoExportModel>(DataExportHelper.Torpedo, cancellationToken);
	}

	private async Task ExportCsv<TMap, TElement>(
		Func<ObservableCollection<SortieRecordViewModel>, ExportProgressViewModel, CancellationToken, Task<List<TElement>>> processData,
		CancellationToken cancellationToken = default
	) where TMap : ClassMap<TElement>
	{
		await App.Current!.Dispatcher.BeginInvoke(async () =>
		{
			string? path = FileService.ExportCsv();

			if (string.IsNullOrEmpty(path)) return;

			ExportProgress = new();

			Task<List<TElement>> processDataTask = Task.Run(async () => await 
				processData(SelectedSorties, ExportProgress, cancellationToken), cancellationToken);

			List<Task> tasks = new() { processDataTask };

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
					ExportDayShellingCancelCommand.Execute(null);
				}

				ExportProgress = null;

				if (ContentDialogService is not null)
				{
					await ContentDialogService.ShowNotificationAsync(CsvExportResources.CsvWasSavedSuccessfully);
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

		await using StreamWriter writer = new(path);
		await using CsvWriter csv = new(writer, config);

		csv.Context.RegisterClassMap<TMap>();
		await csv.WriteRecordsAsync(data);
	}
}
