using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Database;
using ElectronicObserver.Window.Dialog;
using ElectronicObserver.Window.Tools.SortieRecordViewer;

namespace ElectronicObserver.Window.Tools.DatabaseExplorer.ApiFile;

public partial class ApiFileExplorerViewModel : ObservableObject
{
	private ElectronicObserverContext Db { get; } = new();

	public ObservableCollection<Database.KancolleApi.ApiFile> ApiFiles { get; } = new();
	public ObservableCollection<Database.KancolleApi.ApiFile> SelectedFiles { get; } = new();

	public int Limit { get; set; } = 20;

	public string? ApiNameFilter { get; set; }

	[RelayCommand]
	private void Search()
	{
		ApiFiles.Clear();

		IQueryable<Database.KancolleApi.ApiFile> files = Db.ApiFiles.AsQueryable();

		files = files.OrderByDescending(f => f.Id);

		if (!string.IsNullOrEmpty(ApiNameFilter))
		{
			files = files.Where(f => f.Name.Contains(ApiNameFilter));
		}

		files = files.Take(Limit);


		foreach (Database.KancolleApi.ApiFile file in files)
		{
			ApiFiles.Add(file);
		}
	}

	[RelayCommand]
	private void CopyContent()
	{
		if (!SelectedFiles.Any()) return;

		Clipboard.SetText(SelectedFiles.Count switch
		{
			1 => SelectedFiles[0].Content,
			_ => JsonSerializer.Serialize(SelectedFiles
				.Select(f => f.CleanRequest())
				.OrderBy(f => f.Id)),
		});
	}

	[RelayCommand]
	private void OpenApiLoader()
	{
		if (!SelectedFiles.Any()) return;

		DialogLocalAPILoader2 dialog = new(SelectedFiles.ToList());
		dialog.Show();
	}
}
