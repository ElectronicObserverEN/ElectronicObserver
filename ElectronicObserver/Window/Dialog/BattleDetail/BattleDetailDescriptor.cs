using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;
using ElectronicObserverTypes.AntiAir;

namespace ElectronicObserver.Window.Dialog.BattleDetail;

public static class BattleDetailDescriptor
{
	public static string GetBattleDetail(BattleManager bm)
	{
		StringBuilder sb = new();

		if (bm.IsPractice)
		{
			sb.AppendLine(BattleRes.Exercise);

		}
		else
		{
			sb.AppendFormat("{0} ({1}-{2})", bm.Compass.MapInfo.NameEN, bm.Compass.MapAreaID, bm.Compass.MapInfoID);
			if (bm.Compass.MapInfo.EventDifficulty > 0)
				sb.AppendFormat(" [{0}]", Constants.GetDifficulty(bm.Compass.MapInfo.EventDifficulty));
			sb.Append(ConstantsRes.BattleDetail_Node).Append(bm.Compass.CellId.ToString());
			if (bm.Compass.EventID == 5)
				sb.Append(ConstantsRes.BattleDetail_Boss);
			sb.AppendLine();

			MapInfoData mapinfo = bm.Compass.MapInfo;
			if (!mapinfo.IsCleared)
			{
				if (mapinfo.RequiredDefeatedCount != -1)
				{
					sb.AppendFormat(BattleRes.ClearProgress, mapinfo.CurrentDefeatedCount, mapinfo.RequiredDefeatedCount)
						.AppendLine();
				}
				else if (mapinfo.MapHPMax > 0)
				{
					int current = bm.Compass.MapHPCurrent > 0 ? bm.Compass.MapHPCurrent : mapinfo.MapHPCurrent;
					int max = bm.Compass.MapHPMax > 0 ? bm.Compass.MapHPMax : mapinfo.MapHPMax;
					sb.AppendFormat("{0}{1}: {2} / {3}", mapinfo.CurrentGaugeIndex > 0 ? "#" + mapinfo.CurrentGaugeIndex + " " : "", mapinfo.GaugeType == 3 ? "TP" : "HP", current, max)
						.AppendLine();
				}
			}
		}
		if (bm.Result != null)
		{
			sb.AppendLine(bm.Result.EnemyFleetName);
		}
		sb.AppendLine();

		if (bm.HeavyBaseAirRaids.Count > 0)
		{
			foreach (BattleBaseAirRaid baseAirRaid in bm.HeavyBaseAirRaids)
			{
				sb.AppendFormat("◆ {0} ◆\r\n", baseAirRaid.Title)
					.AppendLine(GetBattleDetail(baseAirRaid));
			}
		}
		else
		{
			sb.AppendFormat("◆ {0} ◆\r\n", bm.FirstBattle.Title).AppendLine(GetBattleDetail(bm.FirstBattle));
			if (bm.SecondBattle != null)
				sb.AppendFormat("◆ {0} ◆\r\n", bm.SecondBattle.Title).AppendLine(GetBattleDetail(bm.SecondBattle));
		}


		if (bm.Result != null)
		{
			sb.AppendLine(GetBattleResult(bm));
		}

		return sb.ToString();
	}


