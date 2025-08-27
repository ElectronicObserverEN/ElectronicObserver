namespace ElectronicObserver.Utility.Data;

/// <summary>
/// These values aren't floored so that we can apply the flagship modifier in later calculations.
/// </summary>
public class ExerciseExp
{
	public double BaseA { get; init; }
	public double BaseS { get; init; }

	public double? TrainingCruiserSurfaceA { get; set; }
	public double? TrainingCruiserSurfaceS { get; set; }
	public double? TrainingCruiserSubmarineA { get; set; }
	public double? TrainingCruiserSubmarineS { get; set; }
}
