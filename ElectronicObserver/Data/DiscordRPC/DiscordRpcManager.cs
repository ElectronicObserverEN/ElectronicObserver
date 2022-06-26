using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ElectronicObserver.Data.DiscordRPC;

public class DiscordRpcManager
{
	#region Singleton

	private static readonly DiscordRpcManager instance = new DiscordRpcManager();

	public static DiscordRpcManager Instance => instance;

	#endregion

	private readonly List<string> ClientIds = new()
	{
		"643912955913699363", // --- 1 -> 150
		"643915499888967690", // --- 151 -> 300
		"643915522127298579", // --- 301 -> 450
		"643915540158611544", // --- 451 -> 600
		"644074408389902336", // --- 601 -> 750
		"644074452228898825", // --- 751 -> 900

		"644074508306874389", // --- 1351 -> 1500
	};

	public EoToDiscordRpcClient CurrentClient { get; private set; }

	/// <summary>
	/// Used for secretary image
	/// </summary>
	private int CurrentClientIndex { get; set; } = -1;


	private DiscordRpcManager()
	{

		if (!string.IsNullOrEmpty(Utility.Configuration.Config.Control.DiscordRPCApplicationId))
		{
			string clientID = Utility.Configuration.Config.Control.DiscordRPCApplicationId;
			CurrentClient = new EoToDiscordRpcClient(clientID);
		}
		else if (Utility.Configuration.Config.Control.UseFlagshipIconForRPC)
		{
			CurrentClient = new EoToDiscordRpcClient(null);
		}
		else
		{
			// default application
			CurrentClient = new EoToDiscordRpcClient("391369077991538698");
		}

		StartRPCUpdate();
	}

	private void SetActivity()
	{
		if (!Utility.Configuration.Config.Control.EnableDiscordRPC) return;

		if (Utility.Configuration.Config.Control.UseFlagshipIconForRPC)
		{
			int shipID = CurrentClient.CurrentRpcData.CurrentShipId;
			int clientId = Math.Abs((shipID - 1) / 150);
			if (CurrentClientIndex == clientId)
			{
				CurrentClient.UpdatePresence();
			}
			else
			{
				CurrentClient.ChangeClientId(ClientIds[clientId]);
				CurrentClientIndex = clientId;
			}
		}
		else
		{
			CurrentClient.UpdatePresence();
		}
	}

	private void StartRPCUpdate()
	{
		Task task = Task.Run(async () =>
		{
			for (; ; )
			{
				await Task.Delay(15000);
				SetActivity();
			}
		});
	}
}