	public static string GetBattleDetail(BattleData battle)
	{
		StringBuilder sbmaster = new();
		bool isBaseAirRaid = battle.IsBaseAirRaid;

		foreach (PhaseBase phase in battle.Phases)
		{
			StringBuilder sb = new();

			switch (phase)
			{
				case PhaseBaseAirRaid p:

					sb.AppendLine(ConstantsRes.BattleDetail_AirAttackUnits);
					sb.Append("　").AppendLine(string.Join(", ",
						p.Squadrons.Where(sq => sq.Equipment != null).Select(sq => sq.ToString())
							.DefaultIfEmpty(BattleRes.Empty)));

					GetBattleDetailPhaseAirBattle(sb, p);

					break;

				case PhaseAirBattle p:

					GetBattleDetailPhaseAirBattle(sb, p);

					break;

				case PhaseBaseAirAttack p:

					foreach (PhaseBaseAirAttackUnit a in p.Units)
					{
						sb.AppendFormat(ConstantsRes.BattleDetail_AirAttackWave + "\r\n", a.WaveIndex + 1);

						sb.AppendLine(ConstantsRes.BattleDetail_AirAttackUnits);
						sb.Append("　").AppendLine(string.Join(", ",
							a.Squadrons.Where(sq => sq.Equipment != null).Select(sq => sq.ToString())));

						GetBattleDetailPhaseAirBattle(sb, a);
						sb.Append(a.GetBattleDetail());
					}

					break;

				case PhaseJetAirBattle p:
					GetBattleDetailPhaseAirBattle(sb, p);

					break;

				case PhaseJetBaseAirAttack p:

					foreach (PhaseBaseAirAttackUnit a in p.Units)
					{
						sb.AppendFormat(ConstantsRes.BattleDetail_AirAttackWave + "\r\n", a.WaveIndex + 1);

						sb.AppendLine(ConstantsRes.BattleDetail_AirAttackUnits);
						sb.Append("　").AppendLine(string.Join(", ",
							a.Squadrons.Where(sq => sq.Equipment != null).Select(sq => sq.ToString())));

						GetBattleDetailPhaseAirBattle(sb, a);
						sb.Append(a.GetBattleDetail());
					}

					break;

				case PhaseInitial p:


					if (p.FleetsBeforePhase.EscortFleet != null)
						sb.Append(ConstantsRes.BattleDetail_FriendMainFleet);
					else
						sb.Append(ConstantsRes.BattleDetail_FriendFleet);


					void AppendFleetInfo(IFleetData fleet)
					{
						sb.Append($" {BattleRes.AirSuperiority} ");
						sb.Append(GetRangeString(Calculator.GetAirSuperiority(fleet, false),
							Calculator.GetAirSuperiority(fleet, true)));

						static double Truncate2(double value) => Math.Floor(value * 100) / 100;
						sb.AppendFormat(BattleRes.Los,
							Truncate2(Calculator.GetSearchingAbility_New33(fleet, 1)),
							Truncate2(Calculator.GetSearchingAbility_New33(fleet, 2)),
							Truncate2(Calculator.GetSearchingAbility_New33(fleet, 3)),
							Truncate2(Calculator.GetSearchingAbility_New33(fleet, 4)));
					}

					if (isBaseAirRaid)
					{
						sb.AppendLine();
						OutputFriendBase(sb, p.FriendInitialHPs, p.FriendMaxHPs);
					}
					else
					{
						AppendFleetInfo(p.FleetsBeforePhase.Fleet);
						sb.AppendLine();
						OutputFriendData(sb, p.FleetsBeforePhase.Fleet, p.FriendInitialHPs, p.FriendMaxHPs);
					}

					if (p.FleetsBeforePhase.EscortFleet != null)
					{
						sb.AppendLine();
						sb.Append(ConstantsRes.BattleDetail_FriendEscortFleet);
						AppendFleetInfo(p.FleetsBeforePhase.EscortFleet);
						sb.AppendLine();

						OutputFriendData(sb, p.FleetsBeforePhase.EscortFleet, p.FriendInitialHPsEscort,
							p.FriendMaxHPsEscort);
					}


					sb.AppendLine();


					void AppendEnemyFleetInfo(int[] members)
					{
						int air = 0;
						int airbase = 0;
						bool indeterminate = false;
						for (int i = 0; i < members.Length; i++)
						{
							ShipParameterRecord.ShipParameterElement? param =
								RecordManager.Instance.ShipParameter[members[i]];
							if (param == null) continue;

							if (param.DefaultSlot == null || param.Aircraft == null)
							{
								indeterminate = true;
								continue;
							}

							for (int s = 0; s < Math.Min(param.DefaultSlot.Length, param.Aircraft.Length); s++)
							{
								air += Calculator.GetAirSuperiority(param.DefaultSlot[s], param.Aircraft[s]);
								if (KCDatabase.Instance.MasterEquipments[param.DefaultSlot[s]]?.IsAircraft ?? false)
									airbase += Calculator.GetAirSuperiority(param.DefaultSlot[s], param.Aircraft[s], 0,
										0, AirBaseActionKind.Mission);
							}
						}

						sb.AppendFormat(BattleRes.AirBaseAirPower, air, airbase);
						if (indeterminate)
							sb.Append(BattleRes.ToBeDetermined);
					}

					if (p.EnemyMembersEscort != null)
						sb.Append(ConstantsRes.BattleDetail_EnemyMainFleet);
					else
						sb.Append(ConstantsRes.BattleDetail_EnemyFleet);

					AppendEnemyFleetInfo(p.EnemyMembers);

					if (p.IsBossDamaged)
						sb.Append(BattleRes.BossDebuffed);
					sb.AppendLine();

					OutputEnemyData(sb, p.EnemyMembersInstance, p.EnemyLevels, p.EnemyInitialHPs, p.EnemyMaxHPs,
						p.EnemySlotsInstance, p.EnemyParameters);


					if (p.EnemyMembersEscort != null)
					{
						sb.AppendLine();
						sb.AppendLine(ConstantsRes.BattleDetail_EnemyEscortFleet);

						AppendEnemyFleetInfo(p.EnemyMembersEscort);
						sb.AppendLine();

						OutputEnemyData(sb, p.EnemyMembersEscortInstance, p.EnemyLevelsEscort, p.EnemyInitialHPsEscort,
							p.EnemyMaxHPsEscort, p.EnemySlotsEscortInstance, p.EnemyParametersEscort);
					}

					sb.AppendLine();

					if (battle.Phases.Any(ph => ph is PhaseBaseAirAttack or PhaseBaseAirRaid))
					{
						sb.AppendLine(ConstantsRes.BattleDetail_AirBase);
						GetBattleDetailBaseAirCorps(sb, KCDatabase.Instance.Battle.Compass.MapAreaID); // :(
						sb.AppendLine();
					}

					if (p.ApiCombatRation?.Count > 0)
					{
						sb.AppendLine($"〈{BattleRes.CombatRation}〉");
						foreach (int index in p.ApiCombatRation)
						{
							IShipData? ship = p.FleetsBeforePhase.GetShip(new(index, FleetFlag.Player));

							if (ship != null)
							{
								sb.AppendFormat("　{0} #{1}\r\n", ship.NameWithLevel, index + 1);
							}
						}

						sb.AppendLine();
					}

					break;

				case PhaseNightInitial p:

				{
					IEquipmentDataMaster? eq = p.TouchAircraftFriend;
					if (eq != null)
					{
						sb.Append(ConstantsRes.BattleDetail_FriendlyNightContact).AppendLine(eq.NameEN);
					}

					eq = p.TouchAircraftEnemy;
					if (eq != null)
					{
						sb.Append(ConstantsRes.BattleDetail_EnemyNightContact).AppendLine(eq.NameEN);
					}
				}

				{
					if (p.SearchlightFriend is not null)
					{
						sb.AppendFormat(ConstantsRes.BattleDetail_FriendlySearchlight + "\r\n",
							p.SearchlightFriend.Name, p.SearchlightIndexFriend + 1);
					}

					if (p.SearchlightEnemy is not null)
					{
						sb.AppendFormat(ConstantsRes.BattleDetail_EnemySearchlight + "\r\n",
							p.SearchlightEnemy.MasterShip.NameWithClass, p.SearchlightIndexEnemy + 1);
					}
				}

				if (p.FlareFriend is not null)
				{
					sb.AppendFormat(ConstantsRes.BattleDetail_FriendlyStarshell + "\r\n",
						p.FlareFriend.NameWithLevel, p.FlareIndexFriend + 1);
				}

				if (p.FlareEnemy is not null)
				{
					sb.AppendFormat(ConstantsRes.BattleDetail_EnemyStarshell + "\r\n",
						p.FlareEnemy.MasterShip.NameWithClass, p.FlareIndexEnemy + 1);
				}

				sb.AppendLine();
				break;


				case PhaseSearching p:
					sb.Append($"{BattleRes.Formation}: ").Append(Constants.GetFormation(p.PlayerFormationType));
					sb.Append($" / {BattleRes.EnemyFormation}: ")
						.AppendLine(Constants.GetFormation(p.EnemyFormationType));
					sb.Append($"{BattleRes.Engagement}: ").AppendLine(Constants.GetEngagementForm(p.EngagementType));
					sb.Append($"{BattleRes.Contact}: ").Append(Constants.GetSearchingResult(p.PlayerDetectionType));
					sb.Append($" / {BattleRes.EnemyContact}: ")
						.AppendLine(Constants.GetSearchingResult(p.EnemyDetectionType));

					if (p.SmokeCount > 0)
					{
						sb.AppendLine($"{BattleRes.SmokeScreen} x{p.SmokeCount}");
					}

					sb.AppendLine();

					break;

				case PhaseSupport p:
					if (p.SupportFleet is not null)
					{
						sb.AppendLine($"〈{BattleRes.SupportFleet}〉");
						OutputSupportData(sb, p.SupportFleet);
						sb.AppendLine();
					}

					break;

				case PhaseFriendlySupportInfo p:
					OutputFriendlySupportData(sb, p);
					sb.AppendLine();
					break;

				case PhaseFriendlyShelling p:
					if (p.SearchlightFriend is not null)
					{
						sb.AppendFormat(ConstantsRes.BattleDetail_FriendlySearchlight + "\r\n",
							p.SearchlightFriend.MasterShip.NameWithClass, p.SearchlightIndexFriend + 1);
					}

					if (p.SearchlightEnemy is not null)
					{
						sb.AppendFormat(ConstantsRes.BattleDetail_EnemySearchlight + "\r\n",
							p.SearchlightEnemy.MasterShip.NameWithClass, p.SearchlightIndexEnemy + 1);
					}

					if (p.FlareFriend is not null)
					{
						sb.AppendFormat(ConstantsRes.BattleDetail_FriendlyStarshell + "\r\n",
							p.FlareEnemy.MasterShip.NameWithClass, p.FlareIndexFriend + 1);
					}

					if (p.FlareEnemy is not null)
					{
						sb.AppendFormat(ConstantsRes.BattleDetail_EnemyStarshell + "\r\n",
							p.FlareEnemy.MasterShip.NameWithClass, p.FlareIndexEnemy + 1);
					}

					sb.AppendLine();
					break;

				case PhaseFriendlyAirBattle p:

					GetBattleDetailPhaseAirBattle(sb, p);

					break;

				case AttackPhaseBase attackPhase:

					sb.AppendLine(attackPhase.GetBattleDetail());

					break;
			}

			if (sb.Length > 0)
			{
				sbmaster.AppendFormat("== {0} ==\r\n", phase.Title).Append(sb);
			}
		}


		{
			sbmaster.AppendLine(ConstantsRes.BattleDetail_BattleEnd);

			IFleetData? friend = battle.FleetsAfterBattle.Fleet;
			IFleetData? friendescort = battle.FleetsAfterBattle.EscortFleet;
			List<IShipData?> enemy = battle.Initial.EnemyMembersInstance;
			List<IShipData?>? enemyescort = battle.Initial.EnemyMembersEscortInstance;
			List<int> resultHps = battle.ResultHPs.ToList();

			if (friendescort != null)
				sbmaster.AppendLine(ConstantsRes.BattleDetail_FriendMainFleet);
			else
				sbmaster.AppendLine(ConstantsRes.BattleDetail_FriendFleet);

			if (isBaseAirRaid)
			{

				for (int i = 0; i < 6; i++)
				{
					if (battle.Initial.FriendMaxHPs[i] <= 0)
						continue;

					OutputResultData(sbmaster, i, string.Format(BattleRes.Base, i + 1),
						battle.Initial.FriendInitialHPs[i], resultHps[i], battle.Initial.FriendMaxHPs[i]);
				}

			}
			else
			{
				for (int i = 0; i < friend.Members.Count(); i++)
				{
					var ship = friend.MembersInstance[i];
					if (ship == null)
						continue;

					OutputResultData(sbmaster, i, ship.Name,
						battle.Initial.FriendInitialHPs[i], resultHps[i], battle.Initial.FriendMaxHPs[i]);
				}
			}

			if (friendescort != null)
			{
				sbmaster.AppendLine().AppendLine(ConstantsRes.BattleDetail_FriendEscortFleet);

				for (int i = 0; i < friendescort.Members.Count(); i++)
				{
					var ship = friendescort.MembersInstance[i];
					if (ship == null)
						continue;

					OutputResultData(sbmaster, i + 6, ship.Name,
						battle.Initial.FriendInitialHPsEscort[i], resultHps[i + 6], battle.Initial.FriendMaxHPsEscort[i]);
				}

			}


			sbmaster.AppendLine();
			if (enemyescort != null)
				sbmaster.AppendLine(ConstantsRes.BattleDetail_EnemyMainFleet);
			else
				sbmaster.AppendLine(ConstantsRes.BattleDetail_EnemyFleet);

			for (int i = 0; i < enemy.Count; i++)
			{
				IShipData? ship = enemy[i];
				if (ship == null)
					continue;

				OutputResultData(sbmaster, i,
					ship.MasterShip.NameWithClass,
					battle.Initial.EnemyInitialHPs[i], resultHps[i + 12], battle.Initial.EnemyMaxHPs[i]);
			}

			if (enemyescort != null)
			{
				sbmaster.AppendLine().AppendLine(ConstantsRes.BattleDetail_EnemyEscortFleet);

				for (int i = 0; i < enemyescort.Count; i++)
				{
					IShipData? ship = enemyescort[i];
					if (ship == null)
						continue;

					OutputResultData(sbmaster, i + 6, ship.MasterShip.NameWithClass,
						battle.Initial.EnemyInitialHPsEscort[i], resultHps[i + 18], battle.Initial.EnemyMaxHPsEscort[i]);
				}
			}

			sbmaster.AppendLine();
		}

		return sbmaster.ToString();
	}

