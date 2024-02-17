using System.Collections.Generic;
using System.Threading.Tasks;
using MagicOnion;

namespace BrowserLibCore;

public interface IBrowserHost : IStreamingHub<IBrowserHost, IBrowser>
{
	Task ConnectToBrowser(long handle);
	Task<BrowserConfiguration> Configuration();
	Task SendErrorReport(string exceptionName, string message);
	Task AddLog(int priority, string message);
	Task ConfigurationUpdated(BrowserConfiguration configuration);
	Task SetProxyCompleted();
	Task RequestNavigation(string v);
	Task<byte[][]> GetIconResource();
	Task<bool> IsServerAlive();
	Task<int> GetTheme();
	Task<string> GetFleetData();
	Task<string?> GetFleetAndAirBaseData();
	Task<string> GetShipData(bool allShips);
	Task<string> GetEquipmentData(bool allEquipment);
	Task<Dictionary<int, List<int>>> GetMapList();
	Task<(int?, int?)> GetCurrentMap();
}
