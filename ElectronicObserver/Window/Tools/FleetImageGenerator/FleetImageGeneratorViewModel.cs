using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;
using ElectronicObserverTypes.Serialization.DeckBuilder;

namespace ElectronicObserver.Window.Tools.FleetImageGenerator;

public partial class FleetImageGeneratorViewModel : WindowViewModelBase
{
	private DataSerializationService DataSerialization { get; }
	private ToolService Tools { get; }
	public FleetImageGeneratorTranslationViewModel DialogFleetImageGenerator { get; }

	private FleetImageGeneratorImageDataModel ImageDataModel { get; set; } = new();

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

	public string ImageSaveLocation { get; set; }
	public bool DisableOverwritePrompt { get; set; }
	public bool AutoSetFileNameToDate { get; set; }
	public bool OpenImageAfterOutput { get; set; }

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

	public ObservableCollection<FleetViewModel> Fleets { get; set; } = new();

	public ObservableCollection<AirBaseViewModel> AirBases { get; set; } = new();

	public FleetImageGeneratorViewModel(FleetImageGeneratorImageDataModel model)
	{
		DataSerialization = Ioc.Default.GetRequiredService<DataSerializationService>();
		Tools = Ioc.Default.GetRequiredService<ToolService>();
		DialogFleetImageGenerator = Ioc.Default.GetRequiredService<FleetImageGeneratorTranslationViewModel>();

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ImageType)) return;

			LoadFleets(ImageDataModel);
		};

		PropertyChanged += (sender, args) =>
		{
			bool shouldUpdateFleetVisibility = args.PropertyName is
				nameof(Fleets) or
				nameof(Fleet1Visible) or
				nameof(Fleet2Visible) or
				nameof(Fleet3Visible) or
				nameof(Fleet4Visible);

			if (!shouldUpdateFleetVisibility) return;

			foreach (FleetViewModel fleet in Fleets)
			{
				fleet.FleetEnabled = fleet.Id switch
				{
					1 => Fleet1Visible,
					2 => Fleet2Visible,
					3 => Fleet3Visible,
					4 => Fleet4Visible,

					_ => false,
				};
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

		ImageDataModel = model;

		LoadConfig();
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

		DeckBuilderData = DataSerialization.MakeDeckBuilderData
		(
			HqLevel,
			GetFleet(1, Fleet1Visible),
			GetFleet(2, Fleet2Visible),
			GetFleet(3, Fleet3Visible),
			GetFleet(4, Fleet4Visible),
			AirBases.Skip(0).FirstOrDefault()?.Model,
			AirBases.Skip(1).FirstOrDefault()?.Model,
			AirBases.Skip(2).FirstOrDefault()?.Model
		),
	};

	public void SetImageDataModel(FleetImageGeneratorImageDataModel model)
	{
		ImageDataModel = model;
	}

	private void LoadModel(FleetImageGeneratorImageDataModel model)
	{
		HqLevel = model.DeckBuilderData.HqLevel;

		Title = model.Title;
		Comment = model.Comment;

		Fleet1Visible = model.Fleet1Visible;
		Fleet2Visible = model.Fleet2Visible;
		Fleet3Visible = model.Fleet3Visible;
		Fleet4Visible = model.Fleet4Visible;

		LoadFleets(model);
		LoadAirBases(model);
	}

	private void LoadFleets(FleetImageGeneratorImageDataModel model)
	{
		Fleets = model.DeckBuilderData
			.GetFleetList()
			.Select((f, i) => new FleetViewModel().Initialize(f, i, ImageType))
			.ToObservableCollection();
	}

	private void LoadAirBases(FleetImageGeneratorImageDataModel model)
	{
		AirBases = model.DeckBuilderData
			.GetAirBaseList()
			.Where(a => a is not null)
			.Select(a => new AirBaseViewModel().Initialize(a))
			.ToObservableCollection();
	}

	[ICommand]
	private void ChangeImageType(ImageType? imageType)
	{
		if (imageType is not { } type) return;

		ImageType = type;
	}

	[ICommand]
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

	[ICommand]
	private void OpenConfiguration(System.Windows.Window? window)
	{
		if (window is null) return;

		new FleetImageGeneratorConfigurationWindow(this).Show(window);
	}

	[ICommand]
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
}
