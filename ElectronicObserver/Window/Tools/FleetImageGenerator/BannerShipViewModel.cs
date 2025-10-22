using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.FleetImageGenerator;

public class BannerShipViewModel : ShipViewModel
{
	public override BannerShipViewModel Initialize(IShipData? ship)
	{
		base.Initialize(ship);

		return this;
	}
}