using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using ElectronicObserver.Data;
using ElectronicObserver.Database;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.KancolleApi.Types.ApiReqCombinedBattle.Battleresult;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Start;
using ElectronicObserver.KancolleApi.Types.Interfaces;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Tools.AirControlSimulator;
using ElectronicObserver.Window.Tools.EquipmentUpgradePlanner;
using ElectronicObserver.Window.Tools.ExpChecker;
using ElectronicObserver.Window.Tools.FleetImageGenerator;
using ElectronicObserver.Window.Tools.SortieRecordViewer;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Replay;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Attacks;
using ElectronicObserverTypes.Serialization.DeckBuilder;
using DayAttack = ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase.DayAttack;

namespace ElectronicObserver.Services;

public class ToolService(DataSerializationService dataSerializationService)
{
	private DataSerializationService DataSerializationService { get; } = dataSerializationService;

	public void AirControlSimulator(AirControlSimulatorViewModel? viewModel = null,
		SortieDetailViewModel? sortieDetail = null)
	{
		viewModel ??= new();

		BaseAirCorpsSimulationContentDialog dialog = new(viewModel);

		if (dialog.ShowDialog(App.Current!.MainWindow!) is not true) return;

		AirControlSimulatorViewModel result = dialog.Result!;

		string url = DataSerializationService.AirControlSimulatorLink(result, sortieDetail);

		Window.FormBrowserHost.Instance.Browser.OpenAirControlSimulator(url);

		// todo: this doesn't work and I don't feel like making a workaround right now
		// https://stackoverflow.com/a/3114737
		if (result.SystemBrowser)
		{
			/*
				ProcessStartInfo psi = new()
				{
					FileName = url,
					UseShellExecute = true
				};

				Process.Start(psi);
				*/
		}
	}

	public void AirControlSimulator(SortieRecord sortie)
	{
		string url = GetAirControlSimulatorLink(sortie);

		Window.FormBrowserHost.Instance.Browser.OpenAirControlSimulator(url);
	}

	public void CompassPrediction()
	{
		Window.FormBrowserHost.Instance.Browser.OpenCompassPrediction();
	}

	public void OperationRoom(AirControlSimulatorViewModel? viewModel = null)
	{
		viewModel ??= new(DataSerializationService.OperationRoomLink);

		viewModel.DataSelectionVisible = false;

		BaseAirCorpsSimulationContentDialog dialog = new(viewModel);

		if (dialog.ShowDialog(App.Current!.MainWindow!) is not true) return;

		AirControlSimulatorViewModel result = dialog.Result!;

		string url = DataSerializationService.OperationRoomLink(result);

		ProcessStartInfo psi = new()
		{
			FileName = url,
			UseShellExecute = true,
		};

		Process.Start(psi);
	}

	public void OperationRoom(SortieRecord sortie)
	{
		string url = GetOperationRoomLink(sortie);

		ProcessStartInfo psi = new()
		{
			FileName = url,
			UseShellExecute = true,
		};

		Process.Start(psi);
	}

	public void FleetImageGenerator(FleetImageGeneratorImageDataModel? model = null, DeckBuilderData? data = null)
	{
		if (!KCDatabase.Instance.Ships.Any())
		{
			MessageBox.Show
			(
				ExpCheckerResources.NoShipsAvailable,
				ExpCheckerResources.ShipsUnavailable,
				MessageBoxButton.OK,
				MessageBoxImage.Error
			);

			return;
		}

		data ??= DataSerializationService.MakeDeckBuilderData(new()
		{
			HqLevel = KCDatabase.Instance.Admiral.Level,
			Fleet1 = KCDatabase.Instance.Fleet.Fleets[1],
			Fleet2 = KCDatabase.Instance.Fleet.Fleets[2],
			Fleet3 = KCDatabase.Instance.Fleet.Fleets[3],
			Fleet4 = KCDatabase.Instance.Fleet.Fleets[4],
		});

		model ??= new()
		{
			Fleet1Visible = true,
			Fleet2Visible = KCDatabase.Instance.Fleet.CombinedFlag > 0,
		};

		model.DeckBuilderData = data;

		new FleetImageGeneratorWindow(model).Show(App.Current.MainWindow);
	}

