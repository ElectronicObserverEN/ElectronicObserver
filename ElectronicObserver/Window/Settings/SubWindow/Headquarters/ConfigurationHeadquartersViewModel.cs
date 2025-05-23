using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.DependencyInjection;
using ElectronicObserver.Common;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Data;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.SubWindow.Headquarters;

public class ConfigurationHeadquartersViewModel : ConfigurationViewModelBase
{
	public ConfigurationHeadquartersTranslationViewModel Translation { get; }
	private Configuration.ConfigurationData.ConfigFormHeadquarters Config { get; }

	public List<IUseItemMaster> Items { get; set; } = new();
	public List<CheckBoxEnumViewModel> ItemVisibilities { get; }

	private List<UseItemId> IgnoredItems { get; } = new()
	{
		UseItemId.InstantRepair,
		UseItemId.InstantConstruction,
		UseItemId.DevelopmentMaterial,
		UseItemId.ImproveMaterial,
		UseItemId.RepairTeam,
		UseItemId.RepairGoddess,
		UseItemId.CombatRation,
		UseItemId.UnderwayReplenishment,
		UseItemId.CannedSaury,
	};

	public bool BlinkAtMaximum { get; set; }

	public IUseItemMaster? SelectedItem { get; set; }

	public int DisplayUseItemID { get; set; }

	public int WrappingOffset { get; set; }

	public ConfigurationHeadquartersViewModel(Configuration.ConfigurationData.ConfigFormHeadquarters config)
	{
		Translation = Ioc.Default.GetRequiredService<ConfigurationHeadquartersTranslationViewModel>();

		ItemVisibilities = Enum.GetValues<HeadquartersItem>()
			.Select(i => new CheckBoxEnumViewModel(i))
			.ToList();

		Config = config;
		Load();
	}

	private void Load()
	{
		BlinkAtMaximum = Config.BlinkAtMaximum;
		DisplayUseItemID = Config.DisplayUseItemID;
		WrappingOffset = Config.WrappingOffset;

		foreach ((CheckBoxEnumViewModel item, bool visibility) in ItemVisibilities.Zip(Config.Visibility.List))
		{
			item.IsChecked = visibility;
		}

		Items = KCDatabase.Instance.MasterUseItems.Values
			.Where(i => i.NameTranslated.Length > 0 && i.Description.Length > 0 && !IgnoredItems.Contains(i.ItemID))
			.ToList();

		SelectedItem = KCDatabase.Instance.MasterUseItems[DisplayUseItemID];
	}

	public override void Save()
	{
		Config.BlinkAtMaximum = BlinkAtMaximum;
		Config.Visibility = ItemVisibilities.Select(i => i.IsChecked).ToList();
		Config.DisplayUseItemID = SelectedItem?.ID ?? DisplayUseItemID;
		Config.WrappingOffset = WrappingOffset;
	}
}
