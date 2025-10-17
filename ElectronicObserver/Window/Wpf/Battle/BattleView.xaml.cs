using System.Windows;
using System.Windows.Controls;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Battle;

namespace ElectronicObserver.Window.Wpf.Battle;

/// <summary>
/// Interaction logic for BattleView.xaml
/// </summary>
public partial class BattleView : UserControl
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register(
		"ViewModel", typeof(BattleViewModel), typeof(BattleView), new PropertyMetadata(default(BattleViewModel)));

	public BattleViewModel ViewModel
	{
		get => (BattleViewModel)GetValue(ViewModelProperty);
		set => SetValue(ViewModelProperty, value);
	}

	public BattleView()
	{
		InitializeComponent();
	}

	// not really sure how to do this mvvm style
	private void FrameworkElement_OnContextMenuOpening(object sender, ContextMenuEventArgs e)
	{
		e.Handled = KCDatabase.Instance.Battle.FirstBattle is null;
	}
}
