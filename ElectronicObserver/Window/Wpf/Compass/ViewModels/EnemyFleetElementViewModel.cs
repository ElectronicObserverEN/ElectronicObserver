using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Data;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Wpf.Compass.ViewModels;

public class EnemyFleetElementViewModel : ObservableObject
{
	private KCDatabase Db { get; }
	public FormCompassTranslationViewModel FormCompass { get; }
	public EnemyFleetRecord.EnemyFleetElement EnemyFleetCandidate { get; set; } = default!;

	private IEnumerable<MasterShipViewModel> FleetMember => EnemyFleetCandidate.FleetMember
		.Select(i => i switch
		{
			< 1 => new MasterShipViewModel { IsUnknown = i is 0 },
			_ => new MasterShipViewModel { Ship = Db.MasterShips[i] }
		});

	public IEnumerable<MasterShipViewModel> MainFleetMember => FleetMember.Take(6);
	public IEnumerable<MasterShipViewModel> EscortFleetMember => FleetMember.Skip(6).Take(6);

	public bool HasEscortFleet => EscortFleetMember.Any(ship => ship.ShipId > 0);

	public List<int> Formations { get; init; } = new();

	public string FormationString => string.Join('/', Formations.Select(Constants.GetFormationShort));

	public EquipmentIconType AirIcon => EquipmentIconType.CarrierBasedFighter;
	public string Air => Calculator.GetAirSuperiority(EnemyFleetCandidate.FleetMember).ToString();
	public string? AirToolTip => GetAirSuperiorityString(Calculator.GetAirSuperiority(EnemyFleetCandidate.FleetMember));

	public bool IsPreviewed => IsPreviewedFleet();

	public EnemyFleetElementViewModel()
	{
		Db = KCDatabase.Instance;
		FormCompass = Ioc.Default.GetService<FormCompassTranslationViewModel>()!;
	}

	private string? GetAirSuperiorityString(int air)
	{
		if (air > 0)
		{
			return string.Format(FormCompass.AirValues,
				(int)(air * 3.0),
				(int)Math.Ceiling(air * 1.5),
				(int)(air / 1.5 + 1),
				(int)(air / 3.0 + 1));
		}
		return null;
	}

	private bool IsPreviewedFleet()
	{
		if (Db.Battle.Compass is null) return false;
		List<EDeckInfo> fleets = Db.Battle.Compass.EnemyFleetPreview;

		if (!Db.Battle.Compass.EnemyFleetPreview.Any()) return false;

		if (!CompareFleets(fleets.First(), EnemyFleetCandidate.FleetMember.Take(6))) return false;

		if (fleets.Count is 2 && !CompareFleets(fleets[1], EnemyFleetCandidate.FleetMember.Skip(6).Take(6))) return false;

		return true;
	}

	private bool CompareFleets(EDeckInfo preview, IEnumerable<int> savedFleet)
	{
		int shipCountSaved = savedFleet.Count(id => id > -1);

		bool countMatches = preview.ApiKind switch
		{
			0 => shipCountSaved == preview.ApiShipIds.Count,
			1 => shipCountSaved is 4,
			2 => shipCountSaved >= 5,
			_ => false
		};

		if (!countMatches) return false;

		foreach ((ShipId shipPreview, int shipSaved) in preview.ApiShipIds.Zip(savedFleet.Take(preview.ApiShipIds.Count)))
		{
			if ((int)shipPreview != shipSaved) return false;
		}

		return true;
	}
}
