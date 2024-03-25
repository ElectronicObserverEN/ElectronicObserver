using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Common.Datagrid;
using ElectronicObserver.Data;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog.QuestTrackerManager.Enums;
using ElectronicObserver.Window.Dialog.ShipPicker;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.DropRecordViewer;

public sealed partial class DropRecordViewerViewModel : WindowViewModelBase
{
	public DialogDropRecordViewerTranslationViewModel DialogDropRecordViewer { get; }

	private ShipDropRecord Record { get; }

	private ShipPickerViewModel ShipPickerViewModel { get; }
	public List<object> Items { get; set; } = [];
	public List<ShipTypes> ShipTypeOptions { get; set; } = [];

	public List<object> Worlds { get; set; } = [];
	public List<object> Maps { get; set; } = [];
	public List<MapNode> Cells { get; set; } = [new(MapAny)];
	public List<object> Difficulties { get; set; } = [MapAny];

	public List<DropRecordRowBase> SelectedRows { get; set; } = [];

	// DropRecordOption or IShipDataMaster
	public object ShipSearchOption { get; set; } = DropRecordOption.All;
	// DropRecordOption or UseItemMaster
	public object ItemSearchOption { get; set; } = DropRecordOption.All;
	public ShipTypes ShipTypeSearchOption { get; set; } = ShipTypes.All;

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
	private DateTime SearchStartTime { get; set; }

	private string NameNotExist => DialogDropRecordViewer.NameNotExist;
	private const string MapAny = "*";

	public string Today => $"{DialogDropRecordViewer.Today}: {DateTime.Now:yyyy/MM/dd}";

	public bool IgnoreCommonDrops { get; set; }

	private int NumberOfRecords { get; set; }

	private ObservableCollection<DropRecordRow> RecordRows { get; set; } = [];
	private ObservableCollection<MergedDropRecordRow> MergedRecordRows { get; set; } = [];
	public DataGridViewModel<DropRecordRow> DataGridRawRowsViewModel { get; }
	public DataGridViewModel<MergedDropRecordRow> DataGridMergedRowsViewModel { get; }

