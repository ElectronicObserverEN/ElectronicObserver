using System.Collections.Generic;

namespace ElectronicObserver.Data.DiscordRPC;

public class DiscordRpcModel
{
	public string top { get; set; }
	public List<string> bot { get; set; }
	public string large { get; set; }
	public string small { get; set; }
	public string timestamp { get; set; }
	public int shipId { get; set; }
	public string image { get; set; }
	public string? MapInfo { get; internal set; }
}
