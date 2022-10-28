using System.ComponentModel;

namespace ElectronicObserver.Window.Settings.BGM;

/// <summary>
/// Interaction logic for SoundHandleEditDialog.xaml
/// </summary>
public partial class SoundHandleEditDialog
{
	public SoundHandleEditDialog(SoundHandleViewModel soundHandle) 
		: base(new SoundHandleEditViewModel(soundHandle))
	{
		InitializeComponent();

		ViewModel.PropertyChanged += ViewModelOnPropertyChanged;

		Closing += DialogClosing;
	}

	private void DialogClosing(object? sender, CancelEventArgs e)
	{
		ViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
	}

	private void ViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(ViewModel.DialogResult)) return;

		Close();
	}
}
