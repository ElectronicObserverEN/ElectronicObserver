using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Extensions;
using ElectronicObserverTypes.Serialization.DeckBuilder;

namespace ElectronicObserver.Window.Tools.FleetImageGenerator;

public partial class FleetImageGeneratorViewModel : WindowViewModelBase
{
	private ToolService Tools { get; }
	public FleetImageGeneratorTranslationViewModel DialogFleetImageGenerator { get; }

	private FleetImageGeneratorImageDataModel ImageDataModel { get; set; }

	public string? Title { get; set; }
	public string? Comment { get; set; }

	public int TitleFontSize { get; set; } = 32;
	public int LargeTextFontSize { get; set; } = 24;
	public int MediumTextFontSize { get; set; } = 16;
	public int SmallTextFontSize { get; set; } = 12;
	public int MediumDigitFontSize { get; set; } = 16;
	public int SmallDigitFontSize { get; set; } = 12;

	public bool UseAlbumStatusName { get; set; } = true;
	public int MaxEquipmentNameWidth { get; set; } = 200;
	public bool DownloadMissingShipImage { get; set; }

	public string ImageSaveLocation { get; set; } = "";
	public bool DisableOverwritePrompt { get; set; }
	public bool AutoSetFileNameToDate { get; set; }
	public bool OpenImageAfterOutput { get; set; }
	public bool SynchronizeTitleAndFileName { get; set; }
	public int DesiredFleetColumns { get; set; }
	// if there are more columns than fleets, UniformGrid will still reserve space for those columns
	// making the exported image stretched
	public int FleetColumns => Math.Min(DesiredFleetColumns, DisplayedFleetCount);

	private int DisplayedFleetCount => new List<bool>
	{
		Fleet1Visible,
		Fleet2Visible,
		Fleet3Visible,
		Fleet4Visible,
	}.Count(v => v);

	public bool QuickConfigAccess { get; set; }

	public bool UseCustomTheme { get; set; }
	public Color ForegroundColor { get; set; }
	public Color BackgroundColor { get; set; }
	public SolidColorBrush Foreground => new(ForegroundColor);
	public SolidColorBrush Background => new(BackgroundColor);
	public string? BackgroundImagePath { get; set; }
	public bool BackgroundImageExists => File.Exists(BackgroundImagePath);

	[NotifyPropertyChangedFor(nameof(ShowTankTp))]
	[NotifyPropertyChangedFor(nameof(TankTpGaugeName))]
	[ObservableProperty]
	public partial TpGauge TankTpGauge { get; set; }
	public IEnumerable<TpGauge> TankTpGauges { get; } = Enum.GetValues<TpGauge>().Where(gauge => gauge is not TpGauge.Normal);
	public bool ShowTankTp => TankTpGauge > TpGauge.None;
	public string TankTpGaugeName => TankTpGauge.GetShortGaugeName();

	public int FleetNameFontSize => ImageType switch
	{
		ImageType.Card => LargeTextFontSize,
		ImageType.CutIn => LargeTextFontSize,
		ImageType.Banner => LargeTextFontSize,

		_ => MediumTextFontSize,
	};

	// air power, los
	public int FleetParameterFontSize => ImageType switch
	{
		ImageType.Card => MediumDigitFontSize,
		ImageType.CutIn => MediumDigitFontSize,
		ImageType.Banner => MediumDigitFontSize,

		_ => MediumDigitFontSize,
	};

	public int ShipNameFontSize => ImageType switch
	{
		ImageType.Card => LargeTextFontSize,
		ImageType.CutIn => LargeTextFontSize,

		_ => MediumTextFontSize,
	};

	// level, firepower...
	public int ParameterFontSize => ImageType switch
	{
		ImageType.Card => MediumDigitFontSize,

		_ => MediumDigitFontSize,
	};

	public int EquipmentNameFontSize => ImageType switch
	{
		ImageType.Card => MediumTextFontSize,
		ImageType.Banner => SmallTextFontSize,

		_ => MediumTextFontSize,
	};

	// todo: might be better to have a separate value for this?
	public int CommentFontSize => EquipmentNameFontSize;

	public int AirBaseFontSize => MediumTextFontSize;

	public int HqLevel { get; set; }

	public bool Fleet1Visible { get; set; }
	public bool Fleet2Visible { get; set; }
	public bool Fleet3Visible { get; set; }
	public bool Fleet4Visible { get; set; }

	public ImageType ImageType { get; set; }

	public int ColumnCount => ImageType switch
	{
		ImageType.CutIn => 1,

		_ => 2,
	};

	private FleetViewModel? Fleet1 { get; set; }
	private FleetViewModel? Fleet2 { get; set; }
	private FleetViewModel? Fleet3 { get; set; }
	private FleetViewModel? Fleet4 { get; set; }

