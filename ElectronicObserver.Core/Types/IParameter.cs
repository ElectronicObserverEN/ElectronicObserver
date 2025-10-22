namespace ElectronicObserver.Core.Types;

public interface IParameter
{
	/// <summary>
	/// 初期値(推測値)
	/// </summary>
	public int Minimum { get; }

	/// <summary>
	/// 最大値
	/// </summary>
	public int Maximum { get; set; }

	/// <summary>
	/// 初期値の推測下限
	/// </summary>
	public int MinimumEstMin { get; set; }

	/// <summary>
	/// 初期値の推測上限
	/// </summary>
	public int MinimumEstMax { get; set; }


	/// <summary>
	/// 初期値がデフォルト状態かどうか
	/// </summary>
	public bool IsMinimumDefault { get; }

	/// <summary>
	/// 最大値がデフォルト状態かどうか
	/// </summary>
	public bool IsMaximumDefault { get; }

	/// <summary>
	/// 有効なデータか
	/// </summary>
	public bool IsAvailable { get; }

	/// <summary>
	/// 値が特定されているか
	/// </summary>
	public bool IsDetermined { get; }

	/// <summary>
	/// パラメータを推測します。
	/// </summary>
	/// <param name="level">艦船のレベル。</param>
	/// <param name="current">現在値。</param>
	/// <param name="max">最大値。</param>
	/// <returns>予測パラメータが範囲外の値をとったとき true 。</returns>
	public bool SetEstParameter(int level, int current, int max);



	// level > 99 のとき、最小値と最大値が反転するため
	public int GetEstParameterMin(int level);

	public int GetEstParameterMax(int level);

	public int GetParameter(int level);

	/// <summary>
	/// Returns the level when the parameter will increase, null if increase is impossible.
	/// </summary>
	/// <param name="level">Current ship level.</param>
	/// <param name="current">Current parameter value.</param>
	/// <returns>Level or null if the parameter will never increase.</returns>
	public int? GetNextLevel(int level, int? current = null);
}
