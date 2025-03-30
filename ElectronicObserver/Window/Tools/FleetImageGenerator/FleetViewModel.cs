using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;

namespace ElectronicObserver.Window.Tools.FleetImageGenerator;

public class FleetViewModel : ObservableObject
{
	public IFleetData? Model { get; private set; }

	public int Id { get; private set; }
	public bool Visible => Model is not null;

	public string Name { get; private set; } = "";

	public int AirPower { get; private set; }

	public List<LosValue> LosValues { get; private set; } = [];

	public ObservableCollection<ShipViewModel> Ships { get; private set; } = [];
	public int TpValueA { get; private set; }
	public int TpValueS { get; private set; }

	public int TankTpValueA { get; private set; }
	public int TankTpValueS { get; private set; }

	public FleetViewModel Initialize(IFleetData? fleet, int fleetId, ImageType imageType, TankTpGauge tankTpGauge)
	{
		Model = fleet;

		Id = fleetId + 1;

		if (fleet is null)
		{
			return this;
		}

		Name = fleet.Name;
		AirPower = Calculator.GetAirSuperiority(fleet);

		LosValues = Enumerable.Range(1, 4)
			.Select(w => new LosValue(w, Math.Round(Calculator.GetSearchingAbility_New33(fleet, w), 2, MidpointRounding.ToNegativeInfinity)))
			.ToList();

		TpValueS = Calculator.GetTpDamage(fleet);
		TpValueA = (int)(TpValueS * 0.7);

		TankTpValueS = tankTpGauge.GetTp(fleet);
		TankTpValueA = (int)(TankTpValueS * 0.7);

		Ships = FilterStrikingForce(fleet.MembersInstance)
			.Select(s => imageType switch
			{
				ImageType.Banner => (ShipViewModel)new BannerShipViewModel().Initialize(s),
				ImageType.CutIn => new CutInShipViewModel().Initialize(s),
				ImageType.Card => new CardShipViewModel().Initialize(s),

				_ => throw new NotImplementedException(),
			})
			.ToObservableCollection();

		return this;
	}

	private static IEnumerable<IShipData?> FilterStrikingForce(ReadOnlyCollection<IShipData?> ships)
	{
		if (ships.Count <= 6) return ships;
		if (ships.Last() is not null) return ships;

		return ships.Take(6);
	}
}
