using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Common.Datagrid;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Dialog.ShipPicker;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.DropRecordViewer;

public partial class DropRecordViewerViewModel : WindowViewModelBase
{
	public DialogDropRecordViewerTranslationViewModel DialogDropRecordViewer { get; }

	private ShipDropRecord Record { get; }
	private BackgroundWorker Searcher { get; } = new();

	private ShipPickerViewModel ShipPickerViewModel { get; }
	public List<object> Items { get; set; } = new();

	public List<object> Worlds { get; set; } = new();
	public List<object> Maps { get; set; } = new();
	public List<MapNode> Cells { get; set; } = new() { new(MapAny) };
	public List<object> Difficulties { get; set; } = new() { MapAny };

	public List<DropRecordRow> SelectedRows { get; set; } = new();

	// DropRecordOption or IShipDataMaster
	public object ShipSearchOption { get; set; } = DropRecordOption.All;
	// DropRecordOption or UseItemMaster
	public object ItemSearchOption { get; set; } = DropRecordOption.All;

	private DateTime DateTimeBegin =>
		new(DateBegin.Year, DateBegin.Month, DateBegin.Day, TimeBegin.Hour, TimeBegin.Minute, TimeBegin.Second);
	private DateTime DateTimeEnd =>
		new(DateEnd.Year, DateEnd.Month, DateEnd.Day, TimeEnd.Hour, TimeEnd.Minute, TimeEnd.Second);
	
	public DateTime DateBegin { get; set; }
	public DateTime TimeBegin { get; set; }
	public DateTime DateEnd { get; set; }
	public DateTime TimeEnd { get; set; }
	public DateTime MinDate { get; set; }
	public DateTime MaxDate { get; set; }

	public object MapAreaID { get; set; } = MapAny;
	public object MapInfoID { get; set; } = MapAny;
	public MapNode MapCellID { get; set; } = new(MapAny);
	public bool MapCellIdEnabled { get; set; }
	public object MapDifficulty { get; set; } = MapAny;
	public bool? IsBossOnly { get; set; }
	public bool RankS { get; set; } = true;
	public bool RankA { get; set; } = true;
	public bool RankB { get; set; } = true;
	public bool RankX { get; set; } = true;
	public bool MergeRows { get; set; }
	public bool RawRows => !MergeRows;
	public string StatusInfoText { get; set; }
	public DateTime StatusInfoTag { get; set; }

	private string NameNotExist => DialogDropRecordViewer.NameNotExist;
	private const string MapAny = "*";

	public string Today => $"{DialogDropRecordViewer.Today}: {DateTime.Now:yyyy/MM/dd}";

	private ObservableCollection<DropRecordRow> RecordRows { get; set; } = new();
	public DataGridViewModel<DropRecordRow> DataGridRawRowsViewModel { get; set; }
	public DataGridViewModel<DropRecordRow> DataGridMergedRowsViewModel { get; set; }

	public DropRecordViewerViewModel()
	{
		Record = RecordManager.Instance.ShipDrop;

		DataGridRawRowsViewModel = new(RecordRows);
		DataGridMergedRowsViewModel = new(RecordRows);

		ShipPickerViewModel = Ioc.Default.GetService<ShipPickerViewModel>()!;
		DialogDropRecordViewer = Ioc.Default.GetService<DialogDropRecordViewerTranslationViewModel>()!;

		Searcher.WorkerSupportsCancellation = true;
		Searcher.DoWork += Searcher_DoWork;
		Searcher.RunWorkerCompleted += Searcher_RunWorkerCompleted;

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not (nameof(MapAreaID) or nameof(MapInfoID))) return;

			MapCellID = new(MapAny);

			if (MapAreaID is not int world || MapInfoID is not int map)
			{
				MapCellIdEnabled = false;
				return;
			}

			string Grouping(int id) => Utility.Configuration.Config.UI.UseOriginalNodeId switch
			{
				true => $"{id}",
				_ => KCDatabase.Instance.Translation.Destination.DisplayID(world, map, id)
			};

			MapCellIdEnabled = true;
			var cells = Record.Record
				.Where(r => r.MapAreaID == world && r.MapInfoID == map)
				.Select(r => r.CellID)
				.Distinct()
				.GroupBy(Grouping)
				.Select(g => new MapNode(g.Key, g.OrderBy(i => i).ToList()));

			cells = Utility.Configuration.Config.UI.UseOriginalNodeId switch
			{
				true => cells.OrderBy(c => c.Ids?.First() ?? -1),
				_ => cells.OrderBy(n => n.Letter)
			};

