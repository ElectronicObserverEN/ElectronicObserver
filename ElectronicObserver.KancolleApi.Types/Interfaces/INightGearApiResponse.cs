namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface INightGearApiResponse
{
	/// <summary>
	/// 夜間触接機ID　[2]　[0]=味方, [1]=敵　なければ -1 <br />
	/// <br />
	/// Element type is <see cref="int"/> or <see cref="string"/>.
	/// </summary>
	List<object> ApiTouchPlane { get; set; }

	/// <summary>
	/// 照明弾投射艦インデックス　[2]; [味方, 敵]　0起点、随伴艦隊は 6-11　発動しなければ-1
	/// </summary>
	List<int> ApiFlarePos { get; set; }
}
