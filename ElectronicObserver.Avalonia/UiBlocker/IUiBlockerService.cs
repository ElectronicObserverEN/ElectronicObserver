namespace ElectronicObserver.Avalonia.UiBlocker;

public interface IUiBlockerService
{
	void ShowBlockers(IEnumerable<UiElementViewModel> elements);
	void HideBlockers(IEnumerable<UiElementViewModel> elements);
	void HideAll();
}
