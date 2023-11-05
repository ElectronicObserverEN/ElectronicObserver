using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Window.Wpf.Log;

public partial class LogViewModel : AnchorableViewModel
{
	public FormLogTranslationViewModel FormLog { get; set; }
	public ObservableCollection<string> LogList { get; set; } = new();

	public LogViewModel() : base("Log", "Log", IconContent.FormLog)
	{
		FormLog = Ioc.Default.GetService<FormLogTranslationViewModel>()!;
		Title = FormLog.Title;
		FormLog.PropertyChanged += (_, _) => Title = FormLog.Title;

		foreach (var log in Logger.Log)
		{
			if (log.Priority >= Configuration.Config.Log.LogLevel)
			{
				LogList.Add(log.ToString());
			}
		}

		Logger.Instance.LogAdded += data =>
		{
			if (App.Current is null) return;

			if (!App.Current.Dispatcher.CheckAccess())
			{
				//Invokeはメッセージキューにジョブを投げて待つので、別のBeginInvokeされたジョブが既にキューにあると、
				// それを実行してしまい、BeginInvokeされたジョブの順番が保てなくなる
				// GUIスレッドによる処理は、順番が重要なことがあるので、GUIスレッドからInvokeを呼び出してはいけない
				App.Current.Dispatcher.Invoke(new LogAddedEventHandler(Logger_LogAdded), data);
			}
			else
			{
				Logger_LogAdded(data);
			}
		};
	}

	private void Logger_LogAdded(Logger.LogData data)
	{
		LogList.Add(data.ToString());
	}

	[RelayCommand]
	private void ContextMenuLog_Clear_Click()
	{
		LogList.Clear();
	}
}
