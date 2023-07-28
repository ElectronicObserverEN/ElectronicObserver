using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Common.Datagrid;
using ElectronicObserver.Data;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Storage;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Control.EquipmentFilter;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Tools.DialogAlbumMasterShip;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment;

public partial class DialogAlbumMasterEquipmentViewModel : WindowViewModelBase
{
	private IEnumerable<IEquipmentDataMaster> AllEquipment { get; }
	public DialogAlbumMasterEquipmentTranslationViewModel DialogAlbumMasterEquipment { get; }

	public DataGridViewModel<IEquipmentDataMaster> DataGridViewModel { get; set; } = new();

	[ObservableProperty]
	private IEquipmentDataMaster? selectedEquipmentModel;
	public EquipmentDataViewModel? SelectedEquipmentViewModel { get; set; }
	public bool DetailsVisible => SelectedEquipmentViewModel is not null;

	public EquipmentFilterViewModel Filters { get; } = new(true);

	public string Title => SelectedEquipmentViewModel switch
	{
		null => DialogAlbumMasterEquipment.Title,
		{ } => $"{DialogAlbumMasterEquipment.Title} - {SelectedEquipmentViewModel.Equipment.NameEN}"
	};

	private System.Windows.Forms.SaveFileDialog SaveCSVDialog = new()
	{
		Filter = "CSV|*.csv|File|*",
		Title = "Save As",
	};

	public DialogAlbumMasterEquipmentViewModel()
	{
		AllEquipment = KCDatabase.Instance.MasterEquipments.Values;

		DialogAlbumMasterEquipment =
			Ioc.Default.GetService<DialogAlbumMasterEquipmentTranslationViewModel>()!;

		Filters.PropertyChanged += (_, _) => RefreshGrid();
		
		PropertyChanged += (_, args) =>
		{
			if (args.PropertyName is not nameof(SelectedEquipmentModel)) return;

			if (SelectedEquipmentModel is null)
			{
				SelectedEquipmentViewModel?.UpgradeViewModel?.UnsubscribeFromApis();
				SelectedEquipmentViewModel = null;
				return;
			}

			if (SelectedEquipmentViewModel is null)
			{
				SelectedEquipmentViewModel = new(SelectedEquipmentModel);
			}
			else
			{
				SelectedEquipmentViewModel.ChangeEquipment(SelectedEquipmentModel);
			}
		};

		RefreshGrid();
	}

	private void RefreshGrid()
	{
		DataGridViewModel.ItemsSource.Clear();
		DataGridViewModel.AddRange(AllEquipment.Where(e => Filters.MeetsFilterCondition(e)));
	}

	[RelayCommand]
	private void OpenShipEncyclopedia(IShipDataMaster? ship)
	{
		if (ship is null) return;

		new DialogAlbumMasterShipWpf(ship).Show(App.Current.MainWindow);
	}

	[RelayCommand]
	public void OpenEquipmentEncyclopedia(IEquipmentDataMaster? equip)
	{
		if (equip is null) return;

		new DialogAlbumMasterEquipmentWpf(equip.ID).Show(App.Current.MainWindow);
	}

	[RelayCommand]
	private void StripMenu_File_OutputCSVUser_Click()
	{
		if (SaveCSVDialog.ShowDialog(App.Current.MainWindow) != System.Windows.Forms.DialogResult.OK) return;

		try
		{
			using StreamWriter sw = new(SaveCSVDialog.FileName, false,
				Utility.Configuration.Config.Log.FileEncoding);
			sw.WriteLine(
				"装備ID,図鑑番号,装備種,装備名,大分類,図鑑カテゴリID,カテゴリID,アイコンID,航空機グラフィックID,火力,雷装,対空,装甲,対潜,回避,索敵,運,命中,爆装,射程,レア,廃棄燃料,廃棄弾薬,廃棄鋼材,廃棄ボーキ,図鑑文章,戦闘行動半径,配置コスト");

			foreach (EquipmentDataMaster eq in KCDatabase.Instance.MasterEquipments.Values)
			{

				sw.WriteLine(string.Join(",",
					eq.EquipmentID,
					eq.AlbumNo,
					CsvHelper.EscapeCsvCell(eq.CategoryTypeInstance.NameEN),
					CsvHelper.EscapeCsvCell(eq.NameEN),
					eq.EquipmentType[0],
					eq.EquipmentType[1],
					eq.EquipmentType[2],
					eq.EquipmentType[3],
					eq.EquipmentType[4],
					eq.Firepower,
					eq.Torpedo,
					eq.AA,
					eq.Armor,
					eq.ASW,
					eq.Evasion,
					eq.LOS,
					eq.Luck,
					eq.Accuracy,
					eq.Bomber,
					Constants.GetRange(eq.Range),
					Constants.GetEquipmentRarity(eq.Rarity),
					eq.Material[0],
					eq.Material[1],
					eq.Material[2],
					eq.Material[3],
					CsvHelper.EscapeCsvCell(eq.Message),
					eq.AircraftDistance,
					eq.AircraftCost
				));

			}
		}
		catch (Exception ex)
		{

			Utility.ErrorReporter.SendErrorReport(ex, EncycloRes.FailedOutputEquipCSV);
			MessageBox.Show(EncycloRes.FailedOutputEquipCSV + "\r\n" + ex.Message,
				AlbumMasterEquipmentResources.DialogTitleError, MessageBoxButton.OK,
				MessageBoxImage.Error);
		}


	}

