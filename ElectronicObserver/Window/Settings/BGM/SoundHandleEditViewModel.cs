using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Services;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Settings.BGM;

public partial class SoundHandleEditViewModel : WindowViewModelBase
{
	public SoundHandleEditTranslationViewModel Translation { get; }
	private FileService FileService { get; }

	public SoundHandleViewModel SoundHandle { get; }

	public DoubleFormatter DoubleFormatter { get; } = new();

	public string Title => $"{Translation.Title} - {SyncBGMPlayer.SoundHandleIDToString(SoundHandle.Handle.HandleID)}";

	public bool? DialogResult { get; set; }

	public SoundHandleEditViewModel(SoundHandleViewModel soundHandle)
	{
		Translation = Ioc.Default.GetRequiredService<SoundHandleEditTranslationViewModel>();
		FileService = Ioc.Default.GetRequiredService<FileService>();

		SoundHandle = soundHandle;
	}

	[RelayCommand]
	private void OpenSoundPath()
	{
		string? newPath = FileService.OpenSoundPath(SoundHandle.Path);

		if (newPath is null) return;

		SoundHandle.Path = newPath;
	}

	[RelayCommand]
	private void SoundPathDirectorize()
	{
		if (string.IsNullOrWhiteSpace(SoundHandle.Path)) return;

		try
		{
			SoundHandle.Path = System.IO.Path.GetDirectoryName((string?)SoundHandle.Path);
		}
		catch
		{
			// *ぷちっ*
		}
	}

	[RelayCommand]
	private void Confirm()
	{
		SoundHandle.Save();

		DialogResult = true;
	}

	[RelayCommand]
	private void Cancel()
	{
		SoundHandle.Load();

		DialogResult = false;
	}
}
