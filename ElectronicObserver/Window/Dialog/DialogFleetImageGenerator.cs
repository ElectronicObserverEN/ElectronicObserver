using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using ElectronicObserver.Data;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.Utility.Storage;
using ElectronicObserver.ViewModels;
using Translation = ElectronicObserver.Properties.Window.Dialog.DialogFleetImageGenerator;

namespace ElectronicObserver.Window.Dialog;

public partial class DialogFleetImageGenerator : Form
{

	private FleetImageArgument CurrentArgument;
	private Font GeneralFont;

	private readonly TextBox[] TextFontList;
	private Regex LFtoCRLF = new Regex(@"\n|\r\n", RegexOptions.Multiline | RegexOptions.Compiled);


	public DialogFleetImageGenerator()
	{
		InitializeComponent();

		TextFontList = new TextBox[]{
			TextTitleFont,
			TextLargeFont,
			TextMediumFont,
			TextSmallFont,
			TextMediumDigitFont,
			TextSmallDigitFont,
		};

		for (int i = 0; i < TextFontList.Length; i++)
		{
			int x = i;
			this.Controls.Find("Select" + TextFontList[i].Name.Remove(0, 4), true).First().Click += (sender, e) => SelectFont_Click(sender, e, x);
		}

		LoadConfiguration();

		Translate();
	}

	public DialogFleetImageGenerator(int fleetID) : this()
	{

		if (KCDatabase.Instance.Fleet.CombinedFlag > 0 && fleetID <= 2)
			CurrentArgument.FleetIDs = new int[] { 1, 2 };
		else
			CurrentArgument.FleetIDs = new int[] { fleetID };

		Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		ConfigurationChanged();
	}

	private void ConfigurationChanged()
	{
		Configuration.ConfigurationData c = Configuration.Config;

		Font = c.UI.MainFont.FontData;
	}