	[RelayCommand]
	private void StripMenu_File_OutputCSVData_Click()
	{
		if (SaveCSVDialog.ShowDialog(App.Current.MainWindow) != System.Windows.Forms.DialogResult.OK) return;

		try
		{
			using StreamWriter sw = new(SaveCSVDialog.FileName, false, Utility.Configuration.Config.Log.FileEncoding);
			sw.WriteLine("装備ID,図鑑番号,装備名,装備種1,装備種2,装備種3,装備種4,装備種5,火力,雷装,対空,装甲,対潜,回避,索敵,運,命中,爆装,射程,レア,廃棄燃料,廃棄弾薬,廃棄鋼材,廃棄ボーキ,図鑑文章,戦闘行動半径,配置コスト");

			foreach (EquipmentDataMaster eq in KCDatabase.Instance.MasterEquipments.Values)
			{

				sw.WriteLine(string.Join(",",
					eq.EquipmentID,
					eq.AlbumNo,
					CsvHelper.EscapeCsvCell(eq.NameEN),
					eq.EquipmentType[0],
					eq.EquipmentType[1],
					eq.EquipmentType[2],
					eq.EquipmentType[3],
					eq.EquipmentType[4],
					eq.Firepower,
					eq.Torpedo,
					eq.AA,
					eq.Armor,
					eq.ASW,
					eq.Evasion,
					eq.LOS,
					eq.Luck,
					eq.Accuracy,
					eq.Bomber,
					eq.Range,
					eq.Rarity,
					eq.Material[0],
					eq.Material[1],
					eq.Material[2],
					eq.Material[3],
					CsvHelper.EscapeCsvCell(eq.Message),
					eq.AircraftDistance,
					eq.AircraftCost
				));

			}
		}
		catch (Exception ex)
		{

			Utility.ErrorReporter.SendErrorReport(ex, AlbumMasterEquipmentResources.FailedToExportCsv);

			MessageBox.Show(
				$"{AlbumMasterEquipmentResources.FailedToExportCsv}\r\n" + ex.Message,
				AlbumMasterEquipmentResources.DialogTitleError,
				MessageBoxButton.OK,
				MessageBoxImage.Error);
		}

	}

	[RelayCommand]
	private void StripMenu_Edit_CopyEquipmentName_Click()
	{
		IEquipmentDataMaster? eq = SelectedEquipmentViewModel?.Equipment;
		if (eq != null)
			Clipboard.SetDataObject(eq.NameEN);
		else
			System.Media.SystemSounds.Exclamation.Play();
	}

