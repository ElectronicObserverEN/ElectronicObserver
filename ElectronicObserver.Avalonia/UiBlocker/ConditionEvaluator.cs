namespace ElectronicObserver.Avalonia.UiBlocker;

public sealed class ConditionEvaluator
{
	public static bool Evaluate(ConditionModel model, IGameStateProvider stateProvider)
	{
		bool result = Check(model, stateProvider);

		return model.Invert switch
		{
			true => !result,
			_ => result,
		};
	}

	private static bool Check(ConditionModel model, IGameStateProvider s) => model.Type switch
	{
		ConditionType.CanShipSink => s.CanShipSink,
		ConditionType.DoubleLineTouch => s.CanDoDoubleLineTouch,
		ConditionType.EchelonTouch => s.CanDoEchelonTouch,
		ConditionType.SecondPatrolFormationTouch => s.CanDoSecondPatrolFormationTouch,
		ConditionType.FourthPatrolFormationTouch => s.CanDoFourthPatrolFormationTouch,
		ConditionType.IsBattleNode => s.IsBattleNode,
		ConditionType.IsBoss => s.IsBoss,
		ConditionType.IsEvent => s.IsEvent,
		ConditionType.IsCombinedFleet => s.IsCombinedFleet,
		ConditionType.BattleRankAtMost => s.CurrentRank <= model.Rank,
		ConditionType.BattleRankAtLeast => s.CurrentRank >= model.Rank,
		ConditionType.Location => CheckLocation(model, s),

		_ => false,
	};

	private static bool CheckLocation(ConditionModel model, IGameStateProvider s)
	{
		if (s.Map is null) return false;
		if (model.Map is null) return true;
		if (s.Map.MapAreaID != model.Map.AreaId) return false;
		if (s.Map.MapInfoID != model.Map.InfoId) return false;
		
		if (s.CellId is not int cellId) return false;
		if (model.CellIds.Count is 0) return true;

		return model.CellIds.Contains(cellId);
	}
}
