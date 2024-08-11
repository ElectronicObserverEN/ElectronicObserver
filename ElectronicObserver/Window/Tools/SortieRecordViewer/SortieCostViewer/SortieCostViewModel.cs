using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Database;
using ElectronicObserver.Database.DataMigration;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.KancolleApi.Types.ApiReqMap.Models;
using ElectronicObserver.Services;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Node;
using ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieCostViewer;

public class SortieCostViewModel : ObservableObject
{
	private SortieCostConfigurationViewModel Configuration { get; }

	public DateTime Time { get; }
	public int World { get; }
	public int Map { get; }

	private int SortieFleetId { get; }
	private bool IsCombinedFleet { get; }
	private int NodeSupportFleetId { get; }
	private int BossSupportFleetId { get; }

	private List<IFleetData?> FleetsBeforeSortie { get; }
	private List<IFleetData?>? FleetsAfterSortie { get; }
	private List<IBaseAirCorpsData> AirBases { get; }

	public SortieCostModel SortieFleetSupplyCost { get; }
	public SortieCostModel SortieFleetRepairCost { get; }
	public SortieCostModel NodeSupportSupplyCost { get; }
	public SortieCostModel BossSupportSupplyCost { get; }
	public SortieCostModel TotalSupplyCost { get; }
	public SortieCostModel TotalRepairCost { get; }
	public SortieCostModel TotalAirBaseSortieCost { get; }
	public SortieCostModel TotalAirBaseSupplyCost { get; }

	public SortieCostModel ResourceGain { get; }
	public SortieCostModel SinkingResourceGain { get; }

	public SortieCostModel TotalCost { get; }
	private Dictionary<DamageState, int> DamageStateCounts { get; }
	public List<ConsumableItem> ConsumedItems { get; }

	public int LightDamage => DamageStateCounts[DamageState.Light];
	public int MediumDamage => DamageStateCounts[DamageState.Medium];
	public int HeavyDamage => DamageStateCounts[DamageState.Heavy];

	public int Buckets
	{
		get
		{
			int buckets = 0;

			if (Configuration.IsShouhaBucket)
			{
				buckets += LightDamage;
			}

			if (Configuration.IsChuuhaBucket)
			{
				buckets += MediumDamage;
			}

			if (Configuration.IsTaihaBucket)
			{
				buckets += HeavyDamage;
			}

			return buckets;
		}
	}

	public SortieCostViewModel(ElectronicObserverContext db, ToolService toolService,
		SortieRecordMigrationService sortieRecordMigrationService, SortieRecordViewModel sortie,
		SortieCostConfigurationViewModel configuration)
	{
		Configuration = configuration;

		Configuration.PropertyChanged += Configuration_PropertyChanged;

		Time = sortie.SortieStart.ToUniversalTime();
		World = sortie.World;
		Map = sortie.Map;

		SupplyCostCalculator supplyCostCalculator = new(db, toolService, sortie);
		RepairCostCalculator repairCostCalculator = new(db, toolService, sortie);
		AirBaseCostCalculator airBaseCostCalculator = new(db, toolService, sortie);

		SortieFleetId = sortie.Model.FleetData.FleetId;
		IsCombinedFleet = sortie.Model.FleetData.CombinedFlag > 0;
		NodeSupportFleetId = sortie.Model.FleetData.NodeSupportFleetId;
		BossSupportFleetId = sortie.Model.FleetData.BossSupportFleetId;

		sortieRecordMigrationService.Migrate(db, sortie.Model).Wait();

		FleetsBeforeSortie = sortie.Model.FleetData.MakeFleets();
		FleetsAfterSortie = sortie.Model.FleetAfterSortieData.MakeFleets();
		AirBases = sortie.Model.FleetData.AirBases
			.Select(a => a.MakeAirBase())
			.ToList();

		SortieFleetSupplyCost = supplyCostCalculator.SupplyCost(FleetsBeforeSortie, FleetsAfterSortie, SortieFleetId, IsCombinedFleet);
		SortieFleetRepairCost = repairCostCalculator.RepairCost(FleetsBeforeSortie, FleetsAfterSortie, SortieFleetId, IsCombinedFleet);

		NodeSupportSupplyCost = supplyCostCalculator.NodeSupportSupplyCost(FleetsBeforeSortie, FleetsAfterSortie, NodeSupportFleetId);
		BossSupportSupplyCost = supplyCostCalculator.BossSupportSupplyCost(FleetsBeforeSortie, FleetsAfterSortie, BossSupportFleetId);

		TotalSupplyCost = SortieFleetSupplyCost + NodeSupportSupplyCost + BossSupportSupplyCost;
		TotalRepairCost = SortieFleetRepairCost;

		TotalAirBaseSortieCost = airBaseCostCalculator.AirBaseSortieCost(AirBases);
		TotalAirBaseSupplyCost = airBaseCostCalculator.AirBaseSupplyCost(AirBases);

		ResourceGain = GetResourceGain(db, toolService, sortie);
		SinkingResourceGain = GetSinkingResourceGain(db, toolService, sortie);

		TotalCost = TotalSupplyCost + TotalRepairCost + TotalAirBaseSortieCost + TotalAirBaseSupplyCost;
		TotalCost -= (ResourceGain + SinkingResourceGain);

		DamageStateCounts = repairCostCalculator.DamageStateCounts(FleetsBeforeSortie, FleetsAfterSortie, SortieFleetId, IsCombinedFleet);
		ConsumedItems = GetConsumedItems(sortie.Model);
	}

