using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows;
using ElectronicObserver.Data;
using ElectronicObserver.Database.KancolleApi;
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

public class ToolService
{
	private DataSerializationService DataSerializationService { get; }

	public ToolService(DataSerializationService dataSerializationService)
	{
		DataSerializationService = dataSerializationService;
	}


	public void AirControlSimulator(AirControlSimulatorViewModel? viewModel = null)
	{
		viewModel ??= new();

		BaseAirCorpsSimulationContentDialog dialog = new(viewModel);

		if (dialog.ShowDialog(App.Current!.MainWindow!) is not true) return;

		AirControlSimulatorViewModel result = dialog.Result!;
		
		string url = DataSerializationService.AirControlSimulatorLink(result);

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

	public void AirControlSimulator(SortieRecordViewModel sortie)
	{
		string url = GetAirControlSimulatorLink(sortie);

		Window.FormBrowserHost.Instance.Browser.OpenAirControlSimulator(url);
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

		data ??= DataSerializationService.MakeDeckBuilderData
		(
			KCDatabase.Instance.Admiral.Level,
			KCDatabase.Instance.Fleet.Fleets[1],
			KCDatabase.Instance.Fleet.Fleets[2],
			KCDatabase.Instance.Fleet.Fleets[3],
			KCDatabase.Instance.Fleet.Fleets[4]
		);

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
		DeckBuilderData data = DataSerializationService.MakeDeckBuilderData
		(
			hqLevel,
			selectedSortie.Model.FleetData.Fleets.Skip(0).FirstOrDefault().MakeFleet(),
			selectedSortie.Model.FleetData.Fleets.Skip(1).FirstOrDefault().MakeFleet(),
			selectedSortie.Model.FleetData.Fleets.Skip(2).FirstOrDefault().MakeFleet(),
			selectedSortie.Model.FleetData.Fleets.Skip(3).FirstOrDefault().MakeFleet(),
			selectedSortie.Model.FleetData.AirBases.Skip(0).FirstOrDefault().MakeAirBase(),
			selectedSortie.Model.FleetData.AirBases.Skip(1).FirstOrDefault().MakeAirBase(),
			selectedSortie.Model.FleetData.AirBases.Skip(2).FirstOrDefault().MakeAirBase()
		);

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

		return DataSerializationService.DeckBuilder
		(
			KCDatabase.Instance.Admiral.Level,
			result.Fleet1 ? KCDatabase.Instance.Fleet.Fleets.Values.Skip(0).FirstOrDefault() : null,
			result.Fleet2 ? KCDatabase.Instance.Fleet.Fleets.Values.Skip(1).FirstOrDefault() : null,
			result.Fleet3 ? KCDatabase.Instance.Fleet.Fleets.Values.Skip(2).FirstOrDefault() : null,
			result.Fleet4 ? KCDatabase.Instance.Fleet.Fleets.Values.Skip(3).FirstOrDefault() : null,
			bases.Skip(0).FirstOrDefault(),
			bases.Skip(1).FirstOrDefault(),
			bases.Skip(2).FirstOrDefault(),
			result.MaxAircraftLevelFleet,
			result.MaxAircraftLevelAirBase
		);
	}

	public void ExpChecker(ExpCheckerViewModel? viewModel = null)
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

	public void OpenSortieDetail(SortieRecordViewModel sortie)
	{
		SortieDetailViewModel? sortieDetail = GenerateSortieDetailViewModel(sortie);

		if (sortieDetail is null) return;

		new SortieDetailWindow(sortieDetail).Show();
	}

	private static string CsvSeparator => ";";

	public void CopySmokerDataCsv(IEnumerable<SortieRecordViewModel> sorties)
	{
		List<(SortieDetailViewModel SortieDetail, BattleData Battle)> data = new();

		foreach (SortieRecordViewModel sortieRecord in sorties)
		{
			SortieDetailViewModel? sortieDetail = GenerateSortieDetailViewModel(sortieRecord);

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
					foreach (PhaseShellingAttackViewModel attackDisplay in torpedo.AttackDisplays)
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
		PhaseShellingAttackViewModel attackDisplay, DayAttack attack, string phaseTitle, int sampleNumber, int sortieNumber)
	{
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
			(attackDisplay.AttackerIndex.Index + 1).ToString(),
			attack.Attacker.Name,
			(attackDisplay.DefenderIndex.Index + 1).ToString(),
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
			attackDisplay.AttackerIndex.FleetFlag switch
			{
				FleetFlag.Player => "自軍",
				FleetFlag.Enemy => "敵軍",
				_ => throw new NotImplementedException(),
			},
		});
	}

