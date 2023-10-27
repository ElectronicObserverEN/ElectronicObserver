using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Windows.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Data.Battle.Detail;
using ElectronicObserver.Data.Battle.Phase;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog.BattleDetail;
using ElectronicObserver.Window.Wpf.Battle.ViewModels;
using ElectronicObserverTypes;
using ElectronicObserverTypes.AntiAir;
using Color = System.Drawing.Color;

namespace ElectronicObserver.Window.Wpf.Battle;

public partial class BattleViewModel : AnchorableViewModel
{
	public FormBattleTranslationViewModel FormBattle { get; }

	private SolidColorBrush WinRankColor_Win { get; } = Configuration.Config.UI.ForeColor.ToBrush();
	private SolidColorBrush WinRankColor_Lose { get; } = Configuration.Config.UI.Color_Red.ToBrush();

	public bool ViewVisible { get; set; }

	#region Row 0

	public string? FormationFriendText { get; set; }

	public string? FormationText { get; set; }
	public SolidColorBrush? FormationForeColor { get; set; }

	public string? FormationEnemyText { get; set; }

	#endregion

	#region Row 1

	public string? SearchingFriendText { get; set; }
	public EquipmentIconType? SearchingFriendIcon { get; set; }
	public string? SearchingFriendToolTip { get; set; }
	public bool Smoker1Active { get; set; }
	public bool Smoker2Active { get; set; }
	public bool Smoker3Active { get; set; }

	public string? SearchingText { get; set; }
	public EquipmentIconType? SearchingIcon { get; set; }
	public string? SearchingToolTip { get; set; }

	public string? SearchingEnemyText { get; set; }
	public EquipmentIconType? SearchingEnemyIcon { get; set; }
	public string? SearchingEnemyToolTip { get; set; }

	#endregion

	#region Row 2

	public string? AirStage1FriendText { get; set; }
	public string? AirStage1FriendToolTip { get; set; }
	public SolidColorBrush? AirStage1FriendForeColor { get; set; }
	public EquipmentIconType? AirStage1FriendIcon { get; set; }

	public string? AirSuperiorityText { get; set; }
	public SolidColorBrush? AirSuperiorityForeColor { get; set; }
	public string? AirSuperiorityToolTip { get; set; }

	public string? AirStage1EnemyText { get; set; }
	public string? AirStage1EnemyToolTip { get; set; }
	public SolidColorBrush? AirStage1EnemyForeColor { get; set; }
	public EquipmentIconType? AirStage1EnemyIcon { get; set; }

	#endregion

	#region Row 3

	public string? AirStage2FriendText { get; set; }
	public SolidColorBrush? AirStage2FriendForeColor { get; set; }
	public EquipmentIconType? AirStage2FriendIcon { get; set; }
	public string? AirStage2FriendToolTip { get; set; }

	public string? AACutinText { get; set; }
	public EquipmentIconType? AACutinIcon { get; set; }
	public string? AACutinToolTip { get; set; }

	public string? AirStage2EnemyText { get; set; }
	public SolidColorBrush? AirStage2EnemyForeColor { get; set; }
	public EquipmentIconType? AirStage2EnemyIcon { get; set; }
	public string? AirStage2EnemyToolTip { get; set; }

	#endregion

	#region Row 4

	public string? FleetFriendText { get; set; }
	public string? FleetFriendToolTip { get; set; }
	public EquipmentIconType? FleetFriendIcon { get; set; }

	public bool PlayerFleetVisible { get; set; }

	private bool IsPlayerCombinedFleet { get; set; }
	public bool FleetFriendEscortVisible => IsPlayerCombinedFleet && PlayerFleetVisible;

	public SolidColorBrush? FleetEnemyEscortForeColor { get; set; }
	public SolidColorBrush? FleetEnemyEscortBackColor { get; set; }
	private bool IsEnemyCombinedFleet { get; set; }
	public bool FleetEnemyEscortVisible => IsEnemyCombinedFleet && ViewVisible;

	public SolidColorBrush? FleetEnemyForeColor { get; set; }
	public SolidColorBrush? FleetEnemyBackColor { get; set; }

	#endregion

	#region Row 6

	public string? DamageFriendText { get; set; }

	public string? WinRankText { get; set; }
	public SolidColorBrush? WinRankForeColor { get; set; }

	public string? DamageEnemyText { get; set; }


	#endregion

	public List<HealthBarViewModel> HPBars { get; } = new();
	public ObservableCollection<HealthBarViewModel> PlayerMainHPBars { get; } = new();
	public ObservableCollection<HealthBarViewModel> PlayerEscortHPBars { get; } = new();
	public List<HealthBarViewModel> EnemyMainHPBars { get; } = new();
	public List<HealthBarViewModel> EnemyEscortHPBars { get; } = new();

	public bool CompactMode { get; set; }

	public BattleViewModel() : base("Battle", "Battle", IconContent.FormBattle)
	{
		FormBattle = Ioc.Default.GetService<FormBattleTranslationViewModel>()!;

		Title = FormBattle.Title;
		FormBattle.PropertyChanged += (_, _) => Title = FormBattle.Title;

		for (int i = 0; i < 24; i++)
		{
			HealthBarViewModel vm = new();
			HPBars.Add(vm);

			if (i < 6)
			{
				PlayerMainHPBars.Add(vm);
			}
			else if (i < 12)
			{
				PlayerEscortHPBars.Add(vm);
			}
			else if (i < 18)
			{
				EnemyMainHPBars.Add(vm);
			}
			else
			{
				EnemyEscortHPBars.Add(vm);
			}
		}

		APIObserver o = APIObserver.Instance;

		o.ApiPort_Port.ResponseReceived += Updated;
		o.ApiReqMap_Start.ResponseReceived += Updated;
		o.ApiReqMap_Next.ResponseReceived += Updated;
		o.ApiReqMap_AirRaid.ResponseReceived += Updated;
		o.ApiReqSortie_Battle.ResponseReceived += Updated;
		o.ApiReqSortie_BattleResult.ResponseReceived += Updated;
		o.ApiReqBattleMidnight_Battle.ResponseReceived += Updated;
		o.ApiReqBattleMidnight_SpMidnight.ResponseReceived += Updated;
		o.ApiReqSortie_AirBattle.ResponseReceived += Updated;
		o.ApiReqSortie_LdAirBattle.ResponseReceived += Updated;
		o.ApiReqSortie_NightToDay.ResponseReceived += Updated;
		o.ApiReqSortie_LdShooting.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_Battle.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_MidnightBattle.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_SpMidnight.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_AirBattle.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_BattleWater.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_LdAirBattle.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_EcBattle.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_EcMidnightBattle.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_EcNightToDay.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_EachBattle.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_EachBattleWater.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_LdShooting.ResponseReceived += Updated;
		o.ApiReqCombinedBattle_BattleResult.ResponseReceived += Updated;
		o.ApiReqPractice_Battle.ResponseReceived += Updated;
		o.ApiReqPractice_MidnightBattle.ResponseReceived += Updated;
		o.ApiReqPractice_BattleResult.ResponseReceived += Updated;

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(CompactMode)) return;

