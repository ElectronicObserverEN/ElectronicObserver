﻿using System;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ElectronicObserver.Window.Wpf.Arsenal;

public class ArsenalItemControlViewModel : ObservableObject
{
	public string? Text { get; set; }
	public System.Drawing.Color ForeColor { get; set; }
	public System.Drawing.Color BackColor { get; set; }
	public DateTime? Tag { get; set; }
	public bool Visible { get; set; }
	public string? ToolTip { get; set; }
	public int MaximumWidth { get; set; }
	
	public SolidColorBrush Foreground => ForeColor.ToBrush();
	public SolidColorBrush Background => BackColor.ToBrush();
}
