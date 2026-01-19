using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility;
using ElectronicObserver.Window.Dialog.UiBlocker.Taiha;

namespace ElectronicObserver.Window.Dialog.UiBlocker;

public sealed partial class UiBlockerManagerViewModel : WindowViewModelBase
{
	private UiBlockerService UiBlockerService { get; }

	public TaihaBlockerViewModel TaihaBlocker { get; set; }

	[ObservableProperty] public partial UiBlockerViewModel SelectedBlocker { get; set; }
	public ObservableCollection<UiBlockerViewModel> Blockers { get; } = [];

	public UiBlockerManagerViewModel(UiBlockerService uiBlockerService, Configuration.ConfigurationData configuration)
	{
		UiBlockerService = uiBlockerService;

		TaihaBlocker = new(configuration.TaihaBlocker);
		Blockers.Add(TaihaBlocker);

		SelectedBlocker = TaihaBlocker;

		SubscribeToApis();
	}

	private void SubscribeToApis()
	{
		APIObserver.Instance.ApiPort_Port.ResponseReceived += EnterPort;

		APIObserver.Instance.ApiReqSortie_BattleResult.ResponseReceived += CheckTaihaAdvance;
		APIObserver.Instance.ApiReqCombinedBattle_BattleResult.ResponseReceived += CheckTaihaAdvance;
	}

	private void EnterPort(string apiname, dynamic data)
	{
		HideBlocker(TaihaBlocker);
	}

	private void CheckTaihaAdvance(string apiname, dynamic data)
	{
		if (!TaihaBlocker.IsEnabled) return;

		BattleManager bm = KCDatabase.Instance.Battle;

		if (bm.Compass.IsEndPoint) return;

		BattleData battle = bm.SecondBattle ?? bm.FirstBattle;

		if (CanAnyShipSink(battle.Initial.FriendFleet, battle.ResultHPs))
		{
			ShowBlocker(TaihaBlocker);
			return;
		}

		if (!bm.IsCombinedBattle) return;
		if (!CanAnyShipSink(battle.Initial.FriendFleetEscort, battle.ResultHPs.Skip(6))) return;

		ShowBlocker(TaihaBlocker);

		static bool CanAnyShipSink(FleetData fleet, IEnumerable<int> hps)
		{
			if (fleet.MembersInstance is null) return false;

			return fleet.MembersInstance
				.Zip(hps, (s, hp) => (Ship: s, Hp: hp))
				.Any(t => t.Ship.CanSink(fleet, t.Hp));
		}
	}

	[RelayCommand]
	private void ShowBlocker(UiBlockerViewModel uiBlockerViewModel)
	{
		UiBlockerService.ShowBlocker(uiBlockerViewModel);
	}

	[RelayCommand]
	private void HideBlocker(UiBlockerViewModel uiBlockerViewModel)
	{
		UiBlockerService.HideBlocker(uiBlockerViewModel);
	}

	public override void Loaded()
	{
		base.Loaded();

		foreach (UiBlockerViewModel blocker in Blockers)
		{
			blocker.Loaded();
		}
	}

	public override void Closed()
	{
		base.Closed();

		foreach (UiBlockerViewModel blocker in Blockers)
		{
			blocker.Closed();
		}

		UiBlockerService.HideAll();
	}
}
