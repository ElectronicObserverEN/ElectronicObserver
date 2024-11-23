namespace ElectronicObserver.Window.Wpf.SenkaLeaderboard;

public record SenkaEntryModel
{
	public required int Position { get; set; }
	public required string AdmiralName { get; set; }
	public required int Points { get; set; }
	public required int MedalCount { get; set; }
	public required string Comment { get; set; }
}
