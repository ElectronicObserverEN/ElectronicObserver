using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ElectronicObserver.Utility.ElectronicObserverApi.Models;

namespace ElectronicObserver.Utility.ElectronicObserverApi.DataIssueLogs;

public class SoftwareIssueReporter(ElectronicObserverApiService api)
{
	public void ProcessException(object sender, UnhandledExceptionEventArgs e)
	{
		if (e.ExceptionObject is not Exception exception) return;

		SoftwareIssueModel issue = new()
		{
			SoftwareVersion = SoftwareInformation.VersionEnglish,
			Exception = BuildIssue(exception),
		};
		
		Task.Run(async () => await ReportIssue(issue)).Wait();
	}

	private async Task ReportIssue(SoftwareIssueModel issue)
	{
		await api.PostJson("SoftwareIssue", issue);
	}

	private SoftwareExceptionModel BuildIssue(Exception exception)
	{
		SoftwareExceptionModel issue = new()
		{
			Type = exception.GetType().ToString(),
			Message = exception.Message,
			StackTrace = exception.StackTrace ?? "",
		};

		if (exception is AggregateException aggregateException)
		{
			if (aggregateException.InnerExceptions.Count > 0)
			{
				foreach (Exception innerException in aggregateException.InnerExceptions)
				{
					issue.InnerExceptions.Add(BuildIssue(innerException));
				}
			}
		}
		else if (exception.InnerException is not null)
		{
			issue.InnerExceptions.Add(BuildIssue(exception.InnerException));
		}

		return issue;
	}
}
