namespace ElectronicObserver.Window.Dialog.UiBlocker.Taiha;

public sealed class TaihaBlockerViewModel(UiBlockerConfiguration configuration)
	: UiBlockerViewModel(configuration)
{
	public override string Header => UiBlockerResources.Taiha;

	public static double DefaultDesiredTop => 270;
	public static double DefaultDesiredLeft => 330;
	public static double DefaultDesiredHeight => 190;
	public static double DefaultDesiredWidth => 210;

	protected override void SetDesiredValuesToDefault()
	{
		DesiredTop = DefaultDesiredTop;
		DesiredLeft = DefaultDesiredLeft;
		DesiredHeight = DefaultDesiredHeight;
		DesiredWidth = DefaultDesiredWidth;
	}
}
