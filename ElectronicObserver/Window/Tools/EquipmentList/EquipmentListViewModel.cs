using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Common.Datagrid;
using ElectronicObserver.Data;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Control.EquipmentFilter;
using ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment;
using ElectronicObserver.Window.Wpf.Fleet;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Data;

namespace ElectronicObserver.Window.Tools.EquipmentList;

public partial class EquipmentListViewModel : WindowViewModelBase
{
	public DialogEquipmentListTranslationViewModel DialogEquipmentList { get; }
	private Microsoft.Win32.SaveFileDialog SaveCsvDialog { get; } = new()
	{
		Filter = "CSV|*.csv|File|*",
	};

	public DataGridViewModel<EquipmentListRow> EquipmentGridViewModel { get; set; }
	public GridLength EquipmentGridWidth { get; set; } = GridLength.Auto;

	// todo: doesn't seem to work in the current implementation
	// Select an equipment with multiple detail items, sort by something, select a different equipment, sort data is lost.
	public DataGridViewModel<EquipmentListDetailRow> EquipmentDetailGridViewModel { get; set; }
	public EquipmentListRow? SelectedRow { get; set; }

	public EquipmentFilterViewModel Filters { get; } = new(true);

	public bool ShowLockedEquipmentOnly { get; set; }

	public string Title => SelectedRow switch
	{
		{ } row => DialogEquipmentList.Title + " - " + KCDatabase.Instance.MasterEquipments[row.Id].NameEN,
		_ => DialogEquipmentList.Title
	};

