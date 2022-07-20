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

    }
}
