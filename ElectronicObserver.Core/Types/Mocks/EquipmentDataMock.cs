using System.Text;

namespace ElectronicObserver.Core.Types.Mocks;

public class EquipmentDataMock : IEquipmentData
{
	public int MasterID { get; set; }
	public int EquipmentID => (int)EquipmentId;
	public EquipmentId EquipmentId => MasterEquipment.EquipmentId;
	public bool IsLocked { get; set; }
	public int Level { get; set; }
	public UpgradeLevel UpgradeLevel { get; set; }
	public int AircraftLevel { get; set; }
	public IEquipmentDataMaster MasterEquipment { get; }
	public string Name => MasterEquipment.NameEN;
	public string NameWithLevel
	{
		get
		{
			StringBuilder sb = new(MasterEquipment.NameEN);

			if (Level > 0)
			{
				sb.Append('+').Append(Level);
			}

			if (AircraftLevel > 0)
			{
				sb.Append(' ').Append(AircraftLevel switch
				{
					1 => "|",
					2 => "||",
					3 => "|||",
					4 => "/",
					5 => "//",
					6 => "///",
					7 => ">>",
					_ => "",
				});
			}

			return sb.ToString();
		}
	}
	public bool IsRelocated { get; set; }
	public int ID { get; set; }
	public dynamic RawData { get; set; }
	public bool IsAvailable { get; set; }

	public EquipmentDataMock(IEquipmentDataMaster equip)
	{
		MasterEquipment = equip;
	}

	public void LoadFromResponse(string apiname, dynamic data)
	{
		throw new System.NotImplementedException();
	}

	public override string ToString() => $"[{EquipmentID}] {Name}";
}
