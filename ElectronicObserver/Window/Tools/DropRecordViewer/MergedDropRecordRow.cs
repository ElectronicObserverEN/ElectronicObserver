using System;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.DropRecordViewer;

public class MergedDropRecordRow : DropRecordRowBase
{
	public required int Count { get; init; }
	public required string Name { get; init; }
	public string? CountDisplay => GetRankDisplay(Count, RateOrMaxCountTotal);

	// these 3 need to be public for sorting to work
	public required int? CountS { get; init; }
	public required int? CountA { get; init; }
	public required int? CountB { get; init; }

	public string? RankDisplayS => GetRankDisplay(CountS, RateOrMaxCountS);
	public string? RankDisplayA => GetRankDisplay(CountA, RateOrMaxCountA);
	public string? RankDisplayB => GetRankDisplay(CountB, RateOrMaxCountB);

	private static string? GetRankDisplay(int? count, object? rateOrMaxCount) => (count, rateOrMaxCount) switch
	{
		(int c, double rate) => $"{c} ({rate:p1})",
		(int c, int max) => $"{c}/{max} ({(double)c / Math.Max(max, 1):p1})",
		_ => null,
	};

	public object? RateOrMaxCountTotal { get; set; }
	public object? RateOrMaxCountS { get; set; }
	public object? RateOrMaxCountA { get; set; }
	public object? RateOrMaxCountB { get; set; }
	public ShipId ShipId { get; set; }
}
