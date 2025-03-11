using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.FleetImageGenerator;

public class FleetViewModel : ObservableObject
{
	private IKCDatabase Db { get; }

	public IFleetData? Model { get; set; }

	public int Id { get; set; }
	public bool Visible => Model is not null;

	public string Name { get; set; } = "";

	public int AirPower { get; set; }

	public List<LosValue> LosValues { get; set; } = new();

	public ObservableCollection<ShipViewModel> Ships { get; set; } = new();
	public int TPValueA { get; set; }
	public int TPValueS { get; set; }
	public FleetViewModel()
	{
		Db = Ioc.Default.GetRequiredService<IKCDatabase>();
	}

	public FleetViewModel Initialize(IFleetData? fleet, int fleetId, ImageType imageType)
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
		TPValueS = Calculator.GetTpDamage(fleet);
		TPValueA = (int)(Calculator.GetTpDamage(fleet) * 0.7);
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

	private IEnumerable<IShipData?> FilterStrikingForce(IEnumerable<IShipData?> ships)
	{
		if (ships.Count() <= 6) return ships;
		if (ships.Last() is not null) return ships;

		return ships.Take(6);
	}
}
