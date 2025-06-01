using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.AntiAir;
using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.Data;

namespace ElectronicObserver.Window.Wpf;

public static class Extensions
{
	public static Visibility ToVisibility(this bool visible) => visible switch
	{
		true => Visibility.Visible,
		_ => Visibility.Collapsed,
	};

	public static SolidColorBrush ToBrush(this System.Drawing.Color color) =>
		new(color.ToWpfColor());

	public static Color ToWpfColor(this System.Drawing.Color color) =>
		Color.FromArgb(color.A, color.R, color.G, color.B);

	public static float ToSize(this System.Drawing.Font font) => font.Size * font.Unit switch
	{
		System.Drawing.GraphicsUnit.Point => 4 / 3f,
		_ => 1,
	};

	public static Uri ToAbsolute(this Uri uri) => uri switch
	{
		{ IsAbsoluteUri: true } => uri,
		_ => new(new Uri(Environment.ProcessPath!), uri),
	};

	public static int ToSerializableValue(this ListSortDirection? sortDirection) => sortDirection switch
	{
		null => -1,
		{ } => (int)sortDirection,
	};

	public static ListSortDirection? ToSortDirection(this int sortDirection) => sortDirection switch
	{
		0 or 1 => (ListSortDirection)sortDirection,
		_ => null,
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
		if (aaci.Conditions?.FirstOrDefault()?.ShipClasses is { } shipClasses)
		{
			foreach (ShipClass shipClass in shipClasses)
			{
				sb.AppendLine(Constants.GetShipClass(shipClass));
			}

			sb.AppendLine();
		}

		if (aaci.Conditions?.FirstOrDefault()?.Ships is { } ships)
		{
			foreach (ShipId shipId in ships)
			{
				sb.AppendLine(db.MasterShips[(int)shipId].NameEN);
			}

			sb.AppendLine();
		}

		IEnumerable<string> conditions = aaciConditions
			.Select(c => string.Join("\n", c.EquipmentConditions()));

		sb.Append(string.Join("\n==OR==\n", conditions));

		return sb.ToString();
	}

	public static string EquipmentConditionsSingleLineDisplay(this AntiAirCutIn cutIn) =>
		cutIn.Conditions switch
		{
			null => $"{ConstantsRes.Unknown}({cutIn.Id})",

			{ } conditions => string.Join(" OR ", conditions
				.Select(c => string.Join(", ", c.EquipmentConditions()))),
		};

	public static string EquipmentConditionsMultiLineDisplay(this AntiAirCutIn cutIn) =>
		cutIn.Conditions switch
		{
			null => $"{ConstantsRes.Unknown}({cutIn.Id})",

			{ } conditions => string.Join("\nOR\n", conditions
				.Select(c => string.Join("\n", c.EquipmentConditions()))),
		};

