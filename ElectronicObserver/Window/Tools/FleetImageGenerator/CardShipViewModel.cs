using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.FleetImageGenerator;

public class CardShipViewModel : ShipViewModel
{
	public override CardShipViewModel Initialize(IShipData? ship)
	{
		base.Initialize(ship);

		return this;
	}
}