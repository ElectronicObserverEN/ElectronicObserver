﻿using System;
using System.Collections.Generic;
using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.Utility.Mathematics;

namespace ElectronicObserver.Data;

/// <summary>
/// 任務情報を統括して扱います。
/// </summary>
public class QuestManager : APIWrapper
{

	/// <summary>
	/// 任務リスト
	/// </summary>
	public IDDictionary<QuestData> Quests { get; private set; }

	/// <summary>
	/// 任務数(未ロード含む)
	/// </summary>
	public int Count { get; internal set; }

	/// <summary>
	/// ロードしたかどうか(※全て読み込んでいるとは限らない)
	/// </summary>
	public bool IsLoaded { get; private set; }


	/// <summary>
	/// ロードが完了したかどうか
	/// </summary>
	public bool IsLoadCompleted => IsLoaded && Quests.Count == Count;


	public event Action QuestUpdated = delegate { };



	public QuestManager()
	{
		Quests = new IDDictionary<QuestData>();
		IsLoaded = false;
	}


	public QuestData this[int key] => Quests[key];


	public override void LoadFromResponse(string apiname, dynamic data)
	{
		base.LoadFromResponse(apiname, (object)data);

		var progress = KCDatabase.Instance.QuestProgress;


		//周期任務削除
		if (DateTimeHelper.IsCrossedDay(progress.LastUpdateTime, 5, 0, 0))
		{
			progress.Progresses.RemoveAll(p => (p.QuestType == 1 || p.QuestID == 211 /* 空母3 */ || p.QuestID == 212 /* 輸送5 */ || p.QuestID == 311 /* 演習勝利7 */ || p.QuestID == 330 || p.QuestID == 337 || p.QuestID == 339 || p.QuestID == 341 || p.QuestID == 342 || p.QuestID is 348 /*C53*/ or 349 /*2102 LQ3*/));
			Quests.RemoveAll(q => q.Type == 1 || q.QuestID == 211 /* 空母3 */ || q.QuestID == 212 /* 輸送5 */ || q.QuestID == 311 /* 演習勝利7 */  );
		}
		if (DateTimeHelper.IsCrossedWeek(progress.LastUpdateTime, DayOfWeek.Monday, 5, 0, 0))
		{
			progress.Progresses.RemoveAll(p => p.QuestType == 2);
			Quests.RemoveAll(q => q.Type == 2);
		}
		if (DateTimeHelper.IsCrossedMonth(progress.LastUpdateTime, 1, 5, 0, 0))
		{
			progress.Progresses.RemoveAll(p => p.QuestType == 3);
			Quests.RemoveAll(q => q.Type == 3);
		}
		if (DateTimeHelper.IsCrossedQuarter(progress.LastUpdateTime, 0, 1, 5, 0, 0))
		{
			progress.Progresses.RemoveAll(p => p.QuestType == 5);
			Quests.RemoveAll(p => p.Type == 5);
		}
		for (int i = 1; i <= 12; i++)
		{
			if (DateTimeHelper.IsCrossedYear(progress.LastUpdateTime, i, 1, 5, 0, 0))
			{
				progress.Progresses.RemoveAll(p => p.QuestType == 100 + i);
				Quests.RemoveAll(p => p.LabelType == 100 + i);
			}
		}

		Count = (int)RawData.api_count;

		if (RawData.api_list != null)
		{   //任務完遂時orページ遷移時 null になる

			foreach (dynamic elem in RawData.api_list)
			{

				if (!(elem is double))
				{       //空欄は -1 になるため。

					int id = (int)elem.api_no;
					if (!Quests.ContainsKey(id))
					{
						var q = new QuestData();
						q.LoadFromResponse(apiname, elem);
						Quests.Add(q);

					}
					else
					{
						Quests[id].LoadFromResponse(apiname, elem);
					}

				}
			}

		}


		IsLoaded = true;

	}


	public override void LoadFromRequest(string apiname, Dictionary<string, string> data)
	{
		base.LoadFromRequest(apiname, data);

		switch (apiname)
		{
			case "api_req_quest/clearitemget":
			{
				int id = int.Parse(data["api_quest_id"]);
				var quest = Quests[id];

				Utility.Logger.Add(2, string.Format(LoggerRes.ClearedQuest, quest.NameWithCode));

				Quests.Remove(id);
				Count--;
			}
			break;
			case "api_req_quest/stop":
				Quests[int.Parse(data["api_quest_id"])].State = 1;
				break;
		}

		QuestUpdated();
	}


	public void Clear()
	{
		Quests.Clear();
		IsLoaded = false;
	}


	// QuestProgressManager から呼ばれます
	internal void OnQuestUpdated()
	{
		QuestUpdated();
	}
}
