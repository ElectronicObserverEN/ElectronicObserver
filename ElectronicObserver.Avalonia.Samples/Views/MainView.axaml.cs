using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Input.Raw;
using Avalonia.VisualTree;
using ElectronicObserver.Avalonia.Samples.ViewModels;
using VisualExtensions = Avalonia.VisualExtensions;

namespace ElectronicObserver.Avalonia.Samples.Views;

public partial class MainView : UserControl
{
	public MainView()
	{
		InitializeComponent();

		// failed attempts at making click+drag selection working
		MyDataGrid.PointerPressed += MyDataGridOnPointerPressed;
		MyDataGrid.PointerMoved += MyDataGridOnPointerMoved;

		MyDataGrid.LoadingRow += MyDataGrid_LoadingRow;
		MyDataGrid.UnloadingRow -= MyDataGrid_LoadingRow;
	}

	private void MyDataGrid_LoadingRow(object? sender, DataGridRowEventArgs e)
	{
		e.Row.PointerEntered += Row_PointerEntered;
	}

	private void Row_PointerEntered(object? sender, PointerEventArgs e)
	{
		if (sender is not Visual visual) return;

		PointerPoint a = e.GetCurrentPoint(visual);

		if (DataContext is not MainViewModel viewModel) return;
		if (visual.DataContext is not Test test) return;

		if (test.Description is "TestDescription3")
		{

		}
	}

	private void MyDataGridOnPointerMoved(object? sender, PointerEventArgs e)
	{
		if (sender is not Visual visual) return;

		PointerPoint a = e.GetCurrentPoint(visual);

		if (a.Properties.IsLeftButtonPressed)
		{
			if (e.Pointer.Captured is not Visual hoveredVisual) return;

			DataGridRow? hoveredRow = hoveredVisual
				.GetVisualAncestors()
				.OfType<DataGridRow>()
				.FirstOrDefault();

			if (hoveredRow?.DataContext is not Test test) return;

			MyTextBlock.Text = test.Description;
		}
	}

	private void MyDataGridOnPointerPressed(object? sender, PointerPressedEventArgs e)
	{

	}
}
