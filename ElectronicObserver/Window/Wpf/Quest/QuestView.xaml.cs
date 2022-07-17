using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicObserver.Window.Wpf.Quest;
/// <summary>
/// Interaction logic for QuestView.xaml
/// </summary>
public partial class QuestView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register
	(
		nameof(ViewModel),
		typeof(QuestViewModel),
		typeof(QuestView),
		new PropertyMetadata(default(QuestViewModel), PropertyChangedCallback)
	);

	private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
	{
		if (d is not QuestView { ViewModel: { } viewModel } view) return;

		// hack: this is ModernWPF specific
		view.Resources["DataGridRowMinHeight"] = (double)viewModel.RowMinSize;

		viewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(viewModel.RowMinSize)) return;

			view.Resources["DataGridRowMinHeight"] = (double)viewModel.RowMinSize;
		};
	}

	public QuestViewModel ViewModel
	{
		get => (QuestViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public QuestView()
	{
		InitializeComponent();

		DataGrid.Sorting += (sender, args) =>
		{
			// dispatcher is needed to get the SortDescriptions value after sorting
			// without the dispatcher you get the old value before the sorting
			Dispatcher.BeginInvoke(() => ViewModel.SortDescriptions = DataGrid.Items.SortDescriptions.ToList());
		};
	}
}
