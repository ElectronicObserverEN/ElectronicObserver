using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Utility;

namespace ElectronicObserver.ViewModels.Translations;

public class TranslationBaseViewModel : ObservableObject
{
	private string Culture { get; set; }

	protected TranslationBaseViewModel()
	{
		Culture = Configuration.Config.UI.Culture;

		Configuration.Instance.ConfigurationChanged += () =>
		{
			CultureInfo cultureInfo = new(Configuration.Config.UI.Culture);

			CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
			CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

			Culture = Configuration.Config.UI.Culture;
		};

		PropertyChanged += (_, args) =>
		{
			if (args.PropertyName is not nameof(Culture)) return;

			OnPropertyChanged("");
		};
	}
}
