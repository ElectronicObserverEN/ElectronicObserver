using System.ComponentModel;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserverTypes;

namespace ElectronicObserver.Avalonia.ExpeditionCalculator;

public partial class ExpeditionViewModel : ObservableObject
{
	private Expedition Model { get; }
	private FleetInfoViewModel FleetInfo { get; }
	private ExpeditionScoreWeights Weights { get; }

	public string DisplayId => Model.DisplayId;
	public TimeSpan Duration => Model.Duration;

	[ObservableProperty] private double _score;

	public int Fuel => Model.Fuel;
	public int Ammo => Model.Ammo;
	public int Steel => Model.Steel;
	public int Bauxite => Model.Bauxite;

	public ExpeditionReward? Item1 => Model.Item1;
	public ExpeditionReward? Item2 => Model.Item2;

	[ObservableProperty] private double _greatSuccessRate;

	public ExpeditionViewModel(Expedition expedition, FleetInfoViewModel fleetInfo, ExpeditionScoreWeights weights)
	{
		Model = expedition;
		FleetInfo = fleetInfo;
		Weights = weights;

		FleetInfo.PropertyChanged += UpdateGreatSuccessRate;
		Weights.PropertyChanged += UpdateScore;

		UpdateGreatSuccessRate(null, new(""));
	}

	private void UpdateGreatSuccessRate(object? sender, PropertyChangedEventArgs e)
	{
		GreatSuccessRate = Math.Clamp(GetGreatSuccessRate(Model, FleetInfo), 0, 1);

		UpdateScore(sender, e);
	}

	private void UpdateScore(object? sender, PropertyChangedEventArgs e)
	{
		double rate = 1 + 0.5 * GreatSuccessRate;

		Score =
			HourlyGain(Model.Duration, Model.Fuel, rate) * Weights.Fuel +
			HourlyGain(Model.Duration, Model.Ammo, rate) * Weights.Ammo +
			HourlyGain(Model.Duration, Model.Steel, rate) * Weights.Steel +
			HourlyGain(Model.Duration, Model.Bauxite, rate) * Weights.Bauxite +
			ItemScore(Model, Weights);

		Score = Math.Floor(Score);
	}

	private double ItemScore(Expedition model, ExpeditionScoreWeights weights)
	{
		double score = 0;

		if (model.Item1 is not null)
		{
			score += HourlyGain(model.Duration, ItemAmount(model.Item1.Amount), 0.5) *
				ExpeditionRewardWeight(model.Item1.Type, weights);
		}

		if (model.Item2 is not null)
		{
			score += HourlyGain(model.Duration, ItemAmount(model.Item2.Amount), GreatSuccessRate) *
				ExpeditionRewardWeight(model.Item2.Type, weights);
		}

		return score;

		// random between 1 and max amount
		static double ItemAmount(int amount) => amount switch
		{
			> 1 => (amount + 1) / 2.0,
			_ => amount,
		};
	}

	private static double HourlyGain(TimeSpan duration, double gain, double rate = 1)
		=> TimeSpan.FromHours(1) / duration * gain * rate;

	private static int ExpeditionRewardWeight(UseItemId type, ExpeditionScoreWeights weights)
		=> type switch
		{
			UseItemId.InstantRepair => weights.InstantRepair,
			UseItemId.InstantConstruction => weights.InstantConstruction,
			UseItemId.DevelopmentMaterial => weights.DevelopmentMaterial,
			UseItemId.ImproveMaterial => weights.ImproveMaterial,
			UseItemId.FurnitureBoxSmall => weights.FurnitureBoxSmall,
			UseItemId.FurnitureBoxMedium => weights.FurnitureBoxMedium,
			UseItemId.FurnitureBoxLarge => weights.FurnitureBoxLarge,
			UseItemId.MoraleFoodIrako => weights.MoraleFoodIrako,
			_ => 0,
		};

	private static double GetGreatSuccessRate(Expedition model, FleetInfoViewModel fleetInfo) => model.GreatSuccessType switch
	{
		GreatSuccessType.Regular => RegularGreatSuccessRate(fleetInfo),
		GreatSuccessType.Drum => DrumGreatSuccessRate(model, fleetInfo),
		GreatSuccessType.Level => LevelGreatSuccessRate(fleetInfo),

		_ => 0,
	};

	private static double RegularGreatSuccessRate(FleetInfoViewModel fleetInfo) => fleetInfo.AllSparkled switch
	{
		false => 0,
		_ => fleetInfo.SparkleCount * 0.15 + 0.21
	};

	private static double DrumGreatSuccessRate(Expedition model, FleetInfoViewModel fleetInfo)
		=> fleetInfo.SparkleCount * 0.15 + GetDrumBonus(model, fleetInfo);

	private static double GetDrumBonus(Expedition model, FleetInfoViewModel fleetInfo)
		=> (model.Id, fleetInfo.DrumCount) switch
		{
			(21, >= 4) => 0.41,
			(24, >= 2) => 0.41,
			(37, >= 5) => 0.41,
			(38, >= 10) => 0.41,
			(40, >= 4) => 0.41,
			(44, >= 8) => 0.41,
			(142, >= 6) => 0.41,

			_ => 0.06,
		};

	private static double LevelGreatSuccessRate(FleetInfoViewModel fleetInfo)
		=> fleetInfo.SparkleCount * 0.15 + 0.16
			+ Math.Sqrt(fleetInfo.FlagshipLevel) / 100
			+ fleetInfo.FlagshipLevel / 1000.0;
}
