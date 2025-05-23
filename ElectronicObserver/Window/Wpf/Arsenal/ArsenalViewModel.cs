using System.Collections.Generic;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Wpf.Arsenal;

public class ArsenalViewModel : AnchorableViewModel
{
	public FormArsenalTranslationViewModel FormArsenal { get; }
	public List<ArsenalItemViewModel> Arsenals { get; }
	public bool ShowShipName { get; set; }
	private int BuildingId { get; set; }

	public ArsenalViewModel() : base("Arsenal", "Arsenal", IconContent.FormArsenal)
	{
		FormArsenal = Ioc.Default.GetService<FormArsenalTranslationViewModel>()!;

		Title = FormArsenal.Title;
		FormArsenal.PropertyChanged += (_, _) => Title = FormArsenal.Title;

		Arsenals = new()
		{
			new(),
			new(),
			new(),
			new(),
		};

		APIObserver o = APIObserver.Instance;

		o.ApiReqKousyou_CreateShip.RequestReceived += Updated;
		o.ApiReqKousyou_CreateShipSpeedChange.RequestReceived += Updated;

		o.ApiGetMember_KDock.ResponseReceived += Updated;
		o.ApiReqKousyou_GetShip.ResponseReceived += Updated;
		o.ApiGetMember_RequireInfo.ResponseReceived += Updated;

		Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		SystemEvents.UpdateTimerTick += UpdateTimerTick;

		ConfigurationChanged();

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ShowShipName)) return;

			Configuration.Config.FormArsenal.ShowShipName = ShowShipName;
			UpdateUI();
		};
	}

	private void Updated(string apiname, dynamic data)
	{
		if (BuildingId != -1 && apiname == "api_get_member/kdock")
		{
			ArsenalData arsenal = KCDatabase.Instance.Arsenals[BuildingId];
			IShipDataMaster ship = KCDatabase.Instance.MasterShips[arsenal.ShipID];

			string name = (Configuration.Config.Log.ShowSpoiler && Configuration.Config.FormArsenal.ShowShipName) switch
			{
				true => $"{ship.ShipTypeName} {ship.NameWithClass}",
				_ => GeneralRes.ShipGirl,
			};

			Logger.Add(2, string.Format(GeneralRes.ArsenalLog,
				BuildingId,
				name,
				arsenal.Fuel,
				arsenal.Ammo,
				arsenal.Steel,
				arsenal.Bauxite,
				arsenal.DevelopmentMaterial,
				KCDatabase.Instance.Fleet[1].MembersInstance![0].NameWithLevel
			));

			BuildingId = -1;
		}

		if (apiname == "api_req_kousyou/createship")
		{
			BuildingId = int.Parse(data["api_kdock_id"]);
		}

		UpdateUI();
	}

	private void UpdateUI()
	{
		for (int i = 0; i < Arsenals.Count; i++)
		{
			Arsenals[i].Update(i + 1);
		}
	}

	private void UpdateTimerTick()
	{
		for (int i = 0; i < Arsenals.Count; i++)
		{
			Arsenals[i].Refresh(i + 1);
		}
	}

	private void ConfigurationChanged()
	{
		ShowShipName = Configuration.Config.FormArsenal.ShowShipName;

		foreach (ArsenalItemViewModel c in Arsenals)
		{
			c.ConfigurationChanged();
		}
	}
}