	public void FleetImageGenerator(SortieRecordViewModel selectedSortie, int hqLevel)
	{
		List<IFleetData?> fleets = selectedSortie.Model.FleetData.MakeFleets();

		DeckBuilderData data = DataSerializationService.MakeDeckBuilderData(new()
		{
			HqLevel = hqLevel,
			Fleet1 = fleets.Skip(0).FirstOrDefault(),
			Fleet2 = fleets.Skip(1).FirstOrDefault(),
			Fleet3 = fleets.Skip(2).FirstOrDefault(),
			Fleet4 = fleets.Skip(3).FirstOrDefault(),
			AirBase1 = selectedSortie.Model.FleetData.AirBases.Skip(0).FirstOrDefault().MakeAirBase(),
			AirBase2 = selectedSortie.Model.FleetData.AirBases.Skip(1).FirstOrDefault().MakeAirBase(),
			AirBase3 = selectedSortie.Model.FleetData.AirBases.Skip(2).FirstOrDefault().MakeAirBase(),
		});

		FleetImageGeneratorImageDataModel model = new()
		{
			Fleet1Visible = data.Fleet1 is not null,
			Fleet2Visible = data.Fleet2 is not null,
			Fleet3Visible = data.Fleet3 is not null,
			Fleet4Visible = data.Fleet4 is not null,
		};

		FleetImageGenerator(model, data);
	}

	/// <summary>
	/// Generates deck builder json of data that was selected for export.
	/// </summary>
	/// <returns>null if data selection is canceled, deck builder data otherwise</returns>
	public string? DeckBuilderFleetExport(AirControlSimulatorViewModel? viewModel = null)
	{
		viewModel ??= new();

		BaseAirCorpsSimulationContentDialog dialog = new(viewModel);

		if (dialog.ShowDialog(App.Current.MainWindow) is not true) return null;

		AirControlSimulatorViewModel result = dialog.Result!;

		List<BaseAirCorpsData> bases = KCDatabase.Instance.BaseAirCorps.Values
			.Where(b => b.MapAreaID == result.AirBaseArea?.AreaId)
			.ToList();

		return DataSerializationService.DeckBuilder(new()
		{
			HqLevel = KCDatabase.Instance.Admiral.Level,
			Fleet1 = FleetOrDefault(result.Fleet1, 0),
			Fleet2 = FleetOrDefault(result.Fleet2, 1),
			Fleet3 = FleetOrDefault(result.Fleet3, 2),
			Fleet4 = FleetOrDefault(result.Fleet4, 3),
			AirBase1 = bases.Skip(0).FirstOrDefault(),
			AirBase2 = bases.Skip(1).FirstOrDefault(),
			AirBase3 = bases.Skip(2).FirstOrDefault(),
			MaxAircraftLevelFleet = result.MaxAircraftLevelFleet,
			MaxAircraftLevelAirBase = result.MaxAircraftLevelAirBase,
		});
	}

	public static void ExpChecker(ExpCheckerViewModel? viewModel = null)
	{
		if (!KCDatabase.Instance.Ships.Any())
		{
			MessageBox.Show
			(
				ExpCheckerResources.NoShipsAvailable,
				ExpCheckerResources.ShipsUnavailable,
				MessageBoxButton.OK,
				MessageBoxImage.Error
			);

			return;
		}

		viewModel ??= new();

		new ExpCheckerWindow(viewModel).Show(App.Current.MainWindow);
	}

	public void EquipmentUpgradePlanner(EquipmentUpgradePlannerViewModel? viewModel = null)
	{
		if (!KCDatabase.Instance.Equipments.Any())
		{
			MessageBox.Show(MainResources.EquipmentDataNotLoaded, MainResources.ErrorCaption,
				MessageBoxButton.OK, MessageBoxImage.Error);

			return;
		}

		viewModel ??= new();

		new EquipmentUpgradePlannerWindow(viewModel).Show(App.Current.MainWindow);
	}

