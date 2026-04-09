using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.KancolleApi.Types.Models;
using Riok.Mapperly.Abstractions;

namespace ElectronicObserver.Window.Dialog;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public static partial class Mappers
{
	/// <summary>
	/// The mapping here is only to make local api loader work, some of the data might not make any sense.
	/// </summary>
	[MapProperty(nameof(SortieShip.DropId), nameof(ApiShip.ApiId), Use = nameof(NullableInt))]
	[MapProperty(nameof(SortieShip.Id), nameof(ApiShip.ApiShipId))]
	[MapProperty(nameof(SortieShip.Level), nameof(ApiShip.ApiLv))]
	[MapProperty(nameof(SortieShip.Hp), nameof(ApiShip.ApiNowhp), Use = nameof(NullableInt))]
	[MapProperty(nameof(SortieShip.Hp), nameof(ApiShip.ApiMaxhp), Use = nameof(NullableInt))]
	[MapProperty(nameof(SortieShip.Fuel), nameof(ApiShip.ApiFuel))]
	[MapProperty(nameof(SortieShip.Ammo), nameof(ApiShip.ApiBull))]
	[MapProperty(nameof(SortieShip.Condition), nameof(ApiShip.ApiCond))]
	[MapProperty(nameof(SortieShip.Kyouka), nameof(ApiShip.ApiKyouka))]
	[MapProperty(nameof(SortieShip.Aircraft), nameof(ApiShip.ApiOnslot), Use = nameof(NullableList))]
	[MapProperty(nameof(SortieShip.Range), nameof(ApiShip.ApiLeng))]
	[MapProperty(nameof(SortieShip.Speed), nameof(ApiShip.ApiSoku))]
	[MapProperty(nameof(SortieShip.Firepower), nameof(ApiShip.ApiKaryoku), Use = nameof(ParameterMap))]
	[MapProperty(nameof(SortieShip.Torpedo), nameof(ApiShip.ApiRaisou), Use = nameof(ParameterMap))]
	[MapProperty(nameof(SortieShip.Aa), nameof(ApiShip.ApiTaiku), Use = nameof(ParameterMap))]
	[MapProperty(nameof(SortieShip.Armor), nameof(ApiShip.ApiSoukou), Use = nameof(ParameterMap))]
	[MapProperty(nameof(SortieShip.Evasion), nameof(ApiShip.ApiKaihi), Use = nameof(ParameterMap))]
	[MapProperty(nameof(SortieShip.Asw), nameof(ApiShip.ApiTaisen), Use = nameof(ParameterMap))]
	[MapProperty(nameof(SortieShip.Search), nameof(ApiShip.ApiSakuteki), Use = nameof(ParameterMap))]
	[MapProperty(nameof(SortieShip.Luck), nameof(ApiShip.ApiLucky), Use = nameof(ParameterMap))]
	[MapProperty(nameof(SortieShip.SpecialEffectItems), nameof(ApiShip.ApiSpEffectItems), Use = nameof(SpecialEffectItems))]
	[MapValue(nameof(ApiShip.ApiExp), Use = nameof(ApiExp))]
	[MapValue(nameof(ApiShip.ApiBacks), 0)]
	[MapValue(nameof(ApiShip.ApiLocked), 0)]
	[MapValue(nameof(ApiShip.ApiLockedEquip), 0)]
	[MapValue(nameof(ApiShip.ApiNdockItem), Use = nameof(NdockItem))]
	[MapValue(nameof(ApiShip.ApiNdockTime), 0)]
	[MapValue(nameof(ApiShip.ApiSallyArea), 0)]
	[MapValue(nameof(ApiShip.ApiSortno), 0)]
	[MapValue(nameof(ApiShip.ApiSrate), 0)]
	public static partial ApiShip ToApiShip(this SortieShip ship, List<int> apiSlot, int ApiSlotnum, int apiSlotEx);

	private static int NullableInt(int? value) => value ?? 0;

	private static List<int> NullableList(List<int>? value) => value ?? [];

	private static List<int> ParameterMap(int? value) => [value ?? 0, value ?? 0];

	private static List<int> ApiExp() => [0, 0];

	private static List<int> NdockItem() => [0, 0];

	private static List<ApiSpEffectItem>? SpecialEffectItems(List<SpecialEffectItem>? items) => items switch
	{
		null => null,

		_ => items.Select(item => new ApiSpEffectItem
		{
			ApiKind = item.ApiKind,
			ApiHoug = item.Firepower,
			ApiRaig = item.Torpedo,
			ApiSouk = item.Armor,
			ApiKaih = item.Evasion,
		}).ToList(),
	};

	[MapProperty(nameof(SortieEquipment.Id), nameof(ApiSlotItem.ApiSlotitemId))]
	[MapProperty(nameof(SortieEquipment.Level), nameof(ApiSlotItem.ApiLevel))]
	[MapProperty(nameof(SortieEquipment.AircraftLevel), nameof(ApiSlotItem.ApiAlv))]
	[MapValue(nameof(ApiSlotItem.ApiLocked), 1)]
	public static partial ApiSlotItem ToApiSlotItem(this SortieEquipment eq, int apiId);
}
