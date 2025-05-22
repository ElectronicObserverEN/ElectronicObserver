using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Common.Datagrid;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Tools.ConstructionRecordViewer;

public partial class ConstructionRecordViewerViewModel : WindowViewModelBase
{
	public DialogConstructionRecordViewerTranslationViewModel DialogConstructionRecordViewer { get; }
	private ConstructionRecord Record { get; }
	private BackgroundWorker Searcher { get; } = new();

	public List<object> Categories { get; set; }
	public List<object> Ships { get; set; }
	public List<object> FlagshipTypes { get; set; }
	private List<object> AllFlagships { get; set; }
	public List<object> Flagships { get; set; }
	public List<object> Recipes { get; set; }
	public List<object> DevelopmentMaterials { get; set; }
	public List<object> EmptyDockCounts { get; set; }

	public List<ConstructionRecordRow> SelectedRows { get; set; } = new();

	public object SelectedShipType { get; set; } = ConstructionRecordOption.All;
	public object SelectedShip { get; set; } = ConstructionRecordOption.All;
	public object SelectedFlagshipType { get; set; } = ConstructionRecordOption.All;
	public object? SelectedFlagship { get; set; } = ConstructionRecordOption.All;
	public object SelectedRecipe { get; set; } = ConstructionRecordOption.All;
	public object SelectedDevelopmentMaterial { get; set; } = ConstructionRecordOption.All;
	public object EmptyDockCount { get; set; } = ConstructionRecordOption.All;
	public bool? IsLargeConstruction { get; set; }

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

	public bool MergeRows { get; set; }
	public bool RawRows => !MergeRows;
	public bool MergedShipNameVisible => MergeRows && SelectedShip is ConstructionRecordOption.All;
	public bool MergedRecipeVisible => MergeRows && SelectedShip is IShipDataMaster;
	public string StatusInfoText { get; set; }
	private DateTime StatusInfoTag { get; set; }

	public string Today => $"{DialogConstructionRecordViewer.Today}: {DateTime.Now:yyyy/MM/dd}";

	private ObservableCollection<ConstructionRecordRow> RecordRows { get; set; } = new();
	public DataGridViewModel<ConstructionRecordRow> DataGridRawRowsViewModel { get; set; }
	public DataGridViewModel<ConstructionRecordRow> DataGridMergedRowsAllViewModel { get; set; }
	public DataGridViewModel<ConstructionRecordRow> DataGridMergedRowsFilteredByShipViewModel { get; set; }

	public ConstructionRecordViewerViewModel()
	{
		DataGridRawRowsViewModel = new(RecordRows);
		DataGridMergedRowsAllViewModel = new(RecordRows);
		DataGridMergedRowsFilteredByShipViewModel = new(RecordRows);

		DialogConstructionRecordViewer = Ioc.Default.GetService<DialogConstructionRecordViewerTranslationViewModel>()!;
		Record = RecordManager.Instance.Construction;

		Searcher.WorkerSupportsCancellation = true;
		Searcher.DoWork += Searcher_DoWork;
		Searcher.RunWorkerCompleted += Searcher_RunWorkerCompleted;

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(SelectedRows)) return;

