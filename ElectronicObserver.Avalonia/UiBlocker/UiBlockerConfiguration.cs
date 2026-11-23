namespace ElectronicObserver.Avalonia.UiBlocker;

public sealed class UiBlockerConfiguration
{
	public List<SystemUiBlockerConfiguration> SystemBlockers { get; init; } = [];
	public List<UiBlockerModel> UserBlockers { get; init; } = [];

	public static UiBlockerConfiguration Default => new()
	{
		SystemBlockers =
		[
			.. SystemUiBlockers.All.Select(b => new SystemUiBlockerConfiguration
			{
				Id = b.Id, 
				IsEnabled = b.Id == SystemUiBlockers.TaihaBlocker.Id,
			}),
		],
	};
}
