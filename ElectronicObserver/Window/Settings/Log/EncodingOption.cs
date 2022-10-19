namespace ElectronicObserver.Window.Settings.Log;

public record EncodingOption(int Value)
{
	public string Display => Value switch
	{
		0 => "UTF-8",
		1 => "UTF-8(BOM)",
		2 => "UTF-16",
		3 => "UTF-16(BOM)",
		4 => "Shift-JIS",
	};
}