using System.Globalization;
using System.Threading;
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

			Thread.CurrentThread.CurrentCulture = cultureInfo;
			Thread.CurrentThread.CurrentUICulture = cultureInfo;

			Culture = Configuration.Config.UI.Culture;
		};

		PropertyChanged += (_, args) =>
		{
			if (args.PropertyName is not nameof(Culture)) return;

			OnPropertyChanged("");
		};
	}
}
