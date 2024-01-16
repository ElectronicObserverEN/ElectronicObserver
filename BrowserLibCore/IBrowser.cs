namespace BrowserLibCore;

public interface IBrowser
{
	void ConfigurationChanged();
	void InitialAPIReceived();
	void Navigate(string url);
	void CloseBrowser();
	void SetProxy(string v);
	void OpenExtraBrowser();
	void OpenAirControlSimulator(string url);
	void OpenCompassPrediction();
	void RequestAutoRefresh();
}
