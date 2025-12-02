using System;
using ElectronicObserver.Core;
using ElectronicObserver.Utility.Mathematics;

namespace ElectronicObserver.Window.Tools.DevelopmentRecordViewer;

public class DevelopmentRecordRow
{
	public int Index { get; }
	public string EquipmentName { get; }
	public int Count { get; }
	public string? CountDisplay => GetRankDisplay(Count, RateOrMaxCountTotal);
	private DateTime? Date { get; }
	public string? DateDisplay => Date switch
	{
		{ } date => DateTimeHelper.TimeToCSVString(date),
		_ => null
	};
	public string? GetRecipeString { get; }
	public string? ShipTypeNameEn { get; }
	public string? FlagshipName { get; }
	public string? Summary { get; }

	// when rows aren't merged it's a double value representing rate
	// when rows are merged it represents the total count
	public object RateOrMaxCountTotal { get; set; }
	// int or string
	// used for sorting?
	public object CellTag1 { get; set; }
	public string CellTag3 { get; set; }
	public int CellTag4 { get; set; }
	public string CellTag5 { get; set; }

	private string? GetRankDisplay(int? count, object rateOrMaxCount) => (count, rateOrMaxCount) switch
	{
		(int c, double rate) => string.Format("{0} ({1:p1})", c, rate),
		(int c, int max) => string.Format("{0}/{1} ({2:p1})", c, max, (double)(c) / Math.Max(max, 1)),
		_ => null
	};

	public DevelopmentRecordRow(int indexOrCount, string equipmentName, DateTime? date, string? getRecipeString, string? shipTypeNameEn, string? flagshipName, string? summary)
	{
		Index = indexOrCount;
		EquipmentName = equipmentName;
		Count = indexOrCount;
		Date = date;
		GetRecipeString = getRecipeString;
		ShipTypeNameEn = shipTypeNameEn;
		FlagshipName = flagshipName;
		Summary = summary;
	}
}