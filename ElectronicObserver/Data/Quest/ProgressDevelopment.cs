using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest;

/// <summary>
/// 装備開発任務の進捗を管理します。
/// </summary>
[DataContract(Name = "ProgressDevelopment")]
public class ProgressDevelopment : ProgressData
{

	public ProgressDevelopment(IQuestData quest, int maxCount)
		: base(quest, maxCount)
	{
	}

	public override string GetClearCondition()
	{
		return QuestTracking.Development + ProgressMax;
	}
}
