using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ElectronicObserver.Data;

namespace ElectronicObserver.Services;

public partial class ServerManager
{
	public List<KCServer> Servers { get; set; } = [];

	public KCServer? CurrentServer { get; private set; }

	[GeneratedRegex("ConstServerInfo.World_([0-9]+)[ ]+ = \"(.+)\"", RegexOptions.Multiline)]
	private static partial Regex ServerRegex();

	private string? ServerUrl { get; set; }

	public void LoadServerList(string constBody)
	{
		Servers.Clear();

		MatchCollection matches = ServerRegex().Matches(constBody);

		foreach (GroupCollection groups in matches.Where(m => m.Success).Select(m => m.Groups))
		{
			if (int.TryParse(groups[1].Value, out int num))
			{
				Servers.Add(new()
				{
					Num = num,
					Ip = groups[2].Value,
					Jp = GetJapaneseName(num),
					Name = GetEnglishName(num),
				});
			}
		}

		if (!string.IsNullOrEmpty(ServerUrl))
		{
			LoadCurrentServer(ServerUrl);
			ServerUrl = null;
		}
	}

	public void LoadCurrentServer(string serverUrl)
	{
		if (Servers.Count is 0)
		{
			ServerUrl = serverUrl;
		}
		else
		{
			CurrentServer = Servers.FirstOrDefault(server => server.Ip.Contains(serverUrl)) ?? new()
			{
				Ip = serverUrl,
				Jp = "",
				Name = "",
				Num = 0,
			};
		}
	}

	private string GetJapaneseName(int num) => num switch
	{
		1 => "横須賀鎮守府",
		2 => "呉鎮守府",
		3 => "佐世保鎮守府",
		4 => "舞鶴鎮守府",
		5 => "大湊警備府",
		6 => "トラック泊地",
		7 => "リンガ泊地",
		8 => "ラバウル基地",
		9 => "ショートランド泊地",
		10 => "ブイン基地",
		11 => "タウイタウイ泊地",
		12 => "パラオ泊地",
		13 => "ブルネイ泊地",
		14 => "単冠湾泊地",
		15 => "幌筵泊地",
		16 => "宿毛湾泊地",
		17 => "鹿屋基地",
		18 => "岩川基地",
		19 => "佐伯湾泊地",
		20 => "柱島泊地",
		_ => "",
	};

	private string GetEnglishName(int num) => num switch
	{
		1 => "Yokosuka Naval District",
		2 => "Kure Naval District",
		3 => "Sasebo Naval District",
		4 => "Maizuru Naval District",
		5 => "Ominato Guard District",
		6 => "Truk Anchorage",
		7 => "Lingga Anchorage",
		8 => "Rabaul Naval Base",
		9 => "Shortland Anchorage",
		10 => "Buin Naval Base",
		11 => "Tawi-Tawi Anchorage",
		12 => "Palau Anchorage",
		13 => "Brunei Anchorage",
		14 => "Hitokappu Bay Anchorage",
		15 => "Paramushir Anchorage",
		16 => "Sukumo Bay Anchorage",
		17 => "Kanoya Airfield",
		18 => "Iwagawa Airfield",
		19 => "Saiki Bay Anchorage",
		20 => "Hashirajima Anchorage",
		_ => "",
	};
}
