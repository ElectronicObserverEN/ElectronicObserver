using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElectronicObserver.Window.Wpf.Fleet.Views;
/// <summary>
/// Interaction logic for ToolTipStat.xaml
/// </summary>
public partial class ToolTipStat : UserControl
{
	public static readonly DependencyProperty IconProperty = DependencyProperty.Register(
		nameof(Icon), typeof(ImageSource), typeof(ToolTipStat), new PropertyMetadata(default(ImageSource)));

	public ImageSource Icon
	{
		get => (ImageSource)GetValue(IconProperty);
		set => SetValue(IconProperty, value);
	}

	public static readonly DependencyProperty BaseProperty = DependencyProperty.Register(
		nameof(Base), typeof(string), typeof(ToolTipStat), new PropertyMetadata(default(string)));

	public string? Base
	{
		get => (string?)GetValue(BaseProperty);
		set => SetValue(BaseProperty, value);
	}

	public static readonly DependencyProperty TotalProperty = DependencyProperty.Register(
		nameof(Total), typeof(string), typeof(ToolTipStat), new PropertyMetadata(default(string)));

	public string? Total
	{
		get => (string?)GetValue(TotalProperty);
		set => SetValue(TotalProperty, value);
	}

	public ToolTipStat()
	{
		InitializeComponent();
	}
}
