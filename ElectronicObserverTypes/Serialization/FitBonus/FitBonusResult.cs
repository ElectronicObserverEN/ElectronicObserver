using System.Collections.Generic;
using System.Linq;

namespace ElectronicObserverTypes.Serialization.FitBonus;

public class FitBonusResult
{
	public FitBonusData FitBonusData { get; set; } = new();

	public List<EquipmentTypes> EquipmentTypes { get; set; } = new();

	public List<EquipmentId> EquipmentIds { get; set; } = new();

	public List<FitBonusValue> FitBonusValues { get; set; } = new();

	public FitBonusValue FinalBonus => FitBonusValues
		.Aggregate(
			new FitBonusValue(),
			(bonusA, bonusB) => bonusA + bonusB,
			bonus => bonus
		);

}
