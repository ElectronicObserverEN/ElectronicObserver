using Avalonia.Win32.Interoperability;
using ElectronicObserver.Avalonia.QuestTrackerManager2;

namespace ElectronicObserver.Window.Dialog.QuestTrackerManager;

public partial class QuestTrackerManager2Window
{
	public WpfAvaloniaHost WpfAvaloniaHost { get; }

	public QuestTrackerManager2Window(QuestTrackerManager2ViewModel viewModel) : base(viewModel)
	{
		WpfAvaloniaHost = new()
		{
			Content = new QuestTrackerManager2View
			{
				DataContext = viewModel,
			},
		};

		InitializeComponent();
	}
}
