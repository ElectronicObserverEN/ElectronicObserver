﻿using Avalonia;
using Avalonia.Controls;

namespace ElectronicObserver.Avalonia.Samples.Views;

public partial class MainWindow : Window
{
	public MainWindow()
	{
		InitializeComponent();

#if DEBUG
		this.AttachDevTools();
#endif
	}
}