			Configuration.Config.FormBattle.CompactMode = CompactMode;

			foreach (HealthBarViewModel hpBar in HPBars)
			{
				hpBar.CompactMode = CompactMode;
			}
		};

		Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		ConfigurationChanged();
	}

	[RelayCommand]
	private void ShowBattleDetail()
	{
		BattleManager? bm = KCDatabase.Instance.Battle;

		if (bm == null || bm.BattleMode == BattleManager.BattleModes.Undefined) return;

		BattleDetailView dialog = new(new BattleDetailViewModel
		{
			BattleDetailText = BattleDetailDescriptor.GetBattleDetail(bm),
		});

		dialog.Show(App.Current!.MainWindow!);
	}

	[RelayCommand]
	private void ShowBattleResult()
	{
		ViewVisible = true;
		PlayerFleetVisible = true;
	}

	private void Updated(string apiname, dynamic data)
	{
		KCDatabase db = KCDatabase.Instance;
		BattleManager bm = db.Battle;
		bool hideDuringBattle = Configuration.Config.FormBattle.HideDuringBattle;

		switch (apiname)
		{

			case "api_port/port":
				ViewVisible = false;
				PlayerFleetVisible = false;
				break;

			case "api_req_map/start":
			case "api_req_map/next":
				if (!bm.Compass.HasAirRaid)
				{
					ViewVisible = false;
					break;
				}

				SetFormation(bm);
				ClearSearchingResult();
				ClearBaseAirAttack();
				SetAerialWarfare(null, ((BattleBaseAirRaid)bm.BattleDay).BaseAirRaid);
				SetHPBar(bm.BattleDay);
				SetDamageRate(bm);

				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = true;
				break;

			case "api_req_map/air_raid":
				if (!bm.HeavyBaseAirRaids.Any())
				{
					ViewVisible = false;
					break;
				}

				SetFormation(bm);
				ClearSearchingResult();
				ClearBaseAirAttack();
				SetAerialWarfare(null, bm.HeavyBaseAirRaids.Last().BaseAirRaid);
				SetHPBar(bm.HeavyBaseAirRaids.Last());
				SetDamageRate(bm);

				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = true;
				break;

			case "api_req_sortie/battle":
			case "api_req_practice/battle":
			case "api_req_sortie/ld_airbattle":
			case "api_req_sortie/ld_shooting":
			{

				SetFormation(bm);
				SetSearchingResult(bm.BattleDay);
				SetBaseAirAttack(bm.BattleDay.BaseAirAttack);
				SetAerialWarfare(bm.BattleDay.JetAirBattle, bm.BattleDay.AirBattle);
				SetHPBar(bm.BattleDay);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_battle_midnight/battle":
			case "api_req_practice/midnight_battle":
			{

				SetNightBattleEvent(bm.BattleNight.NightInitial);
				SetHPBar(bm.BattleNight);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_battle_midnight/sp_midnight":
			{

				SetFormation(bm);
				ClearBaseAirAttack();
				ClearAerialWarfare();
				ClearSearchingResult();
				SetNightBattleEvent(bm.BattleNight.NightInitial);
				SetHPBar(bm.BattleNight);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_sortie/airbattle":
			{

				SetFormation(bm);
				SetSearchingResult(bm.BattleDay);
				SetBaseAirAttack(bm.BattleDay.BaseAirAttack);
				SetAerialWarfare(bm.BattleDay.JetAirBattle, bm.BattleDay.AirBattle, ((BattleAirBattle)bm.BattleDay).AirBattle2);
				SetHPBar(bm.BattleDay);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_sortie/night_to_day":
			{
				// 暫定
				var battle = bm.BattleNight as BattleDayFromNight;

				SetFormation(bm);
				ClearAerialWarfare();
				ClearSearchingResult();
				ClearBaseAirAttack();
				SetNightBattleEvent(battle.NightInitial);

				if (battle.NextToDay)
				{
					SetSearchingResult(battle);
					SetBaseAirAttack(battle.BaseAirAttack);
					SetAerialWarfare(battle.JetAirBattle, battle.AirBattle);
				}

				SetHPBar(bm.BattleDay);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_combined_battle/battle":
			case "api_req_combined_battle/battle_water":
			case "api_req_combined_battle/ld_airbattle":
			case "api_req_combined_battle/ec_battle":
			case "api_req_combined_battle/each_battle":
			case "api_req_combined_battle/each_battle_water":
			case "api_req_combined_battle/ld_shooting":
			{

				SetFormation(bm);
				SetSearchingResult(bm.BattleDay);
				SetBaseAirAttack(bm.BattleDay.BaseAirAttack);
				SetAerialWarfare(bm.BattleDay.JetAirBattle, bm.BattleDay.AirBattle);
				SetHPBar(bm.BattleDay);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_combined_battle/airbattle":
			{

				SetFormation(bm);
				SetSearchingResult(bm.BattleDay);
				SetBaseAirAttack(bm.BattleDay.BaseAirAttack);
				SetAerialWarfare(bm.BattleDay.JetAirBattle, bm.BattleDay.AirBattle, ((BattleCombinedAirBattle)bm.BattleDay).AirBattle2);
				SetHPBar(bm.BattleDay);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_combined_battle/midnight_battle":
			case "api_req_combined_battle/ec_midnight_battle":
			{

				SetNightBattleEvent(bm.BattleNight.NightInitial);
				SetHPBar(bm.BattleNight);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_combined_battle/sp_midnight":
			{

				SetFormation(bm);
				ClearAerialWarfare();
				ClearSearchingResult();
				ClearBaseAirAttack();
				SetNightBattleEvent(bm.BattleNight.NightInitial);
				SetHPBar(bm.BattleNight);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_combined_battle/ec_night_to_day":
			{
				var battle = bm.BattleNight as BattleDayFromNight;

				SetFormation(bm);
				ClearAerialWarfare();
				ClearSearchingResult();
				ClearBaseAirAttack();
				SetNightBattleEvent(battle.NightInitial);

				if (battle.NextToDay)
				{
					SetSearchingResult(battle);
					SetBaseAirAttack(battle.BaseAirAttack);
					SetAerialWarfare(battle.JetAirBattle, battle.AirBattle);
				}

				SetHPBar(battle);
				SetDamageRate(bm);

				// BaseLayoutPanel.Visible = !hideDuringBattle;
				ViewVisible = !hideDuringBattle;
				PlayerFleetVisible = !hideDuringBattle;
			}
			break;

			case "api_req_sortie/battleresult":
			case "api_req_combined_battle/battleresult":
			case "api_req_practice/battle_result":
			{

				SetMVPShip(bm);

				// BaseLayoutPanel.Visible = true;
				ViewVisible = true;
				PlayerFleetVisible = true;
			}
			break;

		}
		/*
		TableTop.ResumeLayout();
		TableBottom.ResumeLayout();

		BaseLayoutPanel.ResumeLayout();


		if (Utility.Configuration.Config.UI.IsLayoutFixed)
			TableTop.Width = TableTop.GetPreferredSize(BaseLayoutPanel.Size).Width;
		else
			TableTop.Width = TableBottom.ClientSize.Width;
		TableTop.Height = TableTop.GetPreferredSize(BaseLayoutPanel.Size).Height;
		*/
	}

	/// <summary>
	/// 陣形・交戦形態を設定します。
	/// </summary>
	private void SetFormation(BattleManager bm)
	{
		FormationFriendText = Constants.GetFormationShort(bm.FirstBattle.Searching.FormationFriend);
		FormationEnemyText = Constants.GetFormationShort(bm.FirstBattle.Searching.FormationEnemy);
		FormationText = Constants.GetEngagementForm(bm.FirstBattle.Searching.EngagementForm);



		if (bm.IsEnemyCombined && bm.StartsFromDayBattle)
		{
			// highlights for the fleet you'll fight in night battle
			// todo: this should probably go to config
			Color highlightForeColor = Color.Black;
			Color highlightBackColor = Color.LightSteelBlue;

			bool willMain = bm.WillNightBattleWithMainFleet();

			FleetEnemyForeColor = bm.Compass?.EventID switch
			{
				5 => Configuration.Config.UI.Color_Red.ToBrush(),
				_ when willMain => highlightForeColor.ToBrush(),
				_ => Configuration.Config.UI.ForeColor.ToBrush(),
			};

			FleetEnemyBackColor = willMain switch
			{
				true => highlightBackColor.ToBrush(),
				_ => Color.Transparent.ToBrush(),
			};

			FleetEnemyEscortForeColor = willMain switch
			{
				true => Configuration.Config.UI.ForeColor.ToBrush(),
				_ => highlightForeColor.ToBrush(),
			};

			FleetEnemyEscortBackColor = willMain switch
			{
				true => Color.Transparent.ToBrush(),
				_ => highlightBackColor.ToBrush(),
			};
		}
		else
		{
			FleetEnemyForeColor = bm.Compass?.EventID switch
			{
				5 => Configuration.Config.UI.Color_Red.ToBrush(),
				_ => Configuration.Config.UI.ForeColor.ToBrush(),
			};
			FleetEnemyEscortForeColor = Configuration.Config.UI.ForeColor.ToBrush();
			FleetEnemyBackColor = FleetEnemyEscortBackColor = Color.Transparent.ToBrush();
		}

		FormationForeColor = (bm.FirstBattle.Searching.EngagementForm switch
		{
			3 => Configuration.Config.UI.Color_Green,
			4 => Configuration.Config.UI.Color_Red,
			_ => Configuration.Config.UI.ForeColor,
		}).ToBrush();
	}

	/// <summary>
	/// 索敵結果を設定します。
	/// </summary>
	private void SetSearchingResult(BattleData bd)
	{
		int search = bd.Searching.SearchingFriend;

		SearchingFriendText = Constants.GetSearchingResultShort(search);
		SearchingFriendIcon = search switch
		{
			<= 0 => null,
			< 4 => EquipmentIconType.Seaplane,
			_ => EquipmentIconType.Radar,
		};
		SearchingFriendToolTip = null;
		Smoker1Active = bd.Searching.SmokeCount >= 1;
		Smoker2Active = bd.Searching.SmokeCount >= 2;
		Smoker3Active = bd.Searching.SmokeCount >= 3;

		search = bd.Searching.SearchingEnemy;

		SearchingEnemyText = Constants.GetSearchingResultShort(search);
		SearchingEnemyIcon = search switch
		{
			<= 0 => null,
			< 4 => EquipmentIconType.Seaplane,
			_ => EquipmentIconType.Radar,
		};
		SearchingEnemyToolTip = null;
	}

	/// <summary>
	/// 索敵結果をクリアします。
	/// 索敵フェーズが発生しなかった場合にこれを設定します。
	/// </summary>
	private void ClearSearchingResult()
	{
		SearchingFriendText = "-";
		SearchingFriendIcon = null;
		SearchingFriendToolTip = null;
		Smoker1Active = false;
		Smoker2Active = false;
		Smoker3Active = false;

		SearchingEnemyText = "-";
		SearchingEnemyIcon = null;
		SearchingEnemyToolTip = null;
	}

	/// <summary>
	/// 基地航空隊フェーズの結果を設定します。
	/// </summary>
	private void SetBaseAirAttack(PhaseBaseAirAttack pd)
	{
		if (pd?.IsAvailable != true)
		{
			ClearBaseAirAttack();
			return;
		}

		SearchingText = FormBattle.ABText;
		SearchingIcon = EquipmentIconType.LandBasedAttacker;

		StringBuilder sb = new();
		int index = 1;

		foreach (PhaseBaseAirAttack.PhaseBaseAirAttackUnit phase in pd.AirAttackUnits)
		{
			sb.AppendFormat(GeneralRes.BaseWave + " - " + GeneralRes.BaseAirCorps + " :\r\n",
				index, phase.AirUnitID);

			if (phase.IsStage1Available)
			{
				sb.AppendFormat(
					"　St1: " + GeneralRes.FriendlyAir + " -{0}/{1} | " + GeneralRes.EnemyAir +
					" -{2}/{3} | {4}\r\n",
					phase.AircraftLostStage1Friend, phase.AircraftTotalStage1Friend,
					phase.AircraftLostStage1Enemy, phase.AircraftTotalStage1Enemy,
					Constants.GetAirSuperiority(phase.AirSuperiority));
			}

			if (phase.IsStage2Available)
			{
				sb.AppendFormat(
					"　St2: " + GeneralRes.FriendlyAir + " -{0}/{1} | " + GeneralRes.EnemyAir + " -{2}/{3}\r\n",
					phase.AircraftLostStage2Friend, phase.AircraftTotalStage2Friend,
					phase.AircraftLostStage2Enemy, phase.AircraftTotalStage2Enemy);
			}

			index++;
		}

		SearchingToolTip = sb.ToString();
	}

	/// <summary>
	/// 基地航空隊フェーズの結果をクリアします。
	/// </summary>
	private void ClearBaseAirAttack()
	{
		SearchingText = GeneralRes.ClearBaseAirAttack;
		SearchingIcon = null;
		SearchingToolTip = null;
	}

	/// <summary>
	/// 航空戦表示用ヘルパー
	/// </summary>
	private sealed class AerialWarfareFormatter
	{
		public PhaseAirBattleBase? Air { get; }
		public string PhaseName { get; set; }

		public AerialWarfareFormatter(PhaseAirBattleBase? air, string phaseName)
		{
			Air = air;
			PhaseName = phaseName;
		}

		[MemberNotNullWhen(true, nameof(Air))]
		public bool Enabled => Air is { IsAvailable: true };
		public bool Stage1Enabled => Enabled && Air.IsStage1Available;
		public bool Stage2Enabled => Enabled && Air.IsStage2Available;

		// this should never happen
		private static ArgumentOutOfRangeException InvalidStage(int phase) =>
			new($"Invalid stage {phase}.");

		public bool GetEnabled(int stage) => stage switch
		{
			1 => Stage1Enabled,
			2 => Stage2Enabled,
			_ => throw InvalidStage(stage),
		};

		public int GetAircraftLost(int stage, bool isFriend) => stage switch
		{
			1 when isFriend => Air.AircraftLostStage1Friend,
			1 => Air.AircraftLostStage1Enemy,

			2 when isFriend => Air.AircraftLostStage2Friend,
			2 => Air.AircraftLostStage2Enemy,

			_ => throw InvalidStage(stage),
		};

		public int GetAircraftTotal(int stage, bool isFriend) => stage switch
		{
			1 when isFriend => Air.AircraftTotalStage1Friend,
			1 => Air.AircraftTotalStage1Enemy,

			2 when isFriend => Air.AircraftTotalStage2Friend,
			2 => Air.AircraftTotalStage2Enemy,

			_ => throw InvalidStage(stage),
		};

		public int GetTouchAircraft(bool isFriend) => isFriend switch
		{
			true => Air.TouchAircraftFriend,
			_ => Air.TouchAircraftEnemy,
		};
	}

	private void SetAerialWarfare(PhaseAirBattleBase? phaseJet, PhaseAirBattleBase phase1)
		=> SetAerialWarfare(phaseJet, phase1, null);

	/// <summary>
	/// 航空戦情報を設定します。
	/// </summary>
	/// <param name="phaseJet">噴式航空戦のデータ。発生していなければ null</param>
	/// <param name="phase1">第一次航空戦（通常航空戦）のデータ。</param>
	/// <param name="phase2">第二次航空戦のデータ。発生していなければ null</param>
	private void SetAerialWarfare(PhaseAirBattleBase? phaseJet, PhaseAirBattleBase phase1, PhaseAirBattleBase? phase2)
	{
		List<AerialWarfareFormatter> phases = new()
		{
			new(phaseJet, FormBattle.AerialPhaseJet),
			new(phase1, FormBattle.AerialPhase1),
			new(phase2, FormBattle.AerialPhase2),
		};

		if (!phases[0].Enabled && !phases[2].Enabled)
		{
			phases[1].PhaseName = "";
		}

		(string?, string?, SolidColorBrush?, EquipmentIconType?) SetShootdown(int stage, bool isFriend, bool needAppendInfo)
		{
			string? labelText;
			string? labelToolTip;
			SolidColorBrush? labelForeColor;
			EquipmentIconType? labelIcon;

			List<AerialWarfareFormatter> phasesEnabled = phases
				.Where(p => p.GetEnabled(stage))
				.ToList();

			if (needAppendInfo)
			{
				labelText = string.Join(",", phasesEnabled.Select(p => "-" + p.GetAircraftLost(stage, isFriend)));
				labelToolTip = string.Join("", phasesEnabled.Select(p => $"{p.PhaseName}-{p.GetAircraftLost(stage, isFriend)}/{p.GetAircraftTotal(stage, isFriend)}\r\n"));
			}
			else
			{
				labelText = $"-{phases[1].GetAircraftLost(stage, isFriend)}/{phases[1].GetAircraftTotal(stage, isFriend)}";
				labelToolTip = null;
			}

			if (phasesEnabled.Any(p =>
				p.GetAircraftTotal(stage, isFriend) > 0 &&
				p.GetAircraftLost(stage, isFriend) == p.GetAircraftTotal(stage, isFriend)))
				labelForeColor = Configuration.Config.UI.Color_Red.ToBrush();
			else
				labelForeColor = Configuration.Config.UI.ForeColor.ToBrush();

			labelIcon = null;

			return (labelText, labelToolTip, labelForeColor, labelIcon);
		}

		void ClearAACutinLabel()
		{
			AACutinText = FormBattle.AirDefense;
			AACutinIcon = null;
			AACutinToolTip = null;
		}

		if (phases[1].Stage1Enabled)
		{
			bool needAppendInfo = phases[0].Stage1Enabled || phases[2].Stage1Enabled;
			List<AerialWarfareFormatter> phases1 = phases
				.Where(p => p.Stage1Enabled)
				.ToList();

			AirSuperiorityText = Constants.GetAirSuperiority(phases[1].Air.AirSuperiority);
			AirSuperiorityForeColor = (phases[1].Air.AirSuperiority switch
			{
				// AS+ or AS
				1 or 2 => Configuration.Config.UI.Color_Green,
				// AI or AI-
				3 or 4 => Configuration.Config.UI.Color_Red,

				_ => Configuration.Config.UI.ForeColor,
			}).ToBrush();

			AirSuperiorityToolTip = needAppendInfo switch
			{
				true => string.Join("", phases1.Select(p => $"{p.PhaseName}{Constants.GetAirSuperiority(p.Air.AirSuperiority)}\r\n")),
				_ => null,
			};

			(AirStage1FriendText, AirStage1FriendToolTip, AirStage1FriendForeColor, AirStage1FriendIcon) =
				SetShootdown(1, true, needAppendInfo);

			(AirStage1EnemyText, AirStage1EnemyToolTip, AirStage1EnemyForeColor, AirStage1EnemyIcon) =
				SetShootdown(1, false, needAppendInfo);

			(string?, EquipmentIconType?) SetTouch(bool isFriend, string? currentToolTip)
			{
				string? toolTip = currentToolTip;
				EquipmentIconType? icon;

				if (phases1.Any(p => p.GetTouchAircraft(isFriend) > 0))
				{
					icon = EquipmentIconType.Seaplane;
					toolTip += FormBattle.Contact + "\r\n" + string.Join("\r\n", phases1.Select(p => $"{p.PhaseName}{(KCDatabase.Instance.MasterEquipments[p.GetTouchAircraft(isFriend)]?.NameEN ?? FormBattle.None)}"));
				}
				else
				{
					icon = null;
				}

				return (toolTip, icon);
			}

			(AirStage1FriendToolTip, AirStage1FriendIcon) = SetTouch(true, AirStage1FriendToolTip);
			(AirStage1EnemyToolTip, AirStage1EnemyIcon) = SetTouch(false, AirStage1EnemyToolTip);
		}
		else
		{
			AirSuperiorityText = Constants.GetAirSuperiority(-1);
			AirSuperiorityToolTip = null;

			AirStage1FriendText = "-";
			AirStage1FriendForeColor = Configuration.Config.UI.ForeColor.ToBrush();
			AirStage1FriendIcon = null;
			AirStage1FriendToolTip = null;

			AirStage1EnemyText = "-";
			AirStage1EnemyForeColor = Configuration.Config.UI.ForeColor.ToBrush();
			AirStage1EnemyIcon = null;
			AirStage1EnemyToolTip = null;
		}

		if (phases[1].Stage2Enabled)
		{
			bool needAppendInfo = phases[0].Stage2Enabled || phases[2].Stage2Enabled;
			List<AerialWarfareFormatter> phases2 = phases
				.Where(p => p.Stage2Enabled)
				.ToList();

			(AirStage2FriendText, AirStage2FriendToolTip, AirStage2FriendForeColor, AirStage2FriendIcon) =
				SetShootdown(2, true, needAppendInfo);

			(AirStage2EnemyText, AirStage2EnemyToolTip, AirStage2EnemyForeColor, AirStage2EnemyIcon) =
				SetShootdown(2, false, needAppendInfo);

			if (phases2.Any(p => p.Air.IsAACutinAvailable))
			{
				AACutinText = "#" + string.Join("/", phases2.Select(p => p.Air.IsAACutinAvailable ? (p.Air.AACutInIndex + 1).ToString() : "-"));
				AACutinIcon = EquipmentIconType.HighAngleGun;

				string ConditionDisplay(int id) => AntiAirCutIn.FromId(id).EquipmentConditionsMultiLineDisplay();

				AACutinToolTip = FormBattle.AACI + "\r\n" + string.Join("\r\n", phases2
					.Select(p => p.PhaseName + (p.Air.IsAACutinAvailable ?
						$"{p.Air.AACutInShipName}\r\n{FormBattle.AACIType}{p.Air.AACutInKind}\n{ConditionDisplay(p.Air.AACutInKind)}"
						: FormBattle.DidNotActivate)));
			}
			else
			{
				ClearAACutinLabel();
			}
		}
		else
		{
			// ClearAircraftLabel(AirStage2Friend);
			// ClearAircraftLabel(AirStage2Enemy);
			AirStage2FriendText = "-";
			AirStage2FriendForeColor = Configuration.Config.UI.ForeColor.ToBrush();
			AirStage2FriendIcon = null;
			AirStage2FriendToolTip = null;

			AirStage2EnemyText = "-";
			AirStage2EnemyForeColor = Configuration.Config.UI.ForeColor.ToBrush();
			AirStage2EnemyIcon = null;
			AirStage2EnemyToolTip = null;

			ClearAACutinLabel();
		}
	}

	private void ClearAerialWarfare()
	{
		AirSuperiorityText = "-";
		AirSuperiorityToolTip = null;

		// ClearAircraftLabel(AirStage1Friend);
		// ClearAircraftLabel(AirStage1Enemy);
		// ClearAircraftLabel(AirStage2Friend);
		// ClearAircraftLabel(AirStage2Enemy);

		AirStage1FriendText = "-";
		AirStage1FriendForeColor = Configuration.Config.UI.ForeColor.ToBrush();
		AirStage1FriendIcon = null;
		AirStage1FriendToolTip = null;

		AirStage1EnemyText = "-";
		AirStage1EnemyForeColor = Configuration.Config.UI.ForeColor.ToBrush();
		AirStage1EnemyIcon = null;
		AirStage1EnemyToolTip = null;

		AirStage2FriendText = "-";
		AirStage2FriendForeColor = Configuration.Config.UI.ForeColor.ToBrush();
		AirStage2FriendIcon = null;
		AirStage2FriendToolTip = null;

		AirStage2EnemyText = "-";
		AirStage2EnemyForeColor = Configuration.Config.UI.ForeColor.ToBrush();
		AirStage2EnemyIcon = null;
		AirStage2EnemyToolTip = null;

		AACutinText = "-";
		AACutinIcon = null;
		AACutinToolTip = null;
	}

	/// <summary>
	/// 両軍のHPゲージを設定します。
	/// </summary>
	private void SetHPBar(BattleData bd)
	{
		KCDatabase db = KCDatabase.Instance;
		bool isPractice = bd.IsPractice;
		bool isFriendCombined = bd.IsFriendCombined;
		bool isEnemyCombined = bd.IsEnemyCombined;
		bool isBaseAirRaid = bd.IsBaseAirRaid;
		bool hasFriend7thShip = bd.Initial.FriendMaxHPs.Count(hp => hp > 0) == 7;

		IsPlayerCombinedFleet = isFriendCombined;
		IsEnemyCombinedFleet = isEnemyCombined;

		var initial = bd.Initial;
		var resultHPs = bd.ResultHPs;
		var attackDamages = bd.AttackDamages;

		/*
		foreach (var bar in HPBars)
			bar.SuspendUpdate();
		*/

		void EnableHPBar(int index, int initialHP, int resultHP, int maxHP, bool isTargetable)
		{
			HPBars[index].Value = resultHP;
			HPBars[index].PrevValue = initialHP;
			HPBars[index].MaximumValue = maxHP;
			HPBars[index].BackColor = Color.Transparent;
			HPBars[index].Visible = true;
			HPBars[index].IsTargetable = isTargetable;
		}

		void DisableHPBar(int index)
		{
			HPBars[index].Visible = false;
		}

		void SetEnemyBackground(int index)
		{
			HPBars[index].BackColor = HPBars[index] switch
			{
				{ Value: < 1, IsTargetable: true } => Color.FromArgb(0x40, 0x4D, 0xA6, 0xDF),
				_ => Color.Transparent,
			};
		}

		// friend main
		for (int i = 0; i < initial.FriendInitialHPs.Length; i++)
		{
			int refindex = BattleIndex.Get(BattleSides.FriendMain, i);

			if (initial.FriendInitialHPs[i] != -1)
			{
				EnableHPBar(refindex, initial.FriendInitialHPs[i], resultHPs[refindex], initial.FriendMaxHPs[i], true);

				string name;
				bool isEscaped;
				bool isLandBase;

				var bar = HPBars[refindex];

				if (isBaseAirRaid)
				{
					name = string.Format(FormBattle.AirBase, i + 1);
					isEscaped = false;
					isLandBase = true;
					// it's air base, not land base
					bar.Text = "AB";        //note: Land Base (Landing Boat もあるらしいが考えつかなかったので)

				}
				else
				{
					IShipData ship = bd.Initial.FriendFleet.MembersInstance[i];
					name = ship.NameWithLevel;
					isEscaped = bd.Initial.FriendFleet.EscapedShipList.Contains(ship.MasterID);
					isLandBase = ship.MasterShip.IsLandBase;
					bar.Text = KCDatabase.Instance.Translation.Ship.TypeNameShort(ship.MasterShip.ShipType);
				}

				bar.ToolTip = string.Format
				("{0}\r\nHP: ({1} → {2})/{3} ({4}) [{5}]\r\n" + GeneralRes.DamageDone + ": {6}\r\n\r\n{7}",
					name,
					Math.Max(bar.PrevValue, 0),
					Math.Max(bar.Value, 0),
					bar.MaximumValue,
					bar.Value - bar.PrevValue,
					Constants.GetDamageState((double)bar.Value / bar.MaximumValue, isPractice, isLandBase, isEscaped),
					attackDamages[refindex],
					bd.GetBattleDetail(refindex)
				);

				bar.BackColor = isEscaped switch
				{
					true => Configuration.Config.UI.Battle_ColorHPBarsEscaped,
					_ => Color.Transparent,
				};
				// bar.RepaintHPtext();
			}
			else
			{
				DisableHPBar(refindex);
			}
		}


		// enemy main
		for (int i = 0; i < initial.EnemyInitialHPs.Length; i++)
		{
			int refindex = BattleIndex.Get(BattleSides.EnemyMain, i);

			if (initial.EnemyInitialHPs[i] != -1)
			{
				EnableHPBar(refindex, initial.EnemyInitialHPs[i], resultHPs[refindex], initial.EnemyMaxHPs[i], initial.IsEnemyTargetable[i]);
				IShipDataMaster ship = bd.Initial.EnemyMembersInstance[i];

				var bar = HPBars[refindex];
				bar.Text = KCDatabase.Instance.Translation.Ship.TypeNameShort(ship.ShipType);
				SetEnemyBackground(refindex);

				bar.ToolTip =
					string.Format("{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]\r\n\r\n{7}",
						ship.NameWithClass,
						initial.EnemyLevels[i],
						Math.Max(bar.PrevValue, 0),
						Math.Max(bar.Value, 0),
						bar.MaximumValue,
						bar.Value - bar.PrevValue,
						Constants.GetDamageState((double)bar.Value / bar.MaximumValue, isPractice, ship.IsLandBase),
						bd.GetBattleDetail(refindex)
					);
			}
			else
			{
				DisableHPBar(refindex);
			}
		}


		// friend escort
		if (isFriendCombined)
		{
			for (int i = 0; i < initial.FriendInitialHPsEscort!.Length; i++)
			{
				int refindex = BattleIndex.Get(BattleSides.FriendEscort, i);

				if (initial.FriendInitialHPsEscort[i] != -1)
				{
					EnableHPBar(refindex, initial.FriendInitialHPsEscort[i], resultHPs[refindex], initial.FriendMaxHPsEscort![i], true);

					IShipData? ship = bd.Initial.FriendFleetEscort.MembersInstance![i];
					bool isEscaped = bd.Initial.FriendFleetEscort.EscapedShipList.Contains(ship.MasterID);

					var bar = HPBars[refindex];
					bar.Text = KCDatabase.Instance.Translation.Ship.TypeNameShort(ship.MasterShip.ShipType);

					bar.ToolTip = string.Format(
						"{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]\r\n" + GeneralRes.DamageDone + ": {7}\r\n\r\n{8}",
						ship.MasterShip.NameWithClass,
						ship.Level,
						Math.Max(bar.PrevValue, 0),
						Math.Max(bar.Value, 0),
						bar.MaximumValue,
						bar.Value - bar.PrevValue,
						Constants.GetDamageState((double)bar.Value / bar.MaximumValue, isPractice, ship.MasterShip.IsLandBase, isEscaped),
						attackDamages[refindex],
						bd.GetBattleDetail(refindex)
					);

					bar.BackColor = isEscaped switch
					{
						true => Configuration.Config.UI.Battle_ColorHPBarsEscaped,
						_ => Color.Transparent,
					};
					// bar.RepaintHPtext();
				}
				else
				{
					DisableHPBar(refindex);
				}
			}

		}
		else
		{
			// FleetFriendEscort.Visible = false;
			/*
			foreach (var i in BattleIndex.FriendEscort.Skip(Math.Max(bd.Initial.FriendFleet.Members.Count - 6, 0)))
				DisableHPBar(i);
			*/
		}

		MoveHPBar(hasFriend7thShip);



		// enemy escort
		if (isEnemyCombined)
		{
			// FleetEnemyEscort.Visible = true;

			for (int i = 0; i < 6; i++)
			{
				int refindex = BattleIndex.Get(BattleSides.EnemyEscort, i);

				if (initial.EnemyInitialHPsEscort![i] != -1)
				{
					EnableHPBar(refindex, initial.EnemyInitialHPsEscort[i], resultHPs[refindex], initial.EnemyMaxHPsEscort![i], initial.IsEnemyTargetableEscort[i]);

					IShipDataMaster ship = bd.Initial.EnemyMembersEscortInstance![i];

					var bar = HPBars[refindex];
					bar.Text = KCDatabase.Instance.Translation.Ship.TypeNameShort(ship.ShipType);
					SetEnemyBackground(refindex);

					bar.ToolTip =
						string.Format("{0} Lv. {1}\r\nHP: ({2} → {3})/{4} ({5}) [{6}]\r\n\r\n{7}",
							ship.NameWithClass,
							bd.Initial.EnemyLevelsEscort![i],
							Math.Max(bar.PrevValue, 0),
							Math.Max(bar.Value, 0),
							bar.MaximumValue,
							bar.Value - bar.PrevValue,
							Constants.GetDamageState((double)bar.Value / bar.MaximumValue, isPractice, ship.IsLandBase),
							bd.GetBattleDetail(refindex)
						);
				}
				else
				{
					DisableHPBar(refindex);
				}
			}

		}
		else
		{
			// FleetEnemyEscort.Visible = false;
			/*
			foreach (var i in BattleIndex.EnemyEscort)
				DisableHPBar(i);
			*/
		}

		{   // support
			PhaseSupport? support = null;

			if (bd is BattleDayFromNight bddn && (bddn.NightSupport?.IsAvailable ?? false))
			{
				support = bddn.NightSupport;
			}

			support ??= bd.Support;

			if (support?.IsAvailable ?? false)
			{
				FleetFriendIcon = support.SupportFlag switch
				{
					1 => EquipmentIconType.CarrierBasedTorpedo,
					2 => EquipmentIconType.MainGunLarge,
					3 => EquipmentIconType.Torpedo,
					4 => EquipmentIconType.DepthCharge,
					_ => EquipmentIconType.Unknown,
				};

				FleetFriendToolTip = FormBattle.SupportExpedition + "\r\n" + support.GetBattleDetail();

				FleetFriendText = ((isFriendCombined || hasFriend7thShip) && isEnemyCombined) switch
				{
					true => FormBattle.FleetFriendShort,
					_ => FormBattle.FleetFriend,
				};
			}
			else
			{
				FleetFriendIcon = null;
				FleetFriendText = FormBattle.FleetFriend;
				FleetFriendToolTip = null;
			}
		}

		// sunk background should be prioritized
		if (HPBars[BattleIndex.EnemyMain1].Value > 0)
		{
			HPBars[BattleIndex.EnemyMain1].BackColor = bd.Initial.IsBossDamaged switch
			{
				true => Configuration.Config.UI.Battle_ColorHPBarsBossDamaged,
				_ => Color.Transparent,
			};
		}

		if (!isBaseAirRaid)
		{
			foreach (int i in bd.MVPShipIndexes)
			{
				HPBars[BattleIndex.Get(BattleSides.FriendMain, i)].BackColor = Configuration.Config.UI.Battle_ColorHPBarsMVP;
			}

			if (isFriendCombined)
			{
				foreach (int i in bd.MVPShipCombinedIndexes)
				{
					HPBars[BattleIndex.Get(BattleSides.FriendEscort, i)].BackColor = Configuration.Config.UI.Battle_ColorHPBarsMVP;
				}
			}
		}
	}

	private bool _hpBarMoved = false;
	/// <summary>
	/// 味方遊撃部隊７人目のHPゲージ（通常時は連合艦隊第二艦隊旗艦のHPゲージ）を移動します。
	/// </summary>
	private void MoveHPBar(bool hasFriend7thShip)
	{

		if (Configuration.Config.FormBattle.Display7thAsSingleLine && hasFriend7thShip)
		{
			if (_hpBarMoved) return;

			PlayerMainHPBars.Add(PlayerEscortHPBars[0]);
			PlayerEscortHPBars.RemoveAt(0);

			_hpBarMoved = true;
		}
		else
		{
			if (!_hpBarMoved) return;

			PlayerEscortHPBars.Insert(0, PlayerMainHPBars[6]);
			PlayerMainHPBars.RemoveAt(6);

			// TableBottom.SetCellPosition(HPBars[BattleIndex.FriendEscort1], new TableLayoutPanelCellPosition(1, 1));
			// ControlHelper.SetTableRowStyle(TableBottom, 7, new RowStyle(SizeType.Absolute, 0));
			_hpBarMoved = false;
		}

	}


	/// <summary>
	/// 損害率と戦績予測を設定します。
	/// </summary>
	private void SetDamageRate(BattleManager bm)
	{

		int rank = bm.PredictWinRank(out double friendrate, out double enemyrate);
		bm.PredictedBattleRank = Constants.GetWinRank(rank);
		DamageFriendText = friendrate.ToString("p1");
		DamageEnemyText = enemyrate.ToString("p1");

		if (bm.IsBaseAirRaid)
		{
			int kind = bm.Compass.AirRaidDamageKind;
			WinRankText = Constants.GetAirRaidDamageShort(kind);
			WinRankForeColor = (1 <= kind && kind <= 3) ? WinRankColor_Lose : WinRankColor_Win;
		}
		else
		{
			WinRankText = Constants.GetWinRank(rank);
			WinRankForeColor = rank >= 4 ? WinRankColor_Win : WinRankColor_Lose;
		}
	}

	/// <summary>
	/// 夜戦における各種表示を設定します。
	/// </summary>
	private void SetNightBattleEvent(PhaseNightInitial pd)
	{
		FleetData fleet = pd.FriendFleet;

		//味方探照灯判定
		{
			int index = pd.SearchlightIndexFriend;

			if (index != -1)
			{
				IShipData ship = fleet.MembersInstance[index];

				AirStage1FriendText = "#" + (index + (pd.IsFriendEscort ? 6 : 0) + 1);
				AirStage1FriendForeColor = Configuration.Config.UI.ForeColor.ToBrush();
				AirStage1FriendIcon = EquipmentIconType.Searchlight;
				AirStage1FriendToolTip = GeneralRes.SearchlightUsed + ": " + ship.NameWithLevel;
			}
			else
			{
				AirStage1FriendToolTip = null;
			}
		}

		//敵探照灯判定
		{
			int index = pd.SearchlightIndexEnemy;
			if (index != -1)
			{
				AirStage1EnemyText = "#" + (index + (pd.IsEnemyEscort ? 6 : 0) + 1);
				AirStage1EnemyForeColor = Configuration.Config.UI.ForeColor.ToBrush();
				AirStage1EnemyIcon = EquipmentIconType.Searchlight;
				AirStage1EnemyToolTip = GeneralRes.SearchlightUsed + ": " + pd.SearchlightEnemyInstance.NameWithClass;
			}
			else
			{
				AirStage1EnemyToolTip = null;
			}
		}

		//夜間触接判定
		if (pd.TouchAircraftFriend != -1)
		{
			SearchingFriendText = GeneralRes.NightContact;
			SearchingFriendIcon = EquipmentIconType.Seaplane;
			SearchingFriendToolTip = GeneralRes.NightContacting + ": " + KCDatabase.Instance.MasterEquipments[pd.TouchAircraftFriend].NameEN;
		}
		else
		{
			SearchingFriendToolTip = null;
		}

		if (pd.TouchAircraftEnemy != -1)
		{
			SearchingEnemyText = GeneralRes.NightContact;
			SearchingEnemyIcon = EquipmentIconType.Seaplane;
			SearchingEnemyToolTip = GeneralRes.NightContacting + ": " + KCDatabase.Instance.MasterEquipments[pd.TouchAircraftEnemy].NameEN;
		}
		else
		{
			SearchingEnemyToolTip = null;
		}

		//照明弾投射判定
		{
			int index = pd.FlareIndexFriend;

			if (index != -1)
			{
				AirStage2FriendText = "#" + (index + 1);
				AirStage2FriendForeColor = Configuration.Config.UI.ForeColor.ToBrush();
				AirStage2FriendIcon = EquipmentIconType.StarShell;
				AirStage2FriendToolTip = GeneralRes.StarShellUsed + ": " + pd.FlareFriendInstance.NameWithLevel;

			}
			else
			{
				AirStage2FriendToolTip = null;
			}
		}

		{
			int index = pd.FlareIndexEnemy;

			if (index != -1)
			{
				AirStage2EnemyText = "#" + (index + 1);
				AirStage2EnemyForeColor = Configuration.Config.UI.ForeColor.ToBrush();
				AirStage2EnemyIcon = EquipmentIconType.StarShell;
				AirStage2EnemyToolTip = GeneralRes.StarShellUsed + ": " + pd.FlareEnemyInstance.NameWithClass;
			}
			else
			{
				AirStage2EnemyToolTip = null;
			}
		}
	}


	/// <summary>
	/// 戦闘終了後に、MVP艦の表示を更新します。
	/// </summary>
	/// <param name="bm">戦闘データ。</param>
	private void SetMVPShip(BattleManager bm)
	{
		bool isCombined = bm.IsCombinedBattle;

		BattleData bd = bm.StartsFromDayBattle switch
		{
			true => bm.BattleDay,
			_ => bm.BattleNight,
		};

		BattleResultData br = bm.Result;

		FleetData friend = bd.Initial.FriendFleet;
		FleetData? escort = isCombined switch
		{
			false => null,
			_ => bd.Initial.FriendFleetEscort,
		};

		for (int i = 0; i < friend.Members.Count; i++)
		{
			if (friend.EscapedShipList.Contains(friend.Members[i]))
			{
				HPBars[i].BackColor = Configuration.Config.UI.Battle_ColorHPBarsEscaped;
			}
			else if (br.MVPIndex == i + 1)
			{
				HPBars[i].BackColor = Configuration.Config.UI.Battle_ColorHPBarsMVP;
			}
			else
			{
				HPBars[i].BackColor = Color.Transparent;
			}
		}

		if (escort != null)
		{
			for (int i = 0; i < escort.Members.Count; i++)
			{
				if (escort.EscapedShipList.Contains(escort.Members[i]))
				{
					HPBars[i + 6].BackColor = Configuration.Config.UI.Battle_ColorHPBarsEscaped;
				}
				else if (br.MVPIndexCombined == i + 1)
				{
					HPBars[i + 6].BackColor = Configuration.Config.UI.Battle_ColorHPBarsMVP;
				}
				else
				{
					HPBars[i + 6].BackColor = Color.Transparent;
				}
			}
		}
	}


	private void ConfigurationChanged()
	{
		Configuration.ConfigurationData config = Configuration.Config;

		Color[] colorScheme = config.UI.BarColorScheme
			.Select(col => col.ColorData)
			.ToArray();

		foreach (HealthBarViewModel hpBar in HPBars)
		{
			hpBar.SetBarColorScheme(colorScheme);
			hpBar.ColorMorphing = config.UI.BarColorMorphing;
		}

		CompactMode = Configuration.Config.FormBattle.CompactMode;
	}
}
