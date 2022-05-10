using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.Quest;

[DataContract(Name = "ProgressMultiExpedition")]
public class ProgressMultiExpedition : ProgressData
{
	[DataMember]
	private ProgressExpedition[] ProgressList;

	public ProgressMultiExpedition(IQuestData quest, IEnumerable<ProgressExpedition> progressList)
		: base(quest, 1)
	{
		ProgressList = progressList.ToArray();
		foreach (var p in ProgressList)
			p.IgnoreCheckProgress = true;

		ProgressMax = ProgressList.Sum(p => p.ProgressMax);
	}


	public void Increment(int areaID)
	{
		foreach (var p in ProgressList)
			p.Increment(areaID);

		Progress = ProgressList.Sum(p => p.Progress);
	}

	public override void Increment()
	{
		throw new NotSupportedException();
	}

	public override void Decrement()
	{
		throw new NotSupportedException();
	}

	public override void CheckProgress(IQuestData q)
	{
		foreach (var p in ProgressList)
			p.ApplyTemporaryProgress(q);

		Progress = ProgressList.Sum(p => p.Progress);
	}

	public override string ToString()
	{
		if (ProgressList.All(p => p.IsCleared))
			return "100%";
		else
			return string.Join(" \r\n", ProgressList.Where(p => !p.IsCleared).Select(p => p.ToString() + " " + p.GetClearCondition()));
	}


	public override string GetClearCondition()
	{
		return string.Join("\r\n", ProgressList.Select(p => p.GetClearCondition()));
	}
}
