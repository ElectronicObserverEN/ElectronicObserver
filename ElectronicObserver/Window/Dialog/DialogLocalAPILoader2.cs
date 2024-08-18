using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Forms;
using ElectronicObserver.Database;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.Database.Sortie;
using ElectronicObserver.KancolleApi.Types;
using ElectronicObserver.KancolleApi.Types.ApiPort.Port;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Observer;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ElectronicObserver.Window.Dialog;

public partial class DialogLocalAPILoader2 : Form
{
	private JsonSerializerOptions JsonSerializerOptions { get; } = new()
	{
		DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
	};

	private SortieRecord? SortieRecord { get; }
	private List<ApiFile> ApiFilesBeforeSortie { get; set; } = [];

	private string CurrentPath { get; set; }


	public DialogLocalAPILoader2()
	{
		InitializeComponent();

		Translate();
	}

	public DialogLocalAPILoader2(SortieRecord sortieRecord) : this()
	{
		SortieRecord = sortieRecord;
	}

	public void Translate()
	{
		APIView_FileName.HeaderText = LocalAPILoader2Resources.APIView_FileName;
		ViewMenu_Execute.Text = LocalAPILoader2Resources.ViewMenu_Execute;
		ViewMenu_Delete.Text = LocalAPILoader2Resources.ViewMenu_Delete;

		Menu_File.Text = LocalAPILoader2Resources.Menu_File;
		Menu_File_OpenFolder.Text = LocalAPILoader2Resources.Menu_File_OpenFolder;
		Menu_File_Reload.Text = LocalAPILoader2Resources.Menu_File_Reload;

		ButtonSearchPrev.Text = LocalAPILoader2Resources.ButtonSearchPrev;
		ButtonSearch.Text = LocalAPILoader2Resources.ButtonSearch;
		ButtonExecuteNext.Text = LocalAPILoader2Resources.ButtonExecuteNext;
		ButtonExecute.Text = LocalAPILoader2Resources.ButtonExecute;

		Text = LocalAPILoader2Resources.Title;
	}

	private void DialogLocalAPILoader2_Load(object sender, EventArgs e)
	{
		if (SortieRecord is null)
		{
			LoadFiles(Utility.Configuration.Config.Connection.SaveDataPath);
		}
		else
		{
			LoadSortieRecordFiles(SortieRecord);
		}
	}


	private void Menu_File_OpenFolder_Click(object sender, EventArgs e)
	{

		FolderBrowser.SelectedPath = Utility.Configuration.Config.Connection.SaveDataPath;

		if (FolderBrowser.ShowDialog(App.Current.MainWindow) == System.Windows.Forms.DialogResult.OK)
		{
			LoadFiles(FolderBrowser.SelectedPath);
		}

	}

