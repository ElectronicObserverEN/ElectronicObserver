using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility;
using Microsoft.Win32;

namespace ElectronicObserver.Services;

public class FileService
{
	private static System.Windows.Window MainWindow => App.Current!.MainWindow!;

	private static string LayoutFilter => "Layout File|*.xml";

	/// <summary>
	/// Opens a file selection dialog to select a layout file.
	/// </summary>
	/// <param name="path">Current layout path.</param>
	/// <returns>Selected file path or null if no path was selected.</returns>
	public static string? OpenLayoutPath(string path)
	{
		OpenFileDialog dialog = new()
		{
			Filter = LayoutFilter,
			Title = MainResources.OpenLayoutCaption,
		};

		PathHelper.InitOpenFileDialog(path, dialog);

		return dialog.ShowDialog(MainWindow) switch
		{
			true => PathHelper.GetPathFromOpenFileDialog(dialog),
			_ => null,
		};
	}

	/// <summary>
	/// Opens a file save dialog to save a layout file.
	/// </summary>
	/// <param name="path">Current layout path.</param>
	/// <returns>Selected file path or null if no path was selected.</returns>
	public static string? SaveLayoutPath(string path)
	{
		SaveFileDialog dialog = new()
		{
			Filter = LayoutFilter,
			Title = MainResources.OpenLayoutCaption,
		};

		PathHelper.InitSaveFileDialog(path, dialog);

		return dialog.ShowDialog(MainWindow) switch
		{
			true => PathHelper.GetPathFromSaveFileDialog(dialog),
			_ => null,
		};
	}

	/// <summary>
	/// Opens a folder browser dialog to select a folder.
	/// </summary>
	/// <param name="path">Current folder path.</param>
	/// <returns>Selected folder path or null if no path was selected.</returns>
	public static string? SelectFolder(string? path = null)
	{
		if (string.IsNullOrEmpty(path))
		{
			path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		}

		if (string.IsNullOrEmpty(path)) return null;

		string fullPath = Path.GetFullPath(path);

		OpenFolderDialog dialog = new()
		{
			InitialDirectory = Directory.Exists(fullPath) ? Path.GetFullPath(path) : "",
		};

		return dialog.ShowDialog(MainWindow) switch
		{
			true => dialog.FolderName,
			_ => null,
		};
	}

	public static void ExportConnectionScript(int port)
	{
		string? serverAddress = APIObserver.Instance.ServerAddress;

		if (serverAddress is null)
		{
			MessageBox.Show(ConfigurationResources.PleaseStartKancolle, ConfigurationResources.DialogCaptionErrorTitle,
				MessageBoxButton.OK, MessageBoxImage.Exclamation);
			return;
		}

		SaveFileDialog dialog = new()
		{
			Filter = "Proxy Script|*.pac|File|*",
			Title = ConfigurationResources.SavePacFileAs,
			InitialDirectory = Directory.GetCurrentDirectory(),
			FileName = Directory.GetCurrentDirectory() + "\\proxy.pac",
		};

		if (dialog.ShowDialog(MainWindow) != true) return;

		try
		{
			using (StreamWriter sw = new(dialog.FileName))
			{
				sw.WriteLine("function FindProxyForURL(url, host) {");
				sw.WriteLine("  if (/^" + serverAddress.Replace(".", @"\.") + "/.test(host)) {");
				sw.WriteLine("    return \"PROXY localhost:{0}; DIRECT\";", port);
				sw.WriteLine("  }");
				sw.WriteLine("  return \"DIRECT\";");
				sw.WriteLine("}");
			}

			Clipboard.SetData(DataFormats.StringFormat, "file:///" + dialog.FileName.Replace('\\', '/'));

			MessageBox.Show(ConfigurationResources.ProxyAutoConfigSaved,
				ConfigurationResources.PacSavedTitle,
				MessageBoxButton.OK, MessageBoxImage.Information);
		}
		catch (Exception ex)
		{
			ErrorReporter.SendErrorReport(ex, ConfigurationResources.FailedToSavePac);
			MessageBox.Show(ConfigurationResources.FailedToSavePac + "\r\n" + ex.Message,
				ConfigurationResources.DialogCaptionErrorTitle,
				MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	public static string? OpenApiListPath(string path)
	{
		OpenFileDialog dialog = new()
		{
			Filter = "Text File|*.txt|File|*",
			Title = "API リストを開く",
		};

		PathHelper.InitOpenFileDialog(path, dialog);

		return dialog.ShowDialog(MainWindow) switch
		{
			true => PathHelper.GetPathFromOpenFileDialog(dialog),
			_ => null,
		};
	}

	public static string? OpenSoundPath(string? path)
	{
		OpenFileDialog dialog = new()
		{
			Filter = "音楽ファイル|" + string.Join(";", EOMediaPlayer.SupportedExtensions.Select(s => "*." + s)) + "|File|*",
			Title = NotifyRes.OpenSound,
		};

		if (!string.IsNullOrEmpty(path))
		{
			try
			{
				dialog.InitialDirectory = Path.GetDirectoryName(path);

			}
			catch (Exception) 
			{ 
				// do not throw to avoid issues
			}
		}

		return dialog.ShowDialog(MainWindow) switch
		{
			true => dialog.FileName,
			_ => null,
		};
	}

	public static string? OpenImagePath(string? path)
	{
		OpenFileDialog dialog = new()
		{
			Filter = "Image|*.bmp;*.div;*.jpg;*.jpeg;*.jpe;*.jfif;*.gif;*.png;*.tif;*.tiff|BMP|*.bmp;*.div|JPEG|*.jpg;*.jpeg;*.jpe;*.jfif|GIF|*.gif|PNG|*.png|TIFF|*.tif;*.tiff|File|*",
			Title = NotifyRes.OpenImage,
		};

		if (!string.IsNullOrEmpty(path))
		{
			try
			{
				dialog.InitialDirectory = Path.GetDirectoryName(path);

			}
			catch (Exception) 
			{ 
				// do not throw to avoid issues
			}
		}

		return dialog.ShowDialog(MainWindow) switch
		{
			true => dialog.FileName,
			_ => null,
		};
	}

	public string? ExportCsv(string? path)
	{
		if (string.IsNullOrEmpty(path))
		{
			path = Directory.GetCurrentDirectory();
		}

		SaveFileDialog dialog = new()
		{
			Filter = "CSV|*.csv|File|*",
			Title = EquipmentListResources.SaveCSVDialog,
			InitialDirectory = path,
		};

		return dialog.ShowDialog(MainWindow) switch
		{
			true => dialog.FileName,
			_ => null,
		};
	}
}