	public void OpenSortieDetail(ElectronicObserverContext db, SortieRecord sortie)
	{
		SortieDetailViewModel? sortieDetail = GenerateSortieDetailViewModel(db, sortie);

		if (sortieDetail is null) return;

		new SortieDetailWindow(sortieDetail).Show();
	}

	private static string CsvSeparator => ";";

	public void CopySmokerDataCsv(ElectronicObserverContext db, IEnumerable<SortieRecordViewModel> sorties)
	{
		List<(SortieDetailViewModel SortieDetail, BattleData Battle)> data = new();

		foreach (SortieRecordViewModel sortieRecord in sorties)
		{
			SortieDetailViewModel? sortieDetail = GenerateSortieDetailViewModel(db, sortieRecord.Model);

			if (sortieDetail is null) return;

			BattleData? smokerBattle = null;

			foreach (BattleData battleData in sortieDetail.Nodes.OfType<BattleNode>().Select(n => n.FirstBattle))
			{
				bool activatedSmoker = battleData.Phases.OfType<PhaseSearching>().Any(p => p.SmokeCount > 0);

				if (activatedSmoker)
				{
					smokerBattle = battleData;
					break;
				}
			}

			if (smokerBattle is null) continue;

			data.Add((sortieDetail, smokerBattle));
		}

		List<string> csvData = new()
		{
			string.Join(CsvSeparator, new List<string>
			{
				"開始",
				"SN",
				"出撃回数",
				"煙幕",
				"自軍陣形",
				"敵軍陣形",
				"交戦形態",
				"フェーズ",
				"砲撃・雷撃",
				"攻撃艦_#",
				"攻撃艦",
				"防御艦_#",
				"防御艦",
				"Lv",
				"Cond",
				"回避",
				"CL",
				"自軍・敵軍",
			}),
		};
		int sampleNumber = 1;
		int sortieNumber = 1;

		foreach ((SortieDetailViewModel sortieDetail, BattleData battle) in data)
		{
			PhaseSearching searching = battle.Phases.OfType<PhaseSearching>().First();

			foreach (PhaseBase phase in battle.Phases.Where(p => p is PhaseShelling or PhaseTorpedo))
			{
				if (phase is PhaseShelling shelling)
				{
					foreach (PhaseShellingAttackViewModel attackDisplay in shelling.AttackDisplays)
					{
						foreach (DayAttack attack in attackDisplay.Attacks)
						{
							string csvLine = GetSmokerCsvLine(sortieDetail, searching, attackDisplay, attack, shelling.Title, sampleNumber, sortieNumber);
							csvData.Add(csvLine);
							sampleNumber++;
						}
					}
				}

				if (phase is PhaseTorpedo torpedo)
				{
					foreach (PhaseTorpedoAttackViewModel attackDisplay in torpedo.AttackDisplays)
					{
						foreach (DayAttack attack in attackDisplay.Attacks)
						{
							string csvLine = GetSmokerCsvLine(sortieDetail, searching, attackDisplay, attack, torpedo.Title, sampleNumber, sortieNumber);
							csvData.Add(csvLine);
							sampleNumber++;
						}
					}
				}
			}

			sortieNumber++;
		}

		Clipboard.SetText(string.Join("\n", csvData));
	}

