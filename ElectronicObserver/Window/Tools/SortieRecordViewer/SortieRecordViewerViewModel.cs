using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.KancolleApi.Types;
using ElectronicObserver.KancolleApi.Types.ApiPort.Port;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using Microsoft.EntityFrameworkCore;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer;

public partial class SortieRecordViewerViewModel : WindowViewModelBase
{
	private ToolService ToolService { get; }
	private ElectronicObserverContext Db { get; } = new();

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

	public SortieRecordViewerViewModel()
	{
		ToolService = Ioc.Default.GetRequiredService<ToolService>();
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
	private void CopyReplayLinkToClipboard()
	{
		if (SelectedSortie is null) return;

		EnsureApiFilesLoaded(SelectedSortie);

		ToolService.CopyReplayLinkToClipboard(SelectedSortie);
	}

	[RelayCommand]
	private void CopyReplayDataToClipboard()
	{
		if (SelectedSortie is null) return;

		EnsureApiFilesLoaded(SelectedSortie);

		ToolService.CopyReplayDataToClipboard(SelectedSortie);
	}

	[RelayCommand]
	private void OpenFleetImageGenerator()
	{
		if (SelectedSortie is null) return;

		int hqLevel = KCDatabase.Instance.Admiral.Level;

		if (SelectedSortie.Model.ApiFiles.Any())
		{
			// get the last port response right before the sortie started
			ApiFile? portFile = Db.ApiFiles
				.Where(f => f.ApiFileType == ApiFileType.Response)
				.Where(f => f.Name == "api_port/port")
				.Where(f => f.TimeStamp < SelectedSortie.Model.ApiFiles.First().TimeStamp)
				.OrderByDescending(f => f.TimeStamp)
				.FirstOrDefault();

			if (portFile is not null)
			{
				try
				{
					ApiPortPortResponse? port = JsonSerializer
						.Deserialize<ApiResponse<ApiPortPortResponse>>(portFile.Content)?.ApiData;

					if (port != null)
					{
						hqLevel = port.ApiBasic.ApiLevel;
					}
				}
				catch
				{
					// can probably ignore this
				}
			}
		}

		ToolService.FleetImageGenerator(SelectedSortie, hqLevel);
	}

	[RelayCommand]
	private static void SelectToday(Calendar? calendar)
	{
		if (calendar is null) return;

		calendar.SelectedDate = DateTime.Now.Date;
	}

	[RelayCommand]
	private void ShowSortieDetails()
	{
		if (SelectedSortie is null) return;

		EnsureApiFilesLoaded(SelectedSortie);

		ToolService.OpenSortieDetail(SelectedSortie);
	}

	[RelayCommand]
	private void CopySmokerDataCsv()
	{
		foreach (SortieRecordViewModel sortieRecord in SelectedSorties)
		{
			EnsureApiFilesLoaded(sortieRecord);
		}

		ToolService.CopySmokerDataCsv(SelectedSorties);
	}

	[RelayCommand]
	private void CopySortieData()
	{
		if (SelectedSortie is null) return;

		EnsureApiFilesLoaded(SelectedSortie);

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

	private void EnsureApiFilesLoaded(SortieRecordViewModel sortie)
	{
		if (sortie.Model.ApiFiles.Any()) return;

		sortie.Model.ApiFiles = Db.ApiFiles
			.Where(f => f.SortieRecordId == sortie.Model.Id)
			.ToList();
	}
}