	private static string GetRangeString(int min, int max) => min != max ? $"{min} ～ {max}" : min.ToString();

	private static void GetBattleDetailBaseAirCorps(StringBuilder sb, int mapAreaID)
	{
		foreach (BaseAirCorpsData corps in KCDatabase.Instance.BaseAirCorps.Values.Where(corps => corps.MapAreaID == mapAreaID))
		{
			sb.AppendFormat("{0} [{1}] " + BattleRes.AirSuperiority + " {2}\r\n　{3}\r\n",
				corps.Name, Constants.GetBaseAirCorpsActionKind(corps.ActionKind),
				GetRangeString(Calculator.GetAirSuperiority(corps, false), Calculator.GetAirSuperiority(corps, true)),
				string.Join(", ", corps.Squadrons.Values
					.Where(sq => sq.State == 1 && sq.EquipmentInstance != null)
					.Select(sq => sq.EquipmentInstance.NameWithLevel)));
		}
	}

	private static void GetBattleDetailPhaseAirBattle(StringBuilder sb, PhaseAirBattleBase p)
	{

		if (p.IsStage1Available)
		{
			sb.Append("Stage 1: ").AppendLine(Constants.GetAirSuperiority(p.AirState));
			sb.AppendFormat($"　{BattleRes.Friendly}: -{{0}}/{{1}}\r\n　{BattleRes.Enemy}: -{{2}}/{{3}}\r\n",
				p.AircraftLostStage1Friend, p.AircraftTotalStage1Friend,
				p.AircraftLostStage1Enemy, p.AircraftTotalStage1Enemy);
			if (p.TouchAircraftFriend is not null)
				sb.AppendFormat($"　{BattleRes.Contact}: {{0}}\r\n", p.TouchAircraftFriend);
			if (p.TouchAircraftEnemy is not null)
				sb.AppendFormat($"　{BattleRes.EnemyContact}: {{0}}\r\n", p.TouchAircraftEnemy);
		}
		if (p.IsStage2Available)
		{
			sb.Append("Stage 2: ");
			if (p.IsAACutinAvailable)
			{
				sb.AppendFormat(BattleRes.AaciType,
					p.AACutInShipName,
					AntiAirCutIn.FromId(p.AACutInKind).EquipmentConditionsSingleLineDisplay(),
					p.AACutInKind);
			}
			sb.AppendLine();
			sb.AppendFormat($"　{BattleRes.Friendly}: -{{0}}/{{1}}\r\n　{BattleRes.Enemy}: -{{2}}/{{3}}\r\n",
				p.AircraftLostStage2Friend, p.AircraftTotalStage2Friend,
				p.AircraftLostStage2Enemy, p.AircraftTotalStage2Enemy);
		}

		if (p.IsStage1Available || p.IsStage2Available)
			sb.AppendLine();
	}


