using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Common;
using ElectronicObserver.Common.Datagrid;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Core.Types.Mocks;
using ElectronicObserver.Data;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Wpf;

namespace ElectronicObserver.Window.Tools.ExpChecker;

public class ExpCheckerViewModel : WindowViewModelBase
{
	public ExpCheckerTranslationViewModel ExpChecker { get; }

	public DataGridViewModel<DataGridItem> DataGridViewModel { get; set; }

	private string DefaultTitle => ExpChecker.Title;
	public string Title { get; set; }

	public IShipData? SelectedShip { get; set; }
	public ObservableCollection<IShipData> Ships { get; set; } = new();

	public bool SearchInFleet { get; set; }
	public int SortieExp { get; set; }
	public bool ShowAllASWEquipments { get; set; }
	public int ASWModernization { get; set; }
	public bool ShowAllLevel { get; set; }

	public int LuckModernization { get; set; }
	public int MaxLuckModernization { get; set; }

	public string? WarningText { get; set; }

	public string? GroupExpText { get; set; }

	public ObservableCollection<DataGridItem> DataGridItems { get; set; } = new();

	public ExpCheckerViewModel()
	{
		ExpChecker = Ioc.Default.GetRequiredService<ExpCheckerTranslationViewModel>();

		Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		ConfigurationChanged();

		DataGridViewModel = new(DataGridItems);

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(SearchInFleet)) return;

