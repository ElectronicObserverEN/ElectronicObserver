using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace Browser.CefSharpBrowser.ExtraBrowser.Behaviours;

public class TextBoxBindingUpdateOnEnterBehaviour : Behavior<TextBox>
{
	protected override void OnAttached()
	{
		AssociatedObject.KeyDown += OnTextBoxKeyDown;
	}

	protected override void OnDetaching()
	{
		AssociatedObject.KeyDown -= OnTextBoxKeyDown;
	}

	private void OnTextBoxKeyDown(object sender, KeyEventArgs e)
	{
		if (sender is not TextBox textBox) return;
		if (e.Key is not Key.Enter) return;

		textBox.GetBindingExpression(TextBox.TextProperty)?.UpdateSource();
	}
}
