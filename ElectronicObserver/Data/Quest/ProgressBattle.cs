using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace ElectronicObserver.Data.Quest;

/// <summary>
/// 戦闘系の任務の進捗を管理します。
/// </summary>
[DataContract(Name = "ProgressBattle")]
public class ProgressBattle : ProgressData
{

	/// <summary>
	/// 条件を満たす最低ランク
	/// </summary>
	[DataMember]
	private int LowestRank { get; set; }

	/// <summary>
	/// 対象となる海域
	/// </summary>
	[DataMember]
	private HashSet<int> TargetArea { get; set; }

	/// <summary>
	/// ボス限定かどうか
	/// </summary>
	[DataMember]
	private bool IsBossOnly { get; set; }


	public ProgressBattle(QuestData quest, int maxCount, string lowestRank, int[] targetArea, bool isBossOnly)
		: base(quest, maxCount)
	{

		LowestRank = (int)Constants.GetWinRank(lowestRank);
		TargetArea = targetArea == null ? null : new HashSet<int>(targetArea);
		IsBossOnly = isBossOnly;
	}



	public virtual void Increment(string rank, int areaID, bool isBoss)
	{

		if (TargetArea != null && !TargetArea.Contains(areaID))
			return;

		if ((int)Constants.GetWinRank(rank) < LowestRank)
			return;

		if (IsBossOnly && !isBoss)
			return;


		Increment();
	}



	public override string GetClearCondition()
	{
		StringBuilder sb = new StringBuilder();
		if (TargetArea != null)
		{
			sb.Append(string.Join("・", TargetArea.OrderBy(s => s).Select(s => string.Format("{0}-{1}", s / 10, s % 10))));
		}
		if (IsBossOnly)
		{
			sb.Append(QuestTracking.ClearConditionBoss);
		}

		switch (LowestRank)
		{
			case 0:
				sb.Append(QuestTracking.ClearConditionClear);
				break;
			case 1:
			default:
				sb.Append(QuestTracking.ClearConditionBattle);
				break;
			case 2:
			case 3:
				sb.Append(Constants.GetWinRank(LowestRank) + QuestTracking.ClearConditionOrHigher);
				break;
			case 4:
				sb.Append(QuestTracking.ClearConditionVictories);
				break;
			case 5:
			case 6:
			case 7:
				sb.Append(Constants.GetWinRank(LowestRank) + QuestTracking.ClearConditionRankVictories);
				break;
		}
		sb.Append(ProgressMax);

		return sb.ToString();
	}
}
