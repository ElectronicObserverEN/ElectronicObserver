namespace ElectronicObserver.Data;

public record KCServer
{
	public required int Num { get; set; }
	public required string Name { get; set; }
	public required string Jp { get; set; }
	public required string Ip { get; set; }
}