	public ObservableCollection<FleetViewModel> Fleets { get; set; } = new();

	public ObservableCollection<AirBaseViewModel> AirBases { get; set; } = new();

	public FleetImageGeneratorViewModel(FleetImageGeneratorImageDataModel model)
	{
		Tools = Ioc.Default.GetRequiredService<ToolService>();
		DialogFleetImageGenerator = Ioc.Default.GetRequiredService<FleetImageGeneratorTranslationViewModel>();

		ImageDataModel = model;

		LoadConfig();

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ImageType)) return;

			LoadFleets(ImageDataModel);
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not (nameof(Fleet1) or nameof(Fleet1Visible))) return;

			if (Fleet1?.Model is null)
			{
				Fleet1Visible = false;
				return;
			}

			if (Fleets.Contains(Fleet1) && !Fleet1Visible)
			{
				Fleets.Remove(Fleet1);
			}

			if (!Fleets.Contains(Fleet1) && Fleet1Visible)
			{
				Fleets.Insert(0, Fleet1);
			}
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not (nameof(Fleet2) or nameof(Fleet2Visible))) return;

			if (Fleet2?.Model is null)
			{
				Fleet2Visible = false;
				return;
			}

			if (Fleets.Contains(Fleet2) && !Fleet2Visible)
			{
				Fleets.Remove(Fleet2);
			}

			if (!Fleets.Contains(Fleet2) && Fleet2Visible)
			{
				int index = Fleet1 switch
				{
					null => 0,
					_ => Fleets.IndexOf(Fleet1) + 1,
				};

				Fleets.Insert(index, Fleet2);
			}
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not (nameof(Fleet3) or nameof(Fleet3Visible))) return;

			if (Fleet3?.Model is null)
			{
				Fleet3Visible = false;
				return;
			}

			if (Fleets.Contains(Fleet3) && !Fleet3Visible)
			{
				Fleets.Remove(Fleet3);
			}

			if (!Fleets.Contains(Fleet3) && Fleet3Visible)
			{
				int index = Fleet4 switch
				{
					null => 0,
					_ when Fleets.Contains(Fleet4) => Fleets.IndexOf(Fleet4),
					_ => Fleets.Count,
				};

				Fleets.Insert(index, Fleet3);
			}
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not (nameof(Fleet4) or nameof(Fleet4Visible))) return;

			if (Fleet4?.Model is null)
			{
				Fleet4Visible = false;
				return;
			}

			if (Fleets.Contains(Fleet4) && !Fleet4Visible)
			{
				Fleets.Remove(Fleet4);
			}

			if (!Fleets.Contains(Fleet4) && Fleet4Visible)
			{
				Fleets.Insert(Fleets.Count, Fleet4);
			}
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ImageDataModel)) return;

			LoadModel(ImageDataModel);
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(DownloadMissingShipImage)) return;

			Configuration.Config.FleetImageGenerator.DownloadMissingShipImage = DownloadMissingShipImage;
		};

		PropertyChanged += (_, args) =>
		{
			if (args.PropertyName is not nameof(TankTpGauge)) return;

			if (Fleet1 is not null)
			{
				Fleet1.TankTpGauge = TankTpGauge;
			}

			if (Fleet2 is not null)
			{
				Fleet2.TankTpGauge = TankTpGauge;
			}

			if (Fleet3 is not null)
			{
				Fleet3.TankTpGauge = TankTpGauge;
			}

			if (Fleet4 is not null)
			{
				Fleet4.TankTpGauge = TankTpGauge;
			}
		};
	}

	private void LoadConfig()
	{
		Title = Configuration.Config.FleetImageGenerator.Argument.Title;
		Comment = Configuration.Config.FleetImageGenerator.Argument.Comment;
		ImageType = (ImageType)Configuration.Config.FleetImageGenerator.ImageType;

		TitleFontSize = (int)Configuration.Config.FleetImageGenerator.Argument.TitleFont.ToSize();
		LargeTextFontSize = (int)Configuration.Config.FleetImageGenerator.Argument.LargeFont.ToSize();
		MediumTextFontSize = (int)Configuration.Config.FleetImageGenerator.Argument.MediumFont.ToSize();
		SmallTextFontSize = (int)Configuration.Config.FleetImageGenerator.Argument.SmallFont.ToSize();
		MediumDigitFontSize = (int)Configuration.Config.FleetImageGenerator.Argument.MediumDigitFont.ToSize();
		SmallDigitFontSize = (int)Configuration.Config.FleetImageGenerator.Argument.SmallDigitFont.ToSize();
		MaxEquipmentNameWidth = Configuration.Config.FleetImageGenerator.MaxEquipmentNameWidth;
		DownloadMissingShipImage = Configuration.Config.FleetImageGenerator.DownloadMissingShipImage;

		ImageSaveLocation = Configuration.Config.FleetImageGenerator.ImageSaveLocation;
		DisableOverwritePrompt = Configuration.Config.FleetImageGenerator.DisableOverwritePrompt;
		AutoSetFileNameToDate = Configuration.Config.FleetImageGenerator.AutoSetFileNameToDate;
		OpenImageAfterOutput = Configuration.Config.FleetImageGenerator.OpenImageAfterOutput;
		SynchronizeTitleAndFileName = Configuration.Config.FleetImageGenerator.SyncronizeTitleAndFileName;
		DesiredFleetColumns = Configuration.Config.FleetImageGenerator.Argument.HorizontalFleetCount;
		QuickConfigAccess = Configuration.Config.FleetImageGenerator.QuickConfigAccess;
		UseCustomTheme = Configuration.Config.FleetImageGenerator.UseCustomTheme;
		ForegroundColor = (Color)ColorConverter.ConvertFromString(Configuration.Config.FleetImageGenerator.ForegroundColor);
		BackgroundColor = (Color)ColorConverter.ConvertFromString(Configuration.Config.FleetImageGenerator.BackgroundColor);
		BackgroundImagePath = Configuration.Config.FleetImageGenerator.Argument.BackgroundImagePath;
		TankTpGauge = Configuration.Config.FleetImageGenerator.TankTpGauge;
	}

	private void SaveConfig()
	{
		System.Drawing.Font NewFont(System.Drawing.Font font, int size, bool isTitle = false) =>
			new
			(
				font.FontFamily,
				size,
				isTitle ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular,
				System.Drawing.GraphicsUnit.Pixel
			);

		Configuration.Config.FleetImageGenerator.Argument.Title = Title ?? "";
		Configuration.Config.FleetImageGenerator.Argument.Comment = Comment ?? "";
		Configuration.Config.FleetImageGenerator.ImageType = (int)ImageType;

		Configuration.Config.FleetImageGenerator.Argument.TitleFont =
			NewFont(Configuration.Config.FleetImageGenerator.Argument.TitleFont, TitleFontSize, true);

		Configuration.Config.FleetImageGenerator.Argument.LargeFont =
			NewFont(Configuration.Config.FleetImageGenerator.Argument.LargeFont, LargeTextFontSize);

		Configuration.Config.FleetImageGenerator.Argument.MediumFont =
			NewFont(Configuration.Config.FleetImageGenerator.Argument.MediumFont, MediumTextFontSize);

		Configuration.Config.FleetImageGenerator.Argument.SmallFont =
			NewFont(Configuration.Config.FleetImageGenerator.Argument.SmallFont, SmallTextFontSize);

		Configuration.Config.FleetImageGenerator.Argument.MediumDigitFont =
			NewFont(Configuration.Config.FleetImageGenerator.Argument.MediumDigitFont, MediumDigitFontSize);

		Configuration.Config.FleetImageGenerator.Argument.SmallDigitFont =
			NewFont(Configuration.Config.FleetImageGenerator.Argument.SmallDigitFont, SmallDigitFontSize);

		Configuration.Config.FleetImageGenerator.MaxEquipmentNameWidth = MaxEquipmentNameWidth;
		Configuration.Config.FleetImageGenerator.DownloadMissingShipImage = DownloadMissingShipImage;

		Configuration.Config.FleetImageGenerator.ImageSaveLocation = ImageSaveLocation;
		Configuration.Config.FleetImageGenerator.AutoSetFileNameToDate = AutoSetFileNameToDate;
		Configuration.Config.FleetImageGenerator.DisableOverwritePrompt = DisableOverwritePrompt;
		Configuration.Config.FleetImageGenerator.OpenImageAfterOutput = OpenImageAfterOutput;
		Configuration.Config.FleetImageGenerator.SyncronizeTitleAndFileName = SynchronizeTitleAndFileName;
		Configuration.Config.FleetImageGenerator.Argument.HorizontalFleetCount = DesiredFleetColumns;
		Configuration.Config.FleetImageGenerator.QuickConfigAccess = QuickConfigAccess;
		Configuration.Config.FleetImageGenerator.UseCustomTheme = UseCustomTheme;
		Configuration.Config.FleetImageGenerator.ForegroundColor = ForegroundColor.ToString();
		Configuration.Config.FleetImageGenerator.BackgroundColor = BackgroundColor.ToString();
		Configuration.Config.FleetImageGenerator.Argument.BackgroundImagePath = BackgroundImagePath ?? "";
		Configuration.Config.FleetImageGenerator.TankTpGauge = TankTpGauge;
	}

	/// <inheritdoc />
	public override void Loaded()
	{
		base.Loaded();

		LoadModel(ImageDataModel);
	}

	public override void Closed()
	{
		base.Closed();

		SaveConfig();
	}

	private IFleetData? GetFleet(int fleetId, bool visible) => visible switch
	{
		true => GetFleetById(fleetId),
		_ => null,
	};

	private IFleetData? GetFleetById(int fleetId) => Fleets
		.FirstOrDefault(f => f.Id == fleetId)?.Model;

	public FleetImageGeneratorImageDataModel GetImageDataModel() => new()
	{
		Title = Title,
		Comment = Comment,

		Fleet1Visible = Fleet1Visible,
		Fleet2Visible = Fleet2Visible,
		Fleet3Visible = Fleet3Visible,
		Fleet4Visible = Fleet4Visible,

		DeckBuilderData = DataSerializationService.MakeDeckBuilderData(new()
		{
			HqLevel = HqLevel,
			Fleet1 = GetFleet(1, Fleet1Visible),
			Fleet2 = GetFleet(2, Fleet2Visible),
			Fleet3 = GetFleet(3, Fleet3Visible),
			Fleet4 = GetFleet(4, Fleet4Visible),
			AirBase1 = AirBases.Skip(0).FirstOrDefault()?.Model,
			AirBase2 = AirBases.Skip(1).FirstOrDefault()?.Model,
			AirBase3 = AirBases.Skip(2).FirstOrDefault()?.Model,
		}),
	};

	public void SetImageDataModel(FleetImageGeneratorImageDataModel model)
	{
		ImageDataModel = model;
	}

	private void LoadModel(FleetImageGeneratorImageDataModel model)
	{
		LoadFleets(model);
		LoadAirBases(model);

		HqLevel = model.DeckBuilderData.HqLevel;

		Title = model.Title;
		Comment = model.Comment;

		Fleet1Visible = model.Fleet1Visible;
		Fleet2Visible = model.Fleet2Visible;
		Fleet3Visible = model.Fleet3Visible;
		Fleet4Visible = model.Fleet4Visible;
	}

	private void LoadFleets(FleetImageGeneratorImageDataModel model)
	{
		Fleets.Clear();

		List<FleetViewModel> fleets = model.DeckBuilderData
			.GetFleetList()
			.Select((f, i) => new FleetViewModel().Initialize(f, i, ImageType, TankTpGauge))
			.ToList();

		Fleet1 = fleets.FirstOrDefault();
		Fleet2 = fleets.Skip(1).FirstOrDefault();
		Fleet3 = fleets.Skip(2).FirstOrDefault();
		Fleet4 = fleets.Skip(3).FirstOrDefault();
	}

	private void LoadAirBases(FleetImageGeneratorImageDataModel model)
	{
		AirBases = model.DeckBuilderData
			.GetAirBaseList()
			.Where(a => a is not null)
			.Select(a => new AirBaseViewModel().Initialize(a))
			.ToObservableCollection();
	}

	[RelayCommand]
	private void ChangeImageType(ImageType? imageType)
	{
		if (imageType is not { } type) return;

		ImageType = type;
	}

	[RelayCommand]
	private void LoadAirBases()
	{
		string? abData = Tools.DeckBuilderFleetExport(new()
		{
			FleetSelectionVisible = false,
			DataSelectionVisible = false,
		});

		if (abData is null) return;

		DeckBuilderData? data = JsonSerializer.Deserialize<DeckBuilderData>(abData);

		if (data is null) return;

		AirBases = data
			.GetAirBaseList()
			.Where(a => a is not null)
			.Select(a => new AirBaseViewModel().Initialize(a))
			.ToObservableCollection();
	}

	[RelayCommand]
	private void OpenConfiguration(System.Windows.Window? window)
	{
		if (window is null) return;

		new FleetImageGeneratorConfigurationWindow(this).Show(window);
	}

	[RelayCommand]
	private void SelectImageSaveFolder()
	{
		System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new()
		{
			RootFolder = Environment.SpecialFolder.Desktop,
			SelectedPath = Path.GetFullPath(ImageSaveLocation),
		};

		if (folderBrowserDialog.ShowDialog() is not System.Windows.Forms.DialogResult.OK) return;

		ImageSaveLocation = folderBrowserDialog.SelectedPath;
	}

	[RelayCommand]
	private void SelectBackgroundImage()
	{
		string? newImagePath = FileService.OpenImagePath(BackgroundImagePath);

		if (newImagePath is null) return;

		BackgroundImagePath = newImagePath;
	}
}
