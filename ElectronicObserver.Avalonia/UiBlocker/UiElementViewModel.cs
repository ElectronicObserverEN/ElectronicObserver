using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Converters;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Avalonia.QuestTrackerManager2;
using ElectronicObserver.Avalonia.UiBlocker.Resources;

namespace ElectronicObserver.Avalonia.UiBlocker;

public sealed partial class UiElementViewModel : ObservableObject
{
	public List<PositionPreset> Presets { get; } = [.. Enum.GetValues<PositionPreset>()];

	private IMapInfoProvider Maps { get; }
	private Action<UiElementViewModel> OnRemove { get; }

	public UiElementModel Model { get; }

	public ObservableCollection<UiTriggerOptionViewModel> ShowTriggers { get; }
	public ObservableCollection<UiTriggerOptionViewModel> HideTriggers { get; }
	public ObservableCollection<ConditionViewModel> Conditions { get; }

	[ObservableProperty]
	[NotifyPropertyChangedFor(nameof(Display))]
	public partial PositionPreset Preset { get; set; }

	[ObservableProperty] public partial double DesiredTop { get; set; }
	[ObservableProperty] public partial double DesiredLeft { get; set; }
	[ObservableProperty] public partial double DesiredHeight { get; set; }
	[ObservableProperty] public partial double DesiredWidth { get; set; }
	[ObservableProperty] public partial Color BackgroundColor { get; set; }
	[ObservableProperty] public partial string? ImagePath { get; set; }

	public string Display => $"{Preset.ToDisplay()} | {ShowTriggerDisplay} | {ConditionDisplay}";
	private string ShowTriggerDisplay => string.Join(" | ", ShowTriggers.Where(t => t.IsChecked).Select(t => t.Trigger.ToDisplay()));

	private string ConditionDisplay => string.Join(" | ", Conditions.Select(c => c.Invert switch
	{
		true => $"!{ConditionTypeDisplay(c)}",
		_ => $"{ConditionTypeDisplay(c)}",
	}));

	public static string ConditionTypeDisplay(ConditionViewModel condition) => condition switch
	{
		{ Type: ConditionType.Location, SelectedMapOption: { } map, Model.CellIds: [] } => $"{ConditionTypeResources.Location} {map.AreaId}-{map.InfoId}",
		{ Type: ConditionType.Location, SelectedMapOption: { } map } => $"{ConditionTypeResources.Location} {map.AreaId}-{map.InfoId} ({string.Join(", ", condition.Nodes.Where(n => n.IsChecked).Select(n => n.Letter))})",

		{ Type: ConditionType.BattleRankAtLeast or ConditionType.BattleRankAtMost, Rank: { } rank } => $"{condition.Type.ToDisplay()} {rank.ToDisplay()}",
		
		_ => condition.Type.ToDisplay(),
	};

	public UiElementViewModel(UiElementModel model, bool isSystem, IMapInfoProvider maps, Action<UiElementViewModel> onRemove)
	{
		Model = model;
		Maps = maps;
		OnRemove = onRemove;

		Preset = Model.Preset;

		if (!isSystem || Model.Preset == PositionPreset.Custom)
		{
			DesiredTop = Model.DesiredTop;
			DesiredLeft = Model.DesiredLeft;
			DesiredHeight = Model.DesiredHeight;
			DesiredWidth = Model.DesiredWidth;
		}

		if (ColorToHexConverter.ParseHexString(model.BackgroundColor, AlphaComponentPosition.Leading) is Color color)
		{
			BackgroundColor = color;
		}

		UiBlockerTrigger[] triggers = Enum.GetValues<UiBlockerTrigger>();
		ShowTriggers = [.. triggers.Select(t => new UiTriggerOptionViewModel(t))];
		HideTriggers = [.. triggers.Select(t => new UiTriggerOptionViewModel(t))];

		foreach (UiTriggerOptionViewModel showTrigger in ShowTriggers)
		{
			showTrigger.IsChecked = Model.ShowTriggers.Contains(showTrigger.Trigger);
			showTrigger.PropertyChanged += ShowTriggerOnPropertyChanged;
		}

		foreach (UiTriggerOptionViewModel hideTrigger in HideTriggers)
		{
			hideTrigger.IsChecked = Model.HideTriggers.Contains(hideTrigger.Trigger);
			hideTrigger.PropertyChanged += HideTriggerOnPropertyChanged;
		}

		Conditions = [.. model.Conditions.Select(MakeViewModel)];
		Conditions.CollectionChanged += ConditionsOnCollectionChanged;
	}

	private void ShowTriggerOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		Model.ShowTriggers = [.. ShowTriggers.Where(t => t.IsChecked).Select(t => t.Trigger)];
		OnPropertyChanged(nameof(Display));
	}

	private void HideTriggerOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		Model.HideTriggers = [.. HideTriggers.Where(t => t.IsChecked).Select(t => t.Trigger)];
	}

	private void ConditionOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		OnPropertyChanged(nameof(Display));
	}

	private void ConditionsOnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
	{
		OnPropertyChanged(nameof(Display));
	}

	partial void OnPresetChanged(PositionPreset value)
	{
		Model.Preset = value;
		ApplyPreset();
	}

	partial void OnDesiredTopChanged(double value)
	{
		Model.DesiredTop = value;
	}

	partial void OnDesiredLeftChanged(double value)
	{
		Model.DesiredLeft = value;
	}

	partial void OnDesiredHeightChanged(double value)
	{
		Model.DesiredHeight = value;
	}

	partial void OnDesiredWidthChanged(double value)
	{
		Model.DesiredWidth = value;
	}

	partial void OnImagePathChanged(string? value)
	{
		Model.ImagePath = value;
	}

	partial void OnBackgroundColorChanged(Color value)
	{
		Model.BackgroundColor = ColorToHexConverter.ToHexString(value, AlphaComponentPosition.Leading, true, true);
	}

	[RelayCommand]
	private void ApplyPreset()
	{
		if (Preset is PositionPreset.Custom) return;

		Rect preset = PositionPresets.TryGet(Preset);

		DesiredTop = preset.Top;
		DesiredLeft = preset.Left;
		DesiredHeight = preset.Height;
		DesiredWidth = preset.Width;
	}

	[RelayCommand]
	private void AddCondition()
	{
		ConditionModel model = new();
		Model.Conditions.Add(model);
		Conditions.Add(MakeViewModel(model));
	}

	[RelayCommand]
	private void Remove() => OnRemove(this);

	private void RemoveCondition(ConditionViewModel condition)
	{
		Model.Conditions.Remove(condition.Model);
		Conditions.Remove(condition);
	}

	private ConditionViewModel MakeViewModel(ConditionModel condition)
	{
		ConditionViewModel viewModel = new(condition, Maps, RemoveCondition);
		viewModel.PropertyChanged += ConditionOnPropertyChanged;

		return viewModel;
	}
}
