using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media.Imaging;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Avalonia.Controls;

public partial class EquipmentIcon : UserControl
{
	public static readonly StyledProperty<EquipmentIconType> IconTypeProperty =
		AvaloniaProperty.Register<EquipmentIcon, EquipmentIconType>(nameof(IconType), defaultBindingMode: BindingMode.OneWay);

	public EquipmentIconType IconType
	{
		get => GetValue(IconTypeProperty);
		set => SetValue(IconTypeProperty, value);
	}

	private static Dictionary<EquipmentIconType, Bitmap> IconCache { get; } = [];

	public EquipmentIcon()
	{
		InitializeComponent();
		UpdateIcon();
	}

	/// <inheritdoc />
	protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
	{
		if (change.Sender is EquipmentIcon equipmentIcon)
		{
			equipmentIcon.UpdateIcon();
		}

		base.OnPropertyChanged(change);
	}

	private void UpdateIcon()
	{
		// Image? imageControl = this.FindControl<Image>(nameof(EquipmentIconImage));
		EquipmentIconImage?.Source = GetCachedBitmap(IconType);
	}

	private static Bitmap? GetCachedBitmap(EquipmentIconType iconType)
	{
		if (IconCache.TryGetValue(iconType, out Bitmap? cachedBitmap)) return cachedBitmap;

		string path = iconType switch
		{
			EquipmentIconType.Nothing => "Assets/Equipment/Nothing.png",
			EquipmentIconType.MainGunSmall => "Assets/Equipment/MainGunS.png",
			EquipmentIconType.MainGunMedium => "Assets/Equipment/MainGunM.png",
			EquipmentIconType.MainGunLarge => "Assets/Equipment/MainGunL.png",
			EquipmentIconType.SecondaryGun => "Assets/Equipment/SecondaryGun.png",
			EquipmentIconType.Torpedo => "Assets/Equipment/Torpedo.png",
			EquipmentIconType.CarrierBasedFighter => "Assets/Equipment/CarrierBasedFighter.png",
			EquipmentIconType.CarrierBasedBomber => "Assets/Equipment/CarrierBasedBomber.png",
			EquipmentIconType.CarrierBasedTorpedo => "Assets/Equipment/CarrierBasedTorpedo.png",
			EquipmentIconType.CarrierBasedRecon => "Assets/Equipment/CarrierBasedRecon.png",
			EquipmentIconType.Seaplane => "Assets/Equipment/Seaplane.png",
			EquipmentIconType.Radar => "Assets/Equipment/Radar.png",
			EquipmentIconType.AAShell => "Assets/Equipment/AAShell.png",
			EquipmentIconType.APShell => "Assets/Equipment/APShell.png",
			EquipmentIconType.DamageControl => "Assets/Equipment/DamageControl.png",
			EquipmentIconType.AAGun => "Assets/Equipment/AAGun.png",
			EquipmentIconType.HighAngleGun => "Assets/Equipment/HighAngleGun.png",
			EquipmentIconType.DepthCharge => "Assets/Equipment/DepthCharge.png",
			EquipmentIconType.Sonar => "Assets/Equipment/Sonar.png",
			EquipmentIconType.Engine => "Assets/Equipment/Engine.png",
			EquipmentIconType.LandingCraft => "Assets/Equipment/LandingCraft.png",
			EquipmentIconType.Autogyro => "Assets/Equipment/Autogyro.png",
			EquipmentIconType.ASPatrol => "Assets/Equipment/ASPatrol.png",
			EquipmentIconType.ExtraArmor => "Assets/Equipment/Bulge.png",
			EquipmentIconType.Searchlight => "Assets/Equipment/Searchlight.png",
			EquipmentIconType.TransportContainer => "Assets/Equipment/DrumCanister.png",
			EquipmentIconType.RepairFacility => "Assets/Equipment/RepairFacility.png",
			EquipmentIconType.StarShell => "Assets/Equipment/Flare.png",
			EquipmentIconType.CommandFacility => "Assets/Equipment/CommandFacility.png",
			EquipmentIconType.AviationPersonnel => "Assets/Equipment/MaintenanceTeam.png",
			EquipmentIconType.AADirector => "Assets/Equipment/AADirector.png",
			EquipmentIconType.Rocket => "Assets/Equipment/RocketArtillery.png",
			EquipmentIconType.SurfaceShipPersonnel => "Assets/Equipment/PicketCrew.png",
			EquipmentIconType.FlyingBoat => "Assets/Equipment/FlyingBoat.png",
			EquipmentIconType.Ration => "Assets/Equipment/Ration.png",
			EquipmentIconType.Supplies => "Assets/Equipment/Supplies.png",
			EquipmentIconType.SpecialAmphibiousTank => "Assets/Equipment/AmphibiousVehicle.png",
			EquipmentIconType.LandBasedAttacker => "Assets/Equipment/LandAttacker.png",
			EquipmentIconType.Interceptor => "Assets/Equipment/Interceptor.png",
			EquipmentIconType.JetBomberKeiun => "Assets/Equipment/JetFightingBomberKeiun.png",
			EquipmentIconType.JetBomberKikka => "Assets/Equipment/JetFightingBomberKikka.png",
			EquipmentIconType.JetBomberHo229 => "Assets/Equipment/JetFightingBomberHo229.png",
			EquipmentIconType.TransportMaterial => "Assets/Equipment/TransportMaterials.png",
			EquipmentIconType.SubmarineEquipment => "Assets/Equipment/SubmarineEquipment.png",
			EquipmentIconType.SeaplaneFighter => "Assets/Equipment/SeaplaneFighter.png",
			EquipmentIconType.LandBasedFighter => "Assets/Equipment/ArmyInterceptor.png",
			EquipmentIconType.NightFighter => "Assets/Equipment/NightFighter.png",
			EquipmentIconType.NightAttacker => "Assets/Equipment/NightAttacker.png",
			EquipmentIconType.LandBasedASPatrol => "Assets/Equipment/LandASPatrol.png",
			EquipmentIconType.LandAssaulter => "Assets/Equipment/LandAssaulter.png",
			EquipmentIconType.HeavyBomber => "Assets/Equipment/HeavyBomber.png",
			EquipmentIconType.NightSeaplane => "Assets/Equipment/NightSeaplane.png",
			EquipmentIconType.NightSeaplaneBomber => "Assets/Equipment/NightSeaplaneBomber.png",
			EquipmentIconType.ArmyInfantry => "Assets/Equipment/ArmyInfantry.png",
			EquipmentIconType.SmokeGenerator => "Assets/Equipment/SmokeGenerator.png",
			EquipmentIconType.BarrageBalloon => "Assets/Equipment/BarrageBalloon.png",
			EquipmentIconType.LandBasedFighterJet => "Assets/Equipment/LandBasedFighterJet.png",
			EquipmentIconType.LandBasedFighterShinden => "Assets/Equipment/LandBasedFighterShinden.png",
			EquipmentIconType.NightBomber => "Assets/Equipment/NightBomber.png",

			_ => "Assets/Equipment/Unknown.png",
		};

		if (!File.Exists(path))
		{
			return null;
		}

		Bitmap bitmap = new(path);
		IconCache[iconType] = bitmap;
		return bitmap;
	}
}

