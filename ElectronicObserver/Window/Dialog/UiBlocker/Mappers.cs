using Riok.Mapperly.Abstractions;

namespace ElectronicObserver.Window.Dialog.UiBlocker;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Source)]
public static partial class Mappers
{
	public static partial void ApplyToViewModel(this UiBlockerConfiguration config, UiBlockerViewModel viewModel);

	[MapperRequiredMapping(RequiredMappingStrategy.Target)]
	public static partial void SaveToConfiguration(this UiBlockerViewModel viewModel, UiBlockerConfiguration config);
}
