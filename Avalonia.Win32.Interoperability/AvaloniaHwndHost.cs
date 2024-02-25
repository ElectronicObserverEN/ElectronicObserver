using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Markup;
using Avalonia.Controls.Embedding;
using AvaloniaControl = Avalonia.Controls.Control;

namespace Avalonia.Win32.Interoperability;

/// <summary>
/// An element that allows you to host an Avalonia control on a WPF page.
/// </summary>
/// <see href="https://github.com/maxkatz6/AvaloniaHwndHostSample"/>
[ContentProperty("Content")]
public class WpfAvaloniaHost : HwndHost
{
	private EmbeddableControlRoot? _root;
	private AvaloniaControl? _content;

	/// <summary>
	/// Initializes a new instance of the <see cref="WpfAvaloniaHost"/> class.
	/// </summary>
	public WpfAvaloniaHost()
	{
		DataContextChanged += AvaloniaHwndHost_DataContextChanged;
	}

	private void AvaloniaHwndHost_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
	{
		if (Content != null)
		{
			// todo: need to figure out what the best way is for setting the DataContext
			// Content.DataContext = e.NewValue;
		}
	}

	/// <summary>
	/// Gets or sets the Avalonia control hosted by the <see cref="WpfAvaloniaHost"/> element.
	/// </summary>
	public AvaloniaControl? Content
	{
		get => _content;
		set
		{
			if (_content != value)
			{
				_content = value;

				if (_root is not null)
				{
					_root.Content = value;
				}

				if (value != null)
				{
					// todo: need to figure out what the best way is for setting the DataContext
					// value.DataContext = DataContext;
				}
			}
		}
	}

	/// <inheritdoc />
	protected override HandleRef BuildWindowCore(HandleRef hwndParent)
	{
		_root = new EmbeddableControlRoot
		{
			Content = _content,
		};

		_root.Prepare();
		_root.StartRendering();

		IntPtr handle = _root.TryGetPlatformHandle()?.Handle
			?? throw new InvalidOperationException("WpfAvaloniaHost is unable to create EmbeddableControlRoot.");

		if (PresentationSource.FromVisual(this) is HwndSource source)
		{
			_ = UnmanagedMethods.SetParent(handle, source.Handle);
		}

		return new HandleRef(_root, handle);
	}

	/// <inheritdoc />
	protected override void DestroyWindowCore(HandleRef hwnd)
	{
		_root?.Dispose();
	}
}
