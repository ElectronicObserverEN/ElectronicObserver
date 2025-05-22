using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.Window.Wpf;

namespace ElectronicObserver.Window.Tools.FleetImageGenerator;

public class ShipViewModel : ObservableObject
{
	private KCDatabase Db { get; }
	private IShipData? Model { get; set; }

	public ShipId Id { get; set; }
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

	public ObservableCollection<EquipmentSlotViewModel> Slots { get; set; } = new();
	public EquipmentSlotViewModel? ExpansionSlot { get; set; }

	public ShipViewModel()
	{
		Db = KCDatabase.Instance;
	}

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

		if (Configuration.Config.FleetImageGenerator.DownloadMissingShipImage)
		{
			Task.Run(DownloadImage);
		}

		return this;
	}

	private async void DownloadImage()
	{
		IEnumerable<string> RequiredImageResourceTypes() => this switch
		{
			CardShipViewModel => new List<string> { KCResourceHelper.ResourceTypeShipCard },
			CutInShipViewModel => new List<string> { KCResourceHelper.ResourceTypeShipName, KCResourceHelper.ResourceTypeShipCutin },
			BannerShipViewModel => new List<string> { KCResourceHelper.ResourceTypeShipBanner },

			_ => throw new NotImplementedException(),
		};

		int? shipId = (int)Id;

		if (shipId is not int id) return;

		foreach (string resourceType in RequiredImageResourceTypes())
		{
			try
			{
				string? imageUri = KCResourceHelper.GetShipImagePath(id, false, resourceType);

				if (File.Exists(imageUri)) return;

				await Ioc.Default
					.GetRequiredService<GameAssetDownloaderService>()
					.DownloadImage(id, resourceType);

				Logger.Add(2, string.Format(FleetImageGeneratorResources.SuccessfullyDownloadedImage, resourceType, Name));

				// in xaml the image source binding is set to Id
				// so after the image has been downloaded, this will force the image to reload
				OnPropertyChanged(nameof(Id));
			}
			catch
			{
				Logger.Add(2, string.Format(FleetImageGeneratorResources.FailedToDownloadImage, resourceType, Name));
			}
		}
	}
}
