using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Core.Types.Mocks;
using KancolleProgress.Models;
using KancolleProgress.Translations;
using Microsoft.Win32;
using ShipTypeGroup = KancolleProgress.Models.ShipTypeGroup;
using ShipTypes = ElectronicObserver.Core.Types.ShipTypes;

namespace KancolleProgress.ViewModels;

public partial class KancolleProgressViewModel : ObservableObject
{
	public IEnumerable<IShipData> UserShips { get; set; } = Enumerable.Empty<IShipData>();
	public IEnumerable<IShipDataMaster> AllShips { get; set; } = Enumerable.Empty<IShipDataMaster>();
	public IEnumerable<IEquipmentData> UserEquipment { get; set; } = Enumerable.Empty<IEquipmentData>();
	public IEnumerable<ShipTypeGroup> TypeGroups { get; set; }

	public ObservableCollection<ColorFilter> ColorFilters { get; }
	public IEnumerable<ShipDataMock>? BaseShips { get; private set; }

	public Visibility ColorFilterVisibility => Display switch
	{
		Display.Ships => Visibility.Visible,
		_ => Visibility.Collapsed,
	};

	public SolidColorBrush ShipColorBrush(ShipDataMock ship) => ColorFilters
		.FirstOrDefault(f => ColorFilter.Compare(f, ship))?.Brush ?? new SolidColorBrush();

	public SolidColorBrush ShipColorBrush(int level) => ColorFilters
		.FirstOrDefault(f => ColorFilter.Compare(f, level))?.Brush ?? new SolidColorBrush();

	public Display Display { get; set; } = Display.Ships;

	public KancolleProgressViewModel()
	{
		List<ColorFilter> colorFilters = new()
		{
			new(this, Comparator.Equal, 185, Colors.DeepPink, KancolleProgressResources.Max),
			new(this, Comparator.GreaterOrEqual, 99, Colors.DeepSkyBlue),
			new(this, Comparator.GreaterOrEqual, 90, Colors.LimeGreen),
			new(this, Comparator.GreaterOrEqual, 80, Colors.Yellow),
			new(this, Comparator.GreaterOrEqual, 1, Colors.LightGray, KancolleProgressResources.Collection),
			new(this, Comparator.Equal, 0, Colors.Red, KancolleProgressResources.Missing),
		};

		TypeGroups = new ObservableCollection<ShipTypeGroup>();
		ColorFilters = new ObservableCollection<ColorFilter>(colorFilters);

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is
				not nameof(UserShips)
				and not nameof(AllShips)
				and not nameof(Display))
			{
				return;
			}

