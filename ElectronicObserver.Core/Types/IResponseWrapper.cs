namespace ElectronicObserver.Core.Types;

public interface IResponseWrapper
{
	/// <summary>
	/// Responseを読み込みます。
	/// </summary>
	/// <param name="apiname">読み込むAPIの名前。</param>
	/// <param name="data">受信したデータ。</param>
	void LoadFromResponse(string apiname, dynamic data);
}
