using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ElectronicObserverTypes.AntiAir;

[DebuggerDisplay("Id = {Id}")]
public record AntiAirCutIn
{
	private static List<AntiAirCutIn> RegularCutIns { get; } = new()
	{
		new()
		{
			Id = 0,
			FixedBonus = 0,
			VariableBonus = 1,
			Rate = 1,
			Conditions = new(),
		},
		new()
		{
			Id = 1,
			FixedBonus = 7,
			VariableBonus = 1.7,
			Rate = 0.65,
			Conditions = new()
			{
				new()
				{
					ShipClasses = new()
					{
						ShipClass.Akizuki,
					},
					HighAngle = 2,
					Radar = 1,
				},
			},
		},
		new()
		{
			Id = 2,
			FixedBonus = 6,
			VariableBonus = 1.7,
			Rate = 0.58,
			Conditions = new()
			{
				new()
				{
					ShipClasses = new()
					{
						ShipClass.Akizuki,
					},
					HighAngle = 1,
					Radar = 1,
				},
			},
		},
		new()
		{
			Id = 3,
			FixedBonus = 4,
			VariableBonus = 1.6,
			Rate = 0.5,
			Conditions = new()
			{
				new()
				{
					ShipClasses = new()
					{
						ShipClass.Akizuki,
					},
					HighAngle = 2,
				},
			},
		},
		new()
		{
			Id = 4,
			FixedBonus = 6,
			VariableBonus = 1.5,
			Rate = 0.52,
			Conditions = new()
			{
				new()
				{
					MainGunLarge = 1,
					AaShell = 1,
					AaDirector = 1,
					AntiAirRadar = 1,
				},
			},
		},
		new()
		{
			Id = 5,
			FixedBonus = 4,
			VariableBonus = 1.5,
			Rate = 0.55,
			Conditions = new()
			{
				new()
				{
					HighAngleDirector = 2,
					AntiAirRadar = 1,
				},
			},
		},
		new()
		{
			Id = 6,
			FixedBonus = 4,
			VariableBonus = 1.45,
			Rate = 0.4,
			Conditions = new()
			{
				new()
				{
					MainGunLarge = 1,
					AaShell = 1,
					AaDirector = 1,
				},
			},
		},
		new()
		{
			Id = 7,
			FixedBonus = 3,
			VariableBonus = 1.35,
			Rate = 0.45,
			Conditions = new()
			{
				new()
				{
					HighAngle = 1,
					AaDirector = 1,
					AntiAirRadar = 1,
				},
			},
		},
		new()
		{
			Id = 8,
			FixedBonus = 4,
			VariableBonus = 1.4,
			Rate = 0.5,
			Conditions = new()
			{
				new()
				{
					HighAngleDirector = 1,
					AntiAirRadar = 1,
				},
			},
		},
		new()
		{
			Id = 9,
			FixedBonus = 2,
			VariableBonus = 1.3,
			Rate = 0.4,
			Conditions = new()
			{
				new()
				{
					HighAngle = 1,
					AaDirector = 1,
				},
			},
		},
		new()
		{
			Id = 10,
			FixedBonus = 8,
			VariableBonus = 1.65,
			Rate = 0.6,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.MayaKaiNi,
					},
					HighAngle = 1,
					AaGunConcentrated = 1,
					AntiAirRadar = 1,
				},
			},
		},
		new()
		{
			Id = 11,
			FixedBonus = 6,
			VariableBonus = 1.5,
			Rate = 0.55,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.MayaKaiNi,
					},
					HighAngle = 1,
					AaGunConcentrated = 1,
				},
			},
		},
		new()
		{
			Id = 12,
			FixedBonus = 3,
			VariableBonus = 1.25,
			Rate = 0.45,
			Conditions = new()
			{
				new()
				{
					AaGunConcentrated = 1,
					// the actual condition is 1 AA gun with at least 3 AA and 1 concentrated AA gun (at least 9 AA)
					// the concentrated AA gun will obviously also satisfy the 3 AA gun condition
					AaGun3Aa = 2,
					AntiAirRadar = 1,
				},
			},
		},
		new()
		{
			Id = 13,
			FixedBonus = 4,
			VariableBonus = 1.35,
			Rate = 0.35,
			Conditions = new()
			{
				new()
				{
					HighAngle = 1,
					AaGunConcentrated = 1,
					AntiAirRadar = 1,
				},
			},
		},
		new()
		{
			Id = 14,
			FixedBonus = 4,
			VariableBonus = 1.45,
			Rate = 0.62,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.IsuzuKaiNi,
					},
					HighAngle = 1,
					AaGun = 1,
					AntiAirRadar = 1,
				},
			},
		},
		new()
		{
			Id = 15,
			FixedBonus = 3,
			VariableBonus = 1.3,
			Rate = 0.55,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.IsuzuKaiNi,
					},
					HighAngle = 1,
					AaGun = 1,
				},
			},
		},
		new()
		{
			Id = 16,
			FixedBonus = 4,
			VariableBonus = 1.4,
			Rate = 0.62,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.KasumiKaiNiB,
						ShipId.YuubariKaiNi,
					},
					HighAngle = 1,
					AaGun = 1,
					Radar = 1,
				},
			},
		},
		new()
		{
			Id = 17,
			FixedBonus = 2,
			VariableBonus = 1.25,
			Rate = 0.55,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.KasumiKaiNiB,
						ShipId.YuubariKaiNi,
					},
					HighAngle = 1,
					AaGun = 1,
				},
			},
		},
		new()
		{
			Id = 18,
			FixedBonus = 2,
			VariableBonus = 1.2,
			Rate = 0.6,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.SatsukiKaiNi,
					},
					AaGunConcentrated = 1,
				},
			},
		},
		new()
		{
			Id = 19,
			FixedBonus = 5,
			VariableBonus = 1.45,
			Rate = 0.58,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.KinuKaiNi,
					},
					HighAngleWithoutDirector = 1,
					AaGunConcentrated = 1,
				},
			},
		},
		new()
		{
			Id = 20,
			FixedBonus = 3,
			VariableBonus = 1.25,
			Rate = 0.65,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.KinuKaiNi,
					},
					AaGunConcentrated = 1,
				},
			},
		},
		new()
		{
			Id = 21,
			FixedBonus = 5,
			VariableBonus = 1.45,
			Rate = 0.6,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.YuraKaiNi,
					},
					HighAngle = 1,
					AntiAirRadar = 1,
				},
			},
		},
		new()
		{
			Id = 22,
			FixedBonus = 2,
			VariableBonus = 1.2,
			Rate = 0.6,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.FumizukiKaiNi,
					},
					AaGunConcentrated = 1,
				},
			},
		},
		new()
		{
			Id = 23,
			FixedBonus = 1,
			VariableBonus = 1.05,
			Rate = 0.8,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.UIT25,
						ShipId.I504,
					},
					AaGun3To8Aa = 1,
				},
			},
		},
		new()
		{
			Id = 24,
			FixedBonus = 3,
			VariableBonus = 1.25,
			Rate = 0.4,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.TenryuuKaiNi,
						ShipId.TatsutaKaiNi,
					},
					HighAngle = 1,
					AaGun3To8Aa = 1,
				},
			},
		},
		new()
		{
			Id = 25,
			FixedBonus = 7,
			VariableBonus = 1.55,
			Rate = 0.6,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.IseKai,
						ShipId.IseKaiNi,
						ShipId.HyuugaKai,
						ShipId.HyuugaKaiNi,
					},
					AaRocketMod = 1,
					AntiAirRadar = 1,
					AaShell = 1,
				},
			},
		},
		new()
		{
			Id = 26,
			FixedBonus = 6,
			VariableBonus = 1.4,
			Rate = 0.55,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.YamatoKaiNi,
						ShipId.YamatoKaiNiJuu,
						ShipId.MusashiKaiNi,
					},
					HighAngleMusashi = 1,
					AntiAirRadar = 1,
				},
			},
		},
		new()
		{
			Id = 27,
			FixedBonus = 5,
			VariableBonus = 1.55,
			Rate = 0.60,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.OoyodoKai,
					},
					HighAngleMusashi = 1,
					AaRocketMod = 1,
					AntiAirRadar = 1,
				},
			},
		},
		new()
		{
			Id = 28,
			FixedBonus = 4,
			VariableBonus = 1.4,
			Rate = 0.55,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.IseKai,
						ShipId.IseKaiNi,
						ShipId.HyuugaKai,
						ShipId.HyuugaKaiNi,
						ShipId.MusashiKai,
						ShipId.MusashiKaiNi,
					},
					AaRocketMod = 1,
					AntiAirRadar = 1,
				},
			},
		},
		new()
		{
			Id = 29,
			FixedBonus = 5,
			VariableBonus = 1.55,
			Rate = 0.6,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.IsokazeBKai,
						ShipId.HamakazeBKai,
					},
					HighAngle = 1,
					AntiAirRadar = 1,
				},
			},
		},
		new()
		{
			Id = 30,
			FixedBonus = 3,
			VariableBonus = 1.3,
			Rate = 0.45,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.TenryuuKaiNi,
						ShipId.GotlandKai,
						ShipId.Gotlandandra,
					},
					HighAngle = 3,
				},
			},
		},
		new()
		{
			Id = 31,
			FixedBonus = 2,
			VariableBonus = 1.25,
			Rate = 0.52,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.TenryuuKaiNi,
					},
					HighAngle = 2,
				},
			},
		},
		new()
		{
			Id = 32,
			FixedBonus = 3,
			VariableBonus = 1.2,
			Rate = 0.5,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.KongouKaiNi,
						ShipId.KongouKaiNiC,
						ShipId.HieiKaiNi,
						ShipId.HieiKaiNiC,
						ShipId.HarunaKaiNi,
						ShipId.HarunaKaiNiB,
						ShipId.HarunaKaiNiC,
						ShipId.KirishimaKaiNi,
						ShipId.Jervis,
						ShipId.JervisKai,
						ShipId.Janus,
						ShipId.JanusKai,
						ShipId.Nelson,
						ShipId.NelsonKai,
						ShipId.Warspite,
						ShipId.WarspiteKai,
						ShipId.ArkRoyal,
						ShipId.ArkRoyalKai,
					},
					MainGunLargeFcr = 1,
					AaGunPompom = 1,
				},
				new()
				{
					Ships = new()
					{
						ShipId.KongouKaiNi,
						ShipId.KongouKaiNiC,
						ShipId.HieiKaiNi,
						ShipId.HieiKaiNiC,
						ShipId.HarunaKaiNi,
						ShipId.HarunaKaiNiB,
						ShipId.HarunaKaiNiC,
						ShipId.KirishimaKaiNi,
						ShipId.Jervis,
						ShipId.JervisKai,
						ShipId.Janus,
						ShipId.JanusKai,
						ShipId.Nelson,
						ShipId.NelsonKai,
						ShipId.Warspite,
						ShipId.WarspiteKai,
						ShipId.ArkRoyal,
						ShipId.ArkRoyalKai,
					},
					AaRocketBritish = 1,
					AaGunPompom = 1,
				},
				new()
				{
					Ships = new()
					{
						ShipId.KongouKaiNi,
						ShipId.KongouKaiNiC,
						ShipId.HieiKaiNi,
						ShipId.HieiKaiNiC,
						ShipId.HarunaKaiNi,
						ShipId.HarunaKaiNiB,
						ShipId.HarunaKaiNiC,
						ShipId.KirishimaKaiNi,
						ShipId.Jervis,
						ShipId.JervisKai,
						ShipId.Janus,
						ShipId.JanusKai,
						ShipId.Nelson,
						ShipId.NelsonKai,
						ShipId.Warspite,
						ShipId.WarspiteKai,
						ShipId.ArkRoyal,
						ShipId.ArkRoyalKai,
					},
					AaRocketBritish = 2,
				},
			},
		},
		new()
		{
			Id = 33,
			FixedBonus = 3,
			VariableBonus = 1.35,
			Rate = 0.45,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.GotlandKai,
						ShipId.Gotlandandra,
					},
					HighAngle = 1,
					AaGun4Aa = 1,
				},
			},
		},
		new()
		{
			Id = 42,
			FixedBonus = 10,
			VariableBonus = 1.65,
			Rate = 0.65,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.YamatoKaiNi,
						ShipId.YamatoKaiNiJuu,
						ShipId.MusashiKaiNi,
					},
					HighAngleConcentrated = 2,
					RadarYamato = 1,
					AaGun6Aa = 1,
				},
			},
		},
		new()
		{
			Id = 43,
			FixedBonus = 8,
			VariableBonus = 1.6,
			Rate = 0.6,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.YamatoKaiNi,
						ShipId.YamatoKaiNiJuu,
						ShipId.MusashiKaiNi,
					},
					HighAngleConcentrated = 2,
					RadarYamato = 1,
				},
			},
		},
		new()
		{
			Id = 44,
			FixedBonus = 6,
			VariableBonus = 1.6,
			Rate = 0.55,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.YamatoKaiNi,
						ShipId.YamatoKaiNiJuu,
						ShipId.MusashiKaiNi,
					},
					HighAngleConcentrated = 1,
					RadarYamato = 1,
					AaGun6Aa = 1,
				},
			},
		},
		new()
		{
			Id = 45,
			FixedBonus = 5,
			VariableBonus = 1.55,
			Rate = 0.5,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.YamatoKaiNi,
						ShipId.YamatoKaiNiJuu,
						ShipId.MusashiKaiNi,
					},
					HighAngleConcentrated = 1,
					RadarYamato = 1,
				},
			},
		},
		new()
		{
			Id = 46,
			FixedBonus = 8,
			VariableBonus = 1.55,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.HarunaKaiNiB,
					},
					HarunaGun = 1,
					AaGunConcentrated = 1,
					AntiAirRadar = 1,
				},
			},
		},
	};

	private static List<AntiAirCutIn> FletcherCutIns { get; } = new()
	{
		new()
		{
			Id = 34,
			FixedBonus = 7,
			VariableBonus = 1.6,
			Rate = 0.55,
			Conditions = new()
			{
				new()
				{
					ShipClasses = new()
					{
						ShipClass.Fletcher,
					},
					HighAngleAmericanGfcs = 2,
				},
			},
		},
		new()
		{
			Id = 35,
			FixedBonus = 6,
			VariableBonus = 1.55,
			Rate = 0.55,
			Conditions = new()
			{
				new()
				{
					ShipClasses = new()
					{
						ShipClass.Fletcher,
					},
					HighAngleAmerican = 1,
					HighAngleAmericanGfcs = 1,
				},
				new()
				{
					ShipClasses = new()
					{
						ShipClass.Fletcher,
					},
					HighAngleAmericanKai = 1,
					HighAngleAmericanGfcs = 1,
				},
			},
		},
		new()
		{
			Id = 36,
			FixedBonus = 6,
			VariableBonus = 1.55,
			Rate = 0.5,
			Conditions = new()
			{
				new()
				{
					ShipClasses = new()
					{
						ShipClass.Fletcher,
					},
					HighAngleAmerican = 2,
					RadarGfcs = 1,
				},
				new()
				{
					ShipClasses = new()
					{
						ShipClass.Fletcher,
					},
					HighAngleAmerican = 1,
					HighAngleAmericanKai = 1,
					RadarGfcs = 1,
				},
				new()
				{
					ShipClasses = new()
					{
						ShipClass.Fletcher,
					},
					HighAngleAmericanKai = 2,
					RadarGfcs = 1,
				},
			},
		},
		new()
		{
			Id = 37,
			FixedBonus = 4,
			VariableBonus = 1.45,
			Rate = 0.4,
			Conditions = new()
			{
				new()
				{
					ShipClasses = new()
					{
						ShipClass.Fletcher,
					},
					HighAngleAmericanKai = 2,
				},
			},
		},
	};

	private static List<AntiAirCutIn> AtlantaCutIns { get; } = new()
	{
		new()
		{
			Id = 38,
			FixedBonus = 10,
			VariableBonus = 1.85,
			Rate = 0.6,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.Atlanta,
						ShipId.AtlantaKai,
					},
					HighAngleAtlantaGfcs = 2,
				},
			},
		},
		new()
		{
			Id = 39,
			FixedBonus = 10,
			VariableBonus = 1.7,
			Rate = 0.57,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.Atlanta,
						ShipId.AtlantaKai,
					},
					HighAngleAtlanta = 1,
					HighAngleAtlantaGfcs = 1,
				},
			},
		},
		new()
		{
			Id = 40,
			FixedBonus = 10,
			VariableBonus = 1.7,
			Rate = 0.56,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.Atlanta,
						ShipId.AtlantaKai,
					},
					HighAngleAtlanta = 2,
					RadarGfcs = 1,
				},
				new()
				{
					Ships = new()
					{
						ShipId.Atlanta,
						ShipId.AtlantaKai,
					},
					HighAngleAtlanta = 1,
					HighAngleAtlantaGfcs = 1,
					RadarGfcs = 1,
				},
				new()
				{
					Ships = new()
					{
						ShipId.Atlanta,
						ShipId.AtlantaKai,
					},
					HighAngleAtlantaGfcs = 2,
					RadarGfcs = 1,
				},
			},
		},
		new()
		{
			Id = 41,
			FixedBonus = 9,
			VariableBonus = 1.65,
			Rate = 0.55,
			Conditions = new()
			{
				new()
				{
					Ships = new()
					{
						ShipId.Atlanta,
						ShipId.AtlantaKai,
					},
					HighAngleAtlanta = 2,
					RadarGfcs = 1,
				},
				new()
				{
					Ships = new()
					{
						ShipId.Atlanta,
						ShipId.AtlantaKai,
					},
					HighAngleAtlanta = 1,
					HighAngleAtlantaGfcs = 1,
				},
				new()
				{
					Ships = new()
					{
						ShipId.Atlanta,
						ShipId.AtlantaKai,
					},
					HighAngleAtlantaGfcs = 2,
				},
			},
		},
	};

	public static List<AntiAirCutIn> AllCutIns => RegularCutIns
		.Concat(FletcherCutIns)
		.Concat(AtlantaCutIns)
		.OrderBy(a => ApiPriority.IndexOf(a.Id))
		.ToList();

	private static List<int> ApiPriority { get; } = new()
	{
		// todo: no idea if 46 is highest priority
		46,
		42,
		43,
		44,
		45,
		34,
		35,
		37,
		36,
		38,
		39,
		40,
		41,
		1,
		2,
		3,
		10,
		11,
		14,
		15,
		16,
		17,
		19,
		21,
		25,
		4,
		5,
		6,
		8,
		7,
		26,
		28,
		29,
		9,
		33,
		12,
		13,
		18,
		20,
		22,
		23,
		30,
		31,
		24,
		32,
		0,
	};

	public int Id { get; init; }
	public int FixedBonus { get; init; }
	public double VariableBonus { get; init; }
	private double? Rate { get; init; }

	/// <summary>
	/// Null when unknown
	/// </summary>
	public List<AntiAirCutInCondition>? Conditions { get; init; }

	public string ValueDisplay => Rate switch
	{
		double => $"({Rate:P0} - {FixedBonus} - x{VariableBonus})",
		_ => $"(??? - {FixedBonus} - x{VariableBonus})",
	};

	private AntiAirCutIn()
	{

	}

	public static AntiAirCutIn FromId(int id) => AllCutIns
		.FirstOrDefault(a => a.Id == id)
		?? new AntiAirCutIn
		{
			Id = id,
		};

	private bool CanBeActivatedBy(IShipData ship)
	{
		if (!Conditions.Any()) return true;
		if (Conditions.Any(c => c.CanBeActivatedBy(ship))) return true;

		return false;
	}

	public static List<AntiAirCutIn> PossibleCutIns(IShipData ship)
	{
		var possibleValues = RegularCutIns
			.Where(a => a.CanBeActivatedBy(ship))
			.OrderBy(a => ApiPriority.IndexOf(a.Id))
			.ToList();

		// remove cut-ins with lower priority and lower activation rate
		// keep cut-ins with lower priority but higher activation rate
		var i = 0;

		while (possibleValues.Count > i + 1)
		{
			if (possibleValues[i].Rate < possibleValues[i + 1].Rate)
			{
				i++;
			}
			else
			{
				possibleValues.Remove(possibleValues[i + 1]);
			}
		}

		possibleValues = ship.MasterShip.BaseShip().ShipClassTyped switch
		{
			ShipClass.Fletcher => possibleValues.Concat(FletcherCutIns).ToList(),
			ShipClass.Atlanta => possibleValues.Concat(AtlantaCutIns).ToList(),
			_ => possibleValues,
		};

		return possibleValues
			.Where(a => a.CanBeActivatedBy(ship))
			.OrderBy(a => ApiPriority.IndexOf(a.Id))
			.ToList();
	}

	public static List<AntiAirCutIn> PossibleCutIns(IEnumerable<IShipData?> ships) => ships
		.Where(s => s is not null)
		.SelectMany(PossibleCutIns!)
		.DistinctBy(a => a.Id)
		.OrderBy(a => ApiPriority.IndexOf(a.Id))
		.ToList();
}
