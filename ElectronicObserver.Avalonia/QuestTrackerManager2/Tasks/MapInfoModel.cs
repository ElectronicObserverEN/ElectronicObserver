namespace ElectronicObserver.Avalonia.QuestTrackerManager2.Tasks;

public record MapInfoModel
{
	public int AreaId { get; init; } = 1;
	public int InfoId { get; init; } = 1;

	public string Display => $"{AreaId}-{InfoId}";
}
