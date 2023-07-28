using System.Linq;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserverTypes.Mocks;
using KancolleProgress.ViewModels;

namespace KancolleProgress.Models;

public class ColorFilter : ObservableObject
{
	private KancolleProgressViewModel ViewModel { get; }
	private Comparator Comparator { get; }
	private int Level { get; }
	public SolidColorBrush Brush { get; }
	public string Label { get; }

	public int Count => ViewModel.BaseShips?.Count(s => Compare(this, s)) ?? 0;

	public static bool Compare(ColorFilter filter, ShipDataMock ship) => Compare(filter, ship.Level);

	public static bool Compare(ColorFilter filter, int level) => filter.Comparator switch
	{
		Comparator.Equal => level == filter.Level,
		Comparator.GreaterOrEqual => level >= filter.Level,
		_ => true,
	};

	public ColorFilter(KancolleProgressViewModel vm, Comparator comparator, int level, Color color, string? label = null)
	{
		ViewModel = vm;
		Comparator = comparator;
		Level = level;
		Brush = new SolidColorBrush(color);
		Label = label ?? level + comparator switch
		{
			Comparator.GreaterOrEqual => "+",
			_ => "",
		};

		// todo: might not be needed with Fody
		ViewModel.PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName != nameof(KancolleProgressViewModel.BaseShips)) return;

			OnPropertyChanged(nameof(Count));
		};
	}
}
