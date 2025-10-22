using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Window.Tools.DropRecordViewer;

public class DropRecordFilterViewModel : ObservableObject
{
	public bool IgnoreCommonDrops { get; set; }

	public bool MatchesFilter(DropRecordRow row)
	{
		if (IgnoreCommonDrops)
		{
			if (row.ShipId <= ShipId.Unknown) return false;
			if (IsCommonDrop(row.ShipId)) return false;
		}

		return true;
	}

	public bool MatchesFilter(MergedDropRecordRow row)
	{
		if (IgnoreCommonDrops)
		{
			if (row.ShipId <= ShipId.Unknown) return false;
			if (IsCommonDrop(row.ShipId)) return false;
		}

		return true;
	}

	private static bool IsCommonDrop(ShipId shipId) => shipId is
		ShipId.Mutsuki or
		ShipId.Kisaragi or
		ShipId.Nagatsuki or
		ShipId.Mikazuki or
		ShipId.Fubuki or
		ShipId.Shirayuki or
		ShipId.Miyuki or
		ShipId.Isonami or
		ShipId.Ayanami or
		ShipId.Shikinami or
		ShipId.Akebono or
		ShipId.Ushio or
		ShipId.Kagerou or
		ShipId.Shiranui or
		ShipId.Kuroshio or
		ShipId.Yukikaze or
		ShipId.Nagara or
		ShipId.Isuzu or
		ShipId.Yura or
		ShipId.Ooi or
		ShipId.Kitakami or
		ShipId.Fusou or
		ShipId.Yamashiro or
		ShipId.Satsuki or
		ShipId.Fumizuki or
		ShipId.Kikuzuki or
		ShipId.Mochizuki or
		ShipId.Hatsuyuki or
		ShipId.Murakumo or
		ShipId.Akatsuki or
		ShipId.Hibiki or
		ShipId.Ikazuchi or
		ShipId.Inazuma or
		ShipId.Hatsuharu or
		ShipId.Nenohi or
		ShipId.Wakaba or
		ShipId.Hatsushimo or
		ShipId.Shiratsuyu or
		ShipId.Shigure or
		ShipId.Murasame or
		ShipId.Yuudachi or
		ShipId.Samidare or
		ShipId.Suzukaze or
		ShipId.Arare or
		ShipId.Kasumi or
		ShipId.Shimakaze or
		ShipId.Tenryuu or
		ShipId.Tatsuta or
		ShipId.Natori or
		ShipId.Sendai or
		ShipId.Jintsuu or
		ShipId.Naka or
		ShipId.Furutaka or
		ShipId.Kako or
		ShipId.Aoba or
		ShipId.Myoukou or
		ShipId.Nachi or
		ShipId.Ashigara or
		ShipId.Haguro or
		ShipId.Takao or
		ShipId.Atago or
		ShipId.Maya or
		ShipId.Choukai or
		ShipId.Mogami or
		ShipId.Tone or
		ShipId.Chikuma or
		ShipId.Shouhou or
		ShipId.Hiyou or
		ShipId.Ryuujou or
		ShipId.Ise or
		ShipId.Kongou or
		ShipId.Haruna or
		ShipId.Nagato or
		ShipId.Mutsu or
		ShipId.Akagi or
		ShipId.Kaga or
		ShipId.Kirishima or
		ShipId.Hiei or
		ShipId.Hyuuga or
		ShipId.Houshou or
		ShipId.Souryuu or
		ShipId.Hiryuu or
		ShipId.Junyou or
		ShipId.Oboro or
		ShipId.Sazanami or
		ShipId.Asashio or
		ShipId.Ooshio or
		ShipId.Michishio or
		ShipId.Arashio or
		ShipId.Kuma or
		ShipId.Tama or
		ShipId.Kiso or
		ShipId.Chitose or
		ShipId.Chiyoda or
		ShipId.Shoukaku or
		ShipId.Zuikaku or
		ShipId.Kinu or
		ShipId.Abukuma or
		ShipId.Yuubari or
		ShipId.Zuihou or
		ShipId.Mikuma or
		ShipId.Maikaze or
		ShipId.Kinugasa or
		ShipId.Suzuya or
		ShipId.Kumano or
		ShipId.I168 or
		ShipId.I58 or
		ShipId.I8 or
		ShipId.Akigumo or
		ShipId.Yuugumo or
		ShipId.Makigumo or
		ShipId.Naganami or
		ShipId.Taigei or
		ShipId.I19 or
		ShipId.Hayashimo or
		ShipId.Kiyoshimo or
		ShipId.Asagumo or
		ShipId.Yamagumo or
		ShipId.Nowaki or
		ShipId.Asashimo;
}
