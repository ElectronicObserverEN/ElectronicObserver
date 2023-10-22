using System.Windows.Media;
using ElectronicObserver.Common;
using ElectronicObserver.Resource;

namespace ElectronicObserver.ViewModels;

public class AnchorableViewModel : UserControlViewModelBase
{
	public string Title { get; set; }
	public virtual string ContentId { get; }
	public IconContent? Icon { get; set; }

	/// <summary>
	/// This is needed for the window capture feature.
	/// For known icons, the <see cref="Icon"/> property should be used.
	/// </summary>
	public ImageSource? IconSource { get; set; }

	public bool CanFloat { get; set; }

	protected AnchorableViewModel(string title, string contentId, IconContent? icon = null)
	{
		Title = title;
		ContentId = contentId;
		Icon = icon;
	}
}