			UpdateShipList();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(SortieExp)) return;

			Configuration.Config.Control.ExpCheckerExpUnit = SortieExp;
			UpdateLevelView();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(SelectedShip)) return;
			if (SelectedShip is null) return;

			ASWModernization = SelectedShip.ASWModernized;
			LuckModernization = SelectedShip.LuckModernized;
			MaxLuckModernization = SelectedShip.MasterShip.LuckMax - SelectedShip.MasterShip.LuckMin;

			UpdateLevelView();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ShowAllLevel)) return;

			UpdateLevelView();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ShowAllASWEquipments)) return;

			UpdateLevelView();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ASWModernization)) return;

			UpdateLevelView();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(LuckModernization)) return;

			UpdateLevelView();
		};

		UpdateShipList();
	}

	public ExpCheckerViewModel(int shipId) : this()
	{
		SelectedShip = KCDatabase.Instance.Ships[shipId];
	}

	public override void Loaded()
	{
		base.Loaded();

		SortieExp = Configuration.Config.Control.ExpCheckerExpUnit;
	}

	private void ConfigurationChanged()
	{
		Configuration.ConfigurationData c = Configuration.Config;
	}

	private void UpdateShipList()
	{
		Ships = SearchInFleet switch
		{
			true => KCDatabase.Instance.Ships.Values.Where(s => s.Fleet != -1)
				.OrderBy(s => s.FleetWithIndex)
				.Cast<IShipData>()
				.ToObservableCollection(),

			_ => KCDatabase.Instance.Ships.Values.OrderBy(s => s.MasterShip.ShipType)
				.ThenByDescending(s => s.Level)
				.Cast<IShipData>()
				.ToObservableCollection()
		};
	}

	private void UpdateLevelView()
	{
		var selectedShip = SelectedShip;

		if (selectedShip is null)
		{
			System.Media.SystemSounds.Asterisk.Play();
			return;
		}

		DataGridViewModel.ItemsSource.Clear();


		// 空母系は面倒なので省略
		int openingASWborder = selectedShip.MasterShip.ShipType == ShipTypes.Escort ? 60 : 100;

		var ASWEquipmentPairs = new Dictionary<int, IEnumerable<string>>();
		if (ShowAllASWEquipments)
		{
			ASWEquipmentData[] had = KCDatabase.Instance.Equipments.Values
				.Where(eq => eq.MasterEquipment.CategoryType is EquipmentTypes.Sonar or EquipmentTypes.DepthCharge)
				.GroupBy(eq => eq.EquipmentID)
				.Select(g => new ASWEquipmentData
				{
					ID = g.Key,
					ASW = g.First().MasterEquipment.ASW,
					Name = g.First().MasterEquipment.NameEN,
					IsSonar = g.First().MasterEquipment.IsSonar,
					Count = g.Count(),
				})
				.Concat(new[]
				{
					new ASWEquipmentData
					{
						ID = -1,
						ASW = 0,
						Name = "",
						Count = 99,
						IsSonar = false,
					},
				})
				.OrderByDescending(a => a.ASW)
				.ToArray();

			int[] stack = Enumerable.Repeat(0, selectedShip.SlotSize).ToArray();

			Dictionary<int, List<ASWEquipmentData[]>> pair = new();

			if (had.Length > 0 && stack.Length > 0)
			{
				while (stack[0] != -1)
				{
					ASWEquipmentData[] convert = stack.Select(i => had[i]).ToArray();

					if (convert.Any(c => c.IsSonar) && stack.GroupBy(s => s).All(s => had[s.Key].Count >= s.Count()))
					{
						int aswsum = convert.Sum(c => c.ASW);

						if (!pair.ContainsKey(aswsum))
						{
							pair.Add(aswsum, new List<ASWEquipmentData[]> { convert });
						}
						else
						{
							int equipmentCount = convert.Count(e => e.ID is not -1);

							// count how many entries for the current aswsum use the same number of equipment
							int? entryCount = pair[aswsum]
								.GroupBy(a => a.Count(e => e.ID is not -1))
								.FirstOrDefault(g => g.Key == equipmentCount)
								?.Count();

							// don't display more than 6 different combinations for a given equipment count
							if (entryCount is null or < 6)
							{
								pair[aswsum].Add(convert);
							}
						}
					}

					for (int p = stack.Length - 1; p >= 0; p--)
					{
						stack[p]++;
						if (stack[p] < had.Length)
							break;
						stack[p] = -1;
					}
					for (int p = 1; p < stack.Length; p++)
					{
						if (stack[p] == -1)
							stack[p] = stack[p - 1];
					}
				}
			}

			foreach (var x in pair)
			{
				// 要するに下のようなフォーマットにする
				ASWEquipmentPairs.Add(openingASWborder - x.Key,
					x.Value.OrderBy(a => a.Count(b => b.ID > 0))
						.Select(a => $"[{string.Join(", ", a.Where(b => b.ID > 0).GroupBy(b => b.ID).Select(b => b.Count() == 1 ? b.First().Name : $"{b.First().Name}x{b.Count()}"))}]"));
			}
		}
		else
		{
			string hfdf = KCDatabase.Instance.MasterEquipments[(int)EquipmentId.Sonar_HFDF_Type144147ASDIC].NameEN;
			string aswTorpedo = KCDatabase.Instance.MasterEquipments[(int)EquipmentId.DepthCharge_LightweightASWTorpedo_InitialTestModel].NameEN;
			string rur = KCDatabase.Instance.MasterEquipments[(int)EquipmentId.DepthCharge_RUR4AWeaponAlphaKai].NameEN;
			string aswRocket = KCDatabase.Instance.MasterEquipments[(int)EquipmentId.DepthCharge_Prototype15cm9tubeASWRocketLauncher].NameEN;
			string type4 = KCDatabase.Instance.MasterEquipments[(int)EquipmentId.Sonar_Type4PassiveSONAR].NameEN;
			string type3dc = KCDatabase.Instance.MasterEquipments[(int)EquipmentId.DepthCharge_Type3DepthChargeProjector].NameEN;
			string type3 = KCDatabase.Instance.MasterEquipments[(int)EquipmentId.Sonar_Type3ActiveSONAR].NameEN;
			string type2dc = KCDatabase.Instance.MasterEquipments[(int)EquipmentId.DepthCharge_Type2DepthCharge].NameEN;


			if (selectedShip.SlotSize >= 4)
			{
				ASWEquipmentPairs.Add(openingASWborder - 67, new List<string> { $"[{hfdf}, {aswTorpedo}, {rur}, {aswRocket}]" });
				ASWEquipmentPairs.Add(openingASWborder - 51, new List<string> { $"[{type4}x3, {aswRocket}]" });
				ASWEquipmentPairs.Add(openingASWborder - 48, new List<string> { $"[{type4}x4]" });
				ASWEquipmentPairs.Add(openingASWborder - 44, new List<string> { $"[{type4}x3, {type3dc}]" });
			}
			if (selectedShip.SlotSize >= 3)
			{
				ASWEquipmentPairs.Add(openingASWborder - 52, new List<string> { $"[{hfdf}, {aswTorpedo}, {rur}]" });
				ASWEquipmentPairs.Add(openingASWborder - 47, new List<string> { $"[{hfdf}, {rur}, {aswRocket}]" });

				ASWEquipmentPairs.Add(openingASWborder - 39, new List<string> { $"[{type4}x2, {aswRocket}]" });
				ASWEquipmentPairs.Add(openingASWborder - 36, new List<string> { $"[{type4}x3]" });
				ASWEquipmentPairs.Add(openingASWborder - 32, new List<string> { $"[{type4}x2, {type3dc}]" });
				ASWEquipmentPairs.Add(openingASWborder - 28, new List<string> { $"[{type3}x2, {type3dc}]" });
				ASWEquipmentPairs.Add(openingASWborder - 27, new List<string> { $"[{type4}, {type3dc}, {type2dc}]" });
			}

			if (selectedShip.SlotSize >= 2)
			{
				ASWEquipmentPairs.Add(openingASWborder - 35, new List<string> { $"[{hfdf}, {aswTorpedo}]" });
				if (ASWEquipmentPairs.ContainsKey(openingASWborder - 32))
					ASWEquipmentPairs[openingASWborder - 32] = ASWEquipmentPairs[openingASWborder - 32].Append($"[{hfdf}, {rur}]");
				else
					ASWEquipmentPairs.Add(openingASWborder - 32, new List<string> { $"[{hfdf}, {rur}]" });
				if (ASWEquipmentPairs.ContainsKey(openingASWborder - 27))
					ASWEquipmentPairs[openingASWborder - 27] = ASWEquipmentPairs[openingASWborder - 27].Append($"[{type4}, {aswRocket}]");
				else
					ASWEquipmentPairs.Add(openingASWborder - 27, new List<string> { $"[{type4}, {aswRocket}]" });
				ASWEquipmentPairs.Add(openingASWborder - 20, new List<string> { $"[{type4}, {type3dc}]" });
				ASWEquipmentPairs.Add(openingASWborder - 18, new List<string> { $"[{type3}, {type3dc}]" });
			}
			ASWEquipmentPairs.Add(openingASWborder - 15, new List<string> { $"[{hfdf}]" });
			ASWEquipmentPairs.Add(openingASWborder - 12, new List<string> { $"[{type4}]" });
		}


		IParameter aswdata = selectedShip.MasterShip.ASW;
		int aswmin = aswdata.Minimum;
		int aswmax = aswdata.Maximum;
		int aswmod = ASWModernization;
		int currentlv = selectedShip.Level;
		int minlv = ShowAllLevel ? 1 : (currentlv + 1);
		int unitexp = Math.Max(SortieExp, 1);
		int[] remodelLevelTable = GetRemodelLevelTable(selectedShip.MasterShip);

		WarningText = aswdata switch
		{
			{ IsAvailable: false } => ExpChecker.AswUnknown,
			{ IsDetermined: false } => ExpChecker.AswApproximated,
			_ => null,
		};

		DataGridItem[] rows = new DataGridItem[ExpTable.ShipMaximumLevel - (minlv - 1)];

		for (int lv = minlv; lv <= ExpTable.ShipMaximumLevel; lv++)
		{
			int asw = aswmin + ((aswmax - aswmin) * lv / 99) + aswmod;

			int needexp = ExpTable.ShipExp[lv].Total - selectedShip.ExpTotal;

			IEnumerable<string>? equipmentPairs = ASWEquipmentPairs
				.Where(k => asw >= k.Key)
				.OrderByDescending(p => p.Key)
				.FirstOrDefault().Value;

			DataGridItem row = new()
			{
				Level = lv,
				NeededExp = Math.Max(needexp, 0),
				SortieCount = Math.Max((int)Math.Ceiling((double)needexp / unitexp), 0),
				Asw = !aswdata.IsAvailable ? -1 : asw,
				EquipmentList = !aswdata.IsAvailable ? "-" : equipmentPairs switch
				{
					null => "-",
					_ => string.Join(", ", equipmentPairs),
				},
				ToolTip = equipmentPairs switch
				{
					null => "-",
					_ => string.Join("\n", equipmentPairs),
				},
				IsRemodelLevel = remodelLevelTable.Contains(lv),
				ShipAccuracy = selectedShip
					.Accuracy(lv, selectedShip.MasterShip.LuckMin + LuckModernization)
					.RoundDown(2),
			};

			rows[lv - minlv] = row;
		}

		SetResults(rows);

		Title = DefaultTitle + " - " + selectedShip.NameWithLevel;
		GroupExpText =
			$"{selectedShip.NameWithLevel}: " +
			$"Exp. {selectedShip.ExpTotal}, {ExpChecker.ASW} {selectedShip.ASWBase} " +
			$"({ExpChecker.Modernization}+{selectedShip.ASWModernized})";
	}

	private void SetResults(DataGridItem[] rows)
	{
		DataGridViewModel.ItemsSource.Clear();
		DataGridViewModel.AddRange(rows);
	}

	/*
	https://github.com/ElectronicObserverEN/ElectronicObserver/issues/212

	attempt to fix the opening ASW equipment requirements

	because there are a lot of aircraft with ASW, and number of slots on CV(L)
	the number of possible combinations becomes too large and causes terrible performance

	attempts to fix performance are by limiting aircraft to the top 5 by ASW value
	and limiting CV(L) slots to 3

	performance for CV(L) is okayish with the limits, but will probably be bad on older CPUs

	BBV performance is still not good
	mostly because limiting the slots like with CV(L) isn't viable, because BBV need all slots
	to reach opening ASW values on lower levels
	*/
	private void UpdateLevelViewNew()
	{
		static IEnumerable<string> GroupedDisplay(ASWEquipmentData[] equipment) => equipment
		.Where(b => b.ID > 0)
		.GroupBy(b => b.ID)
		.Select(b => b.Count() switch
		{
			1 => b.First().Name,
			_ => $"{b.First().Name}x{b.Count()}",
		});

		static IEnumerable<string> GetEquipmentDisplay(List<ASWEquipmentData[]> equipmentCombinations) =>
		equipmentCombinations
		.OrderBy(a => a.Count(b => b.ID > 0))
		.Select(a => $"[{string.Join(", ", GroupedDisplay(a))}]");

		IShipData? selectedShip = SelectedShip;

		if (selectedShip == null)
		{
			System.Media.SystemSounds.Asterisk.Play();
			return;
		}

		ShipDataMock mockShip = new(selectedShip.MasterShip)
		{
			Level = 175,
			ASWModernized = selectedShip.ASWModernized,
		};

		DataGridViewModel.ItemsSource.Clear();

		Dictionary<int, List<ASWEquipmentData[]>> allPossibleSetups = new();
		if (ShowAllASWEquipments)
		{
			IEnumerable<IEquipmentData> Top5(IEnumerable<IEquipmentData> equipment)
			{
				List<EquipmentId> top5 = equipment
					.DistinctBy(e => e.EquipmentId)
					.OrderByDescending(e => e.MasterEquipment.ASW)
					.Take(5)
					.Select(e => e.EquipmentId)
					.ToList();

				return equipment.Where(e => top5.Contains(e.EquipmentId));
			}

			List<IEquipmentData> equipment = KCDatabase.Instance.Equipments.Values
				.Where(eq => eq.MasterEquipment.CategoryType is
					EquipmentTypes.Sonar or
					EquipmentTypes.DepthCharge ||
					eq.MasterEquipment.IsAntiSubmarineAircraft)
				.Where(eq => selectedShip.MasterShip.EquippableCategoriesTyped
					.Contains(eq.MasterEquipment.CategoryType))
				.GroupBy(eq => eq.MasterEquipment.CategoryType)
				.SelectMany(g => g.Key switch
				{
					EquipmentTypes.SeaplaneBomber or
						EquipmentTypes.SeaplaneBomber or
						EquipmentTypes.CarrierBasedTorpedo or
						EquipmentTypes.CarrierBasedBomber
						=> Top5(g),

					_ => g,
				})
				.ToList();

			Dictionary<EquipmentId, EquipmentDataMock> mocks = equipment
				.Select(e => new EquipmentDataMock(e.MasterEquipment))
				.DistinctBy(e => e.EquipmentId)
				.ToDictionary(e => e.EquipmentId, e => e);

			ASWEquipmentData[] had = equipment
				.GroupBy(eq => eq.EquipmentID)
				.Select(g => new ASWEquipmentData
				{
					ID = g.Key,
					ASW = g.First().MasterEquipment.ASW,
					Name = g.First().MasterEquipment.NameEN,
					IsSonar = g.First().MasterEquipment.IsSonar,
					Count = g.Count(),
					Equipment = g.First().MasterEquipment,
				})
				.Concat(new[]
				{
				new ASWEquipmentData
				{
					ID = -1,
					ASW = 0,
					Name = "",
					Count = 99,
					IsSonar = false,
				},
				})
				.OrderByDescending(a => a.ASW)
				.ToArray();

			int[] stack = Enumerable.Repeat(0, selectedShip.MasterShip.ShipType switch
			{
				ShipTypes.AircraftCarrier => Math.Min(3, selectedShip.SlotSize),
				ShipTypes.LightAircraftCarrier => Math.Min(3, selectedShip.SlotSize),

				_ => selectedShip.SlotSize,
			}).ToArray();

			if (had.Length > 0 && stack.Length > 0)
			{

				while (stack[0] != -1)
				{
					var convert = stack.Select(i => had[i]).ToArray();

					mockShip.SlotInstance = convert
						.Where(e => e.Equipment is not null)
						.Select(e => mocks[e.Equipment!.EquipmentId])
						.Cast<IEquipmentData?>()
						.ToList();

					mockShip.AswFit = mockShip.GetTheoreticalFitBonus(KCDatabase.Instance.Translation.FitBonus.FitBonusList).ASW;

					bool hasEquipment = stack
						.GroupBy(s => s)
						.All(s => had[s.Key].Count >= s.Count());

					if (hasEquipment && mockShip.CanDoOpeningAsw())
					{
						int aswSum = mockShip.ASWTotal - mockShip.ASWBase;

						if (allPossibleSetups.ContainsKey(aswSum))
						{
							int equipmentCount = convert.Count(e => e.ID is not -1);

							// count how many entries for the current aswsum use the same number of equipment
							int? entryCount = allPossibleSetups[aswSum]
								.GroupBy(a => a.Count(e => e.ID is not -1))
								.FirstOrDefault(g => g.Key == equipmentCount)
								?.Count();

							// don't display more than 6 different combinations for a given equipment count
							if (entryCount is null or < 6)
							{
								allPossibleSetups[aswSum].Add(convert);
							}
						}
						else
						{
							allPossibleSetups.Add(aswSum, new List<ASWEquipmentData[]>
						{
							convert,
						});
						}
					}

					for (int p = stack.Length - 1; p >= 0; p--)
					{
						stack[p]++;
						if (stack[p] < had.Length)
							break;
						stack[p] = -1;
					}
					for (int p = 1; p < stack.Length; p++)
					{
						if (stack[p] == -1)
							stack[p] = stack[p - 1];
					}
				}
			}
		}

		IParameter aswdata = selectedShip.MasterShip.ASW;
		int aswmin = aswdata.Minimum;
		int aswmax = aswdata.Maximum;
		int aswmod = ASWModernization;
		int currentlv = selectedShip.Level;
		int minlv = ShowAllLevel ? 1 : (currentlv + 1);
		int unitexp = Math.Max(SortieExp, 1);
		int[] remodelLevelTable = GetRemodelLevelTable(selectedShip.MasterShip);

		WarningText = aswdata switch
		{
			{ IsAvailable: false } => ExpChecker.AswUnknown,
			{ IsDetermined: false } => ExpChecker.AswApproximated,
			_ => null,
		};

		DataGridItem[] rows = new DataGridItem[ExpTable.ShipMaximumLevel - (minlv - 1)];

		for (int lv = minlv; lv <= ExpTable.ShipMaximumLevel; lv++)
		{
			int asw = aswmin + ((aswmax - aswmin) * lv / 99) + aswmod;

			int needexp = ExpTable.ShipExp[lv].Total - selectedShip.ExpTotal;

			mockShip.Level = lv;

			List<ASWEquipmentData[]>? setups = null;

			foreach ((_, List<ASWEquipmentData[]> value) in allPossibleSetups.OrderBy(k => k.Key))
			{
				List<ASWEquipmentData[]> current = value
				.Where(k =>
				{
					mockShip.SlotInstance = k
		.Where(e => e.Equipment is not null)
		.Select(e => new EquipmentDataMock(e.Equipment!))
		.Cast<IEquipmentData?>()
		.ToList();

					return mockShip.CanDoOpeningAsw();
				})
				.ToList();

				if (current.Any())
				{
					setups = current;
					break;
				}
			}

			IEnumerable<string>? equipmentPairs = setups switch
			{
				{ } => GetEquipmentDisplay(setups.ToList()),
				_ => null,
			};

			DataGridItem row = new()
			{
				Level = lv,
				NeededExp = Math.Max(needexp, 0),
				SortieCount = Math.Max((int)Math.Ceiling((double)needexp / unitexp), 0),
				Asw = !aswdata.IsAvailable ? -1 : asw,
				EquipmentList = !aswdata.IsAvailable ? "-" : equipmentPairs switch
				{
					null => "-",
					_ => string.Join(", ", equipmentPairs)
				},
				ToolTip = equipmentPairs switch
				{
					null => "-",
					_ => string.Join("\n", equipmentPairs)
				},
				IsRemodelLevel = remodelLevelTable.Contains(lv),
			};

			rows[lv - minlv] = row;
		}

		SetResults(rows);

		Title = DefaultTitle + " - " + selectedShip.NameWithLevel;
		GroupExpText = $"{selectedShip.NameWithLevel}: Exp. {selectedShip.ExpTotal}, {ExpChecker.ASW} {selectedShip.ASWBase} ({ExpChecker.Modernization}+{selectedShip.ASWModernized})";
	}

	private int[] GetRemodelLevelTable(IShipDataMaster ship)
	{
		while (ship.RemodelBeforeShip != null)
			ship = ship.RemodelBeforeShip;

		var list = new LinkedList<int>();

		while (ship != null)
		{
			list.AddLast(ship.RemodelAfterLevel);
			ship = ship.RemodelAfterShip;
			if (list.Last() >= ship.RemodelAfterLevel)
				break;
		}

		return list.ToArray();
	}

	/*
	private void LevelView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
	{
		if (e.ColumnIndex == ColumnASW.Index)
		{
			e.Value = (int)e.Value == -1 ? "???" : e.Value.ToString();
			e.FormattingApplied = true;
		}
	}
	*/
}
