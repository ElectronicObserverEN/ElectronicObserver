using System;
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

	public bool HasBonus()
	{
		if (Firepower > 0) return true;
		if (Torpedo > 0) return true;
		if (AntiAir > 0) return true;
		if (Armor > 0) return true;
		if (Evasion > 0) return true;
		if (ASW > 0) return true;
		if (LOS > 0) return true;
		if (Accuracy > 0) return true;
		if (Range > 0) return true;

		return false;
	}

	public virtual bool Equals(FitBonusValue? other)
	{
		if (other is null) return false;
		if (Firepower != other.Firepower) return false;
		if (Torpedo != other.Torpedo) return false;
		if (AntiAir != other.AntiAir) return false;
		if (Armor != other.Armor) return false;
		if (Evasion != other.Evasion) return false;
		if (ASW != other.ASW) return false;
		if (LOS != other.LOS) return false;
		if (Accuracy != other.Accuracy) return false;
		if (Range != other.Range) return false;

		return true;
	}

	public override int GetHashCode()
	{
		var hashCode = new HashCode();
		hashCode.Add(Firepower);
		hashCode.Add(Torpedo);
		hashCode.Add(AntiAir);
		hashCode.Add(Armor);
		hashCode.Add(Evasion);
		hashCode.Add(ASW);
		hashCode.Add(LOS);
		hashCode.Add(Accuracy);
		hashCode.Add(Range);
		return hashCode.ToHashCode();
	}
}
