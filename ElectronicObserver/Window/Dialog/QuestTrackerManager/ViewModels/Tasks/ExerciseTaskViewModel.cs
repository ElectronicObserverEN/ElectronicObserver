using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Data;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Models.Tasks;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager.ViewModels.Tasks;

public class ExerciseTaskViewModel : ObservableObject, IQuestTaskViewModel
{
	public ExerciseTaskModel Model { get; }

	public IEnumerable<BattleRank> Ranks { get; }

	public string Display => $"{Model.Progress}/{Model.Count} {GetClearCondition()}";
	public string Progress => (Model.Progress >= Model.Count) switch
	{
		true => "",
		_ => Display
	};
	public string ClearCondition => GetClearCondition();

	private string GetClearCondition()
	{
		StringBuilder sb = new();

		sb.Append(QuestTracking.Exercise);

		sb.Append(Model.Rank switch
		{
			BattleRank.D or BattleRank.C => Constants.GetWinRank((int)Model.Rank) + QuestTracking.ClearConditionOrHigher,
			>= BattleRank.B => Constants.GetWinRank((int)Model.Rank) + QuestTracking.ClearConditionRankVictories,
			_ => "×"
		});
		sb.Append(Model.Count);

		return sb.ToString();
	}

	public ExerciseTaskViewModel(ExerciseTaskModel model)
	{
		Model = model;

		Ranks = Ranks = Enum.GetValues<BattleRank>();

		Model.PropertyChanged += (sender, args) =>
		{
			OnPropertyChanged(nameof(Display));
		};

		PropertyChanged += (_, e) =>
		{
			if (e.PropertyName is not nameof(Display)) return;

			KCDatabase.Instance.Quest.OnQuestUpdated();
		};
	}

	public void Increment(BattleRank rank)
	{ 
		if (rank >= Model.Rank)
		{
			Model.Progress++;
		}
	}
}