	private static void GetBattleDetailPhaseAirBattle(StringBuilder sb, PhaseBaseAirRaid p)
	{

		if (p.IsStage1Available)
		{
			sb.Append("Stage 1: ").AppendLine(Constants.GetAirSuperiority(p.AirState));
			sb.AppendFormat($"　{BattleRes.Friendly}: -{{0}}/{{1}}\r\n　{BattleRes.Enemy}: -{{2}}/{{3}}\r\n",
				p.AircraftLostStage1Friend, p.AircraftTotalStage1Friend,
				p.AircraftLostStage1Enemy, p.AircraftTotalStage1Enemy);
			if (p.TouchAircraftFriend is not null)
				sb.AppendFormat($"　{BattleRes.Contact}: {{0}}\r\n", p.TouchAircraftFriend);
			if (p.TouchAircraftEnemy is not null)
				sb.AppendFormat($"　{BattleRes.EnemyContact}: {{0}}\r\n", p.TouchAircraftEnemy);
		}
		if (p.IsStage2Available)
		{
			sb.Append("Stage 2: ");
			if (p.IsAACutinAvailable)
			{
				sb.AppendFormat(BattleRes.AaciType,
					p.AACutInShipName,
					AntiAirCutIn.FromId(p.AACutInKind).EquipmentConditionsSingleLineDisplay(),
					p.AACutInKind);
			}
			sb.AppendLine();
			sb.AppendFormat($"　{BattleRes.Friendly}: -{{0}}/{{1}}\r\n　{BattleRes.Enemy}: -{{2}}/{{3}}\r\n",
				p.AircraftLostStage2Friend, p.AircraftTotalStage2Friend,
				p.AircraftLostStage2Enemy, p.AircraftTotalStage2Enemy);
		}

		if (p.IsStage1Available || p.IsStage2Available)
			sb.AppendLine();
	}


