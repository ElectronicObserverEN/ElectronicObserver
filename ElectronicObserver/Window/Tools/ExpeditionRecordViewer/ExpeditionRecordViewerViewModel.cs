using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Database.Expedition;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.KancolleApi.Types;
using ElectronicObserver.KancolleApi.Types.ApiPort.Port;
using ElectronicObserver.KancolleApi.Types.ApiReqMission.Result;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Control.Paging;
using ElectronicObserver.Window.Tools.FleetImageGenerator;
using ElectronicObserver.Window.Tools.SortieRecordViewer;
using ElectronicObserverTypes.Serialization.DeckBuilder;
using Microsoft.EntityFrameworkCore;

namespace ElectronicObserver.Window.Tools.ExpeditionRecordViewer;

public partial class ExpeditionRecordViewerViewModel : WindowViewModelBase
{
	private ToolService ToolService { get; }
	private DataSerializationService DataSerializationService { get; }

	private ElectronicObserverContext Db { get; } = new();

	public SortieRecordViewerTranslationViewModel SortieRecordViewer { get; }

	private static string AllRecords { get; } = "*";
	public List<object> Missions { get; }
	public List<object> Worlds { get; }
	public object Mission { get; set; } = AllRecords;
	public object World { get; set; } = AllRecords;
	public PagingControlViewModel ExpeditionPager { get; }

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

	public ExpeditionRecordViewerViewModel()
	{
		ToolService = Ioc.Default.GetRequiredService<ToolService>();
		DataSerializationService = Ioc.Default.GetRequiredService<DataSerializationService>();
		SortieRecordViewer = Ioc.Default.GetRequiredService<SortieRecordViewerTranslationViewModel>();

		Db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
		ExpeditionPager = new();
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
			.ToList()
			.Cast<object>()
			.Prepend(AllRecords)
			.ToList();
		Missions = KCDatabase.Instance.Mission.Values
			.OrderBy(m => m.SortID)
			.Select(m => m.DisplayID)
			.Distinct()
			.ToList()
			.Cast<object>()
			.Prepend(AllRecords)
			.ToList();
	}

	[RelayCommand]
	private void Search()
	{
		Expeditions.Clear();

		ExpeditionPager.Items = Db.Expeditions
			.Include(e => e.ApiFiles)
			.Select(s => new
			{
				Expedition = s,
				s.ApiFiles.OrderBy(f => f.TimeStamp).First().TimeStamp,
			})
			.Where(s => s.TimeStamp > DateTimeBegin.ToUniversalTime())
			.Where(s => s.TimeStamp < DateTimeEnd.ToUniversalTime())
			.Where(s => s.Expedition.ApiFiles.Count > 0)
			.AsEnumerable()
			.Select(s => (s.Expedition, Response: ParseExpeditionResult(s.Expedition), s.TimeStamp))
			.Where(s => s.Response is not null)
			.Select(s => new ExpeditionRecordViewModel(s.Expedition, s.Response!, s.TimeStamp))
			.Where(s => Mission as string == AllRecords || s.DisplayID == (string)Mission)
			.Where(s => World as string == AllRecords || s.MapAreaID == World as int?)
			.OrderByDescending(s => s.Id)
			.Cast<object>()
			.ToList();
	}

	private static ApiReqMissionResultResponse? ParseExpeditionResult(ExpeditionRecord record)
	{
		try
		{
			ApiFile? apiFile = record.ApiFiles
				.Where(f => f.ApiFileType == ApiFileType.Response)
				.FirstOrDefault(f => f.Name == "api_req_mission/result");

			return apiFile switch
			{
				null => null,
				_ => JsonSerializer.Deserialize<ApiResponse<ApiReqMissionResultResponse>>(apiFile.Content)?.ApiData,
			};
		}
		catch
		{
			return null;
		}
	}

	[RelayCommand]
	private void OpenFleetImageGenerator(ExpeditionRecordViewModel? expedition)
	{
		if (expedition is null) return;

		int hqLevel = KCDatabase.Instance.Admiral.Level;

		if (expedition.Model.ApiFiles.Any())
		{
			// get the last port response right before the sortie started
			ApiFile? portFile = Db.ApiFiles
				.Where(f => f.ApiFileType == ApiFileType.Response)
				.Where(f => f.Name == "api_port/port")
				.Where(f => f.TimeStamp < expedition.Model.ApiFiles.First().TimeStamp)
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

		DeckBuilderData data = DataSerializationService.MakeDeckBuilderData
		(
			hqLevel,
			expedition?.Model.Fleet.MakeFleet(0)
		);

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
