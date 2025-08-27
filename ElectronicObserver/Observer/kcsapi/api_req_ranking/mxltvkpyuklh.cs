using System;
using System.Text;
using System.Text.RegularExpressions;
using ElectronicObserver.Data;

namespace ElectronicObserver.Observer.kcsapi.api_req_ranking;

public class mxltvkpyuklh : APIBase
{
	public override void OnResponseReceived(dynamic data)
	{
		KCDatabase db = KCDatabase.Instance;


		string pattern = new StringBuilder("\"api_.{12}\":[0-9]*,\"api_.{12}\":\"").AppendFormat("{0}\"", db.Admiral.AdmiralName).ToString();
		Regex regex = new Regex(pattern, RegexOptions.None, TimeSpan.FromMilliseconds(1000));

		try
		{
			string rankData = regex.Match(data.ToString()).Value;

			if (!string.IsNullOrEmpty(rankData))
			{
				rankData = rankData.Split(',')[0].Split(':')[1].Replace('"', '\0');
				db.Admiral.Senka = int.Parse(rankData);
			}
		}
		catch (Exception ex)
		{   
			//ファイルがロックされている; 頻繁に出るのでエラーレポートを残さない
			Utility.Logger.Add(3, LoggerRes.FailedSaveAPI + ex.Message);
		}

		base.OnResponseReceived((object)data);
	}

	public override string APIName => "api_req_ranking/mxltvkpyuklh";

	public override bool IsResponseSupported => true;
}
