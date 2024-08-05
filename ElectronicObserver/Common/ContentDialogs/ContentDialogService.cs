using System;
using System.Threading.Tasks;
using ElectronicObserver.Common.ContentDialogs.ExportFilter;
using ElectronicObserver.Common.ContentDialogs.ExportProgress;
using ElectronicObserver.Common.ContentDialogs.Notification;
using ModernWpf.Controls;

namespace ElectronicObserver.Common.ContentDialogs;

/// <summary>
/// This service is used to avoid coupling ViewModels with wpf.
/// </summary>
public class ContentDialogService
{
	public ExportFilterContentDialog? ExportFilterContentDialog { get; init; }
	public ExportProgressContentDialog? ExportProgressContentDialog { get; init; }
	public NotificationContentDialog? NotificationContentDialog { get; init; }

	public async Task<ExportFilterViewModel?> ShowExportFilterAsync(ExportFilterViewModel exportFilter)
	{
		ArgumentNullException.ThrowIfNull(ExportFilterContentDialog);

		ExportFilterContentDialog.ExportFilter = exportFilter;

		_ = await ExportFilterContentDialog.ShowAsync(ContentDialogPlacement.InPlace);

		return exportFilter;
	}

	public async Task<bool> ShowExportProgressAsync(ExportProgressViewModel exportProgress)
	{
		ArgumentNullException.ThrowIfNull(ExportProgressContentDialog);

		ExportProgressContentDialog.ExportProgress = exportProgress;

		bool result = await ExportProgressContentDialog.ShowAsync(ContentDialogPlacement.InPlace) switch
		{
			ContentDialogResult.None => true,
			_ => false,
		};

		ExportProgressContentDialog.ExportProgress = null;

		return result;
	}

	public async Task<bool> ShowNotificationAsync(string? text, string? title = null)
	{
		ArgumentNullException.ThrowIfNull(NotificationContentDialog);

		NotificationContentDialog.Notification = text;
		NotificationContentDialog.NotificationTitle = title;

		await NotificationContentDialog.ShowAsync(ContentDialogPlacement.InPlace);

		return true;
	}
}