			Cells = cells
				.Prepend(new(MapAny))
				.ToList();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(SelectedRows)) return;

			RecordView_SelectionChanged();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(MergeRows)) return;

			RecordRows.Clear();
		};

		Loaded();

		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		ConfigurationChanged();
	}

	private void ConfigurationChanged()
	{
		Font = new FontFamily(Utility.Configuration.Config.UI.MainFont.FontData.FontFamily.Name);
		FontSize = Utility.Configuration.Config.UI.MainFont.FontData.ToSize();
		FontBrush = Utility.Configuration.Config.UI.ForeColor.ToBrush();
	}

	private void Loaded()
	{
		DateBegin = Record.Record.First().Date.Date;
		MinDate = Record.Record.First().Date.Date;

		DateEnd = DateTime.Now.AddDays(1).Date;
		MaxDate = DateTime.Now.AddDays(1).Date;

		Worlds = Record.Record
			.Select(r => r.MapAreaID)
			.Distinct()
			.OrderBy(w => w)
			.Cast<object>()
			.Prepend(MapAny)
			.ToList();

		Maps = Record.Record
			.Select(r => r.MapInfoID)
			.Distinct()
			.OrderBy(m => m)
			.Cast<object>()
			.Prepend(MapAny)
			.ToList();

		IEnumerable<UseItemId> includedItemNames = Record.Record
			.Where(record => record.ItemName != NameNotExist)
			.Select(record => (UseItemId)record.ItemID)
			.Distinct();

		IEnumerable<UseItemMaster> includedItemObjects = includedItemNames
			.Select(id => KCDatabase.Instance.MasterUseItems.Values.FirstOrDefault(item => item.ItemID == id))
			.Where(s => s != null)!;

		Items = includedItemObjects
			.Cast<object>()
			.Prepend(DropRecordOption.NoDrop)
			.Prepend(DropRecordOption.Drop)
			.Prepend(DropRecordOption.All)
			.ToList();

		Difficulties = Record.Record
			.Select(r => r.Difficulty)
			.Distinct()
			.Except(new[] { 0 })
			.OrderBy(i => i)
			.Cast<object>()
			.Prepend(MapAny)
			.ToList();
	}

	private string GetContentString(ShipDropRecord.ShipDropElement elem, bool ignoreShip = false, bool ignoreItem = false, bool ignoreEquipment = false)
	{

		if (elem.ShipID > 0 && !ignoreShip)
		{

			if (elem.ItemID > 0 && !ignoreItem)
			{
				if (elem.EquipmentID > 0 && !ignoreEquipment)
					return elem.ShipName + " + " + elem.ItemName + " + " + elem.EquipmentName;
				else
					return elem.ShipName + " + " + elem.ItemName;
			}
			else
			{
				if (elem.EquipmentID > 0 && !ignoreEquipment)
					return elem.ShipName + " + " + elem.EquipmentName;
				else
					return elem.ShipName;
			}

		}
		else
		{
			if (elem.ItemID > 0 && !ignoreItem)
			{
				if (elem.EquipmentID > 0 && !ignoreEquipment)
					return elem.ItemName + " + " + elem.EquipmentName;
				else
					return elem.ItemName;
			}
			else
			{
				if (elem.EquipmentID > 0 && !ignoreEquipment)
					return elem.EquipmentName;
				else
					return elem.ShipName;
			}
		}

	}

	private string GetContentStringForSorting(ShipDropRecord.ShipDropElement elem, bool ignoreShip = false, bool ignoreItem = false, bool ignoreEquipment = false)
	{
		IShipDataMaster? ship = KCDatabase.Instance.MasterShips[elem.ShipID];
		UseItemMaster? item = KCDatabase.Instance.MasterUseItems[elem.ItemID];
		IEquipmentDataMaster? eq = KCDatabase.Instance.MasterEquipments[elem.EquipmentID];

		if (ship != null && ship.NameEN != elem.ShipName) ship = null;
		if (item != null && item.Name != elem.ItemName) item = null;
		if (eq != null && eq.NameEN != elem.EquipmentName) eq = null;

		StringBuilder sb = new StringBuilder();


		if (elem.ShipID > 0 && !ignoreShip)
		{
			sb.AppendFormat("0{0:D4}{1}/{2}", (int?)ship?.ShipType ?? 0, ship?.NameReading ?? elem.ShipName, elem.ShipName);
		}

		if (elem.ItemID > 0 && !ignoreItem)
		{
			if (sb.Length > 0) sb.Append(",");
			sb.AppendFormat("1{0:D4}{1}", item?.ID ?? 0, elem.ItemName);
		}

		if (elem.EquipmentID > 0 && !ignoreEquipment)
		{
			if (sb.Length > 0) sb.Append(",");
			sb.AppendFormat("2{0:D4}{1}", eq?.EquipmentID ?? 0, elem.EquipmentName);
		}

		return sb.ToString();
	}

	private string ConvertContentString(string str)
	{
		if (str.Length == 0) return NameNotExist;

		StringBuilder sb = new StringBuilder();

		foreach (var s in str.Split(",".ToCharArray()))
		{

			if (sb.Length > 0)
				sb.Append(" + ");

			switch (s[0])
			{
				case '0':
					sb.Append(s.Substring(s.IndexOf("/") + 1));
					break;
				case '1':
				case '2':
					sb.Append(s.Substring(5));
					break;
			}
		}

		return sb.ToString();
	}

	private string GetMapString(int maparea, int mapinfo, int cell = -1, bool isboss = false, int difficulty = -1, bool insertEnemyFleetName = true)
	{
		var sb = new StringBuilder();
		sb.Append(maparea);
		sb.Append("-");
		sb.Append(mapinfo);
		if (difficulty != -1)
			sb.AppendFormat("[{0}]", Constants.GetDifficulty(difficulty));
		if (cell != -1)
		{
			sb.Append("-");
			sb.Append(cell);
		}
		if (isboss)
			sb.Append(DialogDropRecordViewer.Boss);

		if (insertEnemyFleetName)
		{
			var enemy = RecordManager.Instance.EnemyFleet.Record.Values.FirstOrDefault(r => r.MapAreaID == maparea && r.MapInfoID == mapinfo && r.CellID == cell && r.Difficulty == difficulty);
			if (enemy != null)
				sb.AppendFormat(" ({0})", enemy.FleetName);
		}

		return sb.ToString();
	}

	private string GetMapString(int serialID, bool insertEnemyFleetName = true)
	{
		return GetMapString(serialID >> 24 & 0xFF, serialID >> 16 & 0xFF, serialID >> 8 & 0xFF, (serialID & 1) != 0, (sbyte)((serialID >> 1 & 0x7F) << 1) >> 1, insertEnemyFleetName);
	}

	private int GetMapSerialID(int maparea, int mapinfo, int cell, bool isboss, int difficulty = -1)
	{
		return (maparea & 0xFF) << 24 | (mapinfo & 0xFF) << 16 | (cell & 0xFF) << 8 | (difficulty & 0x7F) << 1 | (isboss ? 1 : 0);
	}

	private void Searcher_DoWork(object sender, DoWorkEventArgs e)
	{
		int priorityShip = ShipSearchOption switch
		{
			DropRecordOption.All => 0,
			DropRecordOption.Drop => 1,
			_ => 2
		};

		int priorityItem = ItemSearchOption switch
		{
			DropRecordOption.All => 0,
			DropRecordOption.Drop => 1,
			_ => 2
		};

		int priorityContent = Math.Max(priorityShip, priorityItem);

		List<ShipDropRecord.ShipDropElement> records = RecordManager.Instance.ShipDrop.Record;
		LinkedList<DropRecordRow> rows = new();


		//lock ( records )
		{
			int i = 0;
			var counts = new Dictionary<string, int[]>();
			var allcounts = new Dictionary<string, int[]>();


			foreach (var r in records)
			{

				#region Filtering

				if (r.Date < DateTimeBegin || DateTimeEnd < r.Date)
					continue;

				if (((r.Rank == "SS" || r.Rank == "S") && !RankS) ||
					((r.Rank == "A") && !RankA) ||
					((r.Rank == "B") && !RankB) ||
					((Constants.GetWinRank(r.Rank) <= 3) && !RankX))
					continue;


				if (MapAreaID is int world && world != r.MapAreaID) continue;
				if (MapInfoID is int map && map != r.MapInfoID) continue;
				if (MapCellID.Ids?.Contains(r.CellID) == false) continue;

				switch (IsBossOnly)
				{
					case false:
						if (r.IsBossNode)
							continue;
						break;
					case true:
						if (!r.IsBossNode)
							continue;
						break;
				}
				if (MapDifficulty is int difficulty && difficulty != r.Difficulty)
					continue;



				if (MergeRows)
				{
					string key;

					if (priorityContent == 2)
					{
						key = GetMapSerialID(r.MapAreaID, r.MapInfoID, r.CellID, r.IsBossNode, MapDifficulty is 0 ? -1 : r.Difficulty).ToString("X8");

					}
					else
					{
						key = GetContentString(r, priorityShip < priorityItem && priorityShip < 2, priorityShip >= priorityItem && priorityItem < 2);
					}


					if (!allcounts.ContainsKey(key))
					{
						allcounts.Add(key, new int[4]);
					}

					switch (r.Rank)
					{
						case "B":
							allcounts[key][3]++;
							break;
						case "A":
							allcounts[key][2]++;
							break;
						case "S":
						case "SS":
							allcounts[key][1]++;
							break;
					}
					allcounts[key][0]++;
				}



				switch (ShipSearchOption)
				{
					case DropRecordOption.All:
						break;
					case DropRecordOption.Drop:
						if (r.ShipID < 0)
							continue;
						break;
					case DropRecordOption.NoDrop:
						if (r.ShipID != -1)
							continue;
						break;
					case DropRecordOption.FullPort:
						if (r.ShipID != -2)
							continue;
						break;
					case IShipDataMaster ship:
						if (ship.ShipID != r.ShipID)
							continue;
						break;
				}

				switch (ItemSearchOption)
				{
					case DropRecordOption.All:
						break;
					case DropRecordOption.Drop:
						if (r.ItemID < 0)
							continue;
						break;
					case DropRecordOption.NoDrop:
						if (r.ItemID != -1)
							continue;
						break;
					case UseItemMaster item:
						if (item.ID != r.ItemID)
							continue;
						break;
				}

				#endregion


				if (!MergeRows)
				{
					DropRecordRow row = new(
						i + 1,
						GetContentString(r),
						r.Date,
						GetMapString(r.MapAreaID, r.MapInfoID, r.CellID, r.IsBossNode, r.Difficulty),
						Constants.GetWinRank(r.Rank),
						null,
						null,
						null
					);

					row.CellsTag1 = GetContentStringForSorting(r);
					row.CellsTag3 = GetMapSerialID(r.MapAreaID, r.MapInfoID, r.CellID, r.IsBossNode, r.Difficulty);

					rows.AddLast(row);


				}
				else
				{
					//merged

					string key = priorityContent switch
					{
						2 => GetMapSerialID(r.MapAreaID, r.MapInfoID, r.CellID, r.IsBossNode,
								MapDifficulty is 0 ? -1 : r.Difficulty)
							.ToString("X8"),
						_ => GetContentStringForSorting(r, priorityShip < priorityItem && priorityShip < 2,
							priorityShip >= priorityItem && priorityItem < 2)
					};

					if (!counts.ContainsKey(key))
					{
						counts.Add(key, new int[4]);
					}

					switch (r.Rank)
					{
						case "B":
							counts[key][3]++;
							break;
						case "A":
							counts[key][2]++;
							break;
						case "S":
						case "SS":
							counts[key][1]++;
							break;
					}
					counts[key][0]++;

				}



				if (Searcher.CancellationPending)
					break;

				i++;
			}


			if (MergeRows)
			{

				int[] allcountssum = Enumerable.Range(0, 4).Select(k => allcounts.Values.Sum(a => a[k])).ToArray();

				foreach (var c in counts)
				{
					string name = c.Key;

					if (int.TryParse(name, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture, out int serialID))
						name = GetMapString(serialID);

					// fixme: name != map だった時にソートキーが入れられない

					DropRecordRow row = new(
						c.Value[0],
						serialID != 0 ? name : ConvertContentString(name),
						null,
						null,
						null,
						c.Value[1],
						c.Value[2],
						c.Value[3]
					);


					if (priorityContent == 2)
					{
						row.RateOrMaxCountTotal = allcounts[c.Key][0];
						if (serialID != 0)
							row.CellsTag1 = serialID;
						else
							row.CellsTag1 = name;
						row.RateOrMaxCountS = allcounts[c.Key][1];
						row.RateOrMaxCountA = allcounts[c.Key][2];
						row.RateOrMaxCountB = allcounts[c.Key][3];

					}
					else
					{
						row.RateOrMaxCountTotal = ((double)c.Value[0] / Math.Max(allcountssum[0], 1));
						if (serialID != 0)
							row.CellsTag1 = serialID;
						else
							row.CellsTag1 = name;
						row.RateOrMaxCountS = ((double)c.Value[1] / Math.Max(allcountssum[1], 1));
						row.RateOrMaxCountA = ((double)c.Value[2] / Math.Max(allcountssum[2], 1));
						row.RateOrMaxCountB = ((double)c.Value[3] / Math.Max(allcountssum[3], 1));

					}

					rows.AddLast(row);
				}

			}

		}

		e.Result = rows.ToList();
	}

	private void Searcher_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{

		if (!e.Cancelled)
		{
			if (e.Result is not List<DropRecordRow> rows) return;

			rows = MergeRows switch
			{
				true => rows.OrderByDescending(r => r.Count).ToList(),
				_ => rows.OrderByDescending(r => r.Index).ToList()
			};

			RecordRows = new(rows);
			DataGridRawRowsViewModel.ItemsSource = RecordRows;
			DataGridMergedRowsViewModel.ItemsSource = RecordRows;

			StatusInfoText = EncycloRes.SearchComplete + " (" + (int)(DateTime.Now - StatusInfoTag).TotalMilliseconds + " ms)";
		}
		else
		{

			StatusInfoText = EncycloRes.SearchCancelled;
		}

	}

	private void RecordView_SelectionChanged()
	{
		int selectedCount = SelectedRows.Count;

		if (selectedCount == 0) return;

		if (MergeRows)
		{
			int count = SelectedRows.Select(r => r.Count).Sum();
			int allcount = RecordRows.Select(r => r.Count).Sum();

			StatusInfoText = string.Format(DialogDropRecordViewer.SelectedItems,
				count, allcount, (double)count / allcount);
		}
		else
		{
			int allcount = RecordRows.Count;
			StatusInfoText = string.Format(DialogDropRecordViewer.SelectedItems,
				selectedCount, allcount, (double)selectedCount / allcount);
		}
	}

	[RelayCommand]
	private void OpenShipPicker()
	{
		ShipPickerViewModel.DropRecordOptions = Enum.GetValues<DropRecordOption>().ToList();
		ShipPickerView shipPicker = new(ShipPickerViewModel);

		if (shipPicker.ShowDialog(App.Current.MainWindow) is true)
		{
			ShipSearchOption = shipPicker.PickedShip switch
			{
				{ } => shipPicker.PickedShip!,
				_ => shipPicker.PickedOption!
			};
		}

		ShipPickerViewModel.DropRecordOptions = null;
	}

	[RelayCommand]
	private void StartSearch()
	{

		if (Searcher.IsBusy)
		{
			if (MessageBox.Show(EncycloRes.InterruptSearch, EncycloRes.Searching, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)
				== System.Windows.Forms.DialogResult.Yes)
			{
				Searcher.CancelAsync();
			}
			return;
		}

		StatusInfoText = EncycloRes.SearchingNow;
		StatusInfoTag = DateTime.Now;

		Searcher.RunWorkerAsync();
	}

	[RelayCommand]
	private void SelectToday(Calendar? calendar)
	{
		if (calendar is null) return;

		calendar.SelectedDate = DateTime.Now.Date;
	}

	public void RecordView_CellDoubleClick()
	{
		if (MergeRows) return;

		try
		{
			if (SelectedRows.FirstOrDefault()?.Date is not { } time) return;

			if (!Directory.Exists(Data.Battle.BattleManager.BattleLogPath))
			{
				StatusInfoText = DialogDropRecordViewer.BattleHistoryNotFound;
				return;
			}

			StatusInfoText = DialogDropRecordViewer.SearchingBattleHistory;
			string? battleLogFile = Directory.EnumerateFiles(Data.Battle.BattleManager.BattleLogPath,
					time.ToString("yyyyMMdd_HHmmss", System.Globalization.CultureInfo.InvariantCulture) + "*.txt",
					SearchOption.TopDirectoryOnly)
				.FirstOrDefault();

			if (battleLogFile is null)
			{
				StatusInfoText = DialogDropRecordViewer.BattleHistoryNotFound;
				return;
			}

			StatusInfoText = string.Format(DialogDropRecordViewer.OpenBattleHistory, Path.GetFileName(battleLogFile));
			ProcessStartInfo psi = new ProcessStartInfo
			{
				FileName = battleLogFile,
				UseShellExecute = true
			};
			Process.Start(psi);
		}
		catch (Exception)
		{
			StatusInfoText = DialogDropRecordViewer.CouldNotOpenBattleHistory;
		}

	}
}
