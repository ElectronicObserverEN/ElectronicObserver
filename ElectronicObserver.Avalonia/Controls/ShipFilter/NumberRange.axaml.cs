using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;

namespace ElectronicObserver.Avalonia.Controls.ShipFilter;

public partial class NumberRange : UserControl
{
	public static readonly StyledProperty<string> TextProperty = 
		AvaloniaProperty.Register<NumberRange, string>(nameof(Text));

	public string Text
	{
		get => GetValue(TextProperty);
		set => SetValue(TextProperty, value);
	}

	public static readonly StyledProperty<int> MinimumProperty = 
		AvaloniaProperty.Register<NumberRange, int>(nameof(Minimum), defaultBindingMode: BindingMode.TwoWay);

	public int Minimum
	{
		get => GetValue(MinimumProperty);
		set => SetValue(MinimumProperty, value);
	}

	public static readonly StyledProperty<int> MaximumProperty = 
		AvaloniaProperty.Register<NumberRange, int>(nameof(Maximum), defaultBindingMode: BindingMode.TwoWay);
	
	public int Maximum
	{
		get => GetValue(MaximumProperty);
		set => SetValue(MaximumProperty, value);
	}
	
	public NumberRange()
	{
		InitializeComponent();
	}
}