	public void Translate()
	{
		tabPage1.Text = Translation.Basic;
		GroupOutputPath.Text = Translation.GroupOutputPath;
		SearchOutputPath.Text = Translation.SearchOutputPath;
		ToolTipInfo.SetToolTip(SearchOutputPath, Translation.SearchOutputPathToolTip);
		ToolTipInfo.SetToolTip(OutputPath, Translation.OutputPathToolTip);
		groupBox7.Text = Translation.CustomText;
		label2.Text = Translation.FleetTitle;
		label3.Text = Translation.Comment;
		groupBox3.Text = Translation.Mode;
		ImageTypeBanner.Text = Translation.ImageTypeBanner;
		ImageTypeCutin.Text = Translation.ImageTypeCutin;
		ImageTypeCard.Text = Translation.ImageTypeCard;
		groupBox1.Text = Translation.Fleet;

		tabPage2.Text = Translation.Details;
		groupBox2.Text = Translation.GroupOutputPath;
		SyncronizeTitleAndFileName.Text = Translation.SynchronizeTitleAndFileName;
		ToolTipInfo.SetToolTip(SyncronizeTitleAndFileName, Translation.SynchronizeTitleAndFileNameToolTip);
		ToolTipInfo.SetToolTip(AutoSetFileNameToDate, Translation.AutoSetFileNameToDateToolTip);
		ToolTipInfo.SetToolTip(OutputToClipboard, Translation.OutputToClipboardToolTip);
		ToolTipInfo.SetToolTip(DisableOverwritePrompt, Translation.DisableOverwritePromptToolTip);
		ToolTipInfo.SetToolTip(OpenImageAfterOutput, Translation.OpenImageAfterOutputToolTip);
		groupBox5.Text = Translation.BackgroundImage;
		ToolTipInfo.SetToolTip(ClearBackgroundPath, Translation.ClearBackgroundPathToolTip);
		ToolTipInfo.SetToolTip(SearchBackgroundImagePath, Translation.SearchBackgroundImagePathToolTip);
		ToolTipInfo.SetToolTip(BackgroundImagePath, Translation.BackgroundImagePathToolTip);
		groupBox4.Text = Translation.Layout;
		AvoidTwitterDeterioration.Text = Translation.AvoidTwitterDeterioration;
		ToolTipInfo.SetToolTip(AvoidTwitterDeterioration, Translation.AvoidTwitterDeteriorationToolTip);
		ToolTipInfo.SetToolTip(ReflectDamageGraphic, Translation.ReflectDamageGraphicToolTip);
		label5.Text = Translation.ShipColumn;
		ToolTipInfo.SetToolTip(HorizontalShipCount, Translation.HorizontalShipCountToolTip);
		ToolTipInfo.SetToolTip(HorizontalFleetCount, Translation.HorizontalFleetCountToolTip);
		label4.Text = Translation.FleetColumn;

		tabPage3.Text = Translation.Font;
		groupBox6.Text = Translation.Font;
		ButtonClearFont.Text = Translation.ButtonClearFont;
		// todo: this doesn't exist in the Japanese version, should it be added?
		// ToolTipInfo.SetToolTip(ButtonClearFont, Translation.ButtonClearFontToolTip);
		ToolTipInfo.SetToolTip(ApplyGeneralFont, Translation.ApplyGeneralFontToolTip);
		label12.Text = Translation.DigitSmall;
		ToolTipInfo.SetToolTip(SelectSmallDigitFont, Translation.OpenDialogToSpecifyFontSetting);
		label11.Text = Translation.DigitMedium;
		ToolTipInfo.SetToolTip(SelectMediumDigitFont, Translation.OpenDialogToSpecifyFontSetting);
		label10.Text = Translation.FontSmall;
		ToolTipInfo.SetToolTip(SelectSmallFont, Translation.OpenDialogToSpecifyFontSetting);
		label9.Text = Translation.FontMedium;
		ToolTipInfo.SetToolTip(SelectMediumFont, Translation.OpenDialogToSpecifyFontSetting);
		label8.Text = Translation.FontLarge;
		ToolTipInfo.SetToolTip(SelectLargeFont, Translation.OpenDialogToSpecifyFontSetting);
		label7.Text = Translation.FleetTitle;
		ToolTipInfo.SetToolTip(SelectTitleFont, Translation.OpenDialogToSpecifyFontSetting);
		label6.Text = Translation.ChangeAll;
		ToolTipInfo.SetToolTip(SelectGeneralFont, Translation.OpenDialogToSpecifyFontSetting);
		OpenImageDialog.Title = Translation.OpenImageDialog;
		SaveImageDialog.Title = Translation.SaveImageDialog;

		Text = Translation.Title;
	}

	private void DialogFleetImageGenerator_Load(object sender, EventArgs e)
	{

		this.Icon = ResourceManager.ImageToIcon(ResourceManager.Instance.Icons.Images[(int)IconContent.FormFleetImageGenerator]);

		ApplyToUI(CurrentArgument);

		UpdateButtonAlert();
	}



	private void LoadConfiguration()
	{
		var config = Utility.Configuration.Config.FleetImageGenerator;

		CurrentArgument = config.Argument.Clone();


		switch (config.ImageType)
		{
			case 0:
			default:
				ImageTypeCard.Checked = true;
				break;
			case 1:
				ImageTypeCutin.Checked = true;
				break;
			case 2:
				ImageTypeBanner.Checked = true;
				break;
		}

		OutputToClipboard.Checked = config.OutputType == 1;
		OpenImageAfterOutput.Checked = config.OpenImageAfterOutput;
		DisableOverwritePrompt.Checked = config.DisableOverwritePrompt;

		OutputPath.Text = config.LastOutputPath;
		try
		{
			SaveImageDialog.FileName = Path.GetFileName(config.LastOutputPath);
			SaveImageDialog.InitialDirectory = string.IsNullOrWhiteSpace(config.LastOutputPath) ? "" : Path.GetDirectoryName(config.LastOutputPath);
		}
		catch (Exception)
		{
		}

		SyncronizeTitleAndFileName.Checked = config.SyncronizeTitleAndFileName;
		AutoSetFileNameToDate.Checked = config.AutoSetFileNameToDate;

	}