	private static string GetSmokerCsvLine(SortieDetailViewModel sortieDetail, PhaseSearching searching,
		AttackViewModelBase attackDisplay, DayAttack attack, string phaseTitle, int sampleNumber, int sortieNumber)
	{
		static BattleIndex AttackerIndex(AttackViewModelBase attack) => attack switch
		{
			PhaseShellingAttackViewModel s => s.AttackerIndex,
			PhaseTorpedoAttackViewModel t => t.AttackerIndex,
			_ => throw new NotImplementedException(),
		};

		static BattleIndex DefenderIndex(AttackViewModelBase attack) => attack switch
		{
			PhaseShellingAttackViewModel s => s.DefenderIndex,
			PhaseTorpedoAttackViewModel t => t.DefenderIndex,
			_ => throw new NotImplementedException(),
		};

		return string.Join(CsvSeparator, new List<string>
		{
			sortieDetail.StartTime?.ToLocalTime().ToString(),
			sampleNumber.ToString(),
			sortieNumber.ToString(),
			(searching.SmokeCount ?? 0).ToString(),
			Constants.GetFormation(searching.PlayerFormationType),
			Constants.GetFormation(searching.EnemyFormationType),
			Constants.GetEngagementForm(searching.EngagementType),
			phaseTitle,
			ElectronicObserverTypes.Attacks.DayAttack.AttackDisplay(attack.AttackKind),
			(AttackerIndex(attackDisplay).Index + 1).ToString(),
			attack.Attacker.Name,
			(DefenderIndex(attackDisplay).Index + 1).ToString(),
			attack.Defender.Name,
			attack.Defender.Level.ToString(),
			attack.Defender.Condition.ToString(),
			attack.Defender.EvasionTotal.ToString(),
			attack.CriticalFlag switch
			{
				HitType.Miss => "CL0",
				HitType.Hit => "CL1",
				HitType.Critical => "CL2",
				_ => throw new NotImplementedException(),
			},
			AttackerIndex(attackDisplay).FleetFlag switch
			{
				FleetFlag.Player => "自軍",
				FleetFlag.Enemy => "敵軍",
				_ => throw new NotImplementedException(),
			},
		});
	}

	public SortieDetailViewModel? GenerateSortieDetailViewModel(ElectronicObserverContext db,
		SortieRecord sortie)
	{
		try
		{
			sortie.EnsureApiFilesLoaded(db).Wait();

			ApiFile startRequestFile = sortie.ApiFiles
				.First(f => f.ApiFileType is ApiFileType.Request && f.Name is "api_req_map/start");

			ApiReqMapStartRequest startRequest = JsonSerializer
				.Deserialize<ApiReqMapStartRequest>(startRequestFile.Content)
			?? throw new Exception();

			// fleetId is 1 based, so need to do -1 when fetching data from fleets
			if (!int.TryParse(startRequest.ApiDeckId, out int fleetId)) throw new Exception();

			BattleFleets fleetsBeforeSortie = MakeBattleFleets(sortie.FleetData, fleetId);
			BattleFleets? fleetsAfterSortie = MakeBattleFleets(sortie.FleetAfterSortieData, fleetId);

			SortieDetailViewModel sortieDetail = new(db, sortie, fleetsBeforeSortie, fleetsAfterSortie);

			// todo: battle requests contain a flag if smoke screen was activated
			foreach (ApiFile apiFile in sortie.ApiFiles)
			{
				if (apiFile is { ApiFileType: ApiFileType.Request, Name: not "api_req_map/start_air_base" })
				{
					continue;
				}

				sortieDetail.StartTime ??= apiFile.TimeStamp;

				object? battleData = apiFile.ApiFileType switch
				{
					ApiFileType.Response => apiFile.GetResponseApiData(),
					ApiFileType.Request => apiFile.GetRequestApiData(),
					_ => throw new NotImplementedException("Unknown api file type."),
				};

				if (battleData is null) continue;

				sortieDetail.AddApiFile(battleData, apiFile.TimeStamp);
			}

			sortieDetail.EnsureApiFilesProcessed();

			return sortieDetail;
		}
		catch (Exception e)
		{
			Logger.Add(2, "Failed to load sortie details: " + e.Message + e.StackTrace);
		}

		return null;
	}

	[return: NotNullIfNotNull(nameof(fleetData))]
	private static BattleFleets? MakeBattleFleets(SortieFleetData? fleetData, int fleetId)
	{
		if (fleetData is null) return null;

		List<IFleetData?> fleets = fleetData.MakeFleets();
		bool isCombinedFleet = fleetData.CombinedFlag > 0;
		List<IBaseAirCorpsData> airBases = fleetData.AirBases.Select(f => f.MakeAirBase()).ToList();

		if (fleets.Skip(fleetId - 1).FirstOrDefault() is not IFleetData fleet) throw new Exception();

		IFleetData? escortFleet = isCombinedFleet switch
		{
			true => fleets.Skip(fleetId).FirstOrDefault(),
			_ => null,
		};

		return new(fleet, escortFleet, fleets, airBases)
		{
			FleetId = fleetData.FleetId,
			NodeSupportFleetId = fleetData.NodeSupportFleetId,
			BossSupportFleetId = fleetData.BossSupportFleetId,
			CombinedFlag = fleetData.CombinedFlag,
		};
	}

