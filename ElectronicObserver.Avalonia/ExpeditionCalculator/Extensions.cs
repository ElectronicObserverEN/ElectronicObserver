namespace ElectronicObserver.Avalonia.ExpeditionCalculator;

public static class Extensions
{
	public static double GreatSuccessRate(this FleetInfoViewModel fleetInfo, Expedition model) => model.GreatSuccessType switch
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
		=> fleetInfo.SparkleCount * 0.15 + DrumBonus(model, fleetInfo);

	private static double DrumBonus(Expedition model, FleetInfoViewModel fleetInfo)
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
