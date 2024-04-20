using System;
using System.Windows.Forms;
using ElectronicObserver.Window.Support;

namespace ElectronicObserver.Window.Dialog;

public partial class DialogTextSelect : Form
{

	public int SelectedIndex => TextSelect.SelectedIndex;

	public object? SelectedItem => TextSelect.SelectedItem;

	private DialogTextSelect()
	{
		InitializeComponent();

		ControlHelper.SetDoubleBuffered(tableLayoutPanel1);
	}

	public DialogTextSelect(string title, string description, object[] items)
		: this()
	{

		Initialize(title, description, items);
	}

	private void Initialize(string title, string description, object[] items)
	{
		Text = title;

		tableLayoutPanel1.SuspendLayout();

		Description.Text = description;

		TextSelect.BeginUpdate();
		TextSelect.Items.Clear();
		TextSelect.Items.AddRange(items);

		if (TextSelect.Items.Count > 0)
		{
			TextSelect.SelectedIndex = 0;
		}

		TextSelect.EndUpdate();

		tableLayoutPanel1.ResumeLayout();

	}

	private void ButtonOK_Click(object sender, EventArgs e)
	{
		DialogResult = DialogResult.OK;
	}

	private void ButtonCancel_Click(object sender, EventArgs e)
	{
		DialogResult = DialogResult.Cancel;
	}
}
