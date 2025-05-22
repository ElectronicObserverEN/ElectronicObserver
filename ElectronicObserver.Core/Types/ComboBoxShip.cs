namespace ElectronicObserver.Core.Types;

public record ComboBoxShip(IShipDataMaster Ship)
{
	public string NameEN => Ship switch
	{
		{ ShipId: ShipId.Souya645 or ShipId.Souya650 or ShipId.Souya699 } => $"{Ship.NameEN} ({Ship.ID})",
		_ => Ship.NameEN,
	};
}
