namespace ElectronicObserver.Avalonia.ShipGroup;

public class Fraction(int current, int max, double? rate = null) : IComparable
{
	public int Current { get; } = current;
	public int Max { get; } = max;
	private double? ApiRate { get; } = rate;

	public double Rate => ApiRate ?? (double)Current / Math.Max(Max, 1);

	public int CompareTo(object? obj)
	{
		if (obj is not Fraction other) return 1;

		double diff = Rate - other.Rate;

		return diff switch
		{
			> 0.0 => 1,
			< 0.0 => -1,
			_ => Current - other.Current,
		};
	}

	public override string ToString() => $"{Current}/{Max}";
}
