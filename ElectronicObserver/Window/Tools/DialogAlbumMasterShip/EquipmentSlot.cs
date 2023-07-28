using System.Text;
using ElectronicObserverTypes;

namespace ElectronicObserver.Window.Tools.DialogAlbumMasterShip;

public record EquipmentSlot(int Size, IEquipmentDataMaster? Equipment, EquipmentStatus Status)
{
	public string Name => Equipment?.NameEN ?? Status switch
	{
		EquipmentStatus.Known => AlbumMasterShipResources.Empty,
		EquipmentStatus.Unknown => "??"
	};

	public EquipmentIconType IconType => Equipment?.IconTypeTyped ?? Status switch
	{
		EquipmentStatus.Known => EquipmentIconType.Nothing,
		EquipmentStatus.Unknown => EquipmentIconType.Unknown
	};

	public string? ToolTip => MakeToolTip(Equipment);

	private string? MakeToolTip(IEquipmentDataMaster? eq)
	{
		if (eq is null) return null;

		StringBuilder sb = new();

		sb.AppendFormat("{0} {1} (ID: {2})\r\n", eq.CategoryTypeInstance.NameEN, eq.NameEN, eq.EquipmentID);
		if (eq.Firepower != 0) sb.AppendFormat(AlbumMasterShipResources.Firepower + " {0:+0;-0}\r\n", eq.Firepower);
		if (eq.Torpedo != 0) sb.AppendFormat(AlbumMasterShipResources.Torpedo + " {0:+0;-0}\r\n", eq.Torpedo);
		if (eq.AA != 0) sb.AppendFormat(AlbumMasterShipResources.AA + " {0:+0;-0}\r\n", eq.AA);
		if (eq.Armor != 0) sb.AppendFormat(AlbumMasterShipResources.Armor + " {0:+0;-0}\r\n", eq.Armor);
		if (eq.ASW != 0) sb.AppendFormat(AlbumMasterShipResources.ASW + " {0:+0;-0}\r\n", eq.ASW);
		if (eq.Evasion != 0) sb.AppendFormat("{0} {1:+0;-0}\r\n", eq.CategoryType == EquipmentTypes.Interceptor ? AlbumMasterShipResources.Interception : AlbumMasterShipResources.Evasion, eq.Evasion);
		if (eq.LOS != 0) sb.AppendFormat(AlbumMasterShipResources.LOS + " {0:+0;-0}\r\n", eq.LOS);
		if (eq.Accuracy != 0) sb.AppendFormat("{0} {1:+0;-0}\r\n", eq.CategoryType == EquipmentTypes.Interceptor ? AlbumMasterShipResources.AntiBomb : AlbumMasterShipResources.Accuracy, eq.Accuracy);
		if (eq.Bomber != 0) sb.AppendFormat(AlbumMasterShipResources.Bombing + " {0:+0;-0}\r\n", eq.Bomber);
		sb.AppendLine(AlbumMasterShipResources.RightClickToOpenInNewWindow);

		return sb.ToString();
	}
}