	private SortieDetailViewModel? GenerateSortieDetailViewModel(SortieRecordViewModel sortie)
	{
		try
		{
			List<IFleetData?> fleets = sortie.Model.FleetData.Fleets.Select(f => f.MakeFleet()).ToList();
			bool isCombinedFleet = sortie.Model.FleetData.CombinedFlag > 0;
			List<IBaseAirCorpsData> airBases = sortie.Model.FleetData.AirBases.Select(f => f.MakeAirBase()).ToList();

			ApiFile startRequestFile = sortie.Model.ApiFiles
				.First(f => f.ApiFileType is ApiFileType.Request && f.Name is "api_req_map/start");

			ApiReqMapStartRequest startRequest = JsonSerializer
				.Deserialize<ApiReqMapStartRequest>(startRequestFile.Content)
			?? throw new Exception();

			// fleetId is 1 based, so need to do -1 when fetching data from fleets
			if (!int.TryParse(startRequest.ApiDeckId, out int fleetId)) throw new Exception();
			if (fleets.Skip(fleetId - 1).FirstOrDefault() is not IFleetData fleet) throw new Exception();

			IFleetData? escortFleet = isCombinedFleet switch
			{
				true => fleets.Skip(fleetId).FirstOrDefault(),
				_ => null,
			};

			SortieDetailViewModel sortieDetail = new(sortie, new(fleet, escortFleet, fleets, airBases));

			// todo: battle requests contain a flag if smoke screen was activated
			foreach (ApiFile apiFile in sortie.Model.ApiFiles.Where(f => f.ApiFileType is ApiFileType.Response))
			{
				sortieDetail.StartTime ??= apiFile.TimeStamp;

				object? battleData = apiFile.ApiFileType switch
				{
					ApiFileType.Response => apiFile.GetResponseApiData(),
					ApiFileType.Request => apiFile.GetRequestApiData(),
					_ => throw new NotImplementedException("Unknown api file type."),
				};

				if (battleData is null) continue;

				sortieDetail.AddApiFile(battleData);
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

	public void CopyAirControlSimulatorLink(SortieRecordViewModel sortie)
	{
		string url = GetAirControlSimulatorLink(sortie);

		Clipboard.SetText(url);
	}

	private string GetAirControlSimulatorLink(SortieRecordViewModel sortie)
	{
		List<IFleetData?> fleets = sortie.Model.FleetData.Fleets
			.Select(f => f.MakeFleet())
			.ToList();

		List<IBaseAirCorpsData> airBases = sortie.Model.FleetData.AirBases
			.Select(f => f.MakeAirBase())
			.ToList();

		string airControlSimulatorData = DataSerializationService.AirControlSimulator
		(
			KCDatabase.Instance.Admiral.Level,
			fleets.Skip(0).FirstOrDefault(),
			fleets.Skip(1).FirstOrDefault(),
			fleets.Skip(2).FirstOrDefault(),
			fleets.Skip(3).FirstOrDefault(),
			airBases.Skip(0).FirstOrDefault(),
			airBases.Skip(1).FirstOrDefault(),
			airBases.Skip(2).FirstOrDefault()
		);

		return @$"https://noro6.github.io/kc-web#import:{airControlSimulatorData}";
	}
}
