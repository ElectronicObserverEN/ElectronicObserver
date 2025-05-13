namespace ElectronicObserverTypes.Extensions;

public static class ShipIdExtensions
{
	public static bool IsNightCarrier(this ShipId shipId) =>
		shipId is
			ShipId.SaratogaMkII or
			ShipId.AkagiKaiNiE or
			ShipId.KagaKaiNiE or
			ShipId.ShimaneMaruKai or
			ShipId.RyuuhouKaiNiE;
}
