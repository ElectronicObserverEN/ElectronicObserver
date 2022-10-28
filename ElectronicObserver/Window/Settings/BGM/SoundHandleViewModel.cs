using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.BGM;

public class SoundHandleViewModel : ObservableObject
{
	public SyncBGMPlayer.SoundHandle Handle { get; }

	public bool Enabled { get; set; }

	public string Name => SyncBGMPlayer.SoundHandleIDToString(Handle.HandleID);

	public string Path { get; set; }

	public bool IsLoop { get; set; }

	public double LoopHeadPosition { get; set; }

	public int Volume { get; set; }

	public SoundHandleViewModel(SyncBGMPlayer.SoundHandle handle)
	{
		Handle = handle;
		Load();
	}

	public void Load()
	{
		Enabled = Handle.Enabled;
		Path = Handle.Path;
		IsLoop = Handle.IsLoop;
		LoopHeadPosition = Handle.LoopHeadPosition;
		Volume = Handle.Volume;
	}

	public void Save()
	{
		Handle.Enabled = Enabled;
		Handle.Path = Path;
		Handle.IsLoop = IsLoop;
		Handle.LoopHeadPosition = LoopHeadPosition;
		Handle.Volume = Volume;
	}
}