	private static void OutputFriendData(StringBuilder sb, IFleetData fleet, List<int> initialHPs, List<int> maxHPs)
	{

		for (int i = 0; i < fleet.MembersInstance.Count; i++)
		{
			IShipData? ship = fleet.MembersInstance[i];

			if (ship == null)
				continue;

			sb.AppendFormat($"#{{0}}: {{1}} {{2}} " +
							$"HP: {{3}} / {{4}} - " +
							$"{GeneralRes.Firepower}:{{5}}, " +
							$"{GeneralRes.Torpedo}:{{6}}, " +
							$"{GeneralRes.AntiAir}:{{7}}, " +
							$"{GeneralRes.Armor}:{{8}}{{9}}\r\n",
				i + 1,
				ship.MasterShip.ShipTypeName, ship.NameWithLevel,
				initialHPs[i], maxHPs[i],
				ship.FirepowerBase, ship.TorpedoBase, ship.AABase, ship.ArmorBase,
				fleet.EscapedShipList.Contains(ship.MasterID) ? $" ({BattleRes.Escaped})" : "");

			sb.Append("　");
			sb.AppendLine(string.Join(", ", ship.AllSlotInstance.Zip(
				ship.ExpansionSlot > 0 ? ship.Aircraft.Concat(new[] { 0 }) : ship.Aircraft,
				(eq, aircraft) => eq == null ? null : ((eq.MasterEquipment.IsAircraft ? $"[{aircraft}] " : "") + eq.NameWithLevel)
			).Where(str => str != null)));
		}
	}

