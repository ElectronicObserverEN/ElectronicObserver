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

namespace ElectronicObserver.Window.Tools.DevelopmentRecordViewer;

public partial class DevelopmentRecordViewerViewModel : WindowViewModelBase
{
	public DialogDevelopmentRecordViewerTranslationViewModel DialogDevelopmentRecordViewer { get; }
	private DevelopmentRecord Record { get; }
	private BackgroundWorker Searcher { get; } = new();

	public List<object> Categories { get; set; }
	public List<object> Equipment { get; set; }
	public List<object> FlagshipTypes { get; set; }
	private List<object> AllFlagships { get; set; }
	public List<object> Flagships { get; set; }
	public List<object> Recipes { get; set; }

	public List<DevelopmentRecordRow> SelectedRows { get; set; } = new();

	public object SelectedCategory { get; set; } = DevelopmentRecordOption.All;
	public object SelectedEquipment { get; set; } = DevelopmentRecordOption.All;
	public object SelectedFlagshipType { get; set; } = DevelopmentRecordOption.All;
	public object? SelectedFlagship { get; set; } = DevelopmentRecordOption.All;
	public object SelectedRecipe { get; set; } = DevelopmentRecordOption.All;

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
	public string StatusInfoText { get; set; }
	private DateTime StatusInfoTag { get; set; }

	public string SummaryHeader => (SelectedFlagship, SelectedFlagshipType) switch
	{
		(DevelopmentRecordOption.All, DevelopmentRecordOption.All) => DialogDevelopmentRecordViewer.ShipType,
		_ => DialogDevelopmentRecordViewer.RecipeTries
	};

	private string NameNotExist => DevelopmentRecordViewerResources.NameNotExist; //(失敗)

	public string Today => $"{DialogDevelopmentRecordViewer.Today}: {DateTime.Now:yyyy/MM/dd}";

	private ObservableCollection<DevelopmentRecordRow> RecordRows { get; set; } = new();
	public DataGridViewModel<DevelopmentRecordRow> DataGridRawRowsViewModel { get; set; }
	public DataGridViewModel<DevelopmentRecordRow> DataGridMergedRowsViewModel { get; set; }

	public DevelopmentRecordViewerViewModel()
	{
		DataGridRawRowsViewModel = new(RecordRows);
		DataGridMergedRowsViewModel = new(RecordRows);

		Record = RecordManager.Instance.Development;
		DialogDevelopmentRecordViewer = Ioc.Default.GetService<DialogDevelopmentRecordViewerTranslationViewModel>()!;

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
			if (args.PropertyName is not nameof(MergeRows)) return;

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
				.Prepend(DevelopmentRecordOption.All)
				.ToList();

			SelectedFlagship ??= DevelopmentRecordOption.All;
		};

