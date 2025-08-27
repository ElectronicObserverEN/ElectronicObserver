using System.Collections.Generic;
using System.Linq;
using DiscordRPC;

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
			BottomDisplayText = new List<string>(),
			TopDisplayText = ObserverRes.LoadingIntegration,
			LargeImageHoverText = ObserverRes.KantaiCollection,
			SmallIconHoverText = ObserverRes.Idle
		};

		CurrentRpcData.BottomDisplayText.Add(ObserverRes.RankDataNotLoaded);

		Initialize();
	}

	public void Initialize()
	{
		if (string.IsNullOrEmpty(CurrentClientId)) return;
		if (!Utility.Configuration.Config.Control.EnableDiscordRPC) return;

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

		if (CurrentRpcData.BottomDisplayText != null && CurrentRpcData.BottomDisplayText.Any())
		{
			state = CurrentRpcData.BottomDisplayText[++Count % CurrentRpcData.BottomDisplayText.Count];
		}

		CurrentClient.SetPresence(new RichPresence()
		{
			Details = CurrentRpcData.TopDisplayText,
			State = state,
			Assets = new Assets()
			{
				LargeImageKey = CurrentRpcData.ImageKey,
				LargeImageText = CurrentRpcData.LargeImageHoverText,
				SmallImageText = CurrentRpcData.SmallIconHoverText,
			}
		});
	}

	public void CloseRPC()
	{
		CurrentClient?.ClearPresence();
		CurrentClient?.Dispose();
		CurrentClient = null;
	}

	private void CurrentClient_OnReady(object sender, global::DiscordRPC.Message.ReadyMessage args)
	{
		UpdatePresence();
	}


	private void CurrentClient_OnClose(object sender, global::DiscordRPC.Message.CloseMessage args)
	{
		CloseRPC();
	}
}
