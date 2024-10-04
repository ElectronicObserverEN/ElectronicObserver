using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Web;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Database;
using ElectronicObserver.Database.KancolleApi;
using ElectronicObserver.Window.Tools.SortieRecordViewer;

namespace ElectronicObserver.Window.Tools.DatabaseExplorer.ApiTypeTester;

public partial class ApiTypeTesterViewModel
{
	private ElectronicObserverContext Db { get; } = new();
	private JsonSerializerOptions Options { get; } = new()
	{
		UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
	};

	public ObservableCollection<string> ParsingErrors { get; } = new();

	[RelayCommand]
	private void Run()
	{
		ParsingErrors.Clear();

		IQueryable<Database.KancolleApi.ApiFile> files = Db.ApiFiles.AsQueryable();

		// files = files.Take(100000);

		foreach (Database.KancolleApi.ApiFile file in files)
		{
			switch (file.ApiFileType)
			{
				case ApiFileType.Request:
					if (!file.Content.StartsWith('{'))
					{
						NameValueCollection query = HttpUtility.ParseQueryString(file.Content);

						Dictionary<string, string> dictionary = query.AllKeys
							.ToDictionary(k => k, k => query[k]);

						file.Content = JsonSerializer.Serialize(dictionary);
					}

					ParseRequest(file);
					break;

				case ApiFileType.Response:
				{
					if (file.Content.StartsWith("s"))
					{
						file.Content = file.Content[7..];
					}

					ParseResponse(file);
					break;
				}
			}
		}
	}

	private void ParseRequest(Database.KancolleApi.ApiFile file)
	{
		try
		{
			_ = file.GetRequestApiData(Options);
		}
		catch (Exception e)
		{
			AddError(file, e);
		}
	}

	private void ParseResponse(Database.KancolleApi.ApiFile file)
	{
		try
		{
			_ = file.GetResponseApiData(Options);
		}
		catch (Exception e)
		{
			AddError(file, e);
		}
	}

	private void AddError(Database.KancolleApi.ApiFile file, Exception e)
	{
		try
		{
			// ignore invalid json values
			// can happen if there are connection issues
			JsonDocument.Parse(file.Content);
		}
		catch
		{
			return;
		}

		string error = $"{file.TimeStamp} {file.ApiFileType} {file.Name}: {e.Message}";

		if (!ParsingErrors.Contains(error))
		{
			ParsingErrors.Add(error);
		}
	}
}