			MakeShipList();
		};
	}

	private void MakeShipList()
	{
		Action action = Display switch
		{
			Display.Ships => MakeKancolleProgress,
			Display.Event => MakeEventCheckList,
			_ => () => { },
		};

		action();
	}

	private void MakeKancolleProgress()
	{
		if (!AllShips.Any()) return;
		if (!UserShips.Any()) return;

		Dictionary<int, ShipDataMock> baseShips = AllShips
			.Where(s => !s.IsAbyssalShip && s.RemodelBeforeShipID == 0)
			.Select(s => new ShipDataMock(s)
			{
				Level = 0,
			})
			.ToDictionary(s => s.ShipID, s => s);

		foreach (IShipData ship in UserShips)
		{
			int baseShipId = ship.MasterShip.BaseShip().ShipID;

			if (baseShips[baseShipId].Level < ship.Level)
			{
				baseShips[baseShipId].Level = ship.Level;
			}
		}

		BaseShips = baseShips.Values;
		OnPropertyChanged(nameof(BaseShips));

		IEnumerable<ShipTypeGroup> groups = baseShips.Values
			.OrderBy(s => s.SortID)
			.GroupBy(s => s.MasterShip.ShipType.ToGroup())
			.OrderBy(s => s.Key)
			.Select(g => new ShipTypeGroup
			(
				Label: g.Key.Display(),
				ClassGroups: g.GroupBy(s => s.MasterShip.ShipClass)
					.Select(g => new ShipClassGroup
					(
						Ships: g.Select(s => new ShipViewModel(s))
					))
			));

		TypeGroups = groups;
	}

	private DaihatsuGroup GetDaihatsuGroup(IShipData ship) => ship switch
	{
		_ when ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.LandingCraft) &&
			   ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.SpecialAmphibiousTank)
			=> DaihatsuGroup.DaihatsuAndTank,

		_ when ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.LandingCraft)
			=> DaihatsuGroup.Daihatsu,

		_ when ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.SpecialAmphibiousTank)
			=> DaihatsuGroup.Tank,

		_ => DaihatsuGroup.None,
	};

	private FcfGroup GetFcfGroup(IShipData ship) => ship switch
	{
		_ when ship.MasterShip.EquippableCategoriesTyped.Contains(EquipmentTypes.CommandFacility)
			=> FcfGroup.Fcf,

		_ => FcfGroup.None,
	};

	private AswGroup GetAswGroup(IShipData ship) => ship switch
	{
		_ when ship.CanNoSonarOpeningAsw
			=> AswGroup.NoSonar,

		_ when ship.ASWBase >= 100 - UserEquipment
				.Where(e => e.MasterEquipment.IsSonar)
				.Max(e => e.MasterEquipment.ASW)
			=> AswGroup.SingleSonar,

		_ => AswGroup.None,
	};

	private void MakeEventCheckList()
	{
		if (!AllShips.Any()) return;
		if (!UserShips.Any()) return;
		if (!UserEquipment.Any()) return;

		IEnumerable<IShipData> allShips = AllShips
			.Where(s => !s.IsAbyssalShip)
			.Select(s => new ShipDataMock(s)
			{
				Level = 0,
			})
			.ToList<IShipData>();

		// bug
		// when a ship fits in a group when it gets to a higher remodel and you own the ship
		// on a lower remodel it will get filtered out
		// example: if you have Makigumo kai, Makigumo kai ni won't show up in the daihatsu group
		var destroyers = UserShips
			.Concat(allShips)
			.Where(s => s.MasterShip.ShipType == ShipTypes.Destroyer)
			.OrderBy(s => s.SortID)
			.GroupBy(s => s.MasterShip.BaseShip().ShipID)
			.Select(g => g
				.OrderByDescending(s => s.Level)
				.ThenByDescending(s => s.SortID))
			.SelectMany(g => g.First().Level switch
			{
				0 => g.Take(1),
				_ => g.TakeWhile(s => s.Level > 0),
			})
			.ToList();

		IEnumerable<IGrouping<T, IShipData>> GroupShips<T>(IEnumerable<IShipData> ships,
			Func<IShipData, T> groupSelector, Func<IGrouping<T, IShipData>, bool> groupFilter)
			where T : Enum
		{
			return ships
				.GroupBy(groupSelector)
				.Where(groupFilter)
				.OrderBy(s => s.Key);
		}

		var daihatsuGroup = GroupShips(destroyers,
			s => GetDaihatsuGroup(s),
			g => g.Key != DaihatsuGroup.None);

		var fcfGroup = GroupShips(destroyers,
			s => GetFcfGroup(s),
			g => g.Key != FcfGroup.None);

		var openingAswGroup = GroupShips(destroyers,
			s => GetAswGroup(s),
			g => g.Key != AswGroup.None);


		List<ShipTypeGroup> DisplayGroups<T>(IEnumerable<IGrouping<T, IShipData>> groups) where T : Enum
		{
			return groups.Select(group => new ShipTypeGroup
				(
					Label: group.Key.Display(),
					ClassGroups: group.GroupBy(s => s.MasterShip.ShipClass)
						.Select(g => new ShipClassGroup
						(
							Ships: g.Select(s => new ShipViewModel(s))
						))
				))
				.ToList();
		}

		TypeGroups = DisplayGroups(daihatsuGroup)
			.Concat(DisplayGroups(fcfGroup))
			.Concat(DisplayGroups(openingAswGroup));
	}

	[RelayCommand]
	private void SetDisplay(Display display)
	{
		Display = display;
	}

	[RelayCommand]
	private void ExportAsCsv()
	{
		List<List<ShipViewModel>> shipLists = TypeGroups
			.Select(g => g.ClassGroups.SelectMany(c => c.Ships).ToList())
			.ToList();

		StringBuilder sb = new();

		for (int i = 0; i < shipLists.Max(s => s.Count); i++)
		{
			foreach (List<ShipViewModel> shipList in shipLists)
			{
				if (shipList.Count > i)
				{
					sb.Append(shipList[i].Name);
				}

				sb.Append(",");

				if (shipList.Count > i)
				{
					sb.Append(shipList[i].Level);
				}

				sb.Append(",");
			}

			sb.AppendLine();
		}

		SaveFileDialog dialog = new()
		{
			FileName = "Ships",
			DefaultExt = ".csv",
			Filter = "CSV UTF-8 (Comma delimited) (*.csv)|*.csv",

		};

		bool? result = dialog.ShowDialog();

		if (result is true)
		{
			File.WriteAllText(dialog.FileName, sb.ToString(), Encoding.UTF8);
		}
	}
}
