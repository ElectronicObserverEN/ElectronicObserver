using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ElectronicObserver.Data;
using ElectronicObserverTypes;
using ElectronicObserverTypes.AntiAir;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Wpf;

public static class Extensions
{
	public static Visibility ToVisibility(this bool visible) => visible switch
	{
		true => Visibility.Visible,
		_ => Visibility.Collapsed
	};

	public static SolidColorBrush ToBrush(this System.Drawing.Color color) =>
		new(Color.FromArgb(color.A, color.R, color.G, color.B));

	public static float ToSize(this System.Drawing.Font font) => font.Size * font.Unit switch
	{
		System.Drawing.GraphicsUnit.Point => 4 / 3f,
		_ => 1
	};

	public static Uri ToAbsolute(this Uri uri) => uri switch
	{
		{ IsAbsoluteUri: true } => uri,
		_ => new(new Uri(Process.GetCurrentProcess().MainModule.FileName), uri)
	};

	public static int ToSerializableValue(this ListSortDirection? sortDirection) => sortDirection switch
	{
		null => -1,
		{ } => (int)sortDirection
	};

	public static ListSortDirection? ToSortDirection(this int sortDirection) => sortDirection switch
	{
		0 or 1 => (ListSortDirection)sortDirection,
		_ => null
	};

	// todo: move to EOTypes.AA, not possible right now cause of ship class names
	public static string? ConditionDisplay(this AntiAirCutIn aaci, IKCDatabase db)
	{
		StringBuilder sb = new();

		List<AntiAirCutInCondition> aaciConditions = aaci.Conditions;

		if (aaciConditions is not { Count: > 0 })
		{
			return null;
		}

		// ships/classes should be the same for all possible conditions so only write them once
		if (aaci.Conditions.FirstOrDefault()?.ShipClasses is { } shipClasses)
		{
			foreach (ShipClass shipClass in shipClasses)
			{
				sb.AppendLine(Constants.GetShipClass(shipClass));
			}

			sb.AppendLine();
		}

		if (aaci.Conditions.FirstOrDefault()?.Ships is { } ships)
		{
			foreach (ShipId shipId in ships)
			{
				sb.AppendLine(db.MasterShips[(int)shipId].NameEN);
			}

			sb.AppendLine();
		}

		sb.Append(string.Join("==OR==\n", aaciConditions.Select(c => c.ConditionDisplay())));

		return sb.ToString();
	}

	private static string ConditionDisplay(this AntiAirCutInCondition condition)
	{
		StringBuilder sb = new();

		if (condition.HighAngle > 0)
		{
			sb.AppendLine($"{AaciStrings.HighAngle} >= {condition.HighAngle}");
		}

		if (condition.HighAngleDirector > 0)
		{
			sb.AppendLine($"{AaciStrings.HighAngleDirector} >= {condition.HighAngleDirector}");
		}

		if (condition.HighAngleWithoutDirector > 0)
		{
			sb.AppendLine($"{AaciStrings.HighAngleWithoutDirector} >= {condition.HighAngleWithoutDirector}");
		}

		if (condition.AaDirector > 0)
		{
			sb.AppendLine($"{AaciStrings.AaDirector} >= {condition.AaDirector}");
		}

		if (condition.Radar > 0)
		{
			sb.AppendLine($"{AaciStrings.Radar} >= {condition.Radar}");
		}

		if (condition.AntiAirRadar > 0)
		{
			sb.AppendLine($"{AaciStrings.AntiAirRadar} >= {condition.AntiAirRadar}");
		}

		if (condition.MainGunLarge > 0)
		{
			sb.AppendLine($"{AaciStrings.MainGunLarge} >= {condition.MainGunLarge}");
		}

		if (condition.MainGunLargeFcr > 0)
		{
			sb.AppendLine($"{AaciStrings.MainGunLargeFcr} >= {condition.MainGunLargeFcr}");
		}

		if (condition.AaShell > 0)
		{
			sb.AppendLine($"{AaciStrings.AaShell} >= {condition.AaShell}");
		}

		if (condition.AaGun > 0)
		{
			sb.AppendLine($"{AaciStrings.AaGun} >= {condition.AaGun}");
		}

		if (condition.AaGun3Aa > 0)
		{
			sb.AppendLine($"{AaciStrings.AaGun3AaOrMore} >= {condition.AaGun3Aa}");
		}

		if (condition.AaGun4Aa > 0)
		{
			sb.AppendLine($"{AaciStrings.AaGun4AaOrMore} >= {condition.AaGun4Aa}");
		}

		if (condition.AaGun6Aa > 0)
		{
			sb.AppendLine($"{AaciStrings.AaGun6AaOrMore} >= {condition.AaGun6Aa}");
		}

		if (condition.AaGun3To8Aa > 0)
		{
			sb.AppendLine($"{AaciStrings.AaGun3To8Aa} >= {condition.AaGun3To8Aa}");
		}

		if (condition.AaGunConcentrated > 0)
		{
			sb.AppendLine($"{AaciStrings.AaGunConcentrated} >= {condition.AaGunConcentrated}");
		}

		if (condition.AaGunPompom > 0)
		{
			sb.AppendLine($"{AaciStrings.AaGunPompom} >= {condition.AaGunPompom}");
		}

		if (condition.AaRocketBritish > 0)
		{
			sb.AppendLine($"{AaciStrings.AaRocketBritish} >= {condition.AaRocketBritish}");
		}

		if (condition.AaRocketMod > 0)
		{
			sb.AppendLine($"{AaciStrings.AaRocketMod} >= {condition.AaRocketMod}");
		}

		if (condition.HighAngleMusashi > 0)
		{
			sb.AppendLine($"{AaciStrings.HighAngleMusashi} >= {condition.HighAngleMusashi}");
		}

		if (condition.HighAngleAmerican > 0)
		{
			sb.AppendLine($"{AaciStrings.HighAngleAmerican} >= {condition.HighAngleAmerican}");
		}

		if (condition.HighAngleAmericanKai > 0)
		{
			sb.AppendLine($"{AaciStrings.HighAngleAmericanKai} >= {condition.HighAngleAmericanKai}");
		}

		if (condition.HighAngleAmericanGfcs > 0)
		{
			sb.AppendLine($"{AaciStrings.HighAngleAmericanGfcs} >= {condition.HighAngleAmericanGfcs}");
		}

		if (condition.RadarGfcs > 0)
		{
			sb.AppendLine($"{AaciStrings.RadarGfcs} >= {condition.RadarGfcs}");
		}

		if (condition.HighAngleAtlanta > 0)
		{
			sb.AppendLine($"{AaciStrings.HighAngleAtlanta} >= {condition.HighAngleAtlanta}");
		}

		if (condition.HighAngleAtlantaGfcs > 0)
		{
			sb.AppendLine($"{AaciStrings.HighAngleAtlantaGfcs} >= {condition.HighAngleAtlantaGfcs}");
		}

		if (condition.HighAngleConcentrated > 0)
		{
			sb.AppendLine($"{AaciStrings.HighAngleConcentrated} >= {condition.HighAngleConcentrated}");
		}

		if (condition.RadarYamato > 0)
		{
			sb.AppendLine($"{AaciStrings.RadarYamato} >= {condition.RadarYamato}");
		}

		return sb.ToString();
	}
}
