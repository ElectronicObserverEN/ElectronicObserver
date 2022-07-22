using System.Text.Json.Serialization;

namespace ElectronicObserverTypes.Serialization.FitBonus;

public record FitBonusValue
{

	[JsonPropertyName("houg")] public int Firepower { get; set; }

	[JsonPropertyName("raig")] public int Torpedo { get; set; }

	[JsonPropertyName("tyku")] public int AntiAir { get; set; }

	[JsonPropertyName("souk")] public int Armor { get; set; }

	[JsonPropertyName("kaih")] public int Evasion { get; set; }

	[JsonPropertyName("tais")] public int ASW { get; set; }

	[JsonPropertyName("saku")] public int LOS { get; set; }

	/// <summary>
	/// Visible acc fit actually doesn't work according to some studies
	/// </summary>
	[JsonPropertyName("houm")] public int Accuracy { get; set; }

	[JsonPropertyName("leng")] public int Range { get; set; }

	public static FitBonusValue operator *(FitBonusValue a, int b) => new FitBonusValue()
	{
		Firepower = a.Firepower * b,
		Torpedo = a.Torpedo * b,
		AntiAir = a.AntiAir * b,
		Armor = a.Armor * b,
		Evasion = a.Evasion * b,
		ASW = a.ASW * b,
		LOS = a.LOS * b,
		Accuracy = a.Accuracy * b,
		Range = a.Range * b
	};

	public static FitBonusValue operator +(FitBonusValue a, FitBonusValue b) => new FitBonusValue()
	{
		Firepower = a.Firepower + b.Firepower,
		Torpedo = a.Torpedo + b.Torpedo,
		AntiAir = a.AntiAir + b.AntiAir,
		Armor = a.Armor + b.Armor,
		Evasion = a.Evasion + b.Evasion,
		ASW = a.ASW + b.ASW,
		LOS = a.LOS + b.LOS,
		Accuracy = a.Accuracy + b.Accuracy,
		Range = a.Range + b.Range
	};
}
