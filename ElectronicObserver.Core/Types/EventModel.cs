namespace ElectronicObserver.Core.Types;

public record EventModel
{
	public int AreaId { get; init; }
	public string Name { get; init; } = "";
}
