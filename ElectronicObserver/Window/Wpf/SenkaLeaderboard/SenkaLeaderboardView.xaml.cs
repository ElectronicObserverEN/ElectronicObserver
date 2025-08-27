using System.Windows;

namespace ElectronicObserver.Window.Wpf.SenkaLeaderboard;

public partial class SenkaLeaderboardView
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(SenkaLeaderboardViewModel), typeof(SenkaLeaderboardView), new PropertyMetadata(default(SenkaLeaderboardViewModel)));

	public SenkaLeaderboardViewModel ViewModel
	{
		get => (SenkaLeaderboardViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public SenkaLeaderboardView()
	{
		InitializeComponent();
	}
}
