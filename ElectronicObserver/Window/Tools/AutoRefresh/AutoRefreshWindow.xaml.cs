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
using System.Windows.Shapes;

namespace ElectronicObserver.Window.Tools.AutoRefresh;
/// <summary>
/// Interaction logic for AutoRefreshWindow.xaml
/// </summary>
public partial class AutoRefreshWindow
{
	public AutoRefreshWindow(AutoRefreshViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
	}
}
