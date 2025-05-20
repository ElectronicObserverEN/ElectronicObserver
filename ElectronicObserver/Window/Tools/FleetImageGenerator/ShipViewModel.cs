using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Avalonia.Services;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Wpf;
using ElectronicObserverTypes;
using CroppedBitmap = System.Windows.Media.Imaging.CroppedBitmap;

namespace ElectronicObserver.Window.Tools.FleetImageGenerator;

public class ShipViewModel : ObservableObject
{
	private static ImageLoadService ImageLoadService => Ioc.Default.GetRequiredService<ImageLoadService>();
	private KCDatabase Db { get; } = KCDatabase.Instance;
	private IShipData? Model { get; set; }

	public ShipId Id { get; set; }
	public BitmapSource? ShipImageSource { get; private set; }
	public BitmapSource? NameImageSource { get; private set; }
	public string Name { get; set; } = "";
	public int Level { get; set; }

	public int Hp { get; set; }
	public int Armor { get; set; }
	public int Evasion { get; set; }
	public int AirPower { get; set; }
	public int Speed { get; set; }
	public int Range { get; set; }

	public int Firepower { get; set; }
	public int Torpedo { get; set; }
	public int AntiAir { get; set; }
	public int AntiSubmarine { get; set; }
	public int Los { get; set; }
	public int Luck { get; set; }

	public ObservableCollection<EquipmentSlotViewModel> Slots { get; set; } = [];
	public EquipmentSlotViewModel? ExpansionSlot { get; set; }

	public virtual ShipViewModel Initialize(IShipData? ship)
	{
		Model = ship;

		if (ship is null)
		{
			return this;
		}

		Id = ship.MasterShip.ShipId;
		Name = Db.Translation.Ship.Name(ship.MasterShip.Name, ship.MasterShip.ShipId);
		Level = ship.Level;

		Hp = ship.HPMax;
		Armor = ship.ArmorTotal;
		Evasion = ship.EvasionTotal;
		AirPower = Calculator.GetAirSuperiority(ship);
		Speed = ship.Speed;
		Range = ship.Range;

		Firepower = ship.FirepowerTotal;
		Torpedo = ship.TorpedoTotal;
		AntiAir = ship.AATotal;
		AntiSubmarine = ship.ASWTotal;
		Los = ship.LOSTotal;
		Luck = ship.LuckTotal;

		Slots = ship.SlotInstance
			.Take(ship.MasterShip.SlotSize)
			.Zip(ship.MasterShip.Aircraft, (eq, slot) => new EquipmentSlotViewModel(eq, slot))
			.ToObservableCollection();

		ExpansionSlot = ship.IsExpansionSlotAvailable switch
		{
			true => new EquipmentSlotViewModel(ship.ExpansionSlotInstance, 0),
			_ => null,
		};

		Task.Run(LoadImage);

		return this;
	}

	private List<string> RequiredImageResourceTypes() => this switch
	{
		CardShipViewModel => [KCResourceHelper.ResourceTypeShipCard],
		CutInShipViewModel => [KCResourceHelper.ResourceTypeShipName, KCResourceHelper.ResourceTypeShipCutin],
		BannerShipViewModel => [KCResourceHelper.ResourceTypeShipBanner],

		_ => throw new NotImplementedException(),
	};

	private async Task LoadImage()
	{
		foreach (string resourceType in RequiredImageResourceTypes())
		{
			_ = App.Current!.Dispatcher.InvokeAsync(async () =>
			{
				using Bitmap? image = Configuration.Config.FleetImageGenerator.DownloadMissingShipImage switch
				{
					true => await ImageLoadService.GetShipImage(Id, resourceType),
					_ => ImageLoadService.GetShipImageFromDisk(Id, resourceType),
				};

				if (image is null) return;

				if (resourceType == KCResourceHelper.ResourceTypeShipName)
				{
					NameImageSource = ToCroppedImageSource(image);
				}
				else
				{
					ShipImageSource = ToImageSource(image);
				}
			});
		}
	}

	private static BitmapImage ToImageSource(Bitmap bitmap)
	{
		using MemoryStream memoryStream = new();
		bitmap.Save(memoryStream);
		memoryStream.Seek(0, SeekOrigin.Begin);

		BitmapImage bitmapImage = new();
		bitmapImage.BeginInit();
		bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
		bitmapImage.StreamSource = memoryStream;
		bitmapImage.EndInit();

		return bitmapImage;
	}

	private static CroppedBitmap ToCroppedImageSource(Bitmap bitmap)
	{
		using MemoryStream memoryStream = new();
		bitmap.Save(memoryStream);
		memoryStream.Seek(0, SeekOrigin.Begin);

		BitmapImage image = new();
		image.BeginInit();
		image.CacheOption = BitmapCacheOption.OnLoad;
		image.StreamSource = memoryStream;
		image.EndInit();

		const int leftOffset = 140;
		const int topOffset = 5;
		const int rightOffset = 60;
		const int bottomOffset = 25;

		Int32Rect region = new
		(
			leftOffset,
			topOffset,
			(int)image.Width - leftOffset - rightOffset,
			(int)image.Height - topOffset - bottomOffset
		);

		CroppedBitmap cropped = new(image, region);

		return cropped;
	}
}
