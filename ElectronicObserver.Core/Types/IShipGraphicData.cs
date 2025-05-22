using System.Drawing;

namespace ElectronicObserver.Core.Types;

public interface IShipGraphicData
{
	/// <summary>
	/// 艦船ID
	/// </summary>
	public int ShipID { get; }

	/// <summary>
	/// リソースファイル名
	/// </summary>
	public string ResourceName { get; }

	/// <summary>
	/// 画像バージョン
	/// </summary>
	public string GraphicVersion { get; }

	/// <summary>
	/// ボイスバージョン
	/// </summary>
	public string VoiceVersion { get; }

	/// <summary>
	/// 母港ボイスバージョン
	/// </summary>
	public string PortVoiceVersion { get; }


	/// <summary>
	/// 母港での表示座標（通常時）
	/// </summary>
	public Point PortLocation { get; }

	/// <summary>
	/// 母港での表示座標（中破時）
	/// </summary>
	public Point PortLocationDamaged { get; }


	/// <summary>
	/// 改修時の表示座標（通常時）
	/// </summary>
	public Point ModernizationLocation { get; }

	/// <summary>
	/// 改修時の表示座標（中破時）
	/// </summary>
	public Point ModernizationLocationDamaged { get; }

	/// <summary>
	/// 改造時の表示座標（通常時）
	/// </summary>
	public Point RemodelLocation { get; }

	/// <summary>
	/// 改造時の表示座標（中破時）
	/// </summary>
	public Point RemodelLocationDamaged { get; }


	/// <summary>
	/// 出撃時の表示座標（通常時）
	/// </summary>
	public Point SortieLocation { get; }

	/// <summary>
	/// 出撃時の表示座標（中破時）
	/// </summary>
	public Point SortieLocationDamaged { get; }

	/// <summary>
	/// 味方側での演習開始時の表示座標（通常時）
	/// </summary>
	public Point PracticeFriendLocation { get; }

	/// <summary>
	/// 味方側での演習開始時の表示座標（中破時）
	/// </summary>
	public Point PracticeFriendLocationDamaged { get; }


	/// <summary>
	/// 敵側での演習開始時の表示座標（通常時）
	/// </summary>
	public Point PracticeEnemyLocation { get; }


	/// <summary>
	/// 戦闘時の表示座標（通常時）
	/// </summary>
	public Point BattleFriendLocation { get; }

	/// <summary>
	/// 戦闘時の表示座標（中破時）
	/// </summary>
	public Point BattleFriendLocationDamaged { get; }


	/// <summary>
	/// ケッコンカッコカリ時の表示エリア
	/// ≒　顔座標
	/// </summary>
	public Rectangle FaceArea { get; }

	/// <summary>
	/// Corolado Touch の演出用座標
	/// </summary>
	public Point CoroladoCutinLocation { get; }

	/// <summary>
	/// 僚艦夜戦突撃 の演出用座標
	/// </summary>
	public Point KongoCutinLocation { get; }


	public int ID { get; }
}