	private static List<string> EquipmentConditions(this AntiAirCutInCondition condition)
	{
		List<string> conditions = [];

		if (condition.HighAngle > 0)
		{
			conditions.Add($"{AaciResources.HighAngle} >= {condition.HighAngle}");
		}

		if (condition.HighAngleDirector > 0)
		{
			conditions.Add($"{AaciResources.HighAngleDirector} >= {condition.HighAngleDirector}");
		}

		if (condition.HighAngleWithoutDirector > 0)
		{
			conditions.Add($"{AaciResources.HighAngleWithoutDirector} >= {condition.HighAngleWithoutDirector}");
		}

		if (condition.HatsuzukiGun > 0)
		{
			string hatsuzukiGun = KCDatabase.Instance
				.MasterEquipments[(int)EquipmentId.MainGunSmall_10cmTwinHighangleMountKai_AntiAircraftFireDirectorKai]
				.NameEN;

			conditions.Add($"{hatsuzukiGun} >= {condition.HatsuzukiGun}");
		}

		if (condition.AaDirector > 0)
		{
			conditions.Add($"{AaciResources.AaDirector} >= {condition.AaDirector}");
		}

		if (condition.Radar > 0)
		{
			conditions.Add($"{AaciResources.Radar} >= {condition.Radar}");
		}

		if (condition.AntiAirRadar > 0)
		{
			conditions.Add($"{AaciResources.AntiAirRadar} >= {condition.AntiAirRadar}");
		}

		if (condition.MainGunLarge > 0)
		{
			conditions.Add($"{AaciResources.MainGunLarge} >= {condition.MainGunLarge}");
		}

		if (condition.MainGunLargeFcr > 0)
		{
			conditions.Add($"{AaciResources.MainGunLargeFcr} >= {condition.MainGunLargeFcr}");
		}

		if (condition.AaShell > 0)
		{
			conditions.Add($"{AaciResources.AaShell} >= {condition.AaShell}");
		}

		if (condition.AaGun > 0)
		{
			conditions.Add($"{AaciResources.AaGun} >= {condition.AaGun}");
		}

		if (condition.AaGun3Aa > 0)
		{
			conditions.Add($"{AaciResources.AaGun3AaOrMore} >= {condition.AaGun3Aa}");
		}

		if (condition.AaGun4Aa > 0)
		{
			conditions.Add($"{AaciResources.AaGun4AaOrMore} >= {condition.AaGun4Aa}");
		}

		if (condition.AaGun5Aa > 0)
		{
			conditions.Add($"{AaciResources.AaGun5AaOrMore} >= {condition.AaGun5Aa}");
		}

		if (condition.AaGun6Aa > 0)
		{
			conditions.Add($"{AaciResources.AaGun6AaOrMore} >= {condition.AaGun6Aa}");
		}

		if (condition.AaGun3To8Aa > 0)
		{
			conditions.Add($"{AaciResources.AaGun3To8Aa} >= {condition.AaGun3To8Aa}");
		}

		if (condition.AaGunConcentrated > 0)
		{
			conditions.Add($"{AaciResources.AaGunConcentrated} >= {condition.AaGunConcentrated}");
		}

		if (condition.AaGunPompom > 0)
		{
			conditions.Add($"{AaciResources.AaGunPompom} >= {condition.AaGunPompom}");
		}

		if (condition.AaRocketBritish > 0)
		{
			conditions.Add($"{AaciResources.AaRocketBritish} >= {condition.AaRocketBritish}");
		}

		if (condition.AaRocketMod > 0)
		{
			conditions.Add($"{AaciResources.AaRocketMod} >= {condition.AaRocketMod}");
		}

		if (condition.HighAngleMusashi > 0)
		{
			conditions.Add($"{AaciResources.HighAngleMusashi} >= {condition.HighAngleMusashi}");
		}

		if (condition.HighAngleAmerican > 0)
		{
			conditions.Add($"{AaciResources.HighAngleAmerican} >= {condition.HighAngleAmerican}");
		}

		if (condition.HighAngleAmericanKai > 0)
		{
			conditions.Add($"{AaciResources.HighAngleAmericanKai} >= {condition.HighAngleAmericanKai}");
		}

		if (condition.HighAngleAmericanGfcs > 0)
		{
			conditions.Add($"{AaciResources.HighAngleAmericanGfcs} >= {condition.HighAngleAmericanGfcs}");
		}

		if (condition.RadarGfcs > 0)
		{
			conditions.Add($"{AaciResources.RadarGfcs} >= {condition.RadarGfcs}");
		}

		if (condition.HighAngleAtlanta > 0)
		{
			conditions.Add($"{AaciResources.HighAngleAtlanta} >= {condition.HighAngleAtlanta}");
		}

		if (condition.HighAngleAtlantaGfcs > 0)
		{
			conditions.Add($"{AaciResources.HighAngleAtlantaGfcs} >= {condition.HighAngleAtlantaGfcs}");
		}

		if (condition.HighAngleConcentrated > 0)
		{
			conditions.Add($"{AaciResources.HighAngleConcentrated} >= {condition.HighAngleConcentrated}");
		}

		if (condition.RadarYamato > 0)
		{
			conditions.Add($"{AaciResources.RadarYamato} >= {condition.RadarYamato}");
		}

		if (condition.HarunaGun > 0)
		{
			conditions.Add($"{AaciResources.HarunaGun} >= {condition.HarunaGun}");
		}

		if (condition.HarusameGun > 0)
		{
			string harusameGun = KCDatabase.Instance
				.MasterEquipments[(int)EquipmentId.MainGunSmall_12_7cmTwinGunModelCKaiSanH]
				.NameEN;

			conditions.Add($"{harusameGun} >= {condition.HarusameGun}");
		}

		if (condition.AaGunShigure > 0)
		{
			string aaGunShigure = KCDatabase.Instance
				.MasterEquipments[(int)EquipmentId.AAGun_25mmAntiaircraftMachineGunExtra]
				.NameEN;

			conditions.Add($"{aaGunShigure} >= {condition.AaGunShigure}");
		}

		if (condition.Radar4AaOrMore > 0)
		{
			conditions.Add($"{AaciResources.Radar4AaOrMore} >= {condition.Radar4AaOrMore}");
		}

		if (condition.AkizukiGunKai > 0)
		{
			conditions.Add($"{AaciResources.AkizukiGunKai} >= {condition.AkizukiGunKai}");
		}

		if (condition.AkizukiPotatoGun > 0)
		{
			string akizukiPotatoGun = KCDatabase.Instance
				.MasterEquipments[(int)EquipmentId.MainGunSmall_10cmTwinHighAngleGunKai]
				.NameEN;

			conditions.Add($"{akizukiPotatoGun} >= {condition.AkizukiPotatoGun}");
		}

		if (condition.Aafd94 > 0)
		{
			string aafd94 = KCDatabase.Instance
				.MasterEquipments[(int)EquipmentId.AADirector_Type94AAFD]
				.NameEN;

			conditions.Add($"{aafd94} >= {condition.Aafd94}");
		}

		return conditions;
	}

	public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
		=> new(enumerable);

	public static ReadOnlyCollection<T> ToReadOnlyCollection<T>(this IList<T> enumerable)
		=> new(enumerable);
}