	private void SaveConfiguration()
	{
		var config = Utility.Configuration.Config.FleetImageGenerator;

		if (config.Argument != null)
			config.Argument.DisposeResources();

		config.Argument = CurrentArgument.Clone();

		if (ImageTypeCard.Checked)
			config.ImageType = 0;
		else if (ImageTypeCutin.Checked)
			config.ImageType = 1;
		else if (ImageTypeBanner.Checked)
			config.ImageType = 2;

		config.OutputType = OutputToClipboard.Checked ? 1 : 0;
		config.OpenImageAfterOutput = OpenImageAfterOutput.Checked;
		config.DisableOverwritePrompt = DisableOverwritePrompt.Checked;
		config.AutoSetFileNameToDate = AutoSetFileNameToDate.Checked;
		config.SyncronizeTitleAndFileName = SyncronizeTitleAndFileName.Checked;

		config.LastOutputPath = OutputPath.Text;
	}



	private void ApplyToUI(FleetImageArgument args)
	{

		int[] fleetIDs = args.FleetIDs ?? new int[0];

		TargetFleet1.Checked = fleetIDs.Contains(1);
		TargetFleet2.Checked = fleetIDs.Contains(2);
		TargetFleet3.Checked = fleetIDs.Contains(3);
		TargetFleet4.Checked = fleetIDs.Contains(4);

		if (!SyncronizeTitleAndFileName.Checked)
			Title.Text = args.Title;
		Comment.Text = string.IsNullOrWhiteSpace(args.Comment) ? "" : LFtoCRLF.Replace(args.Comment, "\r\n");       // 保存データからのロード時に \n に変換されてしまっているため


		HorizontalFleetCount.Value = args.HorizontalFleetCount;
		HorizontalShipCount.Value = args.HorizontalShipCount;

		ReflectDamageGraphic.Checked = args.ReflectDamageGraphic;
		AvoidTwitterDeterioration.Checked = args.AvoidTwitterDeterioration;

		BackgroundImagePath.Text = args.BackgroundImagePath;

		for (int i = 0; i < TextFontList.Length; i++)
		{
			TextFontList[i].Text = SerializableFont.FontToString(args.Fonts[i], true);
		}
	}

	private FleetImageArgument ApplyToArgument(FleetImageArgument defaultValue = null)
	{

		var ret = defaultValue?.Clone() ?? new FleetImageArgument();

		ret.FleetIDs = new[]{
			TargetFleet1.Checked ? 1 : 0,
			TargetFleet2.Checked ? 2 : 0,
			TargetFleet3.Checked ? 3 : 0,
			TargetFleet4.Checked ? 4 : 0
		}.Where(i => i > 0).ToArray();

		ret.HorizontalFleetCount = (int)HorizontalFleetCount.Value;
		ret.HorizontalShipCount = (int)HorizontalShipCount.Value;

		ret.ReflectDamageGraphic = ReflectDamageGraphic.Checked;
		ret.AvoidTwitterDeterioration = AvoidTwitterDeterioration.Checked;

		var fonts = ret.Fonts;
		for (int i = 0; i < fonts.Length; i++)
		{
			if (fonts[i] != null)
				fonts[i].Dispose();
			fonts[i] = SerializableFont.StringToFont(TextFontList[i].Text, true);
		}
		ret.Fonts = fonts;

		ret.BackgroundImagePath = BackgroundImagePath.Text;

		ret.Title = Title.Text;
		ret.Comment = Comment.Text;

		return ret;
	}

