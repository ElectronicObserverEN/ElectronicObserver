using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Database.MapData;

public class CellModel
{
	// global cell ID 3000~
	public int Id { get; set; }
	public int WorldId { get; set; }
	public int MapId { get; set; }
	// local cell ID 0~
	public int CellId { get; set; }
	public CellType CellType { get; set; }
}
