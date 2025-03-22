using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronicObserver.Utility;

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
		"995320365498777620", // --- 901 -> 1050
		"995320998041763891", // --- 1051 -> 1200
		"995321106024108112", // --- 1201 -> 1350
		"644074508306874389", // --- 1351 -> 1500
	};

	private EoToDiscordRpcClient CurrentClient { get; set; }

	/// <summary>
	/// Used for secretary image
	/// </summary>
	private int CurrentClientIndex { get; set; } = -1;


	private DiscordRpcManager()
	{
		Configuration.Instance.ConfigurationChanged += ConfigurationChanged;

		if (!string.IsNullOrEmpty(Utility.Configuration.Config.Control.DiscordRPCApplicationId))
		{
			string clientID = Utility.Configuration.Config.Control.DiscordRPCApplicationId;
			CurrentClient = new EoToDiscordRpcClient(clientID);
		}
		else if (Utility.Configuration.Config.Control.RpcIconKind is RpcIconKind.Default)
		{
			// default application
			CurrentClient = new EoToDiscordRpcClient("391369077991538698");
		}
		else
		{
			CurrentClient = new EoToDiscordRpcClient(null);
		}

		StartRPCUpdate();
	}

	private void ConfigurationChanged()
	{
		if (!Utility.Configuration.Config.Control.EnableDiscordRPC)
		{
			// --- Disable RPC
			CurrentClient.CloseRPC();
		}
		else
		{
			if (!string.IsNullOrEmpty(Utility.Configuration.Config.Control.DiscordRPCApplicationId))
			{
				string clientID = Utility.Configuration.Config.Control.DiscordRPCApplicationId;
				CurrentClient.ChangeClientId(clientID);
			}
			else if (Utility.Configuration.Config.Control.RpcIconKind is RpcIconKind.Default)
			{
				// default application
				CurrentClient.ChangeClientId("391369077991538698");
			}
		}

		SetActivity();
	}

	private void SetActivity()
	{
		if (Utility.Configuration.Config.Control.RpcIconKind is RpcIconKind.Secretary or RpcIconKind.Ship)
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

	public DiscordRpcModel GetRPCData() => CurrentClient.CurrentRpcData;

	private void StartRPCUpdate()
	{
		Task task = Task.Run(async () =>
		{
			for (; ; )
			{
				SetActivity();
				await Task.Delay(15000);
			}
		});
	}
}
