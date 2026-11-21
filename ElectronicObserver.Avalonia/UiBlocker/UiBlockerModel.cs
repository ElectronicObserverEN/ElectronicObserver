using System.Text.Json.Serialization;
using ElectronicObserver.Avalonia.UiBlocker.Resources;

namespace ElectronicObserver.Avalonia.UiBlocker;

public sealed class UiBlockerModel
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public string Name { get; set; } = UiBlockerResources.Name;
	public string? Description { get; set; }
	public bool IsEnabled { get; set; } = true;
	public List<UiElementModel> UiElements { get; set; } = [];

	[JsonIgnore]
	public bool IsSystem { get; init; }

	public UiBlockerModel Clone() => new()
	{
		Id = Id,
		Name = Name,
		Description = Description,
		IsEnabled = IsEnabled,
		UiElements = [.. UiElements.Select(e => e.Clone())],
	};

	public UiBlockerModel Duplicate()
	{
		UiBlockerModel copy = Clone();
		copy.Id = Guid.NewGuid();

		return copy;
	}
}
