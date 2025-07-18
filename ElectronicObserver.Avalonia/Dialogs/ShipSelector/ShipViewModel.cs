using Avalonia.Media.Imaging;
using ElectronicObserver.Avalonia.Services;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Extensions;

namespace ElectronicObserver.Avalonia.Dialogs.ShipSelector;

public class ShipViewModel(IShipData ship, ImageLoadService imageLoadService)
{
	public IShipData Ship { get; } = ship;
	private ImageLoadService ImageLoadService { get; } = imageLoadService;

	public Task<Bitmap?> Image => ImageLoadService.GetShipImage(Ship.MasterShip.ShipId, GameResourceHelper.ResourceTypeShipBanner);
	public int NightPowerBase => Ship.FirepowerBase + Ship.TorpedoBase;
	public bool CanEquipDaihatsu => Ship.CanEquipDaihatsu();
	public bool CanEquipTank => Ship.CanEquipTank();
	public bool CanEquipFcf => Ship.CanEquipFcf();
	public bool CanEquipBulge => Ship.CanEquipBulge();
	public bool CanEquipSeaplaneFighter => Ship.CanEquipSeaplaneFighter();
	public bool IsExpansionSlotAvailable => Ship.IsExpansionSlotAvailable;
}
