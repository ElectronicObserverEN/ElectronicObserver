using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Window.Wpf.ShipTrainingPlanner;
public partial class ShipTrainingPlanViewModel : ObservableObject
{
	public ShipTrainingPlanModel Model { get; }

	public IShipData Ship { get; set; }

	public int TargetLevel { get; set; }

	public int TargetHP => Ship.HPMax + TargetHPBonus;

	public int TargetASW => Ship.ASWBase + TargetASWBonus;

	/// <summary>
	/// From 0 to 2
	/// </summary>
	public int TargetHPBonus { get; set; }

	/// <summary>
	/// From 0 to 9
	/// </summary>
	public int TargetASWBonus { get; set; }

	public int MaximumHPMod => Ship.HpMaxModernizable();

	/// <summary>
	/// Targetted amount of luck 
	/// eg Yukikaze k2 max luck is 120, then i set this value to 120 to target max
	/// </summary>
	public int TargetLuck { get; set; }

	public ShipId? TargetRemodel { get; set; }

	public ShipTrainingPlanViewModel(ShipTrainingPlanModel model)
	{
		Model = model;

		Ship = KCDatabase.Instance.Ships[model.ShipId];

		TargetLevel = model.TargetLevel;
		TargetLuck = model.TargetLuck;
		TargetHPBonus = model.TargetHPBonus;
		TargetASWBonus = model.TargetASWBonus;

		PropertyChanged += OnPropertyChanged;
	}

	private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		if (e.PropertyName is nameof(TargetLevel) or nameof(TargetLuck) or nameof(TargetHPBonus) or nameof(TargetASWBonus))
		{
			Save();
		}

		if (e.PropertyName is nameof(TargetHPBonus)) OnPropertyChanged(nameof(TargetHP));
		if (e.PropertyName is nameof(TargetASWBonus)) OnPropertyChanged(nameof(TargetASW));
	}

	private void Save()
	{
		Model.TargetLevel = TargetLevel;
		Model.TargetLuck = TargetLuck;
		Model.TargetHPBonus = TargetHPBonus;
		Model.TargetASWBonus = TargetASWBonus;
	}
}