	[RelayCommand]
	private void StripMenu_Edit_CopyEquipmentData_Click()
	{
		IEquipmentDataMaster? eq = SelectedEquipmentViewModel?.Equipment;
		if (eq is null)
		{
			System.Media.SystemSounds.Exclamation.Play();
			return;
		}

		StringBuilder? sb = new();

		sb.AppendFormat("{0} {1}\r\n", eq.CategoryTypeInstance.NameEN, eq.NameEN);
		sb.AppendFormat("ID: {0} / 図鑑番号: {1} / カテゴリID: [{2}]\r\n", eq.EquipmentID, eq.AlbumNo, string.Join(", ", eq.EquipmentType));

		sb.AppendLine();

		if (eq.Firepower != 0) sb.AppendFormat("火力: {0:+0;-0;0}\r\n", eq.Firepower);
		if (eq.Torpedo != 0) sb.AppendFormat("雷装: {0:+0;-0;0}\r\n", eq.Torpedo);
		if (eq.AA != 0) sb.AppendFormat("対空: {0:+0;-0;0}\r\n", eq.AA);
		if (eq.Armor != 0) sb.AppendFormat("装甲: {0:+0;-0;0}\r\n", eq.Armor);
		if (eq.ASW != 0) sb.AppendFormat("対潜: {0:+0;-0;0}\r\n", eq.ASW);
		if (eq.Evasion != 0) sb.AppendFormat("{0}: {1:+0;-0;0}\r\n", eq.CategoryType == EquipmentTypes.Interceptor ? "迎撃" : "回避", eq.Evasion);
		if (eq.LOS != 0) sb.AppendFormat("索敵: {0:+0;-0;0}\r\n", eq.LOS);
		if (eq.Accuracy != 0) sb.AppendFormat("{0}: {1:+0;-0;0}\r\n", eq.CategoryType == EquipmentTypes.Interceptor ? "対爆" : "命中", eq.Accuracy);
		if (eq.Bomber != 0) sb.AppendFormat("爆装: {0:+0;-0;0}\r\n", eq.Bomber);
		if (eq.Luck != 0) sb.AppendFormat("運: {0:+0;-0;0}\r\n", eq.Luck);

		if (eq.Range > 0) sb.Append("射程: ").AppendLine(Constants.GetRange(eq.Range));

		if (eq.AircraftCost > 0) sb.AppendFormat("配備コスト: {0}\r\n", eq.AircraftCost);
		if (eq.AircraftDistance > 0) sb.AppendFormat("戦闘行動半径: {0}\r\n", eq.AircraftDistance);

		sb.AppendLine();

		sb.AppendFormat("レアリティ: {0}\r\n", Constants.GetEquipmentRarity(eq.Rarity));
		sb.AppendFormat("廃棄資材: {0}\r\n", string.Join(" / ", eq.Material));

		sb.AppendLine();

		sb.AppendFormat("図鑑説明: \r\n{0}\r\n",
			!string.IsNullOrWhiteSpace(eq.Message) ? eq.Message : "(不明)");

		sb.AppendLine();

		sb.AppendLine("初期装備/開発:");
		string result = GetAppearingArea(eq.EquipmentID);
		if (string.IsNullOrWhiteSpace(result))
			result = "(不明)\r\n";
		sb.AppendLine(result);


		Clipboard.SetDataObject(sb.ToString());
	}

	[RelayCommand]
	private void StripMenu_Edit_GoogleEquipmentName_Click()
	{
		IEquipmentDataMaster? eq = SelectedEquipmentViewModel?.Equipment;
		if (eq == null)
		{
			System.Media.SystemSounds.Exclamation.Play();
			return;
		}

		try
		{
			ProcessStartInfo psi = new()
			{
				FileName = @"https://www.duckduckgo.com/?q=" + Uri.EscapeDataString(eq.NameEN) +
				           AlbumMasterEquipmentResources.KancolleSpecifier,
				UseShellExecute = true
			};
			// google <装備名> 艦これ
			Process.Start(psi);

		}
		catch (Exception ex)
		{
			Utility.ErrorReporter.SendErrorReport(ex, AlbumMasterEquipmentResources.FailedToSearchOnWeb);
		}
	}

	[RelayCommand]
	private void StripMenu_View_ShowAppearingArea_Click()
	{
		IEquipmentDataMaster? eq = SelectedEquipmentViewModel?.Equipment;

		if (eq is null)
		{
			System.Media.SystemSounds.Exclamation.Play();
			return;
		}

		string result = GetAppearingArea(eq.ID);

		if (string.IsNullOrWhiteSpace(result))
		{
			result = string.Format(AlbumMasterEquipmentResources.FailedToFindShipOrRecipe, eq.NameEN);
		}

		MessageBox.Show(result,
			AlbumMasterEquipmentResources.ShipOrRecipeCaption,
			MessageBoxButton.OK,
			MessageBoxImage.Information);
	}

	private string GetAppearingArea(int equipmentID)
	{
		StringBuilder sb = new();

		foreach (ShipDataMaster ship in KCDatabase.Instance.MasterShips.Values
			.Where(s => s.DefaultSlot != null && s.DefaultSlot.Contains(equipmentID)))
		{
			sb.AppendLine(ship.NameWithClass);
		}

		foreach (var record in RecordManager.Instance.Development.Record
			.Where(r => r.EquipmentID == equipmentID)
			.Select(r => new
			{
				r.Fuel,
				r.Ammo,
				r.Steel,
				r.Bauxite
			})
			.Distinct()
			.OrderBy(r => r.Fuel)
			.ThenBy(r => r.Ammo)
			.ThenBy(r => r.Steel)
			.ThenBy(r => r.Bauxite)
		)
		{
			sb.AppendFormat(AlbumMasterEquipmentResources.Recipe + " {0} / {1} / {2} / {3}\r\n",
				record.Fuel, record.Ammo, record.Steel, record.Bauxite);
		}

		return sb.ToString();
	}
}
