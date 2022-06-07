using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Tools.AutoRefresh;

public class CellViewModel : ObservableObject
{
	public int Id { get; }

	public CellViewModel(int id)
	{
		Id = id;
	}
}