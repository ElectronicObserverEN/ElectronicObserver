using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data.Battle.Detail;

namespace ElectronicObserver.Data.Battle.Phase;

/// <summary>
/// 雷撃戦フェーズの処理を行います。
/// </summary>
public class PhaseTorpedo : PhaseBase
{

	/// <summary>
	/// フェーズID 0=開幕雷撃, 1-4=雷撃戦
	/// </summary>
	private readonly int phaseID;

	public PhaseTorpedo(BattleData data, string title, int phaseID)
		: base(data, title)
	{

		this.phaseID = phaseID;

		if (!IsAvailable)
			return;

		Damages = GetConcatArray("api_fdam", "api_edam", 0.0);

		if (IsOpeningTorpedoPhase)
		{
			AttackDamages = GetConcatArray<int[]?>("api_fydam_list_items", "api_eydam_list_items", null);
			Targets = GetConcatArray<int[]?>("api_frai_list_items", "api_erai_list_items", null);
			CriticalFlags = GetConcatArray<int[]?>("api_fcl_list_items", "api_ecl_list_items", null);
		}
		else if (IsClosingTorpedoPhase)
		{
			AttackDamages = GetConcatArray("api_fydam", "api_eydam", 0).Select(i => new[] { i }).ToArray();
			Targets = GetConcatArray("api_frai", "api_erai", -1).Select(i => new[] { i }).ToArray();
			CriticalFlags = GetConcatArray("api_fcl", "api_ecl", 0).Select(i => new[] { i }).ToArray();
		}

	}


	public override bool IsAvailable => IsOpeningTorpedoPhase || IsClosingTorpedoPhase;

	private bool IsOpeningTorpedoPhase => phaseID == 0 && (RawData.api_opening_flag() ? (int)RawData.api_opening_flag != 0 : false);
	private bool IsClosingTorpedoPhase => phaseID != 0 && ((int)RawData.api_hourai_flag[phaseID - 1] != 0);


	public override void EmulateBattle(int[] hps, int[] damages)
	{

		if (!IsAvailable)
			return;

		// 表示上は逐次ダメージ反映のほうが都合がいいが、AddDamage を逐次的にやるとダメコン判定を誤るため
		int[] currentHP = new int[hps.Length];
		Array.Copy(hps, currentHP, currentHP.Length);

		for (int i = 0; i < Targets.Length; i++)
		{
			if (Targets[i] is null) continue;

			for (int j = 0; j < Targets[i].Length; j++)
			{
				if (Targets[i][j] < 0) continue;

				BattleIndex attacker = new BattleIndex(i, Battle.IsFriendCombined, Battle.IsEnemyCombined);
				BattleIndex defender = new BattleIndex(Targets[i][j] + (i < 12 ? 12 : 0), Battle.IsFriendCombined, Battle.IsEnemyCombined);

				BattleDetails.Add(new BattleDayDetail(Battle, attacker, defender, new double[] { AttackDamages[i][j] + Damages[defender] - Math.Floor(Damages[defender]) },    //propagates "guards flagship" flag
					new int[] { CriticalFlags[i][j] }, -1, null, currentHP[defender]));
				currentHP[defender] -= Math.Max(AttackDamages[i][j], 0);
			}
		}

		for (int i = 0; i < hps.Length; i++)
		{
			AddDamage(hps, i, (int)Damages[i]);
			damages[i] += AttackDamages[i]?.Sum() ?? 0;
		}

	}


	public dynamic TorpedoData => phaseID == 0 ? RawData.api_opening_atack : RawData.api_raigeki;


	/// <summary>
	/// 各艦の被ダメージ
	/// </summary>
	public double[] Damages { get; private set; }

	/// <summary>
	/// 各艦の与ダメージ
	/// </summary>
	public int[]?[] AttackDamages { get; private set; }

	/// <summary>
	/// 各艦のターゲットインデックス
	/// </summary>
	public int[]?[] Targets { get; private set; }

	/// <summary>
	/// クリティカルフラグ(攻撃側)
	/// </summary>
	public int[]?[] CriticalFlags { get; private set; }




	private T[] GetConcatArray<T>(string friendName, string enemyName, T defaultValue)
	{
		var friend = ConvertToArray<T>(TorpedoData[friendName], 12, defaultValue);
		var enemy = ConvertToArray<T>(TorpedoData[enemyName], 12, defaultValue);

		var ret = new T[24];

		for (int i = 0; i < 12; i++)
		{
			ret[i] = friend[i];
			ret[i + 12] = enemy[i];
		}

		return ret;
	}

	/// <summary>
	/// 基本的には `(T[])json` と等価ですが、特定の状況下におけるデータエラーを回避するための実装が含まれています
	/// https://github.com/andanteyk/ElectronicObserver/issues/294
	/// </summary>
	private static T[] ConvertToArray<T>(dynamic json, int maxLength, T defaultValue)
	{
		var ret = Enumerable.Repeat(defaultValue, maxLength).ToArray();
		int i = 0;
		foreach (var member in json)
		{
			ret[i++ % maxLength] =
				member is KeyValuePair<string, dynamic> pair ? (T)pair.Value :
				member != null ? (T)member :
				default(T);
		}
		return ret;
	}
}
