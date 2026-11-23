namespace ElectronicObserver.Avalonia.UiBlocker;

public sealed class SystemUiBlockerConfiguration
{
	public required Guid Id { get; init; }
	public required bool IsEnabled { get; set; }
}
