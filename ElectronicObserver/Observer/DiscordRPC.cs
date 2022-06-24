using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DiscordRPC;
using System.Threading;

namespace ElectronicObserver.Observer;

class DiscordRPC
{
	#region Singleton

	private static readonly DiscordRPC instance = new DiscordRPC();

	public static DiscordRPC Instance => instance;

	#endregion
	private int count;

	private DiscordRpcClient forcedClient;

	private List<string> clientIds;

	private DiscordRpcClient currentClient;
	private int currentClientIndex = -1;
	private Thread DiscordThread;
	public DiscordFormat data { get; set; }

	private DiscordRPC()
	{

		if (!String.IsNullOrEmpty(Utility.Configuration.Config.Control.DiscordRPCApplicationId))
		{
			string clientID = Utility.Configuration.Config.Control.DiscordRPCApplicationId;
			// Store the client id somewhere
			forcedClient = new DiscordRpcClient(clientID);
			forcedClient.Initialize();
		}
		else if (Utility.Configuration.Config.Control.UseFlagshipIconForRPC)
		{
			clientIds = new List<string>
			{
				"643912955913699363", // --- 1 -> 150
				"643915499888967690", // --- 151 -> 300
				"643915522127298579", // --- 301 -> 450
				"643915540158611544", // --- 451 -> 600
				"644074408389902336", // --- 601 -> 750
				"644074452228898825", // --- 751 -> 900

				"644074508306874389", // --- 1351 -> 1500
			};
		}
		else
		{
			// default application
			forcedClient = new DiscordRpcClient("391369077991538698");
			forcedClient.Initialize();
		}

		StartRPCUpdate();
	}

	private void SetActivity()
	{
		if (!Utility.Configuration.Config.Control.EnableDiscordRPC) return;

		string state = "";

		if (data.bot != null)
		{
			state = data.bot[++count % data.bot.Count];
		}

		if (forcedClient == null)
		{
			int shipID = data.shipId;
			int clientId = Math.Abs((shipID - 1) / 150);
			if (currentClientIndex == clientId)
			{
				if (currentClient.IsDisposed) {
					DiscordRpcClient client = new DiscordRpcClient(clientIds[clientId]);
					client.SetPresence(new RichPresence()
					{
						Details = data.top,
						State = state,
						Assets = new Assets()
						{
							LargeImageKey = data.image,
							LargeImageText = data.large,
							SmallImageText = data.small,
						}
					});
					client.SkipIdenticalPresence = false;
					client.Initialize();
				}
				else
				{
					DiscordRpcClient client = new DiscordRpcClient(clientIds[clientId]);
					client.SetPresence(new RichPresence()
					{
						Details = data.top,
						State = state,
						Assets = new Assets()
						{
							LargeImageKey = data.image,
							LargeImageText = data.large,
							SmallImageText = data.small,
						}
					});
					if (currentClient != null) currentClient.Dispose();
					currentClient = client;
					currentClientIndex = clientId;
					client.Initialize();
				}
			}
			else
			{
				DiscordRpcClient client = new DiscordRpcClient(clientIds[clientId]);
				client.OnReady += (sender, e) =>
				{
					client.SetPresence(new RichPresence()
					{
						Details = data.top,
						State = state,
						Assets = new Assets()
						{
							LargeImageKey = data.image,
							LargeImageText = data.large,
							SmallImageText = data.small,
						}
					});

					if (currentClient != null) currentClient.Dispose();
					currentClient = client;
					currentClientIndex = clientId;
				};
				client.SkipIdenticalPresence = false;
				client.Initialize();
			}
		}
		else
		{
			forcedClient.SetPresence(new RichPresence()
			{
				Details = data.top,
				State = state,
				Assets = new Assets()
				{
					LargeImageKey = data.image,
					LargeImageText = data.large,
					SmallImageText = data.small,
				}
			});
		}
	}

	private void StartRPCUpdate()
	{
		data = new DiscordFormat()
		{
			bot = new List<string>(),
			top = ObserverRes.LoadingIntegration,
			large = ObserverRes.KantaiCollection,
			small = ObserverRes.Idle
		};

		data.bot.Add(ObserverRes.RankDataNotLoaded);

		SetActivity();
		DiscordThread = new Thread(MainLoop);
		DiscordThread.Start();

	}
	private async void MainLoop()
	{
		for (; ; )
		{
			await Task.Delay(15000);
			SetActivity();
		}
	}
	public class DiscordFormat
	{
		public string top { get; set; }
		public List<string> bot { get; set; }
		public string large { get; set; }
		public string small { get; set; }
		public string timestamp { get; set; }
		public int shipId { get; set; }
		public string image { get; set; }
	}

	public string MapInfo { get; set; }
}
