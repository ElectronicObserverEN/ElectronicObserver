using System.Windows;

namespace ElectronicObserver.Common.ContentDialogs.Notification;

/// <summary>
/// Interaction logic for NotificationContentDialog.xaml
/// </summary>
public partial class NotificationContentDialog
{
	public static readonly DependencyProperty NotificationTitleProperty = DependencyProperty.Register(
		nameof(NotificationTitle), typeof(string), typeof(NotificationContentDialog), new PropertyMetadata(default(string)));

	public string? NotificationTitle
	{
		get => (string?)GetValue(NotificationTitleProperty);
		set => SetValue(NotificationTitleProperty, value);
	}

	public static readonly DependencyProperty NotificationProperty = DependencyProperty.Register(
		nameof(Notification), typeof(string), typeof(NotificationContentDialog), new PropertyMetadata(default(string)));

	public string? Notification
	{
		get => (string?)GetValue(NotificationProperty);
		set => SetValue(NotificationProperty, value);
	}

	public NotificationContentDialog()
	{
		InitializeComponent();
	}
}
