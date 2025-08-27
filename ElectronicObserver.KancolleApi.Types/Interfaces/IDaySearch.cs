using ElectronicObserver.Core.Types;

namespace ElectronicObserver.KancolleApi.Types.Interfaces;

public interface IDaySearch
{
	/// <summary>
	/// 索敵成否　[0]=味方, [1]=敵　1=成功, 2=成功(未帰還機あり), 3=未帰還, 4=失敗, 5=発見, 6=発見できず
	/// </summary>
	List<DetectionType> ApiSearch { get; set; }
}
