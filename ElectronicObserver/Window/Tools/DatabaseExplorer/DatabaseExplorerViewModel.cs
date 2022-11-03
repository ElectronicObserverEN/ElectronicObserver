using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Common;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.Services.ApiFileService;

namespace ElectronicObserver.Window.Tools.DatabaseExplorer;

public partial class DatabaseExplorerViewModel : WindowViewModelBase
{
	private ApiFileService ApiFileService { get; } = new();

	public ObservableCollection<ApiFile> ApiFiles { get; } = new();

	public int Limit { get; set; } = 20;

	public string? ApiNameFilter { get; set; }

	[ICommand]
	private void Search()
	{
		ApiFiles.Clear();

		List<ApiFile> files = ApiFileService.Query(files =>
		{
			files = files.OrderByDescending(f => f.Id);

			if (!string.IsNullOrEmpty(ApiNameFilter))
			{
				files = files.Where(f => f.Name.Contains(ApiNameFilter));
			}

			files = files.Take(Limit);

			return files;
		});


		foreach (ApiFile file in files)
		{
			ApiFiles.Add(file);
		}
	}

	[ICommand]
	private void CopyContent(ApiFile? file)
	{
		if (file is null) return;

		Clipboard.SetText(file.Content);
	}
}
