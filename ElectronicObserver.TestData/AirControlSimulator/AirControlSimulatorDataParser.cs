using ElectronicObserver.Core.Types;
using System.Text.Json;

namespace ElectronicObserver.TestData.AirControlSimulator;

public static class AirControlSimulatorDataParser
{
	public static async Task<Dictionary<ShipId, List<int>>> AbyssalShipAircraft()
	{
		const string link = "https://firebasestorage.googleapis.com/v0/b/development-74af0.appspot.com/o/master.json?alt=media";

		HttpClient client = new();
		string json = await client.GetStringAsync(link);

		AirControlSimulatorData data = JsonSerializer.Deserialize<AirControlSimulatorData>(json) ?? throw new Exception();

		return data.Enemies.ToDictionary(e => e.Id, e => e.Slots.Select(s => s switch
		{
			-1 => 0,
			_ => s,
		}).ToList());
	}
}
