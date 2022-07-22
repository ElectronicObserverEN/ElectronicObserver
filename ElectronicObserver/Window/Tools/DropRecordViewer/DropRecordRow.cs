using System;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;

namespace ElectronicObserver.Window.Tools.DropRecordViewer;

public class DropRecordRow
{
	public int Index { get; }
	public string Name { get; }
	public int Count { get; }
	public string? CountDisplay => GetRankDisplay(Count, RateOrMaxCountTotal);
	public DateTime? Date { get; }
	public string? DateDisplay => Date switch
	{
		{ } date => DateTimeHelper.TimeToCSVString(date),
		_ => null
	};
	public string MapDescription { get; }
	public BattleRank? Rank { get; }
	// these 3 need to be public for sorting to work
	public int? CountS { get; }
	public int? CountA { get; }
	public int? CountB { get; }

	public string? RankDisplayS => GetRankDisplay(CountS, RateOrMaxCountS);
	public string? RankDisplayA => GetRankDisplay(CountA, RateOrMaxCountA);
	public string? RankDisplayB => GetRankDisplay(CountB, RateOrMaxCountB);

	private string? GetRankDisplay(int? count, object rateOrMaxCount) => (count, rateOrMaxCount) switch
	{
		(int c, double rate) => string.Format("{0} ({1:p1})", c, rate),
		(int c, int max) => string.Format("{0}/{1} ({2:p1})", c, max, (double)(c) / Math.Max(max, 1)),
		_ => null
	};

	public DropRecordRow(int indexOrCount, string getContentString, DateTime? rDate, string getMapString, int? getWinRank, int? countS, int? countA, int? countB)
	{
		Index = indexOrCount;
		Name = getContentString;
		Count = indexOrCount;
		Date = rDate;
		MapDescription = getMapString;
		Rank = (BattleRank?)getWinRank;

		CountS = countS;
		CountA = countA;
		CountB = countB;
	}

	// when rows aren't merged it's a double value representing rate
	// when rows are merged it represents the total count
	public object RateOrMaxCountTotal { get; set; }
	// sort parameter?
	public object CellsTag1 { get; set; }
	public int CellsTag3 { get; set; }
	public object RateOrMaxCountS { get; set; }
	public object RateOrMaxCountA { get; set; }
	public object RateOrMaxCountB { get; set; }
}
