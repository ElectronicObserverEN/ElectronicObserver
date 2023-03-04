using System;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ElectronicObserver.Resource;
using ElectronicObserver.Window.Control;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels;

public partial class StateLabel : ObservableObject
{
	public FleetStates State { get; set; }
	public FleetItemControlViewModel Label { get; set; }
	public DateTime Timer { get; set; }
	private bool _onmouse { get; set; }

	private string _text = "";
	public string Text
	{
		get { return _text; }
		set
		{
			_text = value;
			UpdateText();
		}
	}
	private string _shortenedText = "";
	public string ShortenedText
	{
		get { return _shortenedText; }
		set
		{
			_shortenedText = value;
			UpdateText();
		}
	}
	private bool _autoShorten;
	public bool AutoShorten
	{
		get { return _autoShorten; }
		set
		{
			_autoShorten = value;
			UpdateText();
		}
	}

	private bool _enabled;
	public bool Enabled
	{
		get { return _enabled; }
		set
		{
			_enabled = value;
			Label.Visible = value;
			OnPropertyChanged(nameof(Visibility));
		}
	}

	public string? DisplayText => (!AutoShorten || _onmouse) ? Text : ShortenedText;
	public Visibility Visibility => Enabled.ToVisibility();

	public StateLabel()
	{
		Label = GetDefaultLabel();
		// Label.MouseEnter += Label_MouseEnter;
		// Label.MouseLeave += Label_MouseLeave;
		Enabled = false;
	}

	public static FleetItemControlViewModel GetDefaultLabel() => new()
	{
		// Anchor = AnchorStyles.Left,
		// ImageList = ResourceManager.Instance.Icons,
		// Padding = new Padding(2, 2, 2, 2),
		// Margin = new Padding(2, 0, 2, 0),
		// AutoSize = true
	};

	public void SetInformation(FleetStates state, string text, string shortenedText, int imageIndex, Color backColor)
	{
		SetInformation(state, text, shortenedText, imageIndex, backColor, Utility.Configuration.Config.UI.ForeColor.ToWpfColor());
	}
	public void SetInformation(FleetStates state, string text, string shortenedText, int imageIndex, Color backColor, Color forecolor)
	{
		State = state;
		Text = text;
		ShortenedText = shortenedText;
		UpdateText();
		Label.ImageIndex = (IconContent)imageIndex;
		Label.BackColor = backColor;
		Label.ForeColor = forecolor;
	}

	public void SetInformation(FleetStates state, string text, string shortenedText, int imageIndex)
	{
		SetInformation(state, text, shortenedText, imageIndex, Colors.Transparent);
	}

	public void UpdateText()
	{
		// Label.Text = (!AutoShorten || _onmouse) ? Text : ShortenedText;
	}

	[RelayCommand]
	void MouseEnter()
	{
		_onmouse = true;
		UpdateText();
	}

	[RelayCommand]
	void MouseLeave()
	{
		_onmouse = false;
		UpdateText();
	}
}