	public EquipmentListViewModel()
	{
		DialogEquipmentList = Ioc.Default.GetService<DialogEquipmentListTranslationViewModel>()!;

		EquipmentGridViewModel = new();
		EquipmentDetailGridViewModel = new();

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ShowLockedEquipmentOnly)) return;

			UpdateView();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(SelectedRow)) return;
			if (SelectedRow is null) return;

			UpdateDetailView(SelectedRow.Id);
		};

		Filters.PropertyChanged += (_, _) => UpdateView();

		UpdateView();
	}

	private void UpdateView()
	{
		var ships = KCDatabase.Instance.Ships.Values;
		var equipments = ShowLockedEquipmentOnly switch
		{
			true => KCDatabase.Instance.Equipments.Values.Where(eq => eq.IsLocked),
			_ => KCDatabase.Instance.Equipments.Values
		};
		var masterEquipments = KCDatabase.Instance.MasterEquipments;
		int masterCount = masterEquipments.Values.Count(eq => !eq.IsAbyssalEquipment);

		var allCount = equipments
			.Where(eq => !ShowLockedEquipmentOnly || eq?.IsLocked is true)
			.GroupBy(eq => eq.EquipmentID)
			.ToDictionary(group => group.Key, group => group.Count());

		var remainCount = new Dictionary<int, int>(allCount);


		//剰余数計算
		foreach (var eq in ships
			.SelectMany(s => s.AllSlotInstance)
			.Where(eq => !ShowLockedEquipmentOnly || eq?.IsLocked is true)
			.Select(eq => eq?.MasterEquipment)
			.Where(eq => eq != null))
		{

			remainCount[eq!.EquipmentID]--;
		}

		foreach (var eq in KCDatabase.Instance.BaseAirCorps.Values
			.SelectMany(corps => corps.Squadrons.Values.Select(sq => sq.EquipmentInstance))
			.Where(eq => !ShowLockedEquipmentOnly || eq?.IsLocked is true)
			.Where(eq => eq != null))
		{

			remainCount[eq.EquipmentID]--;
		}

		foreach (var eq in KCDatabase.Instance.RelocatedEquipments.Values
			.Where(eq => !ShowLockedEquipmentOnly || eq.EquipmentInstance?.IsLocked is true)
			.Where(eq => eq.EquipmentInstance != null))
		{

			remainCount[eq.EquipmentInstance.EquipmentID]--;
		}

		EquipmentGridViewModel.ItemsSource.Clear();

		List<EquipmentListRow> rows = new(allCount.Count);
		var ids = allCount.Keys
			.Where(id => Filters.MeetsFilterCondition(masterEquipments[id]));

		foreach (int id in ids)
		{

			EquipmentListRow row = new(masterEquipments[id], allCount[id], remainCount[id]);

			{
				StringBuilder sb = new StringBuilder();
				var eq = masterEquipments[id];

				sb.AppendFormat("{0} {1} (ID: {2})\r\n", eq.CategoryTypeInstance.NameEN, eq.NameEN, eq.EquipmentID);
				if (eq.Firepower != 0) sb.AppendFormat(AlbumMasterShipResources.Firepower + " {0:+0;-0}\r\n", eq.Firepower);
				if (eq.Torpedo != 0) sb.AppendFormat(AlbumMasterShipResources.Torpedo + " {0:+0;-0}\r\n", eq.Torpedo);
				if (eq.AA != 0) sb.AppendFormat(AlbumMasterShipResources.AA + " {0:+0;-0}\r\n", eq.AA);
				if (eq.Armor != 0) sb.AppendFormat(AlbumMasterShipResources.Armor + " {0:+0;-0}\r\n", eq.Armor);
				if (eq.ASW != 0) sb.AppendFormat(AlbumMasterShipResources.ASW + " {0:+0;-0}\r\n", eq.ASW);
				if (eq.Evasion != 0) sb.AppendFormat("{0} {1:+0;-0}\r\n", eq.CategoryType == EquipmentTypes.Interceptor ? AlbumMasterShipResources.Interception : AlbumMasterShipResources.Evasion, eq.Evasion);
				if (eq.LOS != 0) sb.AppendFormat(AlbumMasterShipResources.LOS + " {0:+0;-0}\r\n", eq.LOS);
				if (eq.Accuracy != 0) sb.AppendFormat("{0} {1:+0;-0}\r\n", eq.CategoryType == EquipmentTypes.Interceptor ? AlbumMasterShipResources.AntiBomb : AlbumMasterShipResources.Accuracy, eq.Accuracy);
				if (eq.Bomber != 0) sb.AppendFormat(AlbumMasterShipResources.Bombing + " {0:+0;-0}\r\n", eq.Bomber);
				sb.AppendLine(AlbumMasterShipResources.RightClickToOpenInNewWindow);

				row.ToolTipText = sb.ToString();
			}
			rows.Add(row);
		}

		for (int i = 0; i < rows.Count; i++)
			rows[i].Tag = i;

		EquipmentGridViewModel.ItemsSource.Clear();
		EquipmentGridViewModel.AddRange(rows
			.OrderBy(r => masterEquipments[r.Id].CategoryType)
			.ThenBy(r => r.Id)
			.ThenBy(r => r.Name));
	}

	private void UpdateDetailView(int equipmentID)
	{
		// DetailRows.Clear();

		//装備数カウント
		var eqs = KCDatabase.Instance.Equipments.Values
			.Where(eq => !ShowLockedEquipmentOnly || eq?.IsLocked is true)
			.Where(eq => eq.EquipmentID == equipmentID);
		var countlist = new IDDictionary<DetailCounter>();

		foreach (var eq in eqs)
		{
			var c = countlist[DetailCounter.CalculateID(eq)];
			if (c == null)
			{
				countlist.Add(new DetailCounter(eq.Level, eq.AircraftLevel));
				c = countlist[DetailCounter.CalculateID(eq)];
			}
			c.countAll++;
			c.countRemain++;
			c.countRemainPrev++;
		}

		//装備艦集計
		foreach (var ship in KCDatabase.Instance.Ships.Values)
		{

			foreach (var eq in ship.AllSlotInstance.Where(s => s != null && s.EquipmentID == equipmentID))
			{

				countlist[DetailCounter.CalculateID(eq)].countRemain--;
			}

			foreach (var c in countlist.Values)
			{
				if (c.countRemain != c.countRemainPrev)
				{

					int diff = c.countRemainPrev - c.countRemain;

					c.equippedShips.Add(ship.NameWithLevel + (diff > 1 ? (" x" + diff) : ""));

					c.countRemainPrev = c.countRemain;
				}
			}

		}

		// 基地航空隊 - 配備中の装備を集計
		foreach (var corps in KCDatabase.Instance.BaseAirCorps.Values)
		{

			foreach (var sq in corps.Squadrons.Values.Where(sq => sq != null && sq.EquipmentID == equipmentID))
			{

				countlist[DetailCounter.CalculateID(sq.EquipmentInstance)].countRemain--;
			}

			foreach (var c in countlist.Values)
			{
				if (c.countRemain != c.countRemainPrev)
				{
					int diff = c.countRemainPrev - c.countRemain;

					c.equippedShips.Add(string.Format("#{0} {1}{2}", corps.MapAreaID, corps.Name, diff > 1 ? (" x" + diff) : ""));

					c.countRemainPrev = c.countRemain;
				}
			}

		}

		// 基地航空隊 - 配置転換中の装備を集計
		foreach (var eq in KCDatabase.Instance.RelocatedEquipments.Values
			.Select(v => v.EquipmentInstance)
			.Where(eq => eq != null && eq.EquipmentID == equipmentID))
		{

			countlist[DetailCounter.CalculateID(eq)].countRemain--;
		}

		foreach (var c in countlist.Values)
		{
			if (c.countRemain != c.countRemainPrev)
			{

				int diff = c.countRemainPrev - c.countRemain;

				c.equippedShips.Add("配置転換中" + (diff > 1 ? (" x" + diff) : ""));

				c.countRemainPrev = c.countRemain;
			}
		}


		//行に反映
		List<EquipmentListDetailRow> rows = new(eqs.Count());

		foreach (var c in countlist.Values)
		{

			if (!c.equippedShips.Any())
			{
				c.equippedShips.Add("");
			}

			foreach (var s in c.equippedShips)
			{
				EquipmentListDetailRow row = new(c.level, c.aircraftLevel, c.countAll, c.countRemain, s);
				rows.Add(row);
			}
		}

		EquipmentDetailGridViewModel.ItemsSource.Clear();

		foreach (EquipmentListDetailRow detailRow in GroupRows(rows))
		{
			EquipmentDetailGridViewModel.ItemsSource.Add(detailRow);
		}
	}

	private static IEnumerable<EquipmentListDetailRow> GroupRows(IEnumerable<EquipmentListDetailRow> rows) =>
		from detail in rows
		group detail by new { detail.Level, detail.AircraftLevel }
		into grouped
		orderby grouped.Key.Level, grouped.Key.AircraftLevel
		select new EquipmentListDetailRow
		(
			grouped.Key.Level,
			grouped.Key.AircraftLevel,
			grouped.First().CountAll,
			grouped.First().CountRemain,
			string.Join("\n", grouped.Select(r => r.EquippedShip))
		);

	public void OpenEquipmentEncyclopedia(int id)
	{
		new DialogAlbumMasterEquipmentWpf(id).Show(App.Current.MainWindow);
	}

	[RelayCommand]
	private void SaveAsCsv()
	{
		SaveCsvDialog.Title = DialogEquipmentList.SaveCSVDialog;

		if (SaveCsvDialog.ShowDialog(App.Current.MainWindow) != true) return;

		try
		{
			using StreamWriter sw = new StreamWriter(SaveCsvDialog.FileName, false, Utility.Configuration.Config.Log.FileEncoding);
			sw.WriteLine(EncycloRes.EquipListCSVFormat);
			string arg = string.Format("{{{0}}}", string.Join("},{", Enumerable.Range(0, 8)));

			foreach (var eq in KCDatabase.Instance.Equipments.Values.Where(eq => !ShowLockedEquipmentOnly || eq?.IsLocked is true))
			{
				if (eq.Name == "なし") continue;

				ShipData equippedShip = KCDatabase.Instance.Ships.Values.FirstOrDefault(s => s.AllSlot.Contains(eq.MasterID));

				sw.WriteLine(arg,
					eq.MasterID,
					eq.EquipmentID,
					eq.Name,
					eq.Level,
					eq.AircraftLevel,
					eq.IsLocked ? 1 : 0,
					equippedShip?.MasterID ?? -1,
					equippedShip?.NameWithLevel ?? ""
				);
			}
		}
		catch (Exception ex)
		{
			Utility.ErrorReporter.SendErrorReport(ex, EncycloRes.FailedOutputEqListCSV);
			MessageBox.Show(EncycloRes.FailedOutputEqListCSV + "\r\n" + ex.Message, DialogEquipmentList.Error, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}


	[RelayCommand]
	private void Update()
	{
		UpdateView();
	}

	[RelayCommand]
	private void CopyToFleetAnalysis()
	{
		Clipboard.SetDataObject(FleetViewModel.GenerateEquipList(!ShowLockedEquipmentOnly));
	}
}