	private static void OutputFriendBase(StringBuilder sb, List<int> initialHPs, List<int> maxHPs)
	{

		for (int i = 0; i < initialHPs.Count; i++)
		{
			if (maxHPs[i] <= 0)
				continue;

			sb.AppendFormat(BattleRes.OutputFriendBase + "\r\n\r\n",
				i + 1,
				i + 1,
				initialHPs[i], maxHPs[i]);
		}

	}

	private static void OutputSupportData(StringBuilder sb, IFleetData fleet)
	{
		for (int i = 0; i < fleet.MembersInstance.Count; i++)
		{
			IShipData? ship = fleet.MembersInstance[i];

			if (ship == null)
				continue;

			sb.AppendFormat($"#{{0}}: {{1}} {{2}} - " +
							$"{{3}} {GeneralRes.Firepower}, " +
							$"{{4}} {GeneralRes.Torpedo}, " +
							$"{{5}} {GeneralRes.AntiAir}, " +
							$"{{6}} {GeneralRes.Armor}" +
							$"\r\n",
				i + 1,
				ship.MasterShip.ShipTypeName, ship.NameWithLevel,
				ship.FirepowerBase, ship.TorpedoBase, ship.AABase, ship.ArmorBase);

			sb.Append("　");
			sb.AppendLine(string.Join(", ", ship.AllSlotInstance.Where(eq => eq != null)));
		}

	}

