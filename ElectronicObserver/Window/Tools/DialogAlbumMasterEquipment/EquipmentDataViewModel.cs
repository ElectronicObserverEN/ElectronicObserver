using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment.EquipmentUpgrade;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment;

public class EquipmentDataViewModel : ObservableObject
{
	public IEquipmentDataMaster Equipment { get; private set; }

	public string EquipmentIdToolTip => string.Format("Type: [ {0} ]", string.Join(", ", Equipment.EquipmentType));

	public string EquipmentNameToolTip => AlbumMasterEquipmentResources.RightClickToCopy;

	public string EquipmentTypeToolTip => GetEquippableShips(Equipment);

	public bool IsAircraft => Equipment.IsAircraft;
	public bool IsInterceptor => Equipment.CategoryType is EquipmentTypes.Interceptor;
	public bool IsNotInterceptor => !IsInterceptor;

	public string Speed => EncycloRes.None;

	public IEnumerable<IShipDataMaster> DefaultSlotShips => KCDatabase.Instance.MasterShips.Values
		.Cast<IShipDataMaster>()
		.Where(s => s.DefaultSlot?.Contains(Equipment.ID) ?? false);

	public IconContent RarityIcon => Constants.GetEquipmentRarityID(Equipment.Rarity) switch
	{
		0 => IconContent.RarityRed,
		1 => IconContent.RarityBlueC,
		2 => IconContent.RarityBlueB,
		3 => IconContent.RarityBlueA,
		4 => IconContent.RaritySilver,
		5 => IconContent.RarityGold,
		6 => IconContent.RarityHoloB,
		7 => IconContent.RarityHoloA,
		8 => IconContent.RarityCherry,

		_ => IconContent.RarityRed
	};

	public string AircraftCostToolTip =>
		$"{EncycloRes.AircraftCostHint}：{BaseAircraftCost * Equipment.AircraftCost}";

	private int BaseAircraftCost => Equipment.IsCombatAircraft switch
	{
		true => 18,
		_ => 4
	};

	public AlbumMasterEquipmentUpgradeViewModel UpgradeViewModel { get; private set; }

	public EquipmentDataViewModel(IEquipmentDataMaster equipment)
	{
		Equipment = equipment;

		UpgradeViewModel = new(equipment);
	}

	public void ChangeEquipment(IEquipmentDataMaster equipment)
	{
		Equipment = equipment;

		UpgradeViewModel.UnsubscribeFromApis();
		UpgradeViewModel = new(equipment);
	}

	private string GetEquippableShips(IEquipmentDataMaster eq)
	{
		KCDatabase db = KCDatabase.Instance;

		StringBuilder sb = new();
		sb.AppendLine($"{AlbumMasterEquipmentResources.Equippable}:");

		int eqCategory = (int)eq.CategoryType;

		Dictionary<ShipTypes, List<string>> specialShips = new();

		foreach (ShipDataMaster ship in db.MasterShips.Values.Where(s => s.SpecialEquippableCategories != null))
		{
			bool usual = ship.ShipTypeInstance.EquippableCategories.Contains(eqCategory);
			bool special = ship.SpecialEquippableCategories.Contains(eqCategory);

			if (usual != special)
			{
				if (specialShips.ContainsKey(ship.ShipType))
					specialShips[ship.ShipType].Add(ship.NameWithClass);
				else
					specialShips.Add(ship.ShipType, new List<string>(new[] { ship.NameWithClass }));
			}
		}

		foreach (ShipType shiptype in db.ShipTypes.Values)
		{
			if (shiptype.EquippableCategories.Contains(eqCategory))
			{
				sb.Append(shiptype.NameEN);

				if (specialShips.TryGetValue(shiptype.Type, out List<string>? ship))
				{
					sb.Append(" (").Append(string.Join(", ", ship))
						.Append($"{AlbumMasterEquipmentResources.Excluding})");
				}

				sb.AppendLine();
			}
			else
			{
				if (specialShips.TryGetValue(shiptype.Type, out List<string>? ship))
				{
					sb.Append("○ ").AppendLine(string.Join(", ", ship));
				}
			}
		}

		string? ships = eq.EquippableShipsAtExpansion.Any() switch
		{
			true => string.Join(", ", eq.EquippableShipsAtExpansion
				.Select(id => db.MasterShips[(int)id]?.NameWithClass ?? $"{ConstantsRes.Unknown}({id})")),
			_ => null,
		};

		string? shipsTypes = eq.EquippableShipTypesAtExpansion.Any() switch
		{
			true => string.Join(", ", eq.EquippableShipTypesAtExpansion
				.Select(id => id switch
				{
					ShipTypes.All => id.Display(),
					_ => db.ShipTypes[(int)id].NameEN,
				})),
			_ => null,
		};

		string? shipsClasses = eq.EquippableShipClassesAtExpansion.Any() switch
		{
			true => string.Join(", ", eq.EquippableShipClassesAtExpansion
				.Select(c => Constants.GetShipClass(c))),
			_ => null,
		};

		if (ships is not null || shipsTypes is not null || shipsClasses is not null)
		{
			sb.AppendLine($"[{AlbumMasterEquipmentResources.ExpansionSlot}]");
		}

		if (ships is not null)
		{
			sb.AppendLine(ships);
		}

		if (shipsTypes is not null)
		{
			sb.AppendLine(shipsTypes);
		}

		if (shipsClasses is not null)
		{
			sb.AppendLine(shipsClasses);
		}

		return sb.ToString();
	}
}
