namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;

public record SortieCost
{
	public int Fuel { get; init; }
	public int Ammo { get; init; }
	public int Steel { get; init; }
	public int Bauxite { get; init; }

	public static SortieCost operator +(SortieCost a, SortieCost b) => new()
	{
		Fuel = a.Fuel + b.Fuel,
		Ammo = a.Ammo + b.Ammo,
		Steel = a.Steel + b.Steel,
		Bauxite = a.Bauxite + b.Bauxite,
	};
}
