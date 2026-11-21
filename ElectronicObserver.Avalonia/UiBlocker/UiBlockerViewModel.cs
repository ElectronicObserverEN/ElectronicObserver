using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.UiBlocker.Resources;

namespace ElectronicObserver.Avalonia.UiBlocker;

public sealed partial class UiBlockerViewModel : ObservableObject
{
	private IMapInfoProvider Maps { get; }

	public UiBlockerModel Model { get; }

	public bool IsSystem => Model.IsSystem;
	public bool IsEditable => !Model.IsSystem;

	public string Name
	{
		get => SystemUiBlockers.GetName(Model.Id) ?? field;
		set
		{
			if (value == field)
			{
				return;
			}

			field = value;
			Model.Name = value;

			OnPropertyChanged();
			OnPropertyChanged(nameof(Header));
		}
	}

	public string Header => string.IsNullOrWhiteSpace(Name) switch
	{
		true => UiBlockerResources.Unnamed,
		_ => Name,
	};

	public string? Description
	{
		get => SystemUiBlockers.GetDescription(Model.Id) ?? field;
		set
		{
			if (value == field)
			{
				return;
			}

			field = value;
			Model.Description = value;

			OnPropertyChanged();
		}
	}

	[ObservableProperty] public partial bool IsEnabled { get; set; }
	public ObservableCollection<UiElementViewModel> UiElements { get; }

	public UiBlockerViewModel(UiBlockerModel model, IMapInfoProvider maps)
	{
		Model = model;
		Maps = maps;

		Name = model.Name;
		Description = model.Description;
		IsEnabled = model.IsEnabled;

		UiElements = [.. model.UiElements.Select(e => new UiElementViewModel(e, model.IsSystem, maps, RemoveUiElement))];
	}

	partial void OnIsEnabledChanged(bool value)
	{
		Model.IsEnabled = value;
	}

	[RelayCommand]
	private void AddUiElement()
	{
		UiElementModel model = new();
		Model.UiElements.Add(model);
		UiElements.Add(new UiElementViewModel(model, Model.IsSystem, Maps, RemoveUiElement));
	}

	private void RemoveUiElement(UiElementViewModel uiElement)
	{
		Model.UiElements.Remove(uiElement.Model);
		UiElements.Remove(uiElement);
	}
}
