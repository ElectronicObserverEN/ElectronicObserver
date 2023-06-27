using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment.EquipmentUpgrade;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment;

public class EquipmentDataViewModel : ObservableObject
{
	public IEquipmentDataMaster Equipment { get; private set; }

	public string EquipmentIdToolTip => string.Format("Type: [ {0} ]", string.Join(", ", Equipment.EquipmentType));

	public string EquipmentNameToolTip => Properties.Window.Dialog.DialogAlbumMasterEquipment.RightClickToCopy;

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
		sb.AppendLine($"{Properties.Window.Dialog.DialogAlbumMasterEquipment.Equippable}:");

		int eqCategory = (int)eq.CategoryType;

		var specialShips = new Dictionary<ShipTypes, List<string>>();

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

				if (specialShips.ContainsKey(shiptype.Type))
				{
					sb.Append(" (").Append(string.Join(", ", specialShips[shiptype.Type]))
						.Append($"{Properties.Window.Dialog.DialogAlbumMasterEquipment.Excluding})");
				}

				sb.AppendLine();
			}
			else
			{
				if (specialShips.ContainsKey(shiptype.Type))
				{
					sb.Append("○ ").AppendLine(string.Join(", ", specialShips[shiptype.Type]));
				}
			}
		}

		if (eq.EquippableShipsAtExpansion.Any())
			sb.Append($"[{Properties.Window.Dialog.DialogAlbumMasterEquipment.ExpansionSlot}] ").AppendLine(
				string.Join(", ", eq.EquippableShipsAtExpansion.Select(id => db.MasterShips[id].NameWithClass)));

		return sb.ToString();
	}
}