	private void Menu_File_Reload_Click(object sender, EventArgs e)
	{
		if (Directory.Exists(CurrentPath))
			LoadFiles(CurrentPath);
		else
			MessageBox.Show(LocalAPILoader2Resources.FolderNotSpecifiedOrDoesNotExist, LocalAPILoader2Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
	}

	private void ViewMenu_Execute_Click(object sender, EventArgs e)
	{

		/*/
		var rows = APIView.SelectedRows.Cast<DataGridViewRow>().OrderBy( r => r.Cells[APIView_FileName.Index].Value );

		foreach ( DataGridViewRow row in rows ) {
			ExecuteAPI( (string)row.Cells[APIView_FileName.Index].Value );
		}
		/*/
		if (!APICaller.IsBusy)
			APICaller.RunWorkerAsync(APIView.SelectedRows.Cast<DataGridViewRow>().Select(row => row.Cells[APIView_FileName.Index].Value as string).OrderBy(s => s));
		else
		if (MessageBox.Show(LocalAPILoader2Resources.OperationAlreadyInProgress, LocalAPILoader2Resources.Confirmation, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
			== System.Windows.Forms.DialogResult.Yes)
		{
			APICaller.CancelAsync();
		}
		//*/
	}

	private void ViewMenu_Delete_Click(object sender, EventArgs e)
	{

		foreach (DataGridViewRow row in APIView.SelectedRows)
		{
			APIView.Rows.Remove(row);
		}
	}


	private void ButtonExecuteNext_Click(object sender, EventArgs e)
	{
		if (APIView.SelectedRows.Count == 1)
		{
			DataGridViewRow row = APIView.SelectedRows[0];
			int index = APIView.SelectedRows[0].Index;

			if (SortieRecord is null)
			{
				ExecuteAPI((string)row.Cells[APIView_FileName.Index].Value);
			}
			else
			{
				ExecuteSortieRecordApi((string)row.Cells[APIView_FileName.Index].Value);
			}

			APIView.ClearSelection();
			if (index < APIView.Rows.Count - 1)
			{
				APIView.Rows[index + 1].Selected = true;
				APIView.FirstDisplayedScrollingRowIndex = index + 1;
			}
		}
		else
		{
			MessageBox.Show(LocalAPILoader2Resources.SelectSingleRow, LocalAPILoader2Resources.Information, MessageBoxButtons.OK, MessageBoxIcon.Information);

		}

	}



	private void LoadFiles(string path)
	{

		if (!Directory.Exists(path)) return;

		CurrentPath = path;

		APIView.Rows.Clear();

		var rows = new LinkedList<DataGridViewRow>();

		foreach (string file in Directory.GetFiles(path, "*.json", SearchOption.TopDirectoryOnly))
		{

			var row = new DataGridViewRow();
			row.CreateCells(APIView);

			row.SetValues(Path.GetFileName(file));
			rows.AddLast(row);

		}

		APIView.Rows.AddRange(rows.ToArray());
		APIView.Sort(APIView_FileName, ListSortDirection.Ascending);

	}

	private void LoadSortieRecordFiles(SortieRecord sortieRecord)
	{
		if (sortieRecord.ApiFiles.Count is 0) return;

		APIView.Rows.Clear();

		List<DataGridViewRow> rows = [];

		int firstSortieApiFileId = sortieRecord.ApiFiles.MinBy(f => f.Id)!.Id;

		ElectronicObserverContext db = new();
		db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

		int lastPortFileIdBeforeSortieId = db.ApiFiles
			.Where(f => f.Id < firstSortieApiFileId)
			.Where(f => f.Name == "api_port/port")
			.OrderByDescending(f => f.Id)
			.First()
			.Id;

		ApiFilesBeforeSortie = db.ApiFiles
			.Where(f => f.Id < firstSortieApiFileId)
			.Where(f => f.Id >= lastPortFileIdBeforeSortieId)
			.ToList();

		foreach (string file in ApiFilesBeforeSortie.Concat(sortieRecord.ApiFiles).Select(f => $"{f.Id} {f.ApiFileType} {f.Name}"))
		{
			DataGridViewRow row = new();
			row.CreateCells(APIView);

			row.SetValues(file);
			rows.Add(row);
		}

		APIView.Rows.AddRange([.. rows]);
		APIView.Sort(APIView_FileName, ListSortDirection.Ascending);

	}


	//filename format: yyyyMMdd_hhmmssff[Q|S]@apipath@apiname.json
	private string GetAPIName(string fileName)
	{
		int indexa = fileName.IndexOf('@') + 1, indexb = fileName.LastIndexOf('.');
		return fileName.Substring(indexa).Substring(0, indexb - indexa).Replace('@', '/');
	}

	private string GetAPITime(string filename)
	{
		return filename.Substring(0, filename.IndexOf('@') - 1);
	}


	private bool IsRequest(string filename)
	{
		return char.ToLower(filename[filename.IndexOf('@') - 1]) == 'q';
	}

	private bool IsResponse(string filename)
	{
		return char.ToLower(filename[filename.IndexOf('@') - 1]) == 's';
	}


	private void ExecuteAPI(string filename)
	{

		if (APIObserver.Instance.APIList.ContainsKey(GetAPIName(filename)))
		{

			string data;

			try
			{
				using (var sr = new System.IO.StreamReader(CurrentPath + "\\" + filename))
				{
					data = sr.ReadToEnd();
				}

			}
			catch (Exception ex)
			{
				Utility.Logger.Add(3, string.Format(LoggerRes.FailedToLoadAPIFile, filename, ex.Message));
				return;
			}


			if (IsRequest(filename))
				APIObserver.Instance.LoadRequest("/kcsapi/" + GetAPIName(filename), data);

			if (IsResponse(filename))
				APIObserver.Instance.LoadResponse("/kcsapi/" + GetAPIName(filename), data);
		}

	}

	private void ExecuteSortieRecordApi(string filename)
	{
		Debug.Assert(SortieRecord is not null);

		string[] values = filename.Split(" ");

		int recordId = int.Parse(values[0]);
		ApiFileType apiFileType = Enum.Parse<ApiFileType>(values[1]);
		string apiName = values[2];

		if (!APIObserver.Instance.APIList.ContainsKey(apiName)) return;

		ApiFile apiFile = ApiFilesBeforeSortie.Concat(SortieRecord.ApiFiles).First(f => f.Id == recordId);

		if (apiFile.Name is "api_port/port")
		{
			string? savedPortResponseJson = LoadApiResponse("api_port/port");

			if (savedPortResponseJson is not null)
			{
				ApiResponse<ApiPortPortResponse> savedPortResponse = JsonSerializer
					.Deserialize<ApiResponse<ApiPortPortResponse>>(savedPortResponseJson[7..])
					?? throw new NotImplementedException();

				ApiResponse<ApiPortPortResponse> apiFilePortResponse = JsonSerializer
					.Deserialize<ApiResponse<ApiPortPortResponse>>(apiFile.Content)
					?? throw new NotImplementedException();

				apiFilePortResponse.ApiData.ApiShip = savedPortResponse.ApiData.ApiShip;
				apiFilePortResponse.ApiData.ApiLog = savedPortResponse.ApiData.ApiLog;
				apiFilePortResponse.ApiData.ApiNdock = savedPortResponse.ApiData.ApiNdock;

				apiFilePortResponse.ApiData.ApiDeckPort = SortieRecord.FleetData.Fleets
					.Select((f, i) => f switch
					{
						null => null,

						_ => new FleetDataDto
						{
							ApiId = i + 1,
							ApiShip = f.Ships.Select(s => s.DropId ?? 0).ToList(),
							ApiMission = [0, 0, 0, 0],
						},
					})
					.OfType<FleetDataDto>()
					.ToList();

				apiFile.Content = JsonSerializer.Serialize(apiFilePortResponse, JsonSerializerOptions);
			}
		}

		switch (apiFileType)
		{
			case ApiFileType.Request:
				Dictionary<string, string> queryParameters = JsonSerializer
					.Deserialize<Dictionary<string, string>>(apiFile.Content)
					?? throw new NotImplementedException();

				string queryString = string.Join("&", queryParameters.Select(kvp => $"{kvp.Key}={kvp.Value}"));
				APIObserver.Instance.LoadRequest("/kcsapi/" + apiName, queryString);
				break;

			case ApiFileType.Response:
				APIObserver.Instance.LoadResponse("/kcsapi/" + apiName, $"svdata={apiFile.Content}");
				break;
		}
	}

	private void APICaller_DoWork(object sender, DoWorkEventArgs e)
	{
		if (e.Argument is not IEnumerable<string?> files) return;

		Action<string> act = SortieRecord switch
		{
			null => ExecuteAPI,
			_ => ExecuteSortieRecordApi
		};

		foreach (string? file in files)
		{
			Invoke(act, file);
			System.Threading.Thread.Sleep(10);      //ゆるして

			if (APICaller.CancellationPending)
			{
				e.Result = file;
				break;
			}
		}

	}

	private void APICaller_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{

		if (e.Result != null)
		{   //canceled
			int count = APIView.Rows.Count;
			int result = -1;
			string canceledFile = e.Result as string;

			for (int i = 0; i < count; i++)
			{
				if (APIView[APIView_FileName.Index, i].Value.ToString() == canceledFile)
				{
					result = i + 1;     // 探知結果までは処理済みのため
					break;
				}
			}

			if (result != -1 && result < count)
			{
				APIView.ClearSelection();
				APIView.Rows[result].Selected = true;
				APIView.FirstDisplayedScrollingRowIndex = result;
			}
		}
	}


	private void DialogLocalAPILoader2_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (APICaller.IsBusy)
		{
			e.Cancel = true;
			System.Media.SystemSounds.Exclamation.Play();
		}
	}


	private void ButtonSearch_Click(object sender, EventArgs e)
	{

		int count = APIView.Rows.Count;
		int index;
		int result = -1;
		if (APIView.SelectedRows.Count > 0)
			index = APIView.SelectedRows[0].Index + 1;
		else
			index = 0;

		if (index >= count)
			index = 0;

		for (int i = index; i < count; i++)
		{
			if (APIView[APIView_FileName.Index, i].Value.ToString().ToLower().Contains(TextFilter.Text.ToLower()))
			{
				result = i;
				break;
			}
		}

		if (result != -1)
		{
			APIView.ClearSelection();
			APIView.Rows[result].Selected = true;
			APIView.FirstDisplayedScrollingRowIndex = result;
		}
		else
		{
			System.Media.SystemSounds.Asterisk.Play();
		}
	}

	private void ButtonSearchPrev_Click(object sender, EventArgs e)
	{

		int count = APIView.Rows.Count;
		int index;
		int result = -1;
		if (APIView.SelectedRows.Count > 0)
			index = APIView.SelectedRows[0].Index - 1;
		else
			index = count - 1;

		if (index < 0)
			index = count - 1;

		for (int i = index; i >= 0; i--)
		{
			if (APIView[APIView_FileName.Index, i].Value.ToString().ToLower().Contains(TextFilter.Text.ToLower()))
			{
				result = i;
				break;
			}
		}

		if (result != -1)
		{
			APIView.ClearSelection();
			APIView.Rows[result].Selected = true;
			APIView.FirstDisplayedScrollingRowIndex = result;
		}
		else
		{
			System.Media.SystemSounds.Asterisk.Play();
		}
	}



	private void ButtonSearchLastStart2_Click(object sender, EventArgs e)
	{
		for (int i = APIView.Rows.Count - 1; i >= 0; i--)
		{
			if (APIView[APIView_FileName.Index, i].Value.ToString().ToLower().Contains("s@api_start2@getData."))
			{
				APIView.ClearSelection();
				APIView.Rows[i].Selected = true;
				APIView.FirstDisplayedScrollingRowIndex = i;
				return;
			}
		}

		//failed
		System.Media.SystemSounds.Asterisk.Play();
	}


	private void TextFilter_KeyDown(object sender, KeyEventArgs e)
	{
		if (e.KeyCode == Keys.Enter)
		{
			e.SuppressKeyPress = true;
			ButtonSearch.PerformClick();
		}
	}


	private void APIView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
	{
		try
		{
			ProcessStartInfo psi = new()
			{
				FileName = CurrentPath + "\\" + APIView.SelectedCells.OfType<DataGridViewCell>().First().Value,
				UseShellExecute = true
			};
			Process.Start(psi);
		}
		catch (Exception ex)
		{
			Utility.Logger.Add(1, string.Format(LocalAPILoader2Resources.FailedToStart, ex.GetType().Name, ex.Message));
		}
	}

	private string? LoadApiResponse(string apiName)
	{
		Configuration.ConfigurationData Config = Configuration.Config;

		if (string.IsNullOrEmpty(Config.Connection.SaveDataPath)) return null;
		if (!Directory.Exists(Config.Connection.SaveDataPath)) return null;

		string filePath = Path.Combine(Config.Connection.SaveDataPath, "kcsapi", apiName);

		if (!File.Exists(filePath)) return null;

		return File.ReadAllText(filePath);
	}
}
