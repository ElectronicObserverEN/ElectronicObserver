using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ElectronicObserver.Data.Quest;

using DSPair = KeyValuePair<double, string>;

/// <summary>
/// 任務「あ号作戦」の進捗を管理します。
/// </summary>
[DataContract(Name = "ProgressAGo")]
public class ProgressAGo : ProgressData
{

	/// <summary>
	/// 達成に必要な出撃回数
	/// </summary>
	[IgnoreDataMember]
	private int sortieMax => 36;

	/// <summary>
	/// 達成に必要なS勝利回数
	/// </summary>
	[IgnoreDataMember]
	private int sWinMax => 6;

	/// <summary>
	/// 達成に必要なボス戦闘回数
	/// </summary>
	[IgnoreDataMember]
	private int bossMax => 24;

	/// <summary>
	/// 達成に必要なボス勝利回数
	/// </summary>
	[IgnoreDataMember]
	private int bossWinMax => 12;


	/// <summary>
	/// 現在の出撃回数
	/// </summary>
	[IgnoreDataMember]
	private int sortieCount
	{
		get { return Progress & 0xFF; }
		set { Progress = (Progress & ~0xFF) | (Math.Min(value, sortieMax) & 0xFF); }
	}

	/// <summary>
	/// 現在のS勝利回数
	/// </summary>
	[IgnoreDataMember]
	private int sWinCount
	{
		get { return (Progress >> 8) & 0xFF; }
		set { Progress = (Progress & ~(0xFF << 8)) | ((Math.Min(value, sWinMax) & 0xFF) << 8); }
	}

	/// <summary>
	/// 現在のボス戦闘回数
	/// </summary>
	[IgnoreDataMember]
	private int bossCount
	{
		get { return (Progress >> 16) & 0xFF; }
		set { Progress = (Progress & ~(0xFF << 16)) | ((Math.Min(value, bossMax) & 0xFF) << 16); }
	}

	/// <summary>
	/// 現在のボス勝利回数
	/// </summary>
	[IgnoreDataMember]
	private int bossWinCount
	{
		get { return (Progress >> 24) & 0xFF; }
		set { Progress = (Progress & ~(0xFF << 24)) | ((Math.Min(value, bossWinMax) & 0xFF) << 24); }
	}


	#region tempシリーズ

	/// <summary>
	/// 現在の出撃回数(temp)
	/// </summary>
	[IgnoreDataMember]
	private int sortieCountTemp
	{
		get { return TemporaryProgress & 0xFF; }
		set { TemporaryProgress = (TemporaryProgress & ~0xFF) | (Math.Min(value, sortieMax) & 0xFF); }
	}

	/// <summary>
	/// 現在のS勝利回数(temp)
	/// </summary>
	[IgnoreDataMember]
	private int sWinCountTemp
	{
		get { return (TemporaryProgress >> 8) & 0xFF; }
		set { TemporaryProgress = (TemporaryProgress & ~(0xFF << 8)) | ((Math.Min(value, sWinMax) & 0xFF) << 8); }
	}

	/// <summary>
	/// 現在のボス戦闘回数(temp)
	/// </summary>
	[IgnoreDataMember]
	private int bossCountTemp
	{
		get { return (TemporaryProgress >> 16) & 0xFF; }
		set { TemporaryProgress = (TemporaryProgress & ~(0xFF << 16)) | ((Math.Min(value, bossMax) & 0xFF) << 16); }
	}

	/// <summary>
	/// 現在のボス勝利回数(temp)
	/// </summary>
	[IgnoreDataMember]
	private int bossWinCountTemp
	{
		get { return (TemporaryProgress >> 24) & 0xFF; }
		set { TemporaryProgress = (TemporaryProgress & ~(0xFF << 24)) | ((Math.Min(value, bossWinMax) & 0xFF) << 24); }
	}

	#endregion


	public ProgressAGo(QuestData quest)
		: base(quest, 0)
	{
	}


	public override double ProgressPercentage
	{
		get
		{
			double prog = 0;
			prog += Math.Min((double)sortieCount / sortieMax, 1.0) * 0.25;
			prog += Math.Min((double)sWinCount / sWinMax, 1.0) * 0.25;
			prog += Math.Min((double)bossCount / bossMax, 1.0) * 0.25;
			prog += Math.Min((double)bossWinCount / bossWinMax, 1.0) * 0.25;
			return prog;
		}
	}



	public override void Increment()
	{
		throw new NotSupportedException();
	}

	public override void Decrement()
	{
		throw new NotSupportedException();
	}


	public override void CheckProgress(QuestData q)
	{

		if (TemporaryProgress != 0)
		{
			if (q.State == 2)
			{

				sortieCount = sortieCount + sortieCountTemp;
				sWinCount = sWinCount + sWinCountTemp;
				bossCount = bossCount + bossCountTemp;
				bossWinCount = bossWinCount + bossWinCountTemp;

			}

			TemporaryProgress = 0;
		}

	}


	/// <summary>
	/// 出撃回数を増やします。
	/// </summary>
	public void IncrementSortie()
	{

		var q = KCDatabase.Instance.Quest[QuestID];

		if (q == null)
		{
			sortieCountTemp++;
			return;
		}

		if (q.State != 2)
			return;


		CheckProgress(q);

		sortieCount++;
	}

	/// <summary>
	/// 戦闘回数を増やします。
	/// </summary>
	public void IncrementBattle(string rank, bool isBoss)
	{

		var q = KCDatabase.Instance.Quest[QuestID];

		if (q != null)
		{
			if (q.State != 2)
				return;
			else
				CheckProgress(q);
		}


		int irank = (int)Constants.GetWinRank(rank);

		if (isBoss)
		{
			if (q != null) bossCount++; else bossCountTemp++;

			if (irank >= (int)Constants.GetWinRank("B"))
				if (q != null) bossWinCount++; else bossWinCountTemp++;
		}

		if (irank >= (int)Constants.GetWinRank("S"))
			if (q != null) sWinCount++; else sWinCountTemp++;

	}


	public override string ToString()
	{
		var list = new List<DSPair>
		{
			new DSPair(Math.Min((double)sortieCount / sortieMax, 1.0), string.Format(QuestTracking.AGoSorties, sortieCount, sortieMax)),
			new DSPair(Math.Min((double)sWinCount / sWinMax, 1.0), string.Format(QuestTracking.AGoSRanks, sWinCount, sWinMax)),
			new DSPair(Math.Min((double)bossCount / bossMax, 1.0), string.Format(QuestTracking.AGoBossEncounters, bossCount, bossMax)),
			new DSPair(Math.Min((double)bossWinCount / bossWinMax, 1.0), string.Format(QuestTracking.AGoBossVictories, bossWinCount, bossWinMax))
		};

		var slist = list.Where(elem => elem.Key < 1.0).OrderBy(elem => elem.Key).Select(elem => elem.Value);
		return string.Format("{0}\n ({1:p1})", slist.Count() > 0 ? string.Join(" \n", slist) : $"{QuestTracking.Complete}!", ProgressPercentage);
	}

	public override string GetClearCondition()
	{
		return string.Format(QuestTracking.AGoClearCondition, sortieMax, sWinMax, bossMax, bossWinMax);
	}
}
