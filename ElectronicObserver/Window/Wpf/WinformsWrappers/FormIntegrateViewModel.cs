using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;
using ElectronicObserver.Resource;
using ElectronicObserver.ViewModels;
using ElectronicObserver.Window.Integrate;
using ElectronicObserver.Window.Wpf.WinformsHost;

namespace ElectronicObserver.Window.Wpf.WinformsWrappers;

public partial class FormIntegrateViewModel : WinformsHostViewModel
{
	private FormMainViewModel Parent { get; }
	private FormIntegrate Integrate { get; }
	public override string ContentId => Integrate.PersistString;

	public FormIntegrateViewModel(FormIntegrate integrate, FormMainViewModel parent)
		: base("Integrate", "Integrate", ImageSourceIcons.GetIcon(IconContent.FormJson))
	{
		Parent = parent;
		Integrate = integrate;

		Title = integrate.WindowData.CurrentTitle;
		integrate.TopLevel = false;
		WinformsControl = integrate;

		WindowsFormsHost.Child = WinformsControl;

		integrate.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(FormIntegrate.Icon)) return;

			IconSource = Imaging.CreateBitmapSourceFromHIcon(
				Integrate.Icon.Handle,
				Int32Rect.Empty,
				BitmapSizeOptions.FromWidthAndHeight(16, 16));
		};

		integrate.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(FormIntegrate.Text)) return;

			Title = Integrate.Text;
		};
	}

	public void RaiseContentIdChanged() => OnPropertyChanged(nameof(ContentId));

	protected override void Close()
	{
		Parent.CloseIntegrate(this);
	}
}
