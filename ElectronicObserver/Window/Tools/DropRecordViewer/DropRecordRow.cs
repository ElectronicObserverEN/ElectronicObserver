using System;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.DropRecordViewer;

public class DropRecordRow(
	int indexOrCount,
	string getContentString,
	DateTime? rDate,
	string getMapString,
	int? getWinRank,
	int? countS,
	int? countA,
	int? countB)
{
	public int Index { get; } = indexOrCount;
	public string Name { get; } = getContentString;
	public int Count { get; } = indexOrCount;
	public string? CountDisplay => GetRankDisplay(Count, RateOrMaxCountTotal);
	public DateTime? Date { get; } = rDate;

	public string? DateDisplay => Date switch
	{
		{ } date => DateTimeHelper.TimeToCSVString(date),
		_ => null
	};
	public string MapDescription { get; } = getMapString;

	public BattleRank? Rank { get; } = (BattleRank?)getWinRank;

	// these 3 need to be public for sorting to work
	public int? CountS { get; } = countS;
	public int? CountA { get; } = countA;
	public int? CountB { get; } = countB;

	public string? RankDisplayS => GetRankDisplay(CountS, RateOrMaxCountS);
	public string? RankDisplayA => GetRankDisplay(CountA, RateOrMaxCountA);
	public string? RankDisplayB => GetRankDisplay(CountB, RateOrMaxCountB);

	private static string? GetRankDisplay(int? count, object? rateOrMaxCount) => (count, rateOrMaxCount) switch
	{
		(int c, double rate) => $"{c} ({rate:p1})",
		(int c, int max) => $"{c}/{max} ({(double)c / Math.Max(max, 1):p1})",
		_ => null,
	};

	// when rows aren't merged it's a double value representing rate
	// when rows are merged it represents the total count
	public object? RateOrMaxCountTotal { get; set; }
	// sort parameter?
	public object? CellsTag1 { get; set; }
	public int CellsTag3 { get; set; }
	public object? RateOrMaxCountS { get; set; }
	public object? RateOrMaxCountA { get; set; }
	public object? RateOrMaxCountB { get; set; }
	public ShipId ShipId { get; set; }
}
