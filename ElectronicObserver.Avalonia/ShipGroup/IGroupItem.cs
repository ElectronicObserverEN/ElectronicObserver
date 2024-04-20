namespace ElectronicObserver.Avalonia.ShipGroup;

public interface IGroupItem : ICloneable
{
	int GroupID { get; }
	string Name { get; set; }
}
