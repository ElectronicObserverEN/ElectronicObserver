using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordRPC;
using ElectronicObserver.Observer;

namespace ElectronicObserver.Data.DiscordRPC;

public class EoToDiscordRpcClient
{
	/// <summary>
	/// Count used to display different messages
	/// </summary>
	private int Count { get; set; }

	public string? CurrentClientId { get; private set; }

	/// <summary>
	/// Current client
	/// </summary>
	private DiscordRpcClient? CurrentClient { get; set; }


	/// <summary>
	/// Current RPC data 
	/// </summary>
	public DiscordRpcModel CurrentRpcData { get; set; }

	public EoToDiscordRpcClient(string? clientId)
	{
		CurrentClientId = clientId;
		CurrentRpcData =  new DiscordRpcModel()
		{
			bot = new List<string>(),
			top = ObserverRes.LoadingIntegration,
			large = ObserverRes.KantaiCollection,
			small = ObserverRes.Idle
		};

		CurrentRpcData.bot.Add(ObserverRes.RankDataNotLoaded);

		Initialize();
	}

	public void Initialize()
	{
		if (string.IsNullOrEmpty(CurrentClientId)) return;

		CurrentClient = new DiscordRpcClient(CurrentClientId);
		CurrentClient.Initialize();
		CurrentClient.OnReady += CurrentClient_OnReady;
		CurrentClient.OnClose += CurrentClient_OnClose;
	}

	public void ChangeClientId(string newClientID)
	{
		if (string.IsNullOrEmpty(newClientID)) return;
		if (newClientID == CurrentClientId) return;

		if (CurrentClient != null) CurrentClient.Dispose();
		CurrentClientId = newClientID;
		Initialize();
	}

	private bool NeedToReinit => CurrentClient is null || CurrentClient.IsDisposed || !CurrentClient.IsInitialized;

	public void UpdatePresence()
	{
		if (NeedToReinit) Initialize();
		if (CurrentClient is null) return;

		string state = "";

		if (CurrentRpcData.bot != null && CurrentRpcData.bot.Any())
		{
			state = CurrentRpcData.bot[++Count % CurrentRpcData.bot.Count];
		}

		CurrentClient.SetPresence(new RichPresence()
		{
			Details = CurrentRpcData.top,
			State = state,
			Assets = new Assets()
			{
				LargeImageKey = CurrentRpcData.image,
				LargeImageText = CurrentRpcData.large,
				SmallImageText = CurrentRpcData.small,
			}
		});
	}

	private void CurrentClient_OnReady(object sender, global::DiscordRPC.Message.ReadyMessage args)
	{
		UpdatePresence();
	}


	private void CurrentClient_OnClose(object sender, global::DiscordRPC.Message.CloseMessage args)
	{
		CurrentClient?.Dispose();
		CurrentClient = null;
	}
}
