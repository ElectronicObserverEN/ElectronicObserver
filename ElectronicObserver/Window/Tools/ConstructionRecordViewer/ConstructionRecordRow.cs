using System;
using ElectronicObserver.Core;
using ElectronicObserver.Utility.Mathematics;

namespace ElectronicObserver.Window.Tools.ConstructionRecordViewer;

public class ConstructionRecordRow
{
	public int Index { get; }
	public int Count { get; }
	public string? CountDisplay => GetRankDisplay(Count, CellTag0);
	public string? ShipName { get; }
	private DateTime? Date { get; }
	public string? DateDisplay => Date switch
	{
		{ } date => DateTimeHelper.TimeToCSVString(date),
		_ => null
	};
	public string? Recipe { get; }
	public string? FlagshipName { get; }
	public int? DevMats100Count { get; }
	public string? DevMats100CountDisplay => GetRankDisplay(DevMats100Count, CellTag5);
	public int? DevMats20Count { get; }
	public string? DevMats20CountDisplay => GetRankDisplay(DevMats20Count, CellTag6);
	public int? DevMats1Count { get; }
	public string? DevMats1CountDisplay => GetRankDisplay(DevMats1Count, CellTag7);
	public string CellTag1 { get; set; }
	public string CellTag3 { get; set; }
	public string CellTag4 { get; set; }
	// tags 0, 5, 6, 7 are either int or double
	public object CellTag0 { get; set; }
	public object CellTag5 { get; set; }
	public object CellTag6 { get; set; }
	public object CellTag7 { get; set; }

	private string? GetRankDisplay(int? count, object rateOrMaxCount) => (count, rateOrMaxCount) switch
	{
		(int c, double rate) => string.Format("{0} ({1:p1})", c, rate),
		(int c, int max) => string.Format("{0}/{1} ({2:p1})", c, max, (double)(c) / Math.Max(max, 1)),
		_ => null
	};

	public ConstructionRecordRow(int indexOrCount, string? shipName, DateTime? date, string? recipe, string? flagshipName, int? devMats100Count, int? devMats20Count, int? devMats1Count)
	{
		Index = indexOrCount;
		Count = indexOrCount;
		ShipName = shipName;
		Date = date;
		Recipe = recipe;
		FlagshipName = flagshipName;
		DevMats100Count = devMats100Count;
		DevMats20Count = devMats20Count;
		DevMats1Count = devMats1Count;
	}
}