	public DropRecordViewerViewModel()
	{
		Record = RecordManager.Instance.ShipDrop;

		DataGridRawRowsViewModel = new(RecordRows);
		DataGridMergedRowsViewModel = new(MergedRecordRows);

		ShipPickerViewModel = Ioc.Default.GetService<ShipPickerViewModel>()!;
		DialogDropRecordViewer = Ioc.Default.GetService<DialogDropRecordViewerTranslationViewModel>()!;

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
				_ => KCDatabase.Instance.Translation.Destination.CellDisplay(world, map, id)
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
			MergedRecordRows.Clear();
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

	public override void Loaded()
	{
		base.Loaded();

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

		IEnumerable<IUseItemMaster> includedItemObjects = includedItemNames
			.Select(id => KCDatabase.Instance.MasterUseItems.Values.FirstOrDefault(item => item.ItemID == id))
			.OfType<IUseItemMaster>();

		Items = includedItemObjects
			.Cast<object>()
			.Prepend(DropRecordOption.NoDrop)
			.Prepend(DropRecordOption.Drop)
			.Prepend(DropRecordOption.All)
			.ToList();

		ShipTypeOptions = Enum.GetValues<ShipTypes>()
			.Where(t => t is not ShipTypes.Unknown)
			.Where(t => t is not ShipTypes.SuperDreadnoughts)
			.ToList();

		Difficulties = Record.Record
			.Select(r => r.Difficulty)
			.Distinct()
			.Where(d => d is not 0)
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

	private string GetContentStringForSorting(ShipDropRecord.ShipDropElement r, int priorityShip, int priorityItem)
	{
		bool ignoreShip = priorityShip < priorityItem && priorityShip < 2;
		bool ignoreItem = priorityShip >= priorityItem && priorityItem < 2;

		return GetContentStringForSorting(r, ignoreShip, ignoreItem);
	}

	private string GetContentStringForSorting(ShipDropRecord.ShipDropElement elem, bool ignoreShip = false, bool ignoreItem = false, bool ignoreEquipment = false)
	{
		IShipDataMaster? ship = KCDatabase.Instance.MasterShips[elem.ShipID];
		IUseItemMaster? item = KCDatabase.Instance.MasterUseItems[elem.ItemID];
		IEquipmentDataMaster? eq = KCDatabase.Instance.MasterEquipments[elem.EquipmentID];

		if (ship != null && ship.NameEN != elem.ShipName) ship = null;
		if (item != null && item.Name != elem.ItemName) item = null;
		if (eq != null && eq.NameEN != elem.EquipmentName) eq = null;

		StringBuilder sb = new();

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
		StringBuilder sb = new();
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
				sb.Append($" ({enemy.FleetName})");
		}

		return sb.ToString();
	}

	private string GetMapString(int serialID, bool insertEnemyFleetName = true)
	{
		return GetMapString(serialID >> 24 & 0xFF, serialID >> 16 & 0xFF, serialID >> 8 & 0xFF, (serialID & 1) != 0, (sbyte)((serialID >> 1 & 0x7F) << 1) >> 1, insertEnemyFleetName);
	}

	private int GetMapSerialId(ShipDropRecord.ShipDropElement r, int difficulty)
	{
		return GetMapSerialId(r.MapAreaID, r.MapInfoID, r.CellID, r.IsBossNode, difficulty);
	}

	private int GetMapSerialId(int mapAreaId, int mapInfoId, int cell, bool isboss, int difficulty = -1)
	{
		return (mapAreaId & 0xFF) << 24 | (mapInfoId & 0xFF) << 16 | (cell & 0xFF) << 8 | (difficulty & 0x7F) << 1 | (isboss ? 1 : 0);
	}

	[RelayCommand(IncludeCancelCommand = true)]
	private async Task Search(CancellationToken cancellationToken)
	{
		SearchStartTime = DateTime.UtcNow;
		StatusInfoText = EncycloRes.SearchingNow;

		try
		{
			if (MergeRows)
			{
				IEnumerable<MergedDropRecordRow> rows = await Task.Run(MakeMergedDropRecordRows, cancellationToken);

				MergedRecordRows = new(rows);
				DataGridMergedRowsViewModel.ItemsSource = MergedRecordRows;
			}
			else
			{
				IEnumerable<DropRecordRow> rows = await Task.Run(MakeDropRecordRows, cancellationToken);

				RecordRows = new(rows);
				DataGridRawRowsViewModel.ItemsSource = RecordRows;
			}

			StatusInfoText = $"{EncycloRes.SearchComplete} ({(int)(DateTime.UtcNow - SearchStartTime).TotalMilliseconds} ms)";
		}
		catch (OperationCanceledException)
		{
			StatusInfoText = EncycloRes.SearchCancelled;
		}
		catch (Exception e)
		{
		}
	}

	private bool ShouldIncludeInMergedCount(ShipDropRecord.ShipDropElement r)
	{
		if (r.Date < DateTimeBegin || DateTimeEnd < r.Date) return false;
		if (r.Rank is "SS" or "S" && !RankS) return false;
		if (r.Rank is "A" && !RankA) return false;
		if (r.Rank is "B" && !RankB) return false;
		if (Constants.GetWinRank(r.Rank) <= BattleRank.C && !RankX) return false;

		if (MapAreaID is int world && world != r.MapAreaID) return false;
		if (MapInfoID is int map && map != r.MapInfoID) return false;
		if (MapCellID.Ids?.Contains(r.CellID) == false) return false;

		if (IsBossOnly is false && r.IsBossNode) return false;
		if (IsBossOnly is true && !r.IsBossNode) return false;

		if (MapDifficulty is int difficulty && difficulty != r.Difficulty) return false;

		return true;
	}

	private bool ShouldIncludeRecord(ShipDropRecord.ShipDropElement r)
	{
		if (ShipSearchOption is DropRecordOption.Drop && r.ShipID < 0) return false;
		if (ShipSearchOption is DropRecordOption.NoDrop && r.ShipID != -1) return false;
		if (ShipSearchOption is DropRecordOption.FullPort && r.ShipID != -2) return false;
		if (ShipSearchOption is IShipDataMaster ship && ship.ShipID != r.ShipID) return false;

		if (ItemSearchOption is DropRecordOption.Drop && r.ItemID < 0) return false;
		if (ItemSearchOption is DropRecordOption.NoDrop && r.ItemID != -1) return false;
		if (ItemSearchOption is UseItemMaster item && item.ID != r.ItemID) return false;

		if (ShipTypeSearchOption is not ShipTypes.All)
		{
			if (KCDatabase.Instance.MasterShips[r.ShipID]?.ShipType != ShipTypeSearchOption)
			{
				return false;
			}
		}

		return true;
	}

	private IEnumerable<DropRecordRow> MakeDropRecordRows()
	{
		NumberOfRecords = 0;

		IEnumerable<DropRecordRow> rows = RecordManager.Instance.ShipDrop.Record
			.Where(ShouldIncludeInMergedCount)
			.Where(ShouldIncludeRecord)
			.Select((r, i) => new DropRecordRow
			{
				Index = i + 1,
				Name = GetContentString(r),
				Date = r.Date,
				MapDescription = GetMapString(r.MapAreaID, r.MapInfoID, r.CellID, r.IsBossNode, r.Difficulty),
				Rank = Constants.GetWinRank(r.Rank),
				ShipId = (ShipId)r.ShipID,
			})
			.ToList();

		NumberOfRecords = rows.Count();

		if (IgnoreCommonDrops)
		{
			rows = rows
				.Where(r => r.ShipId > ShipId.Unknown)
				.Where(r => !IsCommonDrop(r.ShipId));
		}

		return rows.OrderByDescending(r => r.Index);
	}

	private IEnumerable<MergedDropRecordRow> MakeMergedDropRecordRows()
	{
		NumberOfRecords = 0;
		List<MergedDropRecordRow> rows = [];

		int priorityShip = ShipSearchOption switch
		{
			DropRecordOption.All => 0,
			DropRecordOption.Drop => 1,
			_ => 2,
		};

		int priorityItem = ItemSearchOption switch
		{
			DropRecordOption.All => 0,
			DropRecordOption.Drop => 1,
			_ => 2,
		};

		int priorityContent = Math.Max(priorityShip, priorityItem);

		Dictionary<string, int[]> counts = [];
		Dictionary<string, int[]> allcounts = [];

		foreach (ShipDropRecord.ShipDropElement r in RecordManager.Instance.ShipDrop.Record)
		{
			if (!ShouldIncludeInMergedCount(r)) continue;

			{
				string key = priorityContent switch
				{
					2 => GetMapSerialId(r, MapDifficulty is 0 ? -1 : r.Difficulty).ToString("X8"),
					_ => GetContentString(r, priorityShip < priorityItem && priorityShip < 2, priorityShip >= priorityItem && priorityItem < 2),
				};

				if (!allcounts.TryGetValue(key, out int[]? value))
				{
					value = new int[4];
					allcounts.Add(key, value);
				}

				switch (r.Rank)
				{
					case "B":
						value[3]++;
						break;
					case "A":
						value[2]++;
						break;
					case "S":
					case "SS":
						value[1]++;
						break;
				}

				value[0]++;
			}

			if (!ShouldIncludeRecord(r)) continue;

			{
				string key = priorityContent switch
				{
					2 => GetMapSerialId(r, MapDifficulty is 0 ? -1 : r.Difficulty).ToString("X8"),
					_ => GetContentStringForSorting(r, priorityShip, priorityItem),
				};

				if (!counts.TryGetValue(key, out int[]? value))
				{
					value = (new int[4]);
					counts.Add(key, value);
				}

				switch (r.Rank)
				{
					case "B":
						value[3]++;
						break;
					case "A":
						value[2]++;
						break;
					case "S":
					case "SS":
						value[1]++;
						break;
				}

				value[0]++;
			}

			NumberOfRecords++;
		}

		int[] allcountssum = Enumerable.Range(0, 4)
			.Select(k => allcounts.Values.Sum(a => a[k]))
			.ToArray();

		foreach ((string key, int[] value) in counts)
		{
			string name = key;

			if (int.TryParse(name, System.Globalization.NumberStyles.HexNumber,
					System.Globalization.CultureInfo.InvariantCulture, out int serialId))
			{
				name = GetMapString(serialId);
			}

			// fixme: name != map だった時にソートキーが入れられない

			MergedDropRecordRow row = new()
			{
				Count = value[0],
				Name = serialId switch
				{
					0 => ConvertContentString(name),
					_ => name,
				},
				CountS = value[1],
				CountA = value[2],
				CountB = value[3],
			};

			if (priorityContent == 2)
			{
				row.RateOrMaxCountTotal = allcounts[key][0];
				row.RateOrMaxCountS = allcounts[key][1];
				row.RateOrMaxCountA = allcounts[key][2];
				row.RateOrMaxCountB = allcounts[key][3];
			}
			else
			{
				row.RateOrMaxCountTotal = ((double)value[0] / Math.Max(allcountssum[0], 1));
				row.RateOrMaxCountS = ((double)value[1] / Math.Max(allcountssum[1], 1));
				row.RateOrMaxCountA = ((double)value[2] / Math.Max(allcountssum[2], 1));
				row.RateOrMaxCountB = ((double)value[3] / Math.Max(allcountssum[3], 1));
			}

			rows.Add(row);
		}

		return rows.OrderByDescending(r => r.Count);
	}

	private void RecordView_SelectionChanged()
	{
		int selectedCount = SelectedRows.Count;

		if (selectedCount == 0) return;

		int allCount = NumberOfRecords;

		if (MergeRows)
		{
			int count = SelectedRows.OfType<MergedDropRecordRow>().Select(r => r.Count).Sum();

			StatusInfoText = string.Format(DialogDropRecordViewer.SelectedItems,
				count, allCount, (double)count / allCount);
		}
		else
		{
			StatusInfoText = string.Format(DialogDropRecordViewer.SelectedItems,
				selectedCount, allCount, (double)selectedCount / allCount);
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
			if (SelectedRows.FirstOrDefault() is not DropRecordRow row) return;

			if (!Directory.Exists(Data.Battle.BattleManager.BattleLogPath))
			{
				StatusInfoText = DialogDropRecordViewer.BattleHistoryNotFound;
				return;
			}

			StatusInfoText = DialogDropRecordViewer.SearchingBattleHistory;
			string? battleLogFile = Directory.EnumerateFiles(Data.Battle.BattleManager.BattleLogPath,
					row.Date.ToString("yyyyMMdd_HHmmss", System.Globalization.CultureInfo.InvariantCulture) + "*.txt",
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
				UseShellExecute = true,
			};
			Process.Start(psi);
		}
		catch (Exception)
		{
			StatusInfoText = DialogDropRecordViewer.CouldNotOpenBattleHistory;
		}
	}

	private static bool IsCommonDrop(ShipId shipId) => shipId is
		ShipId.Mutsuki or
		ShipId.Kisaragi or
		ShipId.Nagatsuki or
		ShipId.Mikazuki or
		ShipId.Fubuki or
		ShipId.Shirayuki or
		ShipId.Miyuki or
		ShipId.Isonami or
		ShipId.Ayanami or
		ShipId.Shikinami or
		ShipId.Akebono or
		ShipId.Ushio or
		ShipId.Kagerou or
		ShipId.Shiranui or
		ShipId.Kuroshio or
		ShipId.Yukikaze or
		ShipId.Nagara or
		ShipId.Isuzu or
		ShipId.Yura or
		ShipId.Ooi or
		ShipId.Kitakami or
		ShipId.Fusou or
		ShipId.Yamashiro or
		ShipId.Satsuki or
		ShipId.Fumizuki or
		ShipId.Kikuzuki or
		ShipId.Mochizuki or
		ShipId.Hatsuyuki or
		ShipId.Murakumo or
		ShipId.Akatsuki or
		ShipId.Hibiki or
		ShipId.Ikazuchi or
		ShipId.Inazuma or
		ShipId.Hatsuharu or
		ShipId.Nenohi or
		ShipId.Wakaba or
		ShipId.Hatsushimo or
		ShipId.Shiratsuyu or
		ShipId.Shigure or
		ShipId.Murasame or
		ShipId.Yuudachi or
		ShipId.Samidare or
		ShipId.Suzukaze or
		ShipId.Arare or
		ShipId.Kasumi or
		ShipId.Shimakaze or
		ShipId.Tenryuu or
		ShipId.Tatsuta or
		ShipId.Natori or
		ShipId.Sendai or
		ShipId.Jintsuu or
		ShipId.Naka or
		ShipId.Furutaka or
		ShipId.Kako or
		ShipId.Aoba or
		ShipId.Myoukou or
		ShipId.Nachi or
		ShipId.Ashigara or
		ShipId.Haguro or
		ShipId.Takao or
		ShipId.Atago or
		ShipId.Maya or
		ShipId.Choukai or
		ShipId.Mogami or
		ShipId.Tone or
		ShipId.Chikuma or
		ShipId.Shouhou or
		ShipId.Hiyou or
		ShipId.Ryuujou or
		ShipId.Ise or
		ShipId.Kongou or
		ShipId.Haruna or
		ShipId.Nagato or
		ShipId.Mutsu or
		ShipId.Akagi or
		ShipId.Kaga or
		ShipId.Kirishima or
		ShipId.Hiei or
		ShipId.Hyuuga or
		ShipId.Houshou or
		ShipId.Souryuu or
		ShipId.Hiryuu or
		ShipId.Junyou or
		ShipId.Oboro or
		ShipId.Sazanami or
		ShipId.Asashio or
		ShipId.Ooshio or
		ShipId.Michishio or
		ShipId.Arashio or
		ShipId.Kuma or
		ShipId.Tama or
		ShipId.Kiso or
		ShipId.Chitose or
		ShipId.Chiyoda or
		ShipId.Shoukaku or
		ShipId.Zuikaku or
		ShipId.Kinu or
		ShipId.Abukuma or
		ShipId.Yuubari or
		ShipId.Zuihou or
		ShipId.Mikuma or
		ShipId.Maikaze or
		ShipId.Kinugasa or
		ShipId.Suzuya or
		ShipId.Kumano or
		ShipId.I168 or
		ShipId.I58 or
		ShipId.I8 or
		ShipId.Akigumo or
		ShipId.Yuugumo or
		ShipId.Makigumo or
		ShipId.Naganami or
		ShipId.Taigei or
		ShipId.I19 or
		ShipId.Hayashimo or
		ShipId.Kiyoshimo or
		ShipId.Asagumo or
		ShipId.Yamagumo or
		ShipId.Nowaki or
		ShipId.Asashimo;
}