	private void Configuration_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
	{
		OnPropertyChanged(nameof(Buckets));
	}

	private static SortieCostModel GetSinkingResourceGain(ElectronicObserverContext db, ToolService toolService, SortieRecordViewModel sortie)
	{
		SortieDetailViewModel? sortieDetails = toolService.GenerateSortieDetailViewModel(db, sortie.Model);

		if (sortieDetails is null) return SortieCostModel.Zero;

		SortieCostModel sinkResourceGain = SortieCostModel.Zero;

		foreach (BattleNode battle in sortieDetails.Nodes.OfType<BattleNode>())
		{
			sinkResourceGain += battle.LastBattle.FleetsAfterBattle
				.SortieShips()
				.OfType<IShipData>()
				.Where(s => s.HPCurrent <= 0)
				.Select(s => new SortieCostModel
				{
					Fuel = s.Fuel,
					Ammo = s.Ammo,
				}).Sum();
		}

		return sinkResourceGain;
	}

	private static SortieCostModel GetResourceGain(ElectronicObserverContext db, ToolService toolService,
		SortieRecordViewModel sortie)
	{
		SortieDetailViewModel? sortieDetails = toolService.GenerateSortieDetailViewModel(db, sortie.Model);

		if (sortieDetails is null) return SortieCostModel.Zero;

		return sortieDetails.Nodes
			.Where(n => n.Items is not null)
			.SelectMany(n => n.Items!)
			.OfType<ApiItemget>()
			.Select(i => GetItemId(i) switch
			{
				UseItemId.Fuel => new SortieCostModel { Fuel = i.ApiGetcount },
				UseItemId.Ammo => new SortieCostModel { Ammo = i.ApiGetcount },
				UseItemId.Steel => new SortieCostModel { Steel = i.ApiGetcount },
				UseItemId.Bauxite => new SortieCostModel { Bauxite = i.ApiGetcount },

				// todo: other items
				_ => SortieCostModel.Zero,
			})
			.Sum();
	}

	private static UseItemId GetItemId(ApiItemget item) => item.ApiUsemst switch
	{
		4 => item.ApiId switch
		{
			1 => UseItemId.Fuel,
			2 => UseItemId.Ammo,
			3 => UseItemId.Steel,
			4 => UseItemId.Bauxite,
			5 => UseItemId.InstantConstruction,
			6 => UseItemId.InstantRepair,
			7 => UseItemId.DevelopmentMaterial,
			8 => UseItemId.ImproveMaterial,
			_ => UseItemId.Unknown,
		},

		_ => (UseItemId)item.ApiUsemst,
	};

	private static List<ConsumableItem> GetConsumedItems(SortieRecord sortie)
	{
		// todo: calculate
		if (sortie.FleetAfterSortieData is null) return [];

		List<ConsumableItem> consumedItems = [];

		List<ConsumableItem> itemsBefore = GetConsumableItems(sortie.FleetData);
		List<ConsumableItem> itemsAfter = GetConsumableItems(sortie.FleetAfterSortieData);

		foreach ((ConsumableItem before, ConsumableItem after) in itemsBefore.Zip(itemsAfter))
		{
			Debug.Assert(before.Id == after.Id);

			if (before.Count == after.Count) continue;

			consumedItems.Add(new(before.Equipment, before.Count - after.Count));
		}

		return consumedItems;
	}

	private static List<ConsumableItem> GetConsumableItems(SortieFleetData fleetData)
	{
		List<EquipmentId> trackedEquipmentIds =
		[
			EquipmentId.DamageControl_EmergencyRepairPersonnel,
			EquipmentId.DamageControl_EmergencyRepairGoddess,
		];

		return fleetData
			.MakeFleets()
			.OfType<IFleetData>()
			.SelectMany(f => f.MembersInstance)
			.OfType<IShipData>()
			.SelectMany(s => s.AllSlotInstance)
			.OfType<IEquipmentData>()
			.Where(e => trackedEquipmentIds.Contains(e.EquipmentId))
			.GroupBy(e => e.EquipmentId)
			.Select(g => new ConsumableItem(g.First(), g.Count()))
			.ToList();
	}
}
