using System.Windows.Media;
using ElectronicObserver.Common;

namespace ElectronicObserver.ViewModels;

public partial class AnchorableViewModel : UserControlViewModelBase
{
	public string Title { get; set; }
	public virtual string ContentId { get; }
	public ImageSource? IconSource { get; set; }

	public bool CanFloat { get; set; }

	protected AnchorableViewModel(string title, string contentId, ImageSource? icon = null)
	{
		Title = title;
		ContentId = contentId;
		IconSource = icon;
	}

}
