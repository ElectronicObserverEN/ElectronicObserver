using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Core.Types.AntiAir;

namespace ElectronicObserver.Avalonia.Samples.AntiAirCutInUpdater;

public partial class AntiAirCutInUpdaterViewModel : ObservableObject
{
	[ObservableProperty]
	public partial string? RawAntiAirCutInData { get; set; }

	[ObservableProperty]
	public partial string? Differences { get; set; }

	public string Instructions => """
		copy-paste columns A B D E F (without headers) from the AACI sheet
		example:
		1	38	11	1.85	59.4%
		2	39	11	1.7	55.0%
		""";

	[RelayCommand]
	private void FindDifferences()
	{
		if (string.IsNullOrEmpty(RawAntiAirCutInData)) return;

		List<AntiAirCutInData> data = RawAntiAirCutInData
			.Split(Environment.NewLine)
			.Where(l => !string.IsNullOrEmpty(l))
			.Select(l => l.Split('\t'))
			.Select(v => new AntiAirCutInData
			{
				Priority = int.Parse(v[0]),
				Id = int.Parse(v[1]),
				FixedBonus = int.Parse(v[2]) - 1,
				VariableBonus = double.Parse(v[3]),
				Rate = v[4].TrimEnd('?', '%') switch
				{
					string rate when !string.IsNullOrEmpty(rate) => double.Parse(rate) / 100,
					_ => null,
				},
			})
			.OrderBy(d => d.Id)
			.ToList();

		List<AntiAirCutIn> existingCutIns = AntiAirCutIn.AllCutIns
			.OrderBy(c => c.Id)
			.Where(a => a.Id is not 0)
			.Where(a => data.Any(d => d.Id == a.Id))
			.ToList();

		List<string> errors = existingCutIns
			.Zip(data, (cutIn, newData) =>
			{
				List<string> differences = [];

				if (cutIn.FixedBonus != newData.FixedBonus)
				{
					differences.Add($"FixedBonus: {cutIn.FixedBonus} => {newData.FixedBonus}");
				}

				if (Math.Abs(cutIn.VariableBonus - newData.VariableBonus) > 0.001)
				{
					differences.Add($"VariableBonus: {cutIn.VariableBonus} => {newData.VariableBonus}");
				}

				string existingRate = RateString(cutIn.Rate);
				string newRate = RateString(newData.Rate);

				if (existingRate != newRate)
				{
					differences.Add($"Rate: {existingRate} => {newRate}");
				}

				return differences.Count switch
				{
					0 => null,
					_ => $"{cutIn.Id}: {string.Join(", ", differences)}"
				};
			})
			.OfType<string>()
			.ToList();

		List<string> missing = data
			.Where(d => !existingCutIns.Select(c => c.Id).Contains(d.Id))
			.Select(d => $$"""
			new()
			{
				Id = {{d.Id}},
				FixedBonus = {{d.FixedBonus}},
				VariableBonus = {{d.VariableBonus}},
				Rate = {{RateString(d.Rate)}},
				Conditions = [],
			},
			""")
			.ToList();

		errors.AddRange(missing);

		Differences = errors.Count switch
		{
			0 => "No differences found.",
			_ => string.Join(Environment.NewLine, errors)
		};
	}

	private static string RateString(double? rate) => rate switch
	{
		null => "null",
		_ => $"{rate.Value:F3}"
	};
}
