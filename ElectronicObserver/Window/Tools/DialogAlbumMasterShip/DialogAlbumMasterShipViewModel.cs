using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Common.Datagrid;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Services;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Tools.DialogAlbumMasterEquipment;
using WanaKanaNet;
using static ElectronicObserver.Resource.Record.ShipParameterRecord;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterShip;

public partial class DialogAlbumMasterShipViewModel : WindowViewModelBase
{
	private IEnumerable<ShipDataRecord> AllShips { get; }
	public DialogAlbumMasterShipTranslationViewModel DialogAlbumMasterShip { get; }
	private TransliterationService TransliterationService { get; }

	public DataGridViewModel<ShipDataRecord> DataGridViewModel { get; set; } = new();

	public string? SearchFilter { get; set; } = "";
	public string? RomajiSearchFilter { get; set; } = "";

	public string Title => SelectedShip switch
	{
		{ } ship => DialogAlbumMasterShip.Title + " - " + ship.Ship.NameWithClass,
		_ => DialogAlbumMasterShip.Title
	};

	public string TitleParameterMin => SelectedShip?.Ship.IsAbyssalShip switch
	{
		true => DialogAlbumMasterShip.BaseValue,
		_ => DialogAlbumMasterShip.TitleParameterMin
	};

	public string TitleParameterMax => SelectedShip?.Ship.IsAbyssalShip switch
	{
		true => DialogAlbumMasterShip.WithEquipValue,
		_ => DialogAlbumMasterShip.TitleParameterMax
	};

	public int Level { get; set; }
	public ShipDataRecord? SelectedShip { get; set; }

	public bool DetailsVisible => SelectedShip is not null;

	private System.Windows.Forms.SaveFileDialog SaveCSVDialog = new()
	{
		Filter = "CSV|*.csv|File|*",
		Title = "Save As",
	};

	public DialogAlbumMasterShipViewModel()
	{
		DialogAlbumMasterShip = Ioc.Default.GetService<DialogAlbumMasterShipTranslationViewModel>()!;
		TransliterationService = Ioc.Default.GetService<TransliterationService>()!;

		Level = ExpTable.ShipMaximumLevel;

		AllShips = KCDatabase.Instance.MasterShips.Values.Select(s => new ShipDataRecord(s));
		DataGridViewModel.AddRange(AllShips);

		PopulateCache();

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(Level)) return;
			if (SelectedShip is null) return;

