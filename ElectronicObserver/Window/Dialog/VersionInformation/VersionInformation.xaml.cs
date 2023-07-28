using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Navigation;
using System.Windows.Threading;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Dialog.VersionInformation;
/// <summary>
/// Interaction logic for VersionInformation.xaml
/// </summary>
public partial class VersionInformationWindow
{
	public VersionInformationWindow()
	{
		InitializeComponent();

		Translate();
		string versionText = CultureInfo.CurrentCulture.Name switch
		{
			"ja-JP" => SoftwareInformation.VersionJapanese,
			_ => SoftwareInformation.SoftwareNameEnglish
		};

		TextVersion.Text = string.Format(VersionResources.TextVersionFormat, versionText, SoftwareInformation.VersionEnglish, SoftwareInformation.UpdateTime.ToString("yyyy/MM/dd"));
		RuntimeVersion.Text = RuntimeInformation.FrameworkDescription;

		// https://github.com/Kinnara/ModernWpf/issues/378
		SourceInitialized += (s, a) =>
		{
			Dispatcher.Invoke(InvalidateVisual, DispatcherPriority.Input);
		};
	}

	private void Translate()
	{
		TextVersion.Text = VersionResources.TextVersion;
		TextAuthor.Text = VersionResources.Developer;
		TextAuthorLink.Inlines.Add(VersionResources.TextAuthor);
		ButtonClose.Content = VersionResources.ButtonClose;
		TextProjectSite.Text = VersionResources.ProjectSite;
		TextModifiedBy.Text = VersionResources.ModifiedBy;
		TextMaintainers.Text = VersionResources.Maintainers;
		Title = VersionResources.Title;
	}

	private void OpenHyperlink(object sender, RequestNavigateEventArgs e)
	{
		ProcessStartInfo psi = new()
		{
			FileName = e.Uri.AbsoluteUri,
			UseShellExecute = true
		};
		Process.Start(psi);
	}

	private void ButtonClose_OnClick(object sender, RoutedEventArgs e)
	{
		Close();
	}
}
