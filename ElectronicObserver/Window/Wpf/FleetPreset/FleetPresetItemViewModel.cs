using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Wpf.Fleet.ViewModels;

namespace ElectronicObserver.Window.Wpf.FleetPreset;

public class FleetPresetItemViewModel : ObservableObject
{
	public FormFleetPresetTranslationViewModel FormFleetPreset { get; }

	public FleetPresetItemControlViewModel Name { get; }
	public FleetConditionViewModel Condition { get; } = new();
	public List<FleetPresetItemControlViewModel> Ships { get; }

	public int MaxWidth { get; set; }

	public FleetPresetItemViewModel()
	{
		FormFleetPreset = Ioc.Default.GetRequiredService<FormFleetPresetTranslationViewModel>();

		static FleetPresetItemControlViewModel CreateDefaultLabel() => new()
		{
			Text = "",
		};

		Name = CreateDefaultLabel();
		Condition.ImageAlign = ContentAlignment.MiddleRight;

		// TODO: 本体側がもし 7 隻編成に対応したら変更してください
		Ships = new();
		for (int i = 0; i < 7; i++)
		{
			Ships.Add(CreateDefaultLabel());
		}

		ConfigurationChanged();
	}

	public void Update(int presetId)
	{
		FleetPresetData? preset = KCDatabase.Instance.FleetPreset[presetId];

		if (preset is null)
		{
			Name.Text = "----";
			Name.ToolTip = null;

			foreach (FleetPresetItemControlViewModel ship in Ships)
			{
				ship.Text = string.Empty;
				ship.ToolTip = null;
			}

			return;
		}

		Name.Text = preset.Name;

		int lowestCondition = preset.MembersInstance
			.Where(s => s != null)
			.Select(s => s.Condition)
			.DefaultIfEmpty(49)
			.Min();

		Condition.SetDesign(lowestCondition);

		Name.ToolTip = $"{FormFleetPreset.LowestCondition}: {lowestCondition}";

		for (int i = 0; i < Ships.Count; i++)
		{
			ShipData? ship = (i >= preset.Members.Count) switch
			{
				true => null,
				_ => preset.MembersInstance.ElementAt(i),
			};

			Ships[i].Text = ship?.Name ?? "-";

			if (ship == null)
			{
				Ships[i].ToolTip = null;
			}
			else
			{
				StringBuilder sb = new();
				sb.AppendLine($"{ship.MasterShip.ShipTypeName} {ship.NameWithLevel}");
				sb.AppendLine($"HP: {ship.HPCurrent} / {ship.HPMax} ({ship.HPRate:p1}) [{Constants.GetDamageState(ship.HPRate)}]");
				sb.AppendLine($"cond: {ship.Condition}");
				sb.AppendLine();

				IList<IEquipmentData> slot = ship.AllSlotInstance;
				for (int e = 0; e < slot.Count; e++)
				{
					if (slot[e] == null) continue;

					if (e < ship.MasterShip.Aircraft.Count)
					{
						sb.AppendLine($"[{ship.Aircraft[e]}/{ship.MasterShip.Aircraft[e]}] {slot[e].NameWithLevel}");
					}
					else
					{
						sb.AppendLine(slot[e].NameWithLevel);
					}
				}

				Ships[i].ToolTip = sb.ToString();
			}
		}
	}

	public void ConfigurationChanged()
	{
		Configuration.ConfigurationData config = Configuration.Config;

		MaxWidth = config.FormFleet.FixShipNameWidth switch
		{
			true => config.FormFleet.FixedShipNameWidth,
			_ => int.MaxValue,
		};
	}
}
