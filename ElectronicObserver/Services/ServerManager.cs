using System;
using System.Collections.Generic;
using System.Linq;
using ElectronicObserver.Data;

namespace ElectronicObserver.Services;

public class ServerManager
{
	private static List<KCServer> Servers { get; set; } =
	[
		new() { Num = 1, Host = "w01y.kancolle-server.com", Jp = "横須賀鎮守府", Name = "Yokosuka Naval District" },
		new() { Num = 2, Host = "w02k.kancolle-server.com", Jp = "呉鎮守府", Name = "Kure Naval District" },
		new() { Num = 3, Host = "w03s.kancolle-server.com", Jp = "佐世保鎮守府", Name = "Sasebo Naval District" },
		new() { Num = 4, Host = "w04m.kancolle-server.com", Jp = "舞鶴鎮守府", Name = "Maizuru Naval District" },
		new() { Num = 5, Host = "w05o.kancolle-server.com", Jp = "大湊警備府", Name = "Ominato Guard District" },
		new() { Num = 6, Host = "w06t.kancolle-server.com", Jp = "トラック泊地", Name = "Truk Anchorage" },
		new() { Num = 7, Host = "w07l.kancolle-server.com", Jp = "リンガ泊地", Name = "Lingga Anchorage" },
		new() { Num = 8, Host = "w08r.kancolle-server.com", Jp = "ラバウル基地", Name = "Rabaul Naval Base" },
		new() { Num = 9, Host = "w09s.kancolle-server.com", Jp = "ショートランド泊地", Name = "Shortland Anchorage" },
		new() { Num = 10, Host = "w10b.kancolle-server.com", Jp = "ブイン基地", Name = "Buin Naval Base" },
		new() { Num = 11, Host = "w11t.kancolle-server.com", Jp = "タウイタウイ泊地", Name = "Tawi-Tawi Anchorage" },
		new() { Num = 12, Host = "w12p.kancolle-server.com", Jp = "パラオ泊地", Name = "Palau Anchorage" },
		new() { Num = 13, Host = "w13b.kancolle-server.com", Jp = "ブルネイ泊地", Name = "Brunei Anchorage" },
		new() { Num = 14, Host = "w14h.kancolle-server.com", Jp = "単冠湾泊地", Name = "Hitokappu Bay Anchorage" },
		new() { Num = 15, Host = "w15p.kancolle-server.com", Jp = "幌筵泊地", Name = "Paramushir Anchorage" },
		new() { Num = 16, Host = "w16s.kancolle-server.com", Jp = "宿毛湾泊地", Name = "Sukumo Bay Anchorage" },
		new() { Num = 17, Host = "w17k.kancolle-server.com", Jp = "鹿屋基地", Name = "Kanoya Airfield" },
		new() { Num = 18, Host = "w18i.kancolle-server.com", Jp = "岩川基地", Name = "Iwagawa Airfield" },
		new() { Num = 19, Host = "w19s.kancolle-server.com", Jp = "佐伯湾泊地", Name = "Saiki Bay Anchorage" },
		new() { Num = 20, Host = "w20h.kancolle-server.com", Jp = "柱島泊地", Name = "Hashirajima Anchorage" },
	];

	public KCServer? CurrentServer { get; private set; } =
#if DEBUG
		// need this to test data submission in debug mode
		new() { Num = 0, Host = "", Jp = "DEBUG", Name = "DEBUG" };
#else
		null;
#endif

	public event EventHandler? CurrentServerChanged;

	public void LoadCurrentServer(string serverUrl)
	{
		CurrentServer = Servers.FirstOrDefault(server => server.Host.Contains(serverUrl));
		CurrentServerChanged?.Invoke(this, EventArgs.Empty);
	}
}