		Loaded();
	}

	private void Loaded()
	{
		DateBegin = Record.Record.First().Date.Date;
		MinDate = Record.Record.First().Date.Date;

		DateEnd = DateTime.Now.AddDays(1).Date;
		MaxDate = DateTime.Now.AddDays(1).Date;

		var includedEquipmentNames = Record.Record
			.Select(r => r.EquipmentName)
			.Distinct()
			.Except(new[] { NameNotExist });

		IEnumerable<IEquipmentDataMaster> includedEquipmentObjects = includedEquipmentNames
			.Select(name => KCDatabase.Instance.MasterEquipments.Values.FirstOrDefault(eq => eq.NameEN == name))
			.Where(s => s != null)!;

		var removedEquipments = includedEquipmentNames.Except(includedEquipmentObjects.Select(eq => eq.NameEN));

		var includedSecretaryNames = Record.Record
			.Select(r => r.FlagshipName).Distinct();

		IEnumerable<IShipDataMaster> includedSecretaryObjects = includedSecretaryNames
			.Select(name => KCDatabase.Instance.MasterShips.Values.FirstOrDefault(ship => ship.NameWithClass == name))
			.Where(s => s != null)!;

		var removedSecretaryNames = includedSecretaryNames.Except(includedSecretaryObjects.Select(s => s.NameWithClass));

		var categories = includedEquipmentObjects
			.Select(e => e.CategoryType)
			.Distinct();

		Categories = categories
			.OrderBy(c => c)
			.Cast<object>()
			.Prepend(DevelopmentRecordOption.All)
			.ToList();

		Equipment = includedEquipmentObjects
			.OrderBy(e => e.EquipmentId)
			.Cast<object>()
			.Prepend(DevelopmentRecordOption.Failure)
			.Prepend(DevelopmentRecordOption.Success)
			.Prepend(DevelopmentRecordOption.All)
			.ToList();

		FlagshipTypes = Record.Record
			.Select(s => s.FlagshipID)
			.Select(id => KCDatabase.Instance.MasterShips[id])
			.Select(s => s.ShipType)
			.OrderBy(t => t)
			.Distinct()
			.Cast<object>()
			.Prepend(DevelopmentRecordOption.All)
			.ToList();

		AllFlagships = Record.Record
			.Select(s => s.FlagshipID)
			.Select(id => KCDatabase.Instance.MasterShips[id])
			.DistinctBy(s => s.ShipId)
			.OrderBy(s => s.ShipId)
			.Cast<object>()
			.Prepend(DevelopmentRecordOption.All)
			.ToList();

		Recipes = Record.Record
			.Select(GetRecipeStringForSorting)
			.Distinct()
			.OrderBy(s => s)
			.Select(r => GetRecipeString(GetResources(r)))
			.Cast<object>()
			.Prepend(DevelopmentRecordOption.All)
			.ToList();
	}

	private string GetRecipeString(int[] resources)
	{
		return string.Join("/", resources);
	}

	private string GetRecipeString(int fuel, int ammo, int steel, int bauxite)
	{
		return GetRecipeString(new int[] { fuel, ammo, steel, bauxite });
	}

	private string GetRecipeString(DevelopmentRecord.DevelopmentElement record)
	{
		return GetRecipeString(new int[] { record.Fuel, record.Ammo, record.Steel, record.Bauxite });
	}

	private string GetRecipeStringForSorting(int[] resources)
	{
		return string.Join("/", resources.Select(r => r.ToString("D4")));
	}

	private string GetRecipeStringForSorting(int fuel, int ammo, int steel, int bauxite)
	{
		return GetRecipeStringForSorting(new int[] { fuel, ammo, steel, bauxite });
	}

	private string GetRecipeStringForSorting(DevelopmentRecord.DevelopmentElement record)
	{
		return GetRecipeStringForSorting(new int[] { record.Fuel, record.Ammo, record.Steel, record.Bauxite });
	}

	private int[] GetResources(string recipe)
	{
		return recipe.Split("/".ToCharArray()).Select(s => int.Parse(s)).ToArray();
	}

	private void Searcher_DoWork(object sender, DoWorkEventArgs e)
	{
		var records = RecordManager.Instance.Development.Record;
		var rows = new LinkedList<DevelopmentRecordRow>();

		int prioritySecretary =
			SelectedFlagship is not DevelopmentRecordOption.All ? 2 :
			SelectedFlagshipType is not DevelopmentRecordOption.All ? 1 : 0;

		int priorityEquipment =
			SelectedEquipment is not DevelopmentRecordOption.All &&
			SelectedEquipment is not DevelopmentRecordOption.Success ? 2 :
			SelectedCategory is not DevelopmentRecordOption.All ? 1 : 0;


		int i = 0;
		var counts = new Dictionary<string, int>();
		var allcounts = new Dictionary<string, int>();
		var countsdetail = new Dictionary<string, Dictionary<string, int>>();

		foreach (var r in records)
		{

			#region Filtering

			var eq = KCDatabase.Instance.MasterEquipments[r.EquipmentID];
			var secretary = KCDatabase.Instance.MasterShips[r.FlagshipID];
			string currentRecipe = GetRecipeString(r.Fuel, r.Ammo, r.Steel, r.Bauxite);
			var shiptype = KCDatabase.Instance.ShipTypes[r.FlagshipType];

			if (eq != null && eq.NameEN != r.EquipmentName) eq = null;
			if (secretary != null && secretary.NameEN != r.FlagshipName) secretary = null;

			if (r.Date < DateTimeBegin || DateTimeEnd < r.Date) continue;
			if (SelectedFlagshipType is ShipTypes shipType && (int)shipType != r.FlagshipType) continue;
			if (SelectedFlagship is IShipDataMaster ship && ship.ShipID != r.FlagshipID) continue;

			if (MergeRows)
			{
				string key = priorityEquipment switch
				{
					> 0 => currentRecipe,
					_ => r.EquipmentName
				};

				if (!allcounts.ContainsKey(key))
				{
					allcounts.Add(key, 1);
				}
				else
				{
					allcounts[key]++;
				}
			}

			if (SelectedCategory is EquipmentTypes equipmentType && equipmentType != eq?.CategoryType) continue;

			switch (SelectedEquipment)
			{
				case DevelopmentRecordOption.All:
					break;
				case DevelopmentRecordOption.Success:
					if (r.EquipmentID == -1)
						continue;
					break;
				case DevelopmentRecordOption.Failure:
					if (r.EquipmentID != -1)
						continue;
					break;
				case IEquipmentDataMaster equipment:
					if (equipment.EquipmentID != r.EquipmentID)
						continue;
					break;
			}

			if (SelectedRecipe is string recipe && recipe != currentRecipe) continue;

			#endregion


			if (!MergeRows)
			{
				DevelopmentRecordRow row = new(
					i + 1,
					r.EquipmentName,
					r.Date,
					GetRecipeString(r),
					shiptype?.NameEN ?? DevelopmentRecordViewerResources.Unknown,
					r.FlagshipName,
					null
				);

				row.CellTag1 = (eq?.EquipmentID ?? 0) + 1000 * ((int?)eq?.CategoryType ?? 0);
				row.CellTag3 = GetRecipeStringForSorting(r);
				row.CellTag4 = shiptype?.TypeID ?? 0;
				row.CellTag5 = ((int?)secretary?.ShipType ?? 0).ToString("D4") + (secretary?.NameReading ?? r.FlagshipName);

				rows.AddLast(row);

			}
			else
			{
				string key = priorityEquipment switch
				{
					> 0 => currentRecipe,
					_ => r.EquipmentName
				};

				if (!counts.ContainsKey(key))
				{
					counts.Add(key, 1);
				}
				else
				{
					counts[key]++;
				}

				key = priorityEquipment switch
				{
					> 0 => currentRecipe,
					_ => r.EquipmentName
				};

				string key2 = prioritySecretary switch
				{
					> 0 => currentRecipe,
					_ => shiptype?.NameEN ?? DevelopmentRecordViewerResources.Unknown
				};

				if (!countsdetail.ContainsKey(key))
				{
					countsdetail.Add(key, new Dictionary<string, int>());
				}
				if (!countsdetail[key].ContainsKey(key2))
				{
					countsdetail[key].Add(key2, 1);
				}
				else
				{
					countsdetail[key][key2]++;
				}

			}

			if (Searcher.CancellationPending)
				break;

			i++;
		}


		if (MergeRows)
		{

			int sum = counts.Values.Sum();

			foreach (var c in counts)
			{
				DevelopmentRecordRow? row;

				if (priorityEquipment > 0)
				{

					row = new (
						c.Value,
						c.Key,
						null,
						null,
						null,
						null,
						string.Join(", ", countsdetail[c.Key].OrderByDescending(p => p.Value).Select(d => string.Format("{0}({1})", d.Key, d.Value)))
					);

					row.RateOrMaxCountTotal = allcounts[c.Key];
					row.CellTag1 = GetRecipeStringForSorting(GetResources(c.Key));

				}
				else
				{

					row = new(
						c.Value,
						c.Key,
						null,
						null,
						null,
						null,
						string.Join(", ", countsdetail[c.Key].OrderByDescending(p => p.Value).Select(d => string.Format("{0}({1})", d.Key, d.Value)))
					);

					var eq = KCDatabase.Instance.MasterEquipments.Values.FirstOrDefault(eqm => eqm.NameEN == c.Key);
					row.RateOrMaxCountTotal = (double)c.Value / sum;
					row.CellTag1 = (eq?.EquipmentID ?? 0) + 1000 * ((int?)eq?.CategoryType ?? 0);
				}

				rows.AddLast(row);

				if (Searcher.CancellationPending)
					break;
			}

		}

		e.Result = rows.ToList();
	}

	private void Searcher_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		if (!e.Cancelled)
		{
			if(e.Result is not List<DevelopmentRecordRow> rows) return;

			RecordRows = new(rows.OrderByDescending(r => r.Index));
			DataGridRawRowsViewModel.ItemsSource = RecordRows;
			DataGridMergedRowsViewModel.ItemsSource = RecordRows;

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

		if (MergeRows)
		{
			int count = SelectedRows.Sum(r => r.Count);
			int allcount = RecordRows.Sum(r => r.Count);

			StatusInfoText = string.Format(DialogDevelopmentRecordViewer.SelectedItems + ": {0} / {1} ({2:p1})",
				count, allcount, (double)count / allcount);
		}
		else
		{
			int allcount = RecordRows.Count;
			StatusInfoText = string.Format(DialogDevelopmentRecordViewer.SelectedItems + ": {0} / {1} ({2:p1})",
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