	private static void OutputFriendlySupportData(StringBuilder sb, PhaseFriendlySupportInfo p)
	{

		for (int i = 0; i < p.FleetsBeforePhase.FriendFleet.MembersInstance.Count; i++)
		{
			IShipData? ship = p.FleetsBeforePhase.FriendFleet.MembersInstance[i];

			if (ship == null)
				continue;

			sb.AppendFormat($"#{{0}}: {{1}} {{2}} " +
							$"Lv. {{3}} " +
							$"HP: {{4}} / {{5}} - " +
							$"{GeneralRes.Firepower} {{6}}, " +
							$"{GeneralRes.Torpedo} {{7}}, " +
							$"{GeneralRes.AntiAir} {{8}}, " +
							$"{GeneralRes.Armor} {{9}}" +
							$"\r\n",
				i + 1,
				ship.MasterShip.ShipTypeName, ship.MasterShip.NameWithClass, ship.Level,
				ship.HPCurrent, ship.HPMax,
				ship.FirepowerBase, ship.TorpedoBase, ship.AABase, ship.ArmorBase);

			sb.Append("　");
			sb.AppendLine(string.Join(", ", ship.AllSlotInstance
				.OfType<IEquipmentData>()
				.Select(eq => eq.MasterEquipment.NameEN)));
		}
	}

