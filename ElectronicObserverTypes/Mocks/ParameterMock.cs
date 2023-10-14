using System;

namespace ElectronicObserverTypes.Mocks;

public class ParameterMock : IParameter
{
	public int Minimum { get; }
	public int Maximum { get; set; }
	public int MinimumEstMin { get; set; }
	public int MinimumEstMax { get; set; }
	public bool IsMinimumDefault => false;
	public bool IsMaximumDefault => false;
	public bool IsAvailable => true;
	public bool IsDetermined => true;

	public ParameterMock()
	{

	}

	public ParameterMock(int minimum, int maximum)
	{
		Minimum = minimum;
		MinimumEstMin = minimum;
		MinimumEstMax = minimum;
		Maximum = maximum;
	}

	public bool SetEstParameter(int level, int current, int max)
	{
		throw new System.NotImplementedException();
	}

	public int GetEstParameterMin(int level)
	{
		throw new System.NotImplementedException();
	}

	public int GetEstParameterMax(int level)
	{
		throw new System.NotImplementedException();
	}

	public int GetParameter(int level) => CalculateParameter(level, Minimum, Maximum);

	public int? GetNextLevel(int level, int? current = null)
	{
		if (!IsAvailable) return null;
		if (!IsDetermined) return null;
		if (Maximum <= Minimum) return null;

		int target = (current ?? GetParameter(level)) + 1;

		return (int)Math.Ceiling(((target - Minimum) * 99.0) / (Maximum - Minimum));
	}

	private static int CalculateParameter(int level, int min, int max) =>
		min + (int)((max - min) * level / 99.0);
}
