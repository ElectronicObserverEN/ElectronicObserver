using System.Windows;

namespace ElectronicObserver.Window.Tools.Common;

/// <summary>
/// Interaction logic for ImageTextControl.xaml
/// </summary>
public partial class ImageTextControl
{
	public static readonly DependencyProperty ImageProperty = DependencyProperty.Register(
		nameof(Image), typeof(object), typeof(ImageTextControl), new PropertyMetadata(default(object)));

	public object Image
	{
		get => GetValue(ImageProperty);
		set => SetValue(ImageProperty, value);
	}

	public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
		nameof(Text), typeof(string), typeof(ImageTextControl), new PropertyMetadata(default(string)));

	public string Text
	{
		get => (string)GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public ImageTextControl()
	{
		InitializeComponent();
	}
}
