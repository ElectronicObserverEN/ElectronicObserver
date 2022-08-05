using System.Windows;

namespace ElectronicObserver.Window.Wpf.ExpeditionCheck;
/// <summary>
/// Interaction logic for ExpeditionCheck.xaml
/// </summary>
public partial class ExpeditionCheckView
{
	public static readonly DependencyProperty ViewModelProperty = DependencyProperty.Register
	(
		nameof(ViewModel),
		typeof(ExpeditionCheckViewModel),
		typeof(ExpeditionCheckView),
		new PropertyMetadata(default(ExpeditionCheckViewModel))
	);

	public ExpeditionCheckView() : base (new ExpeditionCheckViewModel())
	{
		InitializeComponent();
	}
}
