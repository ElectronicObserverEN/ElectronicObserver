using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;

namespace Browser.WebView2Browser.Extensions;

public partial class ExtensionManagerViewModel
{
	private WebView2 Browser { get; }
	public FormBrowserTranslationViewModel FormBrowser { get; }

	public ObservableCollection<CoreWebView2BrowserExtension> Extensions { get; } = [];

	public ExtensionManagerViewModel(WebView2 browser)
	{
		Browser = browser;
		FormBrowser = Ioc.Default.GetRequiredService<FormBrowserTranslationViewModel>();
	}

	public async Task InitializeAsync()
	{
		await Browser.EnsureCoreWebView2Async(WebView2ViewModel.Environment);

		await RefreshList();
	}

	private async Task RefreshList()
	{
		Extensions.Clear();

		foreach (CoreWebView2BrowserExtension extension in await Browser.CoreWebView2.Profile.GetBrowserExtensionsAsync())
		{
			Extensions.Add(extension);
		}
	}

	[RelayCommand]
	private async Task AddExtension()
	{
		System.Windows.Forms.FolderBrowserDialog folderBrowserDialog = new()
		{
			RootFolder = Environment.SpecialFolder.Desktop,
		};

		if (folderBrowserDialog.ShowDialog() is not System.Windows.Forms.DialogResult.OK) return;

		try
		{
			await Browser.CoreWebView2.Profile.AddBrowserExtensionAsync(folderBrowserDialog.SelectedPath);
			await RefreshList();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, FormBrowser.Title, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}

	[RelayCommand]
	private async Task RemoveExtension(CoreWebView2BrowserExtension extension)
	{
		try
		{
			await extension.RemoveAsync();
			await RefreshList();
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, FormBrowser.Title, MessageBoxButton.OK, MessageBoxImage.Error);
		}
	}
}