	private int ImageType
	{
		get
		{
			if (ImageTypeCard.Checked)
				return 0;
			if (ImageTypeCutin.Checked)
				return 1;
			if (ImageTypeBanner.Checked)
				return 2;
			if (ImageTypeBaseAirCorps.Checked)
				return 3;
			return 0;
		}
	}

	private int[] ToFleetIDs()
	{
		return new[]{
			TargetFleet1.Checked ? 1 : 0,
			TargetFleet2.Checked ? 2 : 0,
			TargetFleet3.Checked ? 3 : 0,
			TargetFleet4.Checked ? 4 : 0
		}.Where(i => i > 0).ToArray();
	}


	private void ApplyGeneralFont_Click(object sender, EventArgs e)
	{

		if (GeneralFont != null)
		{
			GeneralFont.Dispose();
		}
		GeneralFont = SerializableFont.StringToFont(TextGeneralFont.Text, true);

		if (GeneralFont == null)
		{
			MessageBox.Show(Translation.SpecifiedFontDoesNotExist, Translation.FontConversionFailed, MessageBoxButtons.OK, MessageBoxIcon.Error);
			TextGeneralFont.Text = "";
			return;
		}


		for (int i = 0; i < TextFontList.Length; i++)
		{
			float size = FleetImageArgument.DefaultFontPixels[i];
			var unit = GraphicsUnit.Pixel;
			var style = FontStyle.Regular;

			var font = SerializableFont.StringToFont(TextFontList[i].Text, true);
			if (font != null)
			{
				size = font.Size;
				unit = font.Unit;
				style = font.Style;
				font.Dispose();
			}

			font = new Font(GeneralFont.FontFamily, size, style, unit);
			TextFontList[i].Text = SerializableFont.FontToString(font);
			font.Dispose();
		}

	}


	private void SelectGeneralFont_Click(object sender, EventArgs e)
	{
		fontDialog1.Font = GeneralFont;
		if (fontDialog1.ShowDialog(App.Current.MainWindow) == DialogResult.OK)
		{
			GeneralFont = fontDialog1.Font;
			TextGeneralFont.Text = SerializableFont.FontToString(GeneralFont, true);
		}
	}

	private void SelectFont_Click(object sender, EventArgs e, int index)
	{
		fontDialog1.Font = SerializableFont.StringToFont(TextFontList[index].Text, true);
		if (fontDialog1.ShowDialog(App.Current.MainWindow) == DialogResult.OK)
		{
			TextFontList[index].Text = SerializableFont.FontToString(fontDialog1.Font, true);
		}
	}


	private void SearchBackgroundImagePath_Click(object sender, EventArgs e)
	{
		OpenImageDialog.FileName = BackgroundImagePath.Text;
		if (OpenImageDialog.ShowDialog(App.Current.MainWindow) == DialogResult.OK)
		{
			BackgroundImagePath.Text = OpenImageDialog.FileName;
		}
	}

	private void ClearBackgroundPath_Click(object sender, EventArgs e)
	{
		BackgroundImagePath.Text = "";
	}




