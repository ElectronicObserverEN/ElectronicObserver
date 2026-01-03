using System;
using System.Windows.Forms;
using ElectronicObserver.ViewModels;
using System.ComponentModel;

namespace ElectronicObserver.Window.Dialog;

public partial class DialogShipGroupCSVOutput : Form
{

	/// <summary>
	/// 出力フィルタを指定します。
	/// </summary>
	public enum FilterModeConstants
	{

		/// <summary>全て出力</summary>
		All,

		/// <summary>表示されている行のみ出力</summary>
		VisibleColumnOnly,
	}

	/// <summary>
	/// 出力フォーマットを指定します。
	/// </summary>
	public enum OutputFormatConstants
	{

		/// <summary>閲覧用</summary>
		User,

		/// <summary>データ用</summary>
		Data,
	}


	/// <summary>
	/// 出力ファイルのパス
	/// </summary>
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public string OutputPath
	{
		get { return TextOutputPath.Text; }
		set { TextOutputPath.Text = value; }
	}

	/// <summary>
	/// 出力フィルタ
	/// </summary>
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public FilterModeConstants FilterMode
	{
		get
		{
			if (RadioOutput_All.Checked)
				return FilterModeConstants.All;
			else
				return FilterModeConstants.VisibleColumnOnly;
		}
		set
		{
			switch (value)
			{
				case FilterModeConstants.All:
					RadioOutput_All.Checked = true; break;

				case FilterModeConstants.VisibleColumnOnly:
					RadioOutput_VisibleColumnOnly.Checked = true; break;
			}
		}
	}

	/// <summary>
	/// 出力フォーマット
	/// </summary>
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public OutputFormatConstants OutputFormat
	{
		get
		{
			if (RadioFormat_User.Checked)
				return OutputFormatConstants.User;
			else
				return OutputFormatConstants.Data;
		}
		set
		{
			switch (value)
			{
				case OutputFormatConstants.User:
					RadioFormat_User.Checked = true; break;

				case OutputFormatConstants.Data:
					RadioFormat_Data.Checked = true; break;
			}
		}
	}



	public DialogShipGroupCSVOutput()
	{
		InitializeComponent();

		DialogSaveCSV.InitialDirectory = Utility.Configuration.Config.Connection.SaveDataPath;

		Translate();
	}

	public void Translate()
	{
		groupBox1.Text = GeneralRes.Option;
		RadioFormat_Data.Text = ShipGroupCSVOutputResources.RadioFormat_Data;
		RadioFormat_User.Text = ShipGroupCSVOutputResources.RadioFormat_User;
		RadioOutput_VisibleColumnOnly.Text = ShipGroupCSVOutputResources.RadioOutput_VisibleColumnOnly;
		RadioOutput_All.Text = ShipGroupCSVOutputResources.RadioOutput_All;
		ButtonCancel.Text = GeneralRes.Cancel;
		groupBox2.Text = ShipGroupCSVOutputResources.OutputDestination;
		DialogSaveCSV.Title = ShipGroupCSVOutputResources.DialogSaveCSV;

		Text = ShipGroupCSVOutputResources.Title;
	}

	private void DialogShipGroupCSVOutput_Load(object sender, EventArgs e)
	{


	}

	private void ButtonOutputPathSearch_Click(object sender, EventArgs e)
	{

		if (DialogSaveCSV.ShowDialog(App.Current.MainWindow) == System.Windows.Forms.DialogResult.OK)
		{

			TextOutputPath.Text = DialogSaveCSV.FileName;

		}

		DialogSaveCSV.InitialDirectory = null;

	}

	private void ButtonOK_Click(object sender, EventArgs e)
	{
		DialogResult = System.Windows.Forms.DialogResult.OK;
	}

	private void ButtonCancel_Click(object sender, EventArgs e)
	{
		DialogResult = System.Windows.Forms.DialogResult.Cancel;
	}

}
