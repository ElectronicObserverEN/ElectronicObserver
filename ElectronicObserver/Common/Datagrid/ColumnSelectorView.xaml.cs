using System.Windows.Threading;

namespace ElectronicObserver.Common.Datagrid
{
	/// <summary>
	/// Interaction logic for ColumnSelectorView.xaml
	/// </summary>
	public partial class ColumnSelectorView : WindowBase<ColumnSelectorViewModel>
	{
		public ColumnSelectorView(ColumnSelectorViewModel vm) : base(vm)
		{
			// https://github.com/Kinnara/ModernWpf/issues/378
			SourceInitialized += (s, a) =>
			{
				Dispatcher.Invoke(InvalidateVisual, DispatcherPriority.Input);
			};

			InitializeComponent();
		}

		private void Button_Click_Cancel(object sender, System.Windows.RoutedEventArgs e)
		{
			DialogResult = false;
		}

		private void Button_Click_Ok(object sender, System.Windows.RoutedEventArgs e)
		{
			DialogResult = true;
		}
	}
}