	private void ButtonOK_Click(object sender, EventArgs e)
	{

		var args = ApplyToArgument();

		// validation
		if (args.FleetIDs == null || args.FleetIDs.Length == 0)
		{
			MessageBox.Show(Translation.SelectFleetToExport, Translation.InputError, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			args.DisposeResources();
			return;
		}

		if (args.HorizontalFleetCount <= 0 || args.HorizontalShipCount <= 0)
		{
			MessageBox.Show(Translation.FleetMustContainShip, Translation.InputError, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			args.DisposeResources();
			return;
		}

		if (args.Fonts.Any(f => f == null))
		{
			MessageBox.Show(Translation.SpecifiedFontDoesNotExist, Translation.InputError, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
			args.DisposeResources();
			return;
		}

		if (!OutputToClipboard.Checked)
		{
			if (string.IsNullOrWhiteSpace(OutputPath.Text))
			{
				MessageBox.Show(Translation.EnterDestinationPath, Translation.InputError, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				args.DisposeResources();
				return;
			}

			if (OutputPath.Text.ToCharArray().Intersect(Path.GetInvalidPathChars()).Any())
			{
				MessageBox.Show(Translation.PathContainsInvalidCharacters, Translation.InputError, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				args.DisposeResources();
				return;
			}

			if (!DisableOverwritePrompt.Checked && File.Exists(OutputPath.Text))
			{
				if (MessageBox.Show(string.Format(Translation.OverwriteExistingFile, Path.GetFileName(OutputPath.Text)), Translation.OverwriteConfirmation,
						MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2)
					== System.Windows.Forms.DialogResult.No)
				{
					args.DisposeResources();
					return;
				}
			}
		}

		int mode = ImageType;

		try
		{

			if (!OutputToClipboard.Checked)
			{

				using (var image = GenerateFleetImage(args, mode))
				{

					if (!Directory.Exists(Path.GetDirectoryName(OutputPath.Text)))
					{
						Directory.CreateDirectory(Path.GetDirectoryName(OutputPath.Text));
					}

					switch (Path.GetExtension(OutputPath.Text).ToLower())
					{
						case ".png":
						default:
							image.Save(OutputPath.Text, System.Drawing.Imaging.ImageFormat.Png);
							break;

						case ".bmp":
						case ".dib":
							image.Save(OutputPath.Text, System.Drawing.Imaging.ImageFormat.Bmp);
							break;

						case ".gif":
							image.Save(OutputPath.Text, System.Drawing.Imaging.ImageFormat.Gif);
							break;

						case ".tif":
						case ".tiff":
							image.Save(OutputPath.Text, System.Drawing.Imaging.ImageFormat.Tiff);
							break;

						case ".jpg":
						case ".jpeg":
						case ".jpe":
						case ".jfif":
						{
							// jpeg quality settings
							var encoderParams = new System.Drawing.Imaging.EncoderParameters();
							encoderParams.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 90L);

							var codecInfo = System.Drawing.Imaging.ImageCodecInfo.GetImageEncoders().FirstOrDefault(codec => codec.MimeType == "image/jpeg");

							image.Save(OutputPath.Text, codecInfo, encoderParams);
						}
						break;
					}

					if (OpenImageAfterOutput.Checked)
					{
						ProcessStartInfo psi = new ProcessStartInfo
						{
							FileName = OutputPath.Text,
							UseShellExecute = true
						};
						Process.Start(psi);
					}
				}
			}
			else
			{
				using (var image = GenerateFleetImage(args, mode))
				{
					Clipboard.SetImage(image);
				}
			}



			if (CurrentArgument != null)
				CurrentArgument.DisposeResources();
			CurrentArgument = args;
			SaveConfiguration();

			Utility.Logger.Add(2, Translation.FleetImageExportedSuccessfully);

		}
		catch (Exception ex)
		{

			ErrorReporter.SendErrorReport(ex, Translation.FailedToExportFleetImage);
			MessageBox.Show(Translation.FailedToExportFleetImage + "\r\n" + ex.GetType().Name + "\r\n" + ex.Message, Translation.ExportFailure, MessageBoxButtons.OK, MessageBoxIcon.Error);

		}
		finally
		{
			args.DisposeResources();
		}


		Close();

	}

	private Bitmap GenerateFleetImage(FleetImageArgument args, int mode)
	{
		switch (mode)
		{
			case 0:
			default:
				return FleetImageGenerator.GenerateCardBitmap(args);
			case 1:
				return FleetImageGenerator.GenerateCutinBitmap(args);
			case 2:
				return FleetImageGenerator.GenerateBannerBitmap(args);
			case 3:
				return FleetImageGenerator.GenerateBaseAirCorpsImage(args);
		}
	}


	private void ButtonCancel_Click(object sender, EventArgs e)
	{
		Close();
	}


	private void ImageTypeCard_CheckedChanged(object sender, EventArgs e)
	{
		if (ImageTypeCard.Checked)
			HorizontalShipCount.Value = 2;

		UpdateButtonAlert();
	}

	private void ImageTypeCutin_CheckedChanged(object sender, EventArgs e)
	{
		if (ImageTypeCutin.Checked)
			HorizontalShipCount.Value = 1;

		UpdateButtonAlert();
	}

	private void ImageTypeBanner_CheckedChanged(object sender, EventArgs e)
	{
		if (ImageTypeBanner.Checked)
			HorizontalShipCount.Value = 2;

		UpdateButtonAlert();
	}



	private bool HasShipImage()
	{
		switch (ImageType)
		{
			case 0:
				return FleetImageGenerator.HasShipImageCard(ToFleetIDs(), ReflectDamageGraphic.Checked);
			case 1:
				return FleetImageGenerator.HasShipImageCutin(ToFleetIDs(), ReflectDamageGraphic.Checked);
			case 2:
				return FleetImageGenerator.HasShipImageBanner(ToFleetIDs(), ReflectDamageGraphic.Checked);
			default:
				return true;
		}
	}

	private void UpdateButtonAlert()
	{

		bool visibility = false;

		if (!Utility.Configuration.Config.Connection.SaveReceivedData || !Utility.Configuration.Config.Connection.SaveOtherFile)
		{
			visibility = true;
			ButtonAlert.Text = Translation.InvalidSettings;
		}


		if (!HasShipImage())
		{
			visibility = true;
			ButtonAlert.Text = Translation.ShipImageNotFound;
		}

		ButtonAlert.Visible = visibility;

	}


	private void ButtonAlert_Click(object sender, EventArgs e)
	{
		var config = Utility.Configuration.Config.Connection;

		if (!config.SaveReceivedData || !config.SaveOtherFile)
		{

			if (MessageBox.Show(Translation.EnableShipImageSaving, Translation.InvalidSettings,
					MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
				== System.Windows.Forms.DialogResult.Yes)
			{

				if (!config.SaveReceivedData)
				{
					config.SaveReceivedData = true;
					config.SaveResponse = false;       // もともと不要にしていたユーザーには res は邪魔なだけだと思うので
				}
				config.SaveOtherFile = true;

				UpdateButtonAlert();
			}

		}

		if (!HasShipImage())
		{
			string needs;
			switch (ImageType)
			{
				case 0:
					needs = Translation.OpenOrganizeAndViewDetails;
					break;
				case 1:
					needs = Translation.SortieFleet;
					break;
				case 2:
					needs = Translation.OpenOrganize;
					break;
				default:
					// todo: the English for this is probably wrong
					needs = Translation.OpenOrganize2;
					break;
			}

			MessageBox.Show(string.Format(Translation.MissingImages, needs), Translation.ShipImageMissing,
				MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

			UpdateButtonAlert();
		}

	}



	private void TargetFleet1_CheckedChanged(object sender, EventArgs e)
	{
		UpdateButtonAlert();
	}


	private void ButtonClearFont_Click(object sender, EventArgs e)
	{

		if (MessageBox.Show(Translation.ResetFontSettings, Translation.ClearConfirmation,
				MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
			== System.Windows.Forms.DialogResult.Yes)
		{

			if (GeneralFont != null)
				GeneralFont.Dispose();
			GeneralFont = null;
			TextGeneralFont.Text = "";

			var defaultFonts = FleetImageArgument.GetDefaultFonts();
			for (int i = 0; i < TextFontList.Length; i++)
			{
				TextFontList[i].Text = SerializableFont.FontToString(defaultFonts[i]);
				defaultFonts[i].Dispose();
			}
		}
	}




	private void Title_TextChanged(object sender, EventArgs e)
	{
		if (SyncronizeTitleAndFileName.Checked)
		{
			try
			{
				string replaceTo = Path.GetDirectoryName(OutputPath.Text) + "\\" + Title.Text + Path.GetExtension(OutputPath.Text);

				if (OutputPath.Text != replaceTo)
					OutputPath.Text = replaceTo;
			}
			catch (Exception)
			{
			}
		}
	}

	private void OutputPath_TextChanged(object sender, EventArgs e)
	{

		if (SyncronizeTitleAndFileName.Checked)
		{
			try
			{
				string replaceTo = Path.GetFileNameWithoutExtension(OutputPath.Text);

				if (Title.Text != replaceTo)
					Title.Text = replaceTo;

			}
			catch (Exception)
			{       // path contains invalid char.
			}
		}

		if (string.IsNullOrWhiteSpace(OutputPath.Text) || OutputPath.Text.ToCharArray().Intersect(Path.GetInvalidPathChars()).Any())
		{
			OutputPath.BackColor = Color.MistyRose;
		}
		else if (File.Exists(OutputPath.Text))
		{
			OutputPath.BackColor = Color.Moccasin;
		}
		else
		{
			OutputPath.BackColor = SystemColors.Window;
		}
	}



	private void AutoSetFileNameToDate_CheckedChanged(object sender, EventArgs e)
	{

		if (AutoSetFileNameToDate.Checked)
		{
			try
			{
				OutputPath.Text = Path.GetDirectoryName(OutputPath.Text) + "\\" + Utility.Mathematics.DateTimeHelper.GetTimeStamp() + Path.GetExtension(OutputPath.Text);
			}
			catch (Exception)
			{
			}
		}

	}


	private void SyncronizeTitleAndFileName_CheckedChanged(object sender, EventArgs e)
	{

		if (SyncronizeTitleAndFileName.Checked)
		{

			if (string.IsNullOrWhiteSpace(OutputPath.Text))
			{
				Title_TextChanged(sender, e);
			}
			else
			{
				OutputPath_TextChanged(sender, e);
			}

		}

	}

	private void SearchOutputPath_Click(object sender, EventArgs e)
	{

		try
		{
			SaveImageDialog.FileName = Path.GetFileName(OutputPath.Text);
			SaveImageDialog.InitialDirectory = string.IsNullOrWhiteSpace(OutputPath.Text) ? "" : Path.GetDirectoryName(OutputPath.Text);
		}
		catch (Exception)
		{
		}
		if (SaveImageDialog.ShowDialog(App.Current.MainWindow) == DialogResult.OK)
		{
			OutputPath.Text = SaveImageDialog.FileName;
		}

	}



	private void DialogFleetImageGenerator_FormClosing(object sender, FormClosingEventArgs e)
	{
		CurrentArgument.DisposeResources();
	}



	private void OutputToClipboard_CheckedChanged(object sender, EventArgs e)
	{
		OutputPath.Enabled =
			SearchOutputPath.Enabled =
				OpenImageAfterOutput.Enabled =
					DisableOverwritePrompt.Enabled =
						AutoSetFileNameToDate.Enabled =
							SyncronizeTitleAndFileName.Enabled =
								!OutputToClipboard.Checked;

		ToolTipInfo.SetToolTip(GroupOutputPath, OutputToClipboard.Checked ? Translation.ImageWillBeExportedToClipboard : null);
	}

	private void Comment_KeyDown(object sender, KeyEventArgs e)
	{

		// Multiline == true の TextBox では、 Ctrl-A ショートカットが無効化されるらしいので自家実装

		if (e.Control && e.KeyCode == Keys.A)
		{
			if (sender != null)
			{
				((TextBox)sender).SelectAll();
			}
			e.SuppressKeyPress = true;
			e.Handled = true;
		}
	}



	private void DialogFleetImageGenerator_FormClosed(object sender, FormClosedEventArgs e)
	{
		ResourceManager.DestroyIcon(Icon);
	}


}