			SelectedShip.Level = Level;
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(SelectedShip)) return;
			if (SelectedShip is null) return;

			SelectedShip.Level = Level;
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(SearchFilter)) return;

			RomajiSearchFilter = WanaKana.ToRomaji(SearchFilter ?? "");
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(RomajiSearchFilter)) return;

			DataGridViewModel.ItemsSource.Clear();
			DataGridViewModel.AddRange(AllShips.Where(s => RomajiSearchFilter switch
			{
				null or "" => true,
				string f => TransliterationService.Matches(s.Ship, SearchFilter ?? "", f),
			}));
		};
	}

	private void PopulateCache()
	{
		foreach (ShipDataRecord ship in DataGridViewModel.ItemsSource)
		{
			TransliterationService.GetRomajiName(ship.Ship);
		}
	}

	public DialogAlbumMasterShipViewModel(IShipDataMaster ship) : this()
	{
		ChangeShip(ship);
	}

	[RelayCommand]
	private void ChangeShip(IShipDataMaster? ship)
	{
		if (ship is null) return;

		SelectedShip = DataGridViewModel.ItemsSource.FirstOrDefault(s => s.Ship.ShipID == ship.ShipID)
			?? AllShips.FirstOrDefault(s => s.Ship.ShipID == ship.ShipID);
	}

	[RelayCommand]
	private void OpenShipEncyclopedia(IShipDataMaster? ship)
	{
		if (ship is null) return;

		new DialogAlbumMasterShipWpf(ship).Show(App.Current.MainWindow);
	}

	[RelayCommand]
	private void OpenEquipmentEncyclopedia(IEquipmentDataMaster? equip)
	{
		if (equip is null) return;

		new DialogAlbumMasterEquipmentWpf(equip.ID).Show(App.Current.MainWindow);
	}


	[RelayCommand]
	private void ResourceName_MouseClick()
	{
		if (SelectedShip is null) return;

		Clipboard.SetData(DataFormats.StringFormat, SelectedShip.Ship.ResourceName);
	}



	[RelayCommand]
	private void StripMenu_File_OutputCSVData_Click()
	{
		if (SaveCSVDialog.ShowDialog(App.Current.MainWindow) != System.Windows.Forms.DialogResult.OK) return;

		try
		{
			using StreamWriter sw = new(SaveCSVDialog.FileName, false, Utility.Configuration.Config.Log.FileEncoding);
			sw.WriteLine(string.Format("艦船ID,図鑑番号,艦名,読み,艦種,艦型,ソート順,改装前,改装後,改装Lv,改装弾薬,改装鋼材,改装設計図,カタパルト,戦闘詳報,新型航空兵装資材,改装段階,耐久初期,耐久最大,耐久結婚,耐久改修,火力初期,火力最大,雷装初期,雷装最大,対空初期,対空最大,装甲初期,装甲最大,対潜初期最小,対潜初期最大,対潜最大,対潜{0}最小,対潜{0}最大,回避初期最小,回避初期最大,回避最大,回避{0}最小,回避{0}最大,索敵初期最小,索敵初期最大,索敵最大,索敵{0}最小,索敵{0}最大,運初期,運最大,速力,射程,レア,スロット数,搭載機数1,搭載機数2,搭載機数3,搭載機数4,搭載機数5,初期装備1,初期装備2,初期装備3,初期装備4,初期装備5,建造時間,解体燃料,解体弾薬,解体鋼材,解体ボーキ,改修火力,改修雷装,改修対空,改修装甲,ドロップ文章,図鑑文章,搭載燃料,搭載弾薬,ボイス,リソース名,画像バージョン,ボイスバージョン,母港ボイスバージョン", ExpTable.ShipMaximumLevel));

			foreach (ShipDataMaster ship in KCDatabase.Instance.MasterShips.Values)
			{

				sw.WriteLine(string.Join(",",
					ship.ShipID,
					ship.AlbumNo,
					Utility.Storage.CsvHelper.EscapeCsvCell(ship.NameEN),
					Utility.Storage.CsvHelper.EscapeCsvCell(ship.NameReading),
					(int)ship.ShipType,
					ship.ShipClass,
					ship.SortID,
					ship.RemodelBeforeShipID,
					ship.RemodelAfterShipID,
					ship.RemodelAfterLevel,
					ship.RemodelAmmo,
					ship.RemodelSteel,
					ship.NeedBlueprint,
					ship.NeedCatapult,
					ship.NeedActionReport,
					ship.NeedAviationMaterial,
					ship.RemodelTier,
					ship.HPMin,
					ship.HPMax,
					ship.HPMaxMarried,
					ship.HPMaxMarriedModernized,
					ship.FirepowerMin,
					ship.FirepowerMax,
					ship.TorpedoMin,
					ship.TorpedoMax,
					ship.AAMin,
					ship.AAMax,
					ship.ArmorMin,
					ship.ArmorMax,
					ship.ASW?.MinimumEstMin ?? Parameter.MinimumDefault,
					ship.ASW?.MinimumEstMax ?? Parameter.MaximumDefault,
					ship.ASW?.Maximum ?? Parameter.MaximumDefault,
					ship.ASW?.GetEstParameterMin(ExpTable.ShipMaximumLevel) ?? Parameter.MinimumDefault,
					ship.ASW?.GetEstParameterMax(ExpTable.ShipMaximumLevel) ?? Parameter.MaximumDefault,
					ship.Evasion?.MinimumEstMin ?? Parameter.MinimumDefault,
					ship.Evasion?.MinimumEstMax ?? Parameter.MaximumDefault,
					ship.Evasion?.Maximum ?? Parameter.MaximumDefault,
					ship.Evasion?.GetEstParameterMin(ExpTable.ShipMaximumLevel) ?? Parameter.MinimumDefault,
					ship.Evasion?.GetEstParameterMax(ExpTable.ShipMaximumLevel) ?? Parameter.MaximumDefault,
					ship.LOS?.MinimumEstMin ?? Parameter.MinimumDefault,
					ship.LOS?.MinimumEstMax ?? Parameter.MaximumDefault,
					ship.LOS?.Maximum ?? Parameter.MaximumDefault,
					ship.LOS?.GetEstParameterMin(ExpTable.ShipMaximumLevel) ?? Parameter.MinimumDefault,
					ship.LOS?.GetEstParameterMax(ExpTable.ShipMaximumLevel) ?? Parameter.MaximumDefault,
					ship.LuckMin,
					ship.LuckMax,
					ship.Speed,
					ship.Range,
					ship.Rarity,
					ship.SlotSize,
					ship.Aircraft[0],
					ship.Aircraft[1],
					ship.Aircraft[2],
					ship.Aircraft[3],
					ship.Aircraft[4],
					ship.DefaultSlot?[0] ?? -1,
					ship.DefaultSlot?[1] ?? -1,
					ship.DefaultSlot?[2] ?? -1,
					ship.DefaultSlot?[3] ?? -1,
					ship.DefaultSlot?[4] ?? -1,
					ship.BuildingTime,
					ship.Material[0],
					ship.Material[1],
					ship.Material[2],
					ship.Material[3],
					ship.PowerUp[0],
					ship.PowerUp[1],
					ship.PowerUp[2],
					ship.PowerUp[3],
					Utility.Storage.CsvHelper.EscapeCsvCell(ship.MessageGet),
					Utility.Storage.CsvHelper.EscapeCsvCell(ship.MessageAlbum),
					ship.Fuel,
					ship.Ammo,
					ship.VoiceFlag,
					Utility.Storage.CsvHelper.EscapeCsvCell(ship.ResourceName),
					ship.ResourceGraphicVersion,
					ship.ResourceVoiceVersion,
					ship.ResourcePortVoiceVersion
				));

			}
		}
		catch (Exception ex)
		{

			Utility.ErrorReporter.SendErrorReport(ex, AlbumMasterShipResources.CsvExportFailed);
			MessageBox.Show(AlbumMasterShipResources.CsvExportFailed + "\r\n" + ex.Message, AlbumMasterEquipmentResources.DialogTitleError, MessageBoxButton.OK, MessageBoxImage.Error);
		}

	}

	[RelayCommand]
	private void StripMenu_File_OutputCSVUser_Click()
	{
		if (SaveCSVDialog.ShowDialog(App.Current.MainWindow) != System.Windows.Forms.DialogResult.OK) return;

		try
		{
			using StreamWriter sw = new StreamWriter(SaveCSVDialog.FileName, false, Utility.Configuration.Config.Log.FileEncoding);
			sw.WriteLine("艦船ID,図鑑番号,艦型,艦種,艦名,読み,ソート順,改装前,改装後,改装Lv,改装弾薬,改装鋼材,改装設計図,カタパルト,戦闘詳報,新型航空兵装資材,改装段階,耐久初期,耐久結婚,耐久最大,火力初期,火力最大,雷装初期,雷装最大,対空初期,対空最大,装甲初期,装甲最大,対潜初期,対潜最大,回避初期,回避最大,索敵初期,索敵最大,運初期,運最大,速力,射程,レア,スロット数,搭載機数1,搭載機数2,搭載機数3,搭載機数4,搭載機数5,初期装備1,初期装備2,初期装備3,初期装備4,初期装備5,建造時間,解体燃料,解体弾薬,解体鋼材,解体ボーキ,改修火力,改修雷装,改修対空,改修装甲,ドロップ文章,図鑑文章,搭載燃料,搭載弾薬,ボイス,リソース名,画像バージョン,ボイスバージョン,母港ボイスバージョン");

			foreach (ShipDataMaster ship in KCDatabase.Instance.MasterShips.Values)
			{

				if (ship.Name == "なし") continue;

				sw.WriteLine(string.Join(",",
					ship.ShipID,
					ship.AlbumNo,
					ship.IsAbyssalShip ? "深海棲艦" : Constants.GetShipClass(ship.ShipClass, ship.ShipId),
					Utility.Storage.CsvHelper.EscapeCsvCell(ship.ShipTypeName),
					Utility.Storage.CsvHelper.EscapeCsvCell(ship.NameEN),
					Utility.Storage.CsvHelper.EscapeCsvCell(ship.NameReading),
					ship.SortID,
					Utility.Storage.CsvHelper.EscapeCsvCell(ship.RemodelBeforeShipID > 0 ? ship.RemodelBeforeShip.NameEN : "-"),
					Utility.Storage.CsvHelper.EscapeCsvCell(ship.RemodelAfterShipID > 0 ? ship.RemodelAfterShip.NameEN : "-"),
					ship.RemodelAfterLevel,
					ship.RemodelAmmo,
					ship.RemodelSteel,
					ship.NeedBlueprint > 0 ? ship.NeedBlueprint + "枚" : "-",
					ship.NeedCatapult > 0 ? ship.NeedCatapult + "個" : "-",
					ship.NeedActionReport > 0 ? ship.NeedActionReport + "枚" : "-",
					ship.NeedAviationMaterial > 0 ? ship.NeedAviationMaterial + "個" : "-",
					ship.RemodelTier,
					ship.HPMin,
					ship.HPMaxMarried,
					ship.HPMaxMarriedModernized,
					ship.FirepowerMin,
					ship.FirepowerMax,
					ship.TorpedoMin,
					ship.TorpedoMax,
					ship.AAMin,
					ship.AAMax,
					ship.ArmorMin,
					ship.ArmorMax,
					ship.ASW != null && !ship.ASW.IsMinimumDefault ? ship.ASW.Minimum.ToString() : "???",
					ship.ASW != null && !ship.ASW.IsMaximumDefault ? ship.ASW.Maximum.ToString() : "???",
					ship.Evasion != null && !ship.Evasion.IsMinimumDefault ? ship.Evasion.Minimum.ToString() : "???",
					ship.Evasion != null && !ship.Evasion.IsMaximumDefault ? ship.Evasion.Maximum.ToString() : "???",
					ship.LOS != null && !ship.LOS.IsMinimumDefault ? ship.LOS.Minimum.ToString() : "???",
					ship.LOS != null && !ship.LOS.IsMaximumDefault ? ship.LOS.Maximum.ToString() : "???",
					ship.LuckMin,
					ship.LuckMax,
					Constants.GetSpeed(ship.Speed),
					Constants.GetRange(ship.Range),
					Constants.GetShipRarity(ship.Rarity),
					ship.SlotSize,
					ship.Aircraft[0],
					ship.Aircraft[1],
					ship.Aircraft[2],
					ship.Aircraft[3],
					ship.Aircraft[4],
					ship.DefaultSlot != null ? (ship.DefaultSlot[0] != -1 ? KCDatabase.Instance.MasterEquipments[ship.DefaultSlot[0]].NameEN : (ship.SlotSize > 0 ? "(なし)" : "")) : "???",
					ship.DefaultSlot != null ? (ship.DefaultSlot[1] != -1 ? KCDatabase.Instance.MasterEquipments[ship.DefaultSlot[1]].NameEN : (ship.SlotSize > 1 ? "(なし)" : "")) : "???",
					ship.DefaultSlot != null ? (ship.DefaultSlot[2] != -1 ? KCDatabase.Instance.MasterEquipments[ship.DefaultSlot[2]].NameEN : (ship.SlotSize > 2 ? "(なし)" : "")) : "???",
					ship.DefaultSlot != null ? (ship.DefaultSlot[3] != -1 ? KCDatabase.Instance.MasterEquipments[ship.DefaultSlot[3]].NameEN : (ship.SlotSize > 3 ? "(なし)" : "")) : "???",
					ship.DefaultSlot != null ? (ship.DefaultSlot[4] != -1 ? KCDatabase.Instance.MasterEquipments[ship.DefaultSlot[4]].NameEN : (ship.SlotSize > 4 ? "(なし)" : "")) : "???",
					DateTimeHelper.ToTimeRemainString(TimeSpan.FromMinutes(ship.BuildingTime)),
					ship.Material[0],
					ship.Material[1],
					ship.Material[2],
					ship.Material[3],
					ship.PowerUp[0],
					ship.PowerUp[1],
					ship.PowerUp[2],
					ship.PowerUp[3],
					Utility.Storage.CsvHelper.EscapeCsvCell(ship.MessageGet),
					Utility.Storage.CsvHelper.EscapeCsvCell(ship.MessageAlbum),
					ship.Fuel,
					ship.Ammo,
					Constants.GetVoiceFlag(ship.VoiceFlag),
					Utility.Storage.CsvHelper.EscapeCsvCell(ship.ResourceName),
					ship.ResourceGraphicVersion,
					ship.ResourceVoiceVersion,
					ship.ResourcePortVoiceVersion
				));

			}
		}
		catch (Exception ex)
		{

			Utility.ErrorReporter.SendErrorReport(ex, DialogAlbumMasterShip.CsvExportFailed);
			MessageBox.Show(DialogAlbumMasterShip.CsvExportFailed + "\r\n" + ex.Message, AlbumMasterEquipmentResources.DialogTitleError, MessageBoxButton.OK, MessageBoxImage.Error);
		}

	}

	[RelayCommand]
	private void StripMenu_File_MergeDefaultRecord_Click()
	{
		if (MessageBox.Show("デフォルトレコードの情報をもとに、艦船レコードを更新します。\r\nよろしいですか？", "レコード更新確認", MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.Cancel) == MessageBoxResult.Cancel)
		{
			return;
		}

		var parameterRecord = RecordManager.Instance.ShipParameter;


		string temporaryPath = null;
		try
		{
			temporaryPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
			Resource.ResourceManager.CopyFromArchive("Record/" + parameterRecord.FileName, temporaryPath, false, true);

			int count = 0;
			using (var reader = new StreamReader(temporaryPath, Utility.Configuration.Config.Log.FileEncoding))
			{
				while (!reader.EndOfStream)
				{
					count += parameterRecord.Merge(reader.ReadLine()) ? 1 : 0;
				}
			}

			if (count == 0)
				Utility.Logger.Add(2, "更新できるレコードがありませんでした。お使いのデータは十分に更新されています。");
			else
				Utility.Logger.Add(2, count + " 件の艦船レコードの更新が完了しました。開き直すと反映されます。");
		}
		catch (Exception ex)
		{
			Utility.ErrorReporter.SendErrorReport(ex, "デフォルトレコードとのマージに失敗しました。");
		}
		finally
		{
			if (temporaryPath != null)
				File.Delete(temporaryPath);
		}

	}



	[RelayCommand]
	private void StripMenu_Edit_EditParameter_Click()
	{

		if (SelectedShip is null)
		{
			MessageBox.Show(DialogAlbumMasterShip.SelectAShip, AlbumMasterEquipmentResources.DialogTitleError, MessageBoxButton.OK, MessageBoxImage.Asterisk);
			return;
		}

		using DialogAlbumShipParameter dialog = new(SelectedShip.Ship.ShipID);

		dialog.ShowDialog(App.Current.MainWindow);
		OnPropertyChanged(nameof(SelectedShip));
	}

	[RelayCommand]
	private void StripMenu_Edit_CopyShipName_Click()
	{
		if (SelectedShip is not null)
		{
			Clipboard.SetDataObject(SelectedShip.Ship.NameWithClass);
		}
		else
		{
			System.Media.SystemSounds.Exclamation.Play();
		}
	}

	[RelayCommand]
	private void StripMenu_Edit_CopyShipData_Click()
	{
		IShipDataMaster? ship = SelectedShip?.Ship;
		if (ship is null)
		{
			System.Media.SystemSounds.Exclamation.Play();
			return;
		}

		var sb = new StringBuilder();

		var slot = (ship.DefaultSlot ?? Enumerable.Repeat(-2, 5)).ToArray();

		sb.AppendFormat("{0} {1}\r\n", ship.ShipTypeName, ship.NameWithClass);
		sb.AppendFormat("ID: {0} / 図鑑番号: {1} / リソース: {2} ver. {3} / {4} / {5} ({6})\r\n", ship.ShipID, ship.AlbumNo,
			ship.ResourceName, ship.ResourceGraphicVersion, ship.ResourceVoiceVersion, ship.ResourcePortVoiceVersion,
			Constants.GetVoiceFlag(ship.VoiceFlag));
		sb.AppendLine();
		if (!ship.IsAbyssalShip)
		{
			sb.AppendFormat("耐久: {0} / {1}\r\n", ship.HPMin, ship.HPMaxMarried);
			sb.AppendFormat("火力: {0} / {1}\r\n", ship.FirepowerMin, ship.FirepowerMax);
			sb.AppendFormat("雷装: {0} / {1}\r\n", ship.TorpedoMin, ship.TorpedoMax);
			sb.AppendFormat("対空: {0} / {1}\r\n", ship.AAMin, ship.AAMax);
			sb.AppendFormat("装甲: {0} / {1}\r\n", ship.ArmorMin, ship.ArmorMax);
			sb.AppendFormat("対潜: {0} / {1}\r\n", GetParameterMinBound(ship.ASW), GetParameterMax(ship.ASW));
			sb.AppendFormat("回避: {0} / {1}\r\n", GetParameterMinBound(ship.Evasion), GetParameterMax(ship.Evasion));
			sb.AppendFormat("索敵: {0} / {1}\r\n", GetParameterMinBound(ship.LOS), GetParameterMax(ship.LOS));
			sb.AppendFormat("運: {0} / {1}\r\n", ship.LuckMin, ship.LuckMax);
			sb.AppendFormat("速力: {0} / 射程: {1}\r\n", Constants.GetSpeed(ship.Speed), Constants.GetRange(ship.Range));
			sb.AppendFormat("搭載資源: 燃料 {0} / 弾薬 {1}\r\n", ship.Fuel, ship.Ammo);
			sb.AppendFormat("レアリティ: {0}\r\n", Constants.GetShipRarity(ship.Rarity));
		}
		else
		{
			var availableEquipments = slot.Select(id => KCDatabase.Instance.MasterEquipments[id]).Where(eq => eq != null);
			int luckSum = ship.LuckMax + availableEquipments.Sum(eq => eq.Luck);
			sb.AppendFormat("耐久: {0}\r\n", ship.HPMin > 0 ? ship.HPMin.ToString() : "???");
			sb.AppendFormat("火力: {0} / {1}\r\n", ship.FirepowerMin, ship.FirepowerMax + availableEquipments.Sum(eq => eq.Firepower));
			sb.AppendFormat("雷装: {0} / {1}\r\n", ship.TorpedoMin, ship.TorpedoMax + availableEquipments.Sum(eq => eq.Torpedo));
			sb.AppendFormat("対空: {0} / {1}\r\n", ship.AAMin, ship.AAMax + availableEquipments.Sum(eq => eq.AA));
			sb.AppendFormat("装甲: {0} / {1}\r\n", ship.ArmorMin, ship.ArmorMax + availableEquipments.Sum(eq => eq.Armor));
			sb.AppendFormat("対潜: {0} / {1}\r\n", GetParameterMax(ship.ASW), (ship.ASW != null && !ship.ASW.IsMaximumDefault ? ship.ASW.Maximum : 0) + availableEquipments.Sum(eq => eq.ASW));
			sb.AppendFormat("回避: {0} / {1}\r\n", GetParameterMax(ship.Evasion), (ship.Evasion != null && !ship.Evasion.IsMaximumDefault ? ship.Evasion.Maximum : 0) + availableEquipments.Sum(eq => eq.Evasion));
			sb.AppendFormat("索敵: {0} / {1}\r\n", GetParameterMax(ship.LOS), (ship.LOS != null && !ship.LOS.IsMaximumDefault ? ship.LOS.Maximum : 0) + availableEquipments.Sum(eq => eq.LOS));
			sb.AppendFormat("運: {0} / {1}\r\n", ship.LuckMin > 0 ? ship.LuckMin.ToString() : "???", luckSum > 0 ? luckSum.ToString() : "???");
			sb.AppendFormat("速力: {0} / 射程: {1}\r\n", Constants.GetSpeed(ship.Speed),
				Constants.GetRange(Math.Max(ship.Range, availableEquipments.Any() ? availableEquipments.Max(eq => eq.Range) : 0)));
			if (ship.Fuel > 0 || ship.Ammo > 0)
				sb.AppendFormat("搭載資源: 燃料 {0} / 弾薬 {1}\r\n", ship.Fuel, ship.Ammo);
			if (ship.Rarity > 0)
				sb.AppendFormat("レアリティ: {0}\r\n", Constants.GetShipRarity(ship.Rarity));
		}
		sb.AppendLine();
		sb.AppendLine("初期装備:");
		{
			for (int i = 0; i < slot.Length; i++)
			{
				string name;
				var eq = KCDatabase.Instance.MasterEquipments[slot[i]];
				if (eq == null && i >= ship.SlotSize)
					continue;

				if (eq != null)
					name = eq.NameEN;
				else if (slot[i] == -1)
					name = "(なし)";
				else
					name = "(不明)";

				sb.AppendFormat("[{0}] {1}\r\n", ship.Aircraft[i], name);
			}
		}
		sb.AppendLine();
		if (!ship.IsAbyssalShip)
		{
			sb.AppendFormat("建造時間: {0}\r\n", DateTimeHelper.ToTimeRemainString(TimeSpan.FromMinutes(ship.BuildingTime)));
			sb.AppendFormat("解体資源: {0}\r\n", string.Join(" / ", ship.Material));
			sb.AppendFormat("改修強化: {0}\r\n", string.Join(" / ", ship.PowerUp));
			if (ship.RemodelBeforeShipID != 0)
			{
				var before = ship.RemodelBeforeShip;
				var append = new List<string>(4)
				{
					"弾薬 " + before.RemodelAmmo,
					"鋼材 " + before.RemodelSteel
				};
				if (before.NeedBlueprint > 0)
					append.Add("要改装設計図");
				if (before.NeedCatapult > 0)
					append.Add("要カタパルト");
				if (before.NeedActionReport > 0)
					append.Add("要戦闘詳報");
				if (before.NeedAviationMaterial > 0)
					append.Add("要新型航空兵装資材");

				sb.AppendFormat("改造前: {0} Lv. {1} ({2})\r\n",
					before.NameWithClass, before.RemodelAfterLevel, string.Join(", ", append));
			}
			else
			{
				sb.AppendLine("改造前: (なし)");
			}
			if (ship.RemodelAfterShipID != 0)
			{
				var append = new List<string>(4)
				{
					"弾薬 " + ship.RemodelAmmo,
					"鋼材 " + ship.RemodelSteel
				};
				if (ship.NeedBlueprint > 0)
					append.Add("要改装設計図");
				if (ship.NeedCatapult > 0)
					append.Add("要カタパルト");
				if (ship.NeedActionReport > 0)
					append.Add("要戦闘詳報");
				if (ship.NeedAviationMaterial > 0)
					append.Add("要新型航空兵装資材");

				sb.AppendFormat("改造後: {0} Lv. {1} ({2})\r\n",
					ship.RemodelAfterShip.NameWithClass, ship.RemodelAfterLevel, string.Join(", ", append));
			}
			else
			{
				sb.AppendLine("改造後: (なし)");
			}
			sb.AppendLine();
			sb.AppendFormat("図鑑文章: \r\n{0}\r\n\r\n入手文章: \r\n{1}\r\n\r\n",
				!string.IsNullOrWhiteSpace(ship.MessageAlbum) ? ship.MessageAlbum : "(不明)",
				!string.IsNullOrWhiteSpace(ship.MessageGet) ? ship.MessageGet : "(不明)");
		}

		sb.AppendLine("出現海域:");
		{
			string result = GetAppearingArea(ship.ShipID);
			if (string.IsNullOrEmpty(result))
				result = "(不明)";
			sb.AppendLine(result);
		}

		Clipboard.SetDataObject(sb.ToString());
	}

	[RelayCommand]
	private void StripMenu_Edit_GoogleShipName_Click()
	{
		IShipDataMaster? ship = SelectedShip?.Ship;
		if (ship == null)
		{
			System.Media.SystemSounds.Exclamation.Play();
			return;
		}

		try
		{
			ProcessStartInfo psi = new()
			{
				FileName = @"https://www.duckduckgo.com/?q=" + Uri.EscapeDataString(ship.NameWithClass) + AlbumMasterEquipmentResources.KancolleSpecifier,
				UseShellExecute = true
			};
			// google <艦船名> 艦これ
			Process.Start(psi);
		}
		catch (Exception ex)
		{
			Utility.ErrorReporter.SendErrorReport(ex, AlbumMasterEquipmentResources.FailedToSearchOnWeb);
		}
	}

	[RelayCommand]
	private void StripMenu_Edit_CopySpecialEquipmentTable_Click()
	{
		StringBuilder sb = new();
		sb.AppendLine(DialogAlbumMasterShip.SpecialEquipmentTableHeader1);
		sb.AppendLine("|--:|:--|:--|:--|");

		foreach (IShipDataMaster ship in KCDatabase.Instance.MasterShips.Values)
		{
			if (ship.SpecialEquippableCategories is null)
			{
				continue;
			}

			IEnumerable<int> add = ship.SpecialEquippableCategories.Except(ship.ShipTypeInstance.EquippableCategories);
			IEnumerable<int> sub = ship.ShipTypeInstance.EquippableCategories.Except(ship.SpecialEquippableCategories);

			sb.AppendLine($"|{ship.ShipID}|{ship.NameWithClass}|{string.Join(", ", add.Select(id => KCDatabase.Instance.EquipmentTypes[id].NameEN))}|{string.Join(", ", sub.Select(id => KCDatabase.Instance.EquipmentTypes[id].NameEN))}|");
		}

		sb.AppendLine();

		{
			Dictionary<ShipId, List<int>> nyan = [];

			foreach (IEquipmentDataMaster eq in KCDatabase.Instance.MasterEquipments.Values)
			{
				if (!(eq.EquippableShipsAtExpansion?.Any() ?? false))
				{
					continue;
				}

				foreach (ShipId shipId in eq.EquippableShipsAtExpansion)
				{
					if (nyan.TryGetValue(shipId, out List<int>? value))
					{
						value.Add(eq.EquipmentID);
					}
					else
					{
						nyan.Add(shipId, [eq.EquipmentID]);
					}
				}
			}

			sb.AppendLine(DialogAlbumMasterShip.SpecialEquipmentTableHeader2);
			sb.AppendLine("|--:|:--|:--|:--|");

			foreach ((ShipId shipId, List<int> equipmentIds) in nyan.OrderBy(p => p.Key))
			{
				string ship = KCDatabase.Instance.MasterShips[(int)shipId]?.NameWithClass
					?? $"{ConstantsRes.Unknown}({shipId})";

				sb.AppendLine($"|{(int)shipId}|{ship}|{string.Join(", ", equipmentIds)}|{string.Join(", ", equipmentIds.Select(id => KCDatabase.Instance.MasterEquipments[id].NameEN))}|");
			}

		}
		Clipboard.SetDataObject(sb.ToString());
	}



	[RelayCommand]
	private void StripMenu_View_ShowAppearingArea_Click()
	{
		IShipDataMaster? ship = SelectedShip?.Ship;
		if (ship is null)
		{
			System.Media.SystemSounds.Exclamation.Play();
			return;
		}

		string result = GetAppearingArea(ship.ShipID);

		if (string.IsNullOrEmpty(result))
			result = string.Format(DialogAlbumMasterShip.FailedToFindMapOrRecipe, ship.NameWithClass);

		MessageBox.Show(result, DialogAlbumMasterShip.MapOrRecipeSearchCaption, MessageBoxButton.OK, MessageBoxImage.Information);
	}

	[RelayCommand]
	private void StripMenu_View_ShowShipGraphicViewer_Click()
	{
		if (SelectedShip is not null)
		{
			new DialogShipGraphicViewer(SelectedShip.Ship.ShipID).Show(App.Current.MainWindow);
		}
		else
		{
			MessageBox.Show(DialogAlbumMasterShip.SpecifyTargetShip, DialogAlbumMasterShip.NoShipSelectedCaption, MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
	}



	private string GetParameterMinBound(IParameter param)
	{

		if (param == null || param.MinimumEstMax == Parameter.MaximumDefault)
			return "???";
		else if (param.MinimumEstMin == param.MinimumEstMax)
			return param.MinimumEstMin.ToString();
		else if (param.MinimumEstMin == Parameter.MinimumDefault && param.MinimumEstMax == param.Maximum)
			return "???";
		else
			return $"{param.MinimumEstMin}～{param.MinimumEstMax}";

	}

	private string GetParameterMax(IParameter param)
	{

		if (param == null || param.Maximum == Parameter.MaximumDefault)
			return "???";
		else
			return param.Maximum.ToString();

	}

	private string GetAppearingArea(int shipID)
	{

		var ship = KCDatabase.Instance.MasterShips[shipID];
		if (ship == null)
			return string.Empty;

		var sb = new StringBuilder();

		if (!ship.IsAbyssalShip)
		{

			foreach (var record in RecordManager.Instance.ShipDrop.Record
				.Where(s => s.ShipID == shipID && s.EnemyFleetID != 0)
				.Select(s => new
				{
					s.MapAreaID,
					s.MapInfoID,
					s.CellID,
					s.Difficulty,
					EnemyFleetName = RecordManager.Instance.EnemyFleet.Record.ContainsKey(s.EnemyFleetID) ?
						RecordManager.Instance.EnemyFleet.Record[s.EnemyFleetID].FleetName : DialogAlbumMasterShip.Unknown
				})
				.Distinct()
				.OrderBy(r => r.MapAreaID)
				.ThenBy(r => r.MapInfoID)
				.ThenBy(r => r.CellID)
				.ThenBy(r => r.Difficulty)
			)
			{
				sb.AppendFormat("{0}-{1}-{2}{3} ({4})\r\n",
					record.MapAreaID, record.MapInfoID, record.CellID, record.Difficulty > 0 ? " [" + Constants.GetDifficulty(record.Difficulty) + "]" : "", record.EnemyFleetName);
			}

			foreach (var record in RecordManager.Instance.Construction.Record
				.Where(s => s.ShipID == shipID)
				.Select(s => new
				{
					s.Fuel,
					s.Ammo,
					s.Steel,
					s.Bauxite,
					s.DevelopmentMaterial
				})
				.Distinct()
				.OrderBy(r => r.Fuel)
				.ThenBy(r => r.Ammo)
				.ThenBy(r => r.Steel)
				.ThenBy(r => r.Bauxite)
				.ThenBy(r => r.DevelopmentMaterial)
			)
			{
				sb.AppendFormat(DialogAlbumMasterShip.Recipe + " {0} / {1} / {2} / {3} - {4}\r\n",
					record.Fuel, record.Ammo, record.Steel, record.Bauxite, record.DevelopmentMaterial);
			}

		}
		else
		{

			foreach (var record in RecordManager.Instance.EnemyFleet.Record.Values
				.Where(r => r.FleetMember.Contains(shipID))
				.Select(s => new
				{
					s.MapAreaID,
					s.MapInfoID,
					s.CellID,
					s.Difficulty,
					EnemyFleetName = !string.IsNullOrWhiteSpace(s.FleetName) ? s.FleetName : DialogAlbumMasterShip.Unknown
				})
				.Distinct()
				.OrderBy(r => r.MapAreaID)
				.ThenBy(r => r.MapInfoID)
				.ThenBy(r => r.CellID)
				.ThenBy(r => r.Difficulty)
			)
			{
				sb.AppendFormat("{0}-{1}-{2}{3} ({4})\r\n",
					record.MapAreaID, record.MapInfoID, record.CellID, record.Difficulty > 0 ? " [" + Constants.GetDifficulty(record.Difficulty) + "]" : "", record.EnemyFleetName);
			}

		}

		return sb.ToString();
	}
}
