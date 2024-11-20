namespace ElectronicObserver.Window.Wpf.Bonodere;

public record RankingEntryModel
{
	public required int Position { get; set; }
	public required string AdmiralName { get; set; }
	public required int Points { get; set; }
	public required int MedalCount { get; set; }
	public required string Comment { get; set; }
}
