using System.Text.Json.Serialization;

namespace ElectronicObserverTypes.Serialization.FitBonus
{
    public class FitBonusValue
    {

        [JsonPropertyName("houg")] public int? Firepower { get; set; }

        [JsonPropertyName("raig")] public int? Torpedo { get; set; }

        [JsonPropertyName("tyku")] public int? AntiAir { get; set; }

        [JsonPropertyName("souk")] public int? Armor { get; set; }

        [JsonPropertyName("kaih")] public int? Evasion { get; set; }

        [JsonPropertyName("tais")] public int? ASW { get; set; }

        [JsonPropertyName("saku")] public int? LOS { get; set; }

        /// <summary>
        /// Visible acc fit actually doesn't work according to some studies
        /// </summary>
        [JsonPropertyName("houm")] public int? Accuracy { get; set; }

        [JsonPropertyName("leng")] public int? Range { get; set; }

		public static FitBonusValue operator *(FitBonusValue a, int b) => new FitBonusValue()
		{
			Firepower = a.Firepower ?? 0 * b,
			Torpedo = a.Torpedo ?? 0 * b,
			AntiAir = a.AntiAir ?? 0 * b,
			Armor = a.Armor ?? 0 * b,
			Evasion = a.Evasion ?? 0 * b,
			ASW = a.ASW ?? 0 * b,
			LOS = a.LOS ?? 0 * b,
			Accuracy = a.Accuracy ?? 0 * b,
			Range = a.Range ?? 0 * b
		};

		public static FitBonusValue operator +(FitBonusValue a, FitBonusValue b) => new FitBonusValue()
		{
			Firepower = (a.Firepower ?? 0) + (b.Firepower ?? 0),
			Torpedo = (a.Torpedo ?? 0) + (b.Torpedo ?? 0),
			AntiAir = (a.AntiAir ?? 0) + (b.AntiAir ?? 0),
			Armor = (a.Armor ?? 0) + (b.Armor ?? 0),
			Evasion = (a.Evasion ?? 0) + (b.Evasion ?? 0),
			ASW = (a.ASW ?? 0) + (b.ASW ?? 0),
			LOS = (a.LOS ?? 0) + (b.LOS ?? 0),
			Accuracy = (a.Accuracy ?? 0) + (b.Accuracy ?? 0),
			Range = (a.Range ?? 0) + (b.Range ?? 0)
		};
	}
}