			RecordView_SelectionChanged();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not (nameof(MergeRows) or nameof(IsLargeConstruction) or nameof(SelectedShip))) return;

			RecordRows.Clear();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not (nameof(SelectedFlagshipType) or nameof(AllFlagships))) return;

			if (SelectedFlagshipType is not ShipTypes flagshipType)
			{
				Flagships = AllFlagships!;
				return;
			}

			Flagships = AllFlagships!
				.Where(f => f is IShipDataMaster ship && ship.ShipType == flagshipType)
				.Prepend(ConstructionRecordOption.All)
				.ToList();

			SelectedFlagship ??= ConstructionRecordOption.All;
		};

		Load();
	}

	private void Load()
	{
		DateBegin = Record.Record.First().Date.Date;
		MinDate = Record.Record.First().Date.Date;

		DateEnd = DateTime.Now.AddDays(1).Date;
		MaxDate = DateTime.Now.AddDays(1).Date;

		var includedShipNames = Record.Record
			.Select(r => r.ShipName)
			.Distinct();

		var includedShipObjects = includedShipNames
			.Select(name => KCDatabase.Instance.MasterShips.Values.FirstOrDefault(ship => ship.NameWithClass == name))
			.Where(s => s != null);

		var categories = includedShipObjects
			.Select(s => s?.ShipType)
			.Distinct();

		Categories = categories
			.OrderBy(c => c)
			.Cast<object>()
			.Prepend(ConstructionRecordOption.All)
			.ToList();

		Ships = includedShipObjects
			.OrderBy(s => s?.ShipId)
			.Cast<object>()
			.Prepend(ConstructionRecordOption.All)
			.ToList();

		FlagshipTypes = Record.Record
			.Select(s => s.FlagshipID)
			.Select(id => KCDatabase.Instance.MasterShips[id])
			.Select(s => s.ShipType)
			.OrderBy(t => t)
			.Distinct()
			.Cast<object>()
			.Prepend(ConstructionRecordOption.All)
			.ToList();

		AllFlagships = Record.Record
			.Select(s => s.FlagshipID)
			.Select(id => KCDatabase.Instance.MasterShips[id])
			.DistinctBy(s => s.ShipId)
			.OrderBy(s => s.ShipId)
			.Cast<object>()
			.Prepend(ConstructionRecordOption.All)
			.ToList();

		Recipes = Record.Record
			.Select(r => (Sort: GetRecipeStringForSorting(r), Recipe: GetRecipeString(r)))
			.DistinctBy(t => t.Sort)
			.OrderBy(t => t.Sort)
			.Select(t => t.Recipe)
			.Cast<object>()
			.Prepend(ConstructionRecordOption.All)
			.ToList();

		DevelopmentMaterials = Record.Record
			.DistinctBy(c => c.DevelopmentMaterial)
			.Select(c => c.DevelopmentMaterial)
			.OrderBy(d => d)
			.Cast<object>()
			.Prepend(ConstructionRecordOption.All)
			.ToList();

		EmptyDockCounts = Record.Record
			.DistinctBy(c => c.EmptyDockAmount)
			.Select(c => c.EmptyDockAmount)
			.OrderBy(d => d)
			.Cast<object>()
			.Prepend(ConstructionRecordOption.All)
			.ToList();
	}

	private string GetRecipeString(int[] resources)
	{
		return string.Join("/", resources);
	}

	private string GetRecipeString(ConstructionRecord.ConstructionElement record, bool containsDevmat = false)
	{
		if (containsDevmat)
			return GetRecipeString(new[] { record.Fuel, record.Ammo, record.Steel, record.Bauxite, record.DevelopmentMaterial });
		else
			return GetRecipeString(new[] { record.Fuel, record.Ammo, record.Steel, record.Bauxite });
	}

	private string GetRecipeStringForSorting(int[] resources)
	{
		return string.Join("/", resources.Select(r => r.ToString("D4")));
	}

	private string GetRecipeStringForSorting(ConstructionRecord.ConstructionElement record, bool containsDevmat = false)
	{
		if (containsDevmat)
			return GetRecipeStringForSorting(new[] { record.Fuel, record.Ammo, record.Steel, record.Bauxite, record.DevelopmentMaterial });
		else
			return GetRecipeStringForSorting(new[] { record.Fuel, record.Ammo, record.Steel, record.Bauxite });
	}

	private void Searcher_DoWork(object sender, DoWorkEventArgs e)
	{
		List<ConstructionRecord.ConstructionElement> records = RecordManager.Instance.Construction.Record;
		LinkedList<ConstructionRecordRow> rows = new();

		int priorityShip = SelectedShip switch
		{
			ConstructionRecordOption.All => 0,
			_ => 1
		};

		int i = 0;
		Dictionary<string, int[]> counts = new();
		Dictionary<string, int[]> allcounts = new();


		foreach (var r in records)
		{

			#region filtering

			var ship = KCDatabase.Instance.MasterShips[r.ShipID];
			var secretary = KCDatabase.Instance.MasterShips[r.FlagshipID];

			if (ship != null && ship.NameEN != r.ShipName) ship = null;
			if (secretary != null && secretary.NameEN != r.FlagshipName) secretary = null;


			if (SelectedFlagshipType is ShipTypes type && (secretary == null || type != secretary.ShipType))
				continue;

			if (SelectedFlagship is IShipDataMaster selectedFlagship && (secretary == null || selectedFlagship.NameWithClass != secretary.NameWithClass))
				continue;

			if (r.Date < DateTimeBegin || DateTimeEnd < r.Date) continue;
			if (SelectedDevelopmentMaterial is int developmentMaterial && developmentMaterial != r.DevelopmentMaterial) continue;
			if (EmptyDockCount is int emptyDockCount && emptyDockCount != r.EmptyDockAmount) continue;
			if (IsLargeConstruction is { } isLargeConstruction && isLargeConstruction != r.IsLargeDock) continue;

			if (MergeRows)
			{
				string key = GetRecipeString(r);

				if (!allcounts.ContainsKey(key))
					allcounts.Add(key, new int[4]);

				allcounts[key][0]++;

				switch (r.DevelopmentMaterial)
				{
					case 100:
						allcounts[key][1]++;
						break;
					case 20:
						allcounts[key][2]++;
						break;
					case 1:
						allcounts[key][3]++;
						break;
				}
			}

			if (SelectedShipType is ShipTypes selectedShipType && (ship == null || selectedShipType != ship.ShipType)) continue;
			if (SelectedShip is IShipDataMaster selectedShip && selectedShip.ShipID != r.ShipID) continue;
			if (SelectedRecipe is string selectedRecipe && selectedRecipe != GetRecipeString(r)) continue;

			#endregion


			if (!MergeRows)
			{
				ConstructionRecordRow row = new
				(
					i + 1,
					r.ShipName,
					r.Date,
					GetRecipeString(r, true),
					r.FlagshipName,
					null,
					null,
					null
				);

				row.CellTag1 = ((int?)ship?.ShipType ?? 0).ToString("D4") + (ship?.NameReading ?? r.ShipName);
				row.CellTag3 = GetRecipeStringForSorting(r, true);
				row.CellTag4 = ((int?)secretary?.ShipType ?? 0).ToString("D4") + (secretary?.NameReading ?? r.FlagshipName);

				rows.AddLast(row);
			}
			else
			{
				string key = priorityShip switch
				{
					> 0 => GetRecipeString(r),
					_ => r.ShipName
				};

				if (!counts.ContainsKey(key))
					counts.Add(key, new int[4]);

				counts[key][0]++;

				switch (r.DevelopmentMaterial)
				{
					case 100:
						counts[key][1]++;
						break;
					case 20:
						counts[key][2]++;
						break;
					case 1:
						counts[key][3]++;
						break;
				}

			}

			if (Searcher.CancellationPending)
				return;

			i++;
		}


		if (MergeRows)
		{
			foreach (var (key, value) in counts)
			{
				ConstructionRecordRow row;

				if (priorityShip > 0)
				{
					row = new
					(
						value[0],
						null,
						null,
						key,
						null,
						value[1],
						value[2],
						value[3]
					);

					row.CellTag3 = GetRecipeStringForSorting(key.Split("/".ToCharArray()).Select(s => int.Parse(s)).ToArray());

					row.CellTag0 = allcounts[key][0];
					row.CellTag5 = allcounts[key][1];
					row.CellTag6 = allcounts[key][2];
					row.CellTag7 = allcounts[key][3];

				}
				else
				{

					row = new(
						value[0],
						key,
						null,
						null,
						null,
						value[1],
						value[2],
						value[3]
					);

					var ship = KCDatabase.Instance.MasterShips.Values.FirstOrDefault(s => s.NameEN == key);
					row.CellTag1 = (ship != null ? (int)ship.ShipType : 0).ToString("D4") + (ship?.NameReading ?? key);

					if (SelectedRecipe is string selectedRecipe)
					{
						row.CellTag0 = (double)value[0] / Math.Max(allcounts[selectedRecipe][0], 1);
						row.CellTag5 = (double)value[1] / Math.Max(allcounts[selectedRecipe][1], 1);
						row.CellTag6 = (double)value[2] / Math.Max(allcounts[selectedRecipe][2], 1);
						row.CellTag7 = (double)value[3] / Math.Max(allcounts[selectedRecipe][3], 1);

					}
					else
					{
						row.CellTag0 = (double)value[0] / Math.Max(allcounts.Values.Sum(a => a[0]), 1);
						row.CellTag5 = (double)value[1] / Math.Max(allcounts.Values.Sum(a => a[1]), 1);
						row.CellTag6 = (double)value[2] / Math.Max(allcounts.Values.Sum(a => a[2]), 1);
						row.CellTag7 = (double)value[3] / Math.Max(allcounts.Values.Sum(a => a[3]), 1);
					}

				}

				rows.AddLast(row);

				if (Searcher.CancellationPending)
					return;
			}

		}

		e.Result = rows
			.OrderByDescending(r => r.Index)
			.ToList();
	}

	private void Searcher_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (!e.Cancelled)
		{
			if (e.Result is not List<ConstructionRecordRow> rows) return;

			RecordRows = new(rows);
			DataGridRawRowsViewModel.ItemsSource = RecordRows;
			DataGridMergedRowsAllViewModel.ItemsSource = RecordRows;
			DataGridMergedRowsFilteredByShipViewModel.ItemsSource = RecordRows;

			StatusInfoText = EncycloRes.SearchComplete + "(" + (int)(DateTime.Now - StatusInfoTag).TotalMilliseconds + " ms)";
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

		if (!MergeRows)
		{
			int allcount = RecordRows.Count;
			StatusInfoText = string.Format(DropRecordViewerResources.SelectedItems,
				selectedCount, allcount, (double)selectedCount / allcount);
		}
	}

	[RelayCommand]
	private void StartSearch()
	{
		if (Searcher.IsBusy)
		{
			if (MessageBox.Show(EncycloRes.InterruptSearch, EncycloRes.Searching, MessageBoxButton.YesNo, MessageBoxImage.Exclamation, MessageBoxResult.No)
				== MessageBoxResult.Yes)
			{
				Searcher.CancelAsync();
			}
			return;
		}

		RecordRows.Clear();

		StatusInfoText = EncycloRes.Searching + "...";
		StatusInfoTag = DateTime.Now;

		Searcher.RunWorkerAsync();

	}

	[RelayCommand]
	private void SelectToday(Calendar? calendar)
	{
		if (calendar is null) return;

		calendar.SelectedDate = DateTime.Now.Date;
	}
}
