using System;
using System.Linq;
using DynaJson;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Data.TsunDbSubmission.Battle;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Data.TsunDbSubmission;

public class TsunDbSubmissionManager : ResponseWrapper
{
	private TsunDbRouting RoutingSubmission { get; set; } = new();

	/// <summary>
	/// When start API is called, the amount of nodes of the map is stored here (didn't find elsewhere to store it w)
	/// </summary>
	public static int CurrentMapAmountOfNodes { get; private set; }

	public TsunDbSubmissionManager()
	{
		Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
	}

	private void ConfigurationChanged()
	{
		// if tsundb disabled => disable initialization of RoutingSubmission 
		// Cause if you reenable it it will send wrong start data
		RoutingSubmission.IsInitialized = false;
	}

	/// <summary>
	/// Response wrapper for getting API data
	/// </summary>
	/// <param name="apiname">apiname from battle</param>
	/// <param name="data">api_data or RawData</param>
	public override void LoadFromResponse(string apiname, dynamic data)
	{
		if (!Configuration.Config.DataSubmission.SubmitDataToTsunDb) return;

		KCDatabase db = KCDatabase.Instance;

		try
		{
			JsonObject jData = (JsonObject)data;

			switch (apiname)
			{
				case "api_req_sortie/battleresult":
				case "api_req_combined_battle/battleresult":
					if (db.Ships.Count < db.Admiral.MaxShipCount && (db.Equipments.Values.Count(e => e.MasterEquipment.UsesSlotSpace()) < db.Admiral.MaxEquipmentCount))
					{
						new ShipDrop(data).SendData();
						new ShipDropLoc(data).SendData();
					}
					break;
				case "api_req_map/start":
				{
					object[] cellData = data["api_cell_data"];
					CurrentMapAmountOfNodes = cellData.Length;

					RoutingSubmission = jData.IsDefined("api_eventmap") ? new TsunDbEventRouting() : new TsunDbRouting();

					RoutingSubmission.ProcessStart(data);

					if (RoutingSubmission is TsunDbEventRouting routing)
					{
						routing.ProcessEvent(data);
					}

					RoutingSubmission.SendData();
					break;
				}
				case "api_req_map/next":
				{
					RoutingSubmission.ProcessNext(data);

					if (RoutingSubmission is TsunDbEventRouting routing)
					{
						routing.ProcessEvent(data);
					}

					RoutingSubmission.SendData();
					break;
				}
				case "api_req_sortie/battle":
				case "api_req_sortie/airbattle":
				case "api_req_sortie/night_to_day":
				case "api_req_sortie/ld_airbattle":
				case "api_req_sortie/ld_shooting":
				case "api_req_battle_midnight/sp_midnight":
				case "api_req_combined_battle/airbattle":
				case "api_req_combined_battle/battle":
				case "api_req_combined_battle/sp_midnight":
				case "api_req_combined_battle/ld_airbattle":
				case "api_req_combined_battle/ld_shooting":
				case "api_req_combined_battle/ec_battle":
				case "api_req_combined_battle/each_battle":
				case "api_req_combined_battle/each_airbattle":
				case "api_req_combined_battle/each_sp_midnight":
				case "api_req_combined_battle/each_battle_water":
				case "api_req_combined_battle/ec_night_to_day":
				case "api_req_combined_battle/each_ld_airbattle":
				case "api_req_combined_battle/each_ld_shooting":
				case "api_req_battle_midnight/battle":
				case "api_req_combined_battle/midnight_battle":
				case "api_req_combined_battle/ec_midnight_battle":
				{
					if (db.Battle.Compass.MapAreaID > 20)
					{
						new TsunDbBattleData(apiname, data).SendData();
					}
					break;
				}
			}
		}
		catch (Exception ex)
		{
			ErrorReporter.SendErrorReport(ex, "TsunDb Submission module");
		}
	}
}
