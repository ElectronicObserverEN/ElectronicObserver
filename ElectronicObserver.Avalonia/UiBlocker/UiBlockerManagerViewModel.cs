using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Core.Services;

namespace ElectronicObserver.Avalonia.UiBlocker;

public sealed partial class UiBlockerManagerViewModel : ObservableObject
{
	private IUiBlockerService UiBlockerService { get; }
	private IMapInfoProvider Maps { get; }
	private IClipboardService Clipboard { get; }
	private UiBlockerConfiguration UiBlockerConfiguration { get; }

	public BlockerEngine Engine { get; }

	public ObservableCollection<UiBlockerViewModel> Blockers { get; } = [];

	[ObservableProperty]
	[NotifyCanExecuteChangedFor(nameof(DuplicateCommand))]
	[NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
	[NotifyCanExecuteChangedFor(nameof(CopySelectedCommand))]
	public partial UiBlockerViewModel? SelectedBlocker { get; set; }

	public UiBlockerManagerViewModel(
		IUiBlockerService uiBlockerService,
		IMapInfoProvider maps,
		IClipboardService clipboard,
		UiBlockerConfiguration uiBlockerConfiguration)
	{
		UiBlockerService = uiBlockerService;
		Maps = maps;
		Clipboard = clipboard;
		UiBlockerConfiguration = uiBlockerConfiguration;
		Engine = new(UiBlockerService, Blockers);
		Reload();
	}

	private bool HasSelection() => SelectedBlocker is not null;
	private bool CanEditSelected() => SelectedBlocker is { IsSystem: false };

	[RelayCommand]
	private void NewBlocker() => AddUserBlocker(new UiBlockerModel { UiElements = { new UiElementModel() } });

	[RelayCommand(CanExecute = nameof(HasSelection))]
	private void Duplicate() => AddUserBlocker(SelectedBlocker!.Model.Duplicate());

	[RelayCommand(CanExecute = nameof(CanEditSelected))]
	private void Delete()
	{
		if (SelectedBlocker is not { IsSystem: false } vm)
		{
			return;
		}

		int index = Blockers.IndexOf(vm);
		UiBlockerConfiguration.UserBlockers.Remove(vm.Model);
		Engine.HideBlocker(vm);
		Blockers.Remove(vm);

		SelectedBlocker = Blockers.Count switch
		{
			0 => null,
			_ => Blockers[Math.Min(index, Blockers.Count - 1)],
		};
	}

	[RelayCommand(CanExecute = nameof(CanEditSelected))]
	private void CopySelected() =>
		Clipboard.SetTextAndLogErrors(UiBlockerSerializer.Export([SelectedBlocker!.Model]));

	[RelayCommand]
	private void CopyAll() =>
		Clipboard.SetTextAndLogErrors(UiBlockerSerializer.Export(UiBlockerConfiguration.UserBlockers));

	[RelayCommand]
	private void PasteFromClipboard()
	{
		string? text = Clipboard.GetTextAndLogErrors();
		if (string.IsNullOrWhiteSpace(text))
		{
			return;
		}

		UiBlockerSerializer.Import(text, UiBlockerConfiguration.UserBlockers);
		Reload();
	}

	[RelayCommand]
	private void HideAll() => UiBlockerService.HideAll();

	[RelayCommand]
	private void ShowBlockers()
	{
		if (SelectedBlocker is null) return;

		UiBlockerService.ShowBlockers(SelectedBlocker.UiElements);
	}

	private void AddUserBlocker(UiBlockerModel model)
	{
		UiBlockerConfiguration.UserBlockers.Add(model);
		UiBlockerViewModel vm = CreateViewModel(model);
		Blockers.Add(vm);
		SelectedBlocker = vm;
	}

	private void Reload()
	{
		Guid? selectedId = SelectedBlocker?.Model.Id;

		Blockers.Clear();
		foreach (UiBlockerModel model in SystemUiBlockers.All.Concat(UiBlockerConfiguration.UserBlockers))
		{
			Blockers.Add(CreateViewModel(model));
		}

		foreach (SystemUiBlockerConfiguration systemBlockerData in UiBlockerConfiguration.SystemBlockers)
		{
			UiBlockerViewModel? blocker = Blockers.FirstOrDefault(b => b.Model.Id == systemBlockerData.Id);
			blocker?.IsEnabled = systemBlockerData.IsEnabled;
		}

		SelectedBlocker = Blockers.FirstOrDefault(b => b.Model.Id == selectedId) ?? Blockers.FirstOrDefault();
	}

	private UiBlockerViewModel CreateViewModel(UiBlockerModel model) => new(model, Maps);
}
