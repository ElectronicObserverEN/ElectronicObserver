using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ElectronicObserver.Avalonia.UiBlocker;
using ElectronicObserver.Core.Services;
using ElectronicObserver.Observer;

namespace ElectronicObserver.Window.Dialog.UiBlocker;

public class UiBlockerAdapter
{
	private UiBlockerService UiBlockerService { get; }
	private IGameStateProvider GameStateProvider { get; }
	private IClipboardService ClipboardService { get; }

	private Avalonia.UiBlocker.UiBlockerConfiguration UiBlockerConfiguration { get; set; }
	public UiBlockerManagerViewModel UiBlockerManagerViewModel { get; private set; }

	private static string UiBlockersPath => Path.Combine("Settings", "UiBlockers.json");

	public UiBlockerAdapter(
		UiBlockerService uiBlockerService,
		IGameStateProvider gameStateProvider,
		IClipboardService clipboardService)
	{
		UiBlockerService = uiBlockerService;
		GameStateProvider = gameStateProvider;
		ClipboardService = clipboardService;

		UiBlockerConfiguration = UiBlockerSerializer.LoadFromFile(UiBlockersPath);

		UiBlockerManagerViewModel = new(UiBlockerService, new MapInfoProvider(), ClipboardService, UiBlockerConfiguration);

		SubscribeToApis(UiBlockerManagerViewModel);
	}

	private void SubscribeToApis(UiBlockerManagerViewModel uiBlockerManagerViewModel)
	{
		APIObserver.Instance.ApiPort_Port.ResponseReceived +=
			(_, _) => uiBlockerManagerViewModel.Engine.OnPort(GameStateProvider);

		APIObserver.Instance.ApiReqMap_Start.ResponseReceived +=
			(_, _) => uiBlockerManagerViewModel.Engine.OnSortieAdvance(GameStateProvider);
		APIObserver.Instance.ApiReqMap_Next.ResponseReceived +=
			(_, _) => uiBlockerManagerViewModel.Engine.OnSortieAdvance(GameStateProvider);

		foreach (APIBase api in APIObserver.Instance.BattleApis())
		{
			api.ResponseReceived += (_, _) => uiBlockerManagerViewModel.Engine.OnBattleStarted(GameStateProvider);
		}

		APIObserver.Instance.ApiReqSortie_BattleResult.ResponseReceived +=
			(_, _) => uiBlockerManagerViewModel.Engine.OnBattleEnded(GameStateProvider);
		APIObserver.Instance.ApiReqCombinedBattle_BattleResult.ResponseReceived +=
			(_, _) => uiBlockerManagerViewModel.Engine.OnBattleEnded(GameStateProvider);
	}

	public async Task Save()
	{
		foreach (Guid id in SystemUiBlockers.Ids)
		{
			UiBlockerViewModel? blockerViewModel = UiBlockerManagerViewModel.Blockers
				.FirstOrDefault(b => b.Model.Id == id);

			SystemUiBlockerConfiguration? systemBlockerData = UiBlockerConfiguration.SystemBlockers
				.FirstOrDefault(b => b.Id == id);

			if (systemBlockerData is null)
			{
				systemBlockerData = new()
				{
					Id = id,
					IsEnabled = id == SystemUiBlockers.TaihaBlocker.Id,
				};
				UiBlockerConfiguration.SystemBlockers.Add(systemBlockerData);
			}

			systemBlockerData.IsEnabled = blockerViewModel?.IsEnabled ?? true;
		}

		await UiBlockerSerializer.SaveToFile(UiBlockersPath, UiBlockerConfiguration);
	}
}
