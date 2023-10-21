using System.ComponentModel;
using System.Windows;

namespace ElectronicObserver.Common.ContentDialogs.ExportProgress;

/// <summary>
/// Interaction logic for ExportProgressContentDialog.xaml
/// </summary>
public partial class ExportProgressContentDialog
{
	public static readonly DependencyProperty ExportProgressProperty = DependencyProperty.Register(
		nameof(ExportProgress), typeof(ExportProgressViewModel), typeof(ExportProgressContentDialog), new PropertyMetadata(default(ExportProgressViewModel)));

	public ExportProgressViewModel? ExportProgress
	{
		get => (ExportProgressViewModel?)GetValue(ExportProgressProperty);
		set
		{
			if (value != ExportProgress)
			{
				if (ExportProgress is not null)
				{
					ExportProgress.PropertyChanged -= ProgressChanged;
				}

				if (value is not null)
				{
					value.PropertyChanged += ProgressChanged;
				}
			}

			SetValue(ExportProgressProperty, value);
		}
	}

	public ExportProgressContentDialog()
	{
		InitializeComponent();
	}

	private async void ProgressChanged(object? sender, PropertyChangedEventArgs e)
	{
		await App.Current!.Dispatcher.BeginInvoke(() =>
		{
			if (e.PropertyName is not nameof(ExportProgress.Progress)) return;
			if (ExportProgress is null) return;
			if (ExportProgress.Progress < ExportProgress.Total) return;

			Hide();
		});
	}
}
