using System.Collections.Generic;
using ElectronicObserver.Core.Types.Data;

namespace ElectronicObserver.Core.Types;

public interface IMapAreaData : IIdentifiable
{
	/// <summary>
	/// 海域カテゴリID
	/// </summary>
	public int MapAreaID { get; }

	/// <summary>
	/// 海域カテゴリ名
	/// </summary>
	public string Name { get; }

	public string NameEN { get; }

	/// <summary>
	/// 海域タイプ　0=通常, 1=イベント
	/// </summary>
	public int MapType { get; }

	public bool IsEventArea { get; }

	public void LoadFromResponse(string apiname, dynamic data);

	public void LoadFromRequest(string apiname, Dictionary<string, string> data);
}
