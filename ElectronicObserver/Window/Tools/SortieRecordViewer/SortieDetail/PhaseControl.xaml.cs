using System.Windows;
using ElectronicObserver.Window.Tools.SortieRecordViewer.Sortie.Battle.Phase;

namespace ElectronicObserver.Window.Tools.SortieRecordViewer.SortieDetail;
/// <summary>
/// Interaction logic for PhaseControl.xaml
/// </summary>
public partial class PhaseControl
{
	public static readonly DependencyProperty PhaseDataProperty = DependencyProperty.Register(
		nameof(PhaseData), typeof(PhaseBase), typeof(PhaseControl), new PropertyMetadata(default(PhaseBase)));

	public PhaseBase PhaseData
	{
		get => (PhaseBase)GetValue(PhaseDataProperty);
		set => SetValue(PhaseDataProperty, value);
	}

	public PhaseControl()
	{
		InitializeComponent();
	}
}
