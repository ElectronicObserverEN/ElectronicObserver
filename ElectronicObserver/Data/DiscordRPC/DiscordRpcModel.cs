using System.Collections.Generic;

namespace ElectronicObserver.Data.DiscordRPC;

public class DiscordRpcModel
{
	public string TopDisplayText { get; set; } = "";

	public List<string> BottomDisplayText { get; set; } = [];

	public string LargeImageHoverText { get; set; } = "";

	public string SmallIconHoverText { get; set; } = "";

	public string TimeStamp { get; set; } = "";

	public int CurrentShipId { get; set; }

	public string ImageKey { get; set; } = "";

	public string? MapInfo { get; internal set; }
}
