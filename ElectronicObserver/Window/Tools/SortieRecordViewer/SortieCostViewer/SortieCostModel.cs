namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;

public record SortieCostModel
{
	public int Fuel { get; init; }
	public int Ammo { get; init; }
	public int Steel { get; init; }
	public int Bauxite { get; init; }

	public static SortieCostModel operator +(SortieCostModel a, SortieCostModel b) => new()
	{
		Fuel = a.Fuel + b.Fuel,
		Ammo = a.Ammo + b.Ammo,
		Steel = a.Steel + b.Steel,
		Bauxite = a.Bauxite + b.Bauxite,
	};

	public static SortieCostModel operator -(SortieCostModel a, SortieCostModel b) => new()
	{
		Fuel = a.Fuel - b.Fuel,
		Ammo = a.Ammo - b.Ammo,
		Steel = a.Steel - b.Steel,
		Bauxite = a.Bauxite - b.Bauxite,
	};
}
