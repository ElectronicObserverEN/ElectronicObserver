using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Common;

namespace ElectronicObserver.Window.Wpf.RankingLeaderboard;

/// <summary>
/// Interaction logic for HeadquartersView.xaml
/// </summary>
public partial class RankingLeaderboardView
{
	public RankingLeaderboardView(RankingLeaderboardViewModel viewModel) : base(viewModel)
	{
		InitializeComponent();
	}
}
