using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Common;

public abstract class CanBeUpdatedByApiViewModel : ObservableObject
{
	protected CanBeUpdatedByApiViewModel(bool shouldUpdate)
	{
		Initialize(shouldUpdate);
	}

	protected void OnApiResponseReceived(string apiname, dynamic data)
	{
		Update();
	}

	private void Initialize(bool shouldUpdate)
	{
		if (shouldUpdate) SubscribeToApis();
		Update();
	}
	
	public virtual void SubscribeToApis()
	{

	}

	public virtual void UnsubscribeFromApis()
	{

	}

	protected virtual void Update()
	{

	}
}