	public void CopyReplayLinkToClipboard(SortieRecordViewModel sortie)
	{
		string replayData = GenerateReplayData(sortie);
		string link = $"https://kc3kai.github.io/kancolle-replay/battleplayer.html#{replayData}";

		Clipboard.SetText(link);
	}

	public void CopyReplayDataToClipboard(SortieRecordViewModel sortie)
	{
		string replayData = GenerateReplayData(sortie);

		Clipboard.SetText(replayData);
	}

	private static string GenerateReplayData(SortieRecordViewModel sortie)
	{
		ReplayData replay = sortie.Model.ToReplayData();

		replay.Battles = new();

		ReplayBattle battle = new();

		foreach (ApiFile apiFile in sortie.Model.ApiFiles.Where(f => f.ApiFileType is ApiFileType.Response))
		{
			if (apiFile.IsMapProgressApi())
			{
				IMapProgressApi? progress = apiFile.GetMapProgressApiData();

				if (progress is null)
				{
					// this shouldn't happen
					continue;
				}

				battle.Node = progress.ApiNo;
			}

			if (apiFile.IsFirstBattleApi())
			{
				try
				{
					battle.FirstBattle = apiFile.GetResponseApiData();
				}
				catch (Exception e)
				{
					Logger.Add(2, SortieRecordViewerResources.FailedToParseApiData + e.StackTrace);
				}
			}

			if (apiFile.IsSecondBattleApi())
			{
				try
				{
					battle.SecondBattle = apiFile.GetResponseApiData();
				}
				catch (Exception e)
				{
					Logger.Add(2, SortieRecordViewerResources.FailedToParseApiData + e.StackTrace);
				}
			}

			if (apiFile.IsBattleEndApi())
			{
				ISortieBattleResultApi? result = apiFile.GetSortieBattleResultApi();

				if (result is null)
				{
					// this shouldn't happen
					continue;
				}

				battle.FirstBattle ??= new();
				battle.SecondBattle ??= new();
				battle.BaseExp = result.ApiGetBaseExp;
				battle.HqExp = result.ApiGetExp;
				battle.Drop = result.ApiGetShip?.ApiShipId ?? ShipId.Unknown;
				battle.Rating = result.ApiWinRank;
				battle.Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				battle.Mvp = new() { result.ApiMvp, };

				if (result is ApiReqCombinedBattleBattleresultResponse combinedResult)
				{
					battle.Mvp.Add(combinedResult.ApiMvpCombined ?? -1);
				}

				replay.Time = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				replay.Battles.Add(battle);
				battle = new();
			}
		}

		// there was battle data but no battle end
		if (battle.FirstBattle is not null || battle.SecondBattle is not null)
		{
			battle.FirstBattle ??= new();
			battle.SecondBattle ??= new();

			replay.Battles.Add(battle);
		}

		return JsonSerializer.Serialize(replay);
	}

	public void CopyAirControlSimulatorLink(SortieRecord sortie,
		SortieDetailViewModel? sortieDetail = null)
	{
		string url = GetAirControlSimulatorLink(sortie, sortieDetail);

		Clipboard.SetText(url);
	}

