using System;

namespace ElectronicObserver.Core.Types.Evasion;

// https://wikiwiki.jp/kancolle/%E5%91%BD%E4%B8%AD%E3%81%A8%E5%9B%9E%E9%81%BF%E3%81%AB%E3%81%A4%E3%81%84%E3%81%A6
public abstract class EvasionBase
{
	protected IShipData Ship { get; }
	private IFleetData? Fleet { get; }
	protected BattleDataMock Battle { get; }

	protected EvasionBase(IShipData ship, IFleetData? fleet = null, BattleDataMock? battle = null)
	{
		Ship = ship;
		Fleet = fleet;
		Battle = battle ?? new();
	}

	public double PostcapValue =>
		Math.Floor(CappedBaseValue + PostcapBonus + EquipmentUpgradeBonus + VanguardBonus - FuelPenalty)
		* PostcapModifier;

	private double FuelPenalty => Ship.FuelRate switch
	{
		>= 0.75 => 0,
		_ => 75 - Ship.FuelRate * 100
	};

	public double CappedBaseValue => FlooredBaseValue switch
	{
		< 40 => FlooredBaseValue,
		< 65 => Math.Floor(40 + 3 * Math.Sqrt(FlooredBaseValue - 40)),
		_ => Math.Floor(55 + 2 * Math.Sqrt(FlooredBaseValue - 65))
	};

	private double FlooredBaseValue => Math.Floor(BaseValue);

	public double BaseValue => FormationModifier * Math.Floor(ShipEvasion);

	private double ShipEvasion => Ship.EvasionTotal + Math.Sqrt(2 * Ship.LuckTotal);

	protected abstract double FormationModifier { get; }
	protected abstract double VanguardBonus { get; }
	protected abstract double PostcapBonus { get; }
	protected abstract double EquipmentUpgradeBonus { get; }
	protected abstract double PostcapModifier { get; }
}