	private static void OutputEnemyData(StringBuilder sb, List<IShipData?> members, int[] levels, List<int> initialHPs, int[] maxHPs, IEquipmentData?[][] slots, List<List<int>> parameters)
	{

		for (int i = 0; i < members.Count; i++)
		{
			if (members[i] == null)
				continue;

			sb.AppendFormat("#{0}: ID:{1} {2} {3} Lv. {4} HP: {5} / {6}",
				i + 1,
				members[i].ShipID,
				members[i].MasterShip.ShipTypeName, members[i].MasterShip.NameWithClass,
				levels[i],
				initialHPs[i], maxHPs[i]);

			if (parameters != null)
			{
				sb.AppendFormat($" - " +
								$"{GeneralRes.Firepower}:{{0}}, " +
								$"{GeneralRes.Torpedo}:{{1}}, " +
								$"{GeneralRes.AntiAir}:{{2}}, " +
								$"{GeneralRes.Armor}:{{3}}",
					parameters[i][0], parameters[i][1], parameters[i][2], parameters[i][3]);
			}

			sb.AppendLine().Append("　");
			sb.AppendLine(string.Join(", ", slots[i].Where(eq => eq != null)));
		}
	}


	private static void OutputResultData(StringBuilder sb, int index, string name, int initialHP, int resultHP, int maxHP)
	{
		sb.AppendFormat("#{0}: {1} HP: ({2} → {3})/{4} ({5})\r\n",
			index + 1, name,
			Math.Max(initialHP, 0),
			Math.Max(resultHP, 0),
			Math.Max(maxHP, 0),
			resultHP - initialHP);
	}


	private static string GetBattleResult(BattleManager bm)
	{
		BattleResult result = bm.Result;

		StringBuilder sb = new();


		sb.AppendLine(ConstantsRes.BattleDetail_Result);
		sb.AppendFormat(ConstantsRes.BattleDetail_ResultRank + "\r\n", result.Rank);

		if (bm.IsCombinedBattle)
		{
			sb.AppendFormat(ConstantsRes.BattleDetail_ResultMVPMain + "\r\n",
				result.MvpIndex == -1 ? "(なし)" : bm.FirstBattle.FleetsBeforeBattle.Fleet.MembersInstance[result.MvpIndex].NameWithLevel);
			sb.AppendFormat(ConstantsRes.BattleDetail_ResultMVPEscort + "\r\n",
				result.MvpIndexCombined == -1 ? "(なし)" : bm.FirstBattle.FleetsBeforeBattle.EscortFleet.MembersInstance[result.MvpIndexCombined.Value].NameWithLevel);
		}
		else
		{
			sb.AppendFormat("MVP: {0}\r\n",
				result.MvpIndex == -1 ? "(なし)" : bm.FirstBattle.FleetsBeforeBattle.Fleet.MembersInstance[result.MvpIndex].NameWithLevel);
		}

		sb.AppendFormat(ConstantsRes.BattleDetail_AdmiralExp + "\r\n", result.AdmiralExp);
		sb.AppendFormat(ConstantsRes.BattleDetail_ShipExp + "\r\n", result.BaseExp);

		if (!bm.IsPractice)
		{
			sb.AppendLine().AppendLine(ConstantsRes.BattleDetail_Drop);


			int length = sb.Length;

			IShipDataMaster? ship = KCDatabase.Instance.MasterShips[(int?)result.DroppedShipId ?? -1];
			if (ship != null)
			{
				sb.AppendFormat("　{0} {1}\r\n", ship.ShipTypeName, ship.NameWithClass);
			}

			IEquipmentDataMaster? eq = KCDatabase.Instance.MasterEquipments[result.DroppedEquipmentId ?? -1];
			if (eq != null)
			{
				sb.AppendFormat("　{0} {1}\r\n", eq.CategoryTypeInstance.NameEN, eq.NameEN);
			}

			IUseItemMaster? item = KCDatabase.Instance.MasterUseItems[result.DroppedItemId ?? -1];
			if (item != null)
			{
				sb.Append("　").AppendLine(item.NameTranslated);
			}

			if (length == sb.Length)
			{
				sb.AppendLine("　(なし)");
			}
		}


		return sb.ToString();
	}

}

