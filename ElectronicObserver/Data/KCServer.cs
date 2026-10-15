namespace ElectronicObserver.Data;

public record KCServer
{
	public required int Num { get; init; }
	public required string Name { get; init; }
	public required string Jp { get; init; }
	public required string Host { get; init; }
}