	private string GetAirControlSimulatorLink(SortieRecord sortie,
		SortieDetailViewModel? sortieDetail = null)
	{
		sortie.EnsureApiFilesLoaded(new()).Wait();

		sortieDetail ??= GenerateSortieDetailViewModel(new(), sortie);

		List<IFleetData?> fleets = sortie.FleetData.MakeFleets();

		List<IBaseAirCorpsData> airBases = sortie.FleetData.AirBases
			.Select(f => f.MakeAirBase())
			.ToList();

		string airControlSimulatorData = DataSerializationService.AirControlSimulator(new()
		{
			HqLevel = KCDatabase.Instance.Admiral.Level,
			Fleet1 = fleets.Skip(0).FirstOrDefault(),
			Fleet2 = fleets.Skip(1).FirstOrDefault(),
			Fleet3 = fleets.Skip(2).FirstOrDefault(),
			Fleet4 = fleets.Skip(3).FirstOrDefault(),
			AirBase1 = airBases.Skip(0).FirstOrDefault(),
			AirBase2 = airBases.Skip(1).FirstOrDefault(),
			AirBase3 = airBases.Skip(2).FirstOrDefault(),
			SortieDetails = sortieDetail,
		});

		return @$"https://noro6.github.io/kc-web#import:{airControlSimulatorData}";
	}

	public void CopyOperationRoomLink(SortieRecord sortie)
	{
		string url = GetOperationRoomLink(sortie);

		Clipboard.SetText(url);
	}

	private string GetOperationRoomLink(SortieRecord sortie)
	{
		List<IFleetData?> fleets = sortie.FleetData.MakeFleets();

		List<IBaseAirCorpsData> airBases = sortie.FleetData.AirBases
			.Select(f => f.MakeAirBase())
			.ToList();

		string operationRoomData = DataSerializationService.DeckBuilder(new()
		{
			HqLevel = KCDatabase.Instance.Admiral.Level,
			Fleet1 = fleets.Skip(0).FirstOrDefault(),
			Fleet2 = fleets.Skip(1).FirstOrDefault(),
			Fleet3 = fleets.Skip(2).FirstOrDefault(),
			Fleet4 = fleets.Skip(3).FirstOrDefault(),
			AirBase1 = airBases.Skip(0).FirstOrDefault(),
			AirBase2 = airBases.Skip(1).FirstOrDefault(),
			AirBase3 = airBases.Skip(2).FirstOrDefault(),
		});

		return @$"https://jervis.vercel.app?predeck={Uri.EscapeDataString(operationRoomData)}";
	}

	public async Task CopySortieDataToClipboard(ElectronicObserverContext db, SortieRecord sortie)
	{
		await CopySortieDataToClipboard(db, [sortie]);
	}

	public async Task CopySortieDataToClipboard(ElectronicObserverContext db,
		IEnumerable<SortieRecordViewModel> selectedSorties)
	{
		List<SortieRecord> sortieRecords = selectedSorties
			.OrderBy(s => s.SortieStart)
			.Select(s => s.Model)
			.ToList();

		await CopySortieDataToClipboard(db, sortieRecords);
	}

	private async Task CopySortieDataToClipboard(ElectronicObserverContext db, List<SortieRecord> sortieRecords)
	{
		foreach (SortieRecord sortieRecord in sortieRecords)
		{
			await sortieRecord.EnsureApiFilesLoaded(db);
			sortieRecord.CleanRequests();
		}

		List<SortieRecord> sorties = sortieRecords
			.Select(s => new SortieRecord
			{
				Version = s.Version,
				Id = s.Id,
				World = s.World,
				Map = s.Map,
				ApiFiles = s.ApiFiles
					.Where(f => f.ApiFileType is ApiFileType.Response || f.Name is "api_req_map/start" or "api_req_map/start_air_base")
					.ToList(),
				FleetData = s.FleetData,
				FleetAfterSortieData = s.FleetAfterSortieData,
				MapData = s.MapData,
			}).ToList();

		Clipboard.SetText(JsonSerializer.Serialize(sorties));
	}

	public void LoadSortieDataFromClipboard(ElectronicObserverContext db)
	{
		try
		{
			List<SortieRecord>? sorties = JsonSerializer
				.Deserialize<List<SortieRecord>>(Clipboard.GetText());

			if (sorties is null) return;

			OpenSortieDetail(db, sorties.First());
		}
		catch (Exception e)
		{
			Logger.Add(2, $"Failed to load sortie details: {e.Message}{e.StackTrace}");
		}
	}

	private static FleetData? FleetOrDefault(bool include, int index) => include switch
	{
		false => null,
		_ => KCDatabase.Instance.Fleet.Fleets.Values.Skip(index).FirstOrDefault(),
	};
}
