using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ElectronicObserver.Notifier;
using ElectronicObserver.Utility.Data;
using ElectronicObserver.ViewModels;

namespace ElectronicObserver.Window.Dialog;

/// <summary>
/// 通知システムの設定ダイアログを扱います。
/// </summary>
public partial class DialogConfigurationNotifier : Form
{

	private NotifierBase Notifier { get; }
	private bool SoundChanged { get; set; }
	private bool ImageChanged { get; set; }

	public DialogConfigurationNotifier(NotifierBase notifier)
	{
		InitializeComponent();

		Translate();

		Notifier = notifier;

		//init base
		SoundChanged = false;
		ImageChanged = false;

		GroupSound.AllowDrop = true;
		GroupImage.AllowDrop = true;



		//init from data

		IsEnabled.Checked = notifier.IsEnabled;

		PlaysSound.Checked = notifier.PlaysSound;
		SoundPath.Text = notifier.SoundPath;
		SoundVolume.Value = notifier.SoundVolume;
		LoopsSound.Checked = notifier.LoopsSound;

		DrawsImage.Checked = notifier.DialogData.DrawsImage;
		ImagePath.Text = notifier.DialogData.ImagePath;

		ShowsDialog.Checked = notifier.ShowsDialog;
		TopMostFlag.Checked = notifier.DialogData.TopMost;
		Alignment.SelectedIndex = (int)notifier.DialogData.Alignment;
		LocationX.Value = notifier.DialogData.Location.X;
		LocationY.Value = notifier.DialogData.Location.Y;
		DrawsMessage.Checked = notifier.DialogData.DrawsMessage;
		HasFormBorder.Checked = notifier.DialogData.HasFormBorder;
		AccelInterval.Value = (decimal)notifier.AccelInterval / 1000;
		ClosingInterval.Value = (decimal)notifier.DialogData.ClosingInterval / 1000;
		for (int i = 0; i < (int)NotifierDialogClickFlags.HighestBit; i++)
			CloseList.SetItemChecked(i, ((int)notifier.DialogData.ClickFlag & (1 << i)) != 0);
		CloseList.SetItemChecked((int)NotifierDialogClickFlags.HighestBit, notifier.DialogData.CloseOnMouseMove);
		ShowWithActivation.Checked = notifier.DialogData.ShowWithActivation;
		ForeColorPreview.ForeColor = notifier.DialogData.ForeColor;
		BackColorPreview.ForeColor = notifier.DialogData.BackColor;
		LevelBorder.Maximum = ExpTable.ShipMaximumLevel;

		if (notifier is NotifierDamage ndmg)
		{
			NotifiesBefore.Checked = ndmg.NotifiesBefore;
			NotifiesNow.Checked = ndmg.NotifiesNow;
			NotifiesAfter.Checked = ndmg.NotifiesAfter;
			ContainsNotLockedShip.Checked = ndmg.ContainsNotLockedShip;
			ContainsSafeShip.Checked = ndmg.ContainsSafeShip;
			ContainsFlagship.Checked = ndmg.ContainsFlagship;
			LevelBorder.Value = ndmg.LevelBorder;
			NotifiesAtEndpoint.Checked = ndmg.NotifiesAtEndpoint;

		}
		else
		{
			GroupDamage.Visible = false;
			GroupDamage.Enabled = false;
		}

		if (notifier is NotifierAnchorageRepair nanc)
		{
			AnchorageRepairNotificationLevel.SelectedIndex = nanc.NotificationLevel;

		}
		else
		{
			GroupAnchorageRepair.Visible = false;
			GroupAnchorageRepair.Enabled = false;
		}

		if (notifier is NotifierBaseAirCorps nbac)
		{
			BaseAirCorps_NotSupplied.Checked = nbac.NotifiesNotSupplied;
			BaseAirCorps_Tired.Checked = nbac.NotifiesTired;
			BaseAirCorps_NotOrganized.Checked = nbac.NotifiesNotOrganized;

			BaseAirCorps_Standby.Checked = nbac.NotifiesStandby;
			BaseAirCorps_Retreat.Checked = nbac.NotifiesRetreat;
			BaseAirCorps_Rest.Checked = nbac.NotifiesRest;

			BaseAirCorps_NormalMap.Checked = nbac.NotifiesNormalMap;
			BaseAirCorps_EventMap.Checked = nbac.NotifiesEventMap;

			BaseAirCorps_SquadronRelocation.Checked = nbac.NotifiesSquadronRelocation;
			BaseAirCorps_EquipmentRelocation.Checked = nbac.NotifiesEquipmentRelocation;
		}
		else
		{
			GroupBaseAirCorps.Visible = false;
			GroupBaseAirCorps.Enabled = false;
		}

		if (notifier is NotifierBattleEnd nba)
		{
			BattleEnd_IdleTimerEnabled.Checked = nba.Config.IdleTimerEnabled;
			BattleEnd_IdleTime.Value = nba.Config.IdleSeconds;
		}
		else
		{
			GroupBattleEnd.Visible = false;
			GroupBattleEnd.Enabled = false;
		}

		DialogOpenSound.Filter = "音楽ファイル|" + string.Join(";", Utility.EOMediaPlayer.SupportedExtensions.Select(s => "*." + s)) + "|File|*";
	}

	public void Translate()
	{
		ButtonCancel.Text = ConfigRes.Cancel;
		GroupSound.Text = NotifyRes.Sound;
		LoopsSound.Text = ConfigRes.Loop;
		label9.Text = ConfigRes.Volume;
		ToolTipText.SetToolTip(SoundVolume, ConfigurationNotifierResources.SoundVolumeToolTip);
		PlaysSound.Text = NotifyRes.Enable;
		ButtonTest.Text = NotifyRes.Test;
		IsEnabled.Text = NotifyRes.EnableNotify;
		GroupImage.Text = NotifyRes.Image;
		DrawsImage.Text = NotifyRes.Enable;
		GroupDialog.Text = NotifyRes.NotifyDialog;
		label5.Text = NotifyRes.CloseOn + "：";
		CloseList.Items.Clear();
		CloseList.Items.AddRange(new object[]
		{
			NotifyRes.LeftClick,
			NotifyRes.LeftDoubleClick,
			NotifyRes.RightClick,
			NotifyRes.RightDoubleClick,
			NotifyRes.MiddleClick,
			NotifyRes.MiddleDoubleClick,
			NotifyRes.MouseOver
		});
		ShowWithActivation.Text = NotifyRes.ShowWithActivation;
		ToolTipText.SetToolTip(ShowWithActivation, ConfigurationNotifierResources.ShowWithActivationToolTip);
		label4.Text = NotifyRes.Location + ":";
		DrawsMessage.Text = NotifyRes.DisplayMessage;
		ToolTipText.SetToolTip(DrawsMessage, NotifyRes.DisplayMessageHint);
		HasFormBorder.Text = NotifyRes.DisplayWindowBorder;
		ToolTipText.SetToolTip(HasFormBorder, NotifyRes.WindowBorderHint);
		label6.Text = NotifyRes.Sec;
		label7.Text = NotifyRes.AutoClose + ":";
		ToolTipText.SetToolTip(ClosingInterval, NotifyRes.IntervalHint);
		BackColorPreview.Text = NotifyRes.BackColorDisplay + ":";
		ToolTipText.SetToolTip(BackColorPreview, NotifyRes.BackColorDispHint);
		ToolTipText.SetToolTip(BackColorSelect, NotifyRes.BackColorSelect);
		ForeColorPreview.Text = NotifyRes.ForeColorDisplay + ":";
		ToolTipText.SetToolTip(ForeColorPreview, NotifyRes.ForeColorDispHint);
		ToolTipText.SetToolTip(ForeColorSelect, NotifyRes.ForeColorSelect);
		label3.Text = NotifyRes.Sec;
		label2.Text = NotifyRes.HurryBy + ":";
		ToolTipText.SetToolTip(AccelInterval, NotifyRes.HurryHint);
		TopMostFlag.Text = NotifyRes.ShowOnTop;
		ToolTipText.SetToolTip(LocationY, NotifyRes.LocYHint);
		ToolTipText.SetToolTip(LocationX, NotifyRes.LocXHint);
		label1.Text = NotifyRes.Alignment + ":";
		Alignment.Items.Clear();
		Alignment.Items.AddRange(new object[]
		{
			NotifyRes.AlignUnset,
			NotifyRes.AlignTopLeft,
			NotifyRes.AlignTop,
			NotifyRes.AlignTopRight,
			NotifyRes.AlignLeft,
			NotifyRes.AlignCenter,
			NotifyRes.AlignRight,
			NotifyRes.AlignBottomLeft,
			NotifyRes.AlignBottom,
			NotifyRes.AlignBottomRight,
			NotifyRes.AlignManualAbs,
			NotifyRes.AlignManualRel
		});
		ToolTipText.SetToolTip(Alignment, NotifyRes.AlignHint);
		ShowsDialog.Text = NotifyRes.Enable;
		GroupDamage.Text = NotifyRes.DamageOptions;
		NotifiesAtEndpoint.Text = NotifyRes.NotifyEndNodes;
		ContainsFlagship.Text = NotifyRes.IncludeFlagship;
		ContainsSafeShip.Text = NotifyRes.IncludeDamecon;
		ContainsNotLockedShip.Text = NotifyRes.IncludeUnlocked;
		label8.Text = NotifyRes.MinLv + ":";
		ToolTipText.SetToolTip(LevelBorder, NotifyRes.LvHint);
		NotifiesAfter.Text = NotifyRes.NotifyAfter;
		ToolTipText.SetToolTip(NotifiesAfter, NotifyRes.NotifyAfterHint);
		NotifiesNow.Text = NotifyRes.NotifyNow;
		ToolTipText.SetToolTip(NotifiesNow, NotifyRes.NotifyNowHint);
		NotifiesBefore.Text = NotifyRes.NotifyBefore;
		ToolTipText.SetToolTip(NotifiesBefore, NotifyRes.NotifyBeforeHint);
		DialogOpenSound.Title = NotifyRes.OpenSound;
		DialogOpenImage.Title = NotifyRes.OpenImage;
		label10.Text = ConfigurationNotifierResources.SettingsWillBeAppliedForTest;
		GroupAnchorageRepair.Text = ConfigurationNotifierResources.GroupAnchorageRepair;
		label11.Text = ConfigurationNotifierResources.TriggerWhen;
		GroupBaseAirCorps.Text = ConfigurationNotifierResources.GroupBaseAirCorps;
		BaseAirCorps_NotSupplied.Text = ConfigurationNotifierResources.BaseAirCorps_NotSupplied;
		ToolTipText.SetToolTip(BaseAirCorps_NotSupplied, ConfigurationNotifierResources.BaseAirCorps_NotSuppliedToolTip);
		BaseAirCorps_Tired.Text = ConfigurationNotifierResources.BaseAirCorps_Tired;
		ToolTipText.SetToolTip(BaseAirCorps_Tired, ConfigurationNotifierResources.BaseAirCorps_TiredToolTip);
		BaseAirCorps_Rest.Text = ConfigurationNotifierResources.BaseAirCorps_Rest;
		ToolTipText.SetToolTip(BaseAirCorps_Rest, ConfigurationNotifierResources.BaseAirCorps_RestToolTip);
		BaseAirCorps_Retreat.Text = ConfigurationNotifierResources.BaseAirCorps_Retreat;
		ToolTipText.SetToolTip(BaseAirCorps_Retreat, ConfigurationNotifierResources.BaseAirCorps_RetreatToolTip);
		BaseAirCorps_Standby.Text = ConfigurationNotifierResources.BaseAirCorps_Standby;
		ToolTipText.SetToolTip(BaseAirCorps_Standby, ConfigurationNotifierResources.BaseAirCorps_StandbyToolTip);
		BaseAirCorps_NormalMap.Text = ConfigurationNotifierResources.BaseAirCorps_NormalMap;
		ToolTipText.SetToolTip(BaseAirCorps_NormalMap, ConfigurationNotifierResources.BaseAirCorps_NormalMapToolTip);
		BaseAirCorps_EventMap.Text = ConfigurationNotifierResources.BaseAirCorps_EventMap;
		ToolTipText.SetToolTip(BaseAirCorps_EventMap, ConfigurationNotifierResources.BaseAirCorps_EventMapToolTip);
		BaseAirCorps_EquipmentRelocation.Text = ConfigurationNotifierResources.BaseAirCorps_EquipmentRelocation;
		ToolTipText.SetToolTip(BaseAirCorps_EquipmentRelocation, ConfigurationNotifierResources.BaseAirCorps_EquipmentRelocationToolTip);
		BaseAirCorps_SquadronRelocation.Text = ConfigurationNotifierResources.BaseAirCorps_SquadronRelocation;
		ToolTipText.SetToolTip(BaseAirCorps_SquadronRelocation, ConfigurationNotifierResources.BaseAirCorps_SquadronRelocationToolTip);
		GroupBattleEnd.Text = ConfigurationNotifierResources.GroupBattleEnd;
		BattleEnd_IdleTimerEnabled.Text = ConfigurationNotifierResources.BattleEnd_IdleTimerEnabled;
		ToolTipText.SetToolTip(BattleEnd_IdleTimerEnabled, ConfigurationNotifierResources.BattleEnd_IdleTimerEnabledToolTip);
		BattleEnd_IdleTime.Text = ConfigurationNotifierResources.BattleEnd_IdleTime;
		ToolTipText.SetToolTip(BattleEnd_IdleTime, ConfigurationNotifierResources.BattleEnd_IdleTimeToolTip);
		AnchorageRepairNotificationLevel.Items.Clear();
		AnchorageRepairNotificationLevel.Items.AddRange(new object[]
		{
			ConfigurationNotifierResources.AnchorageRepairNotificationLevel_Always,
			ConfigurationNotifierResources.AnchorageRepairNotificationLevel_AkashiFlagship,
			ConfigurationNotifierResources.AnchorageRepairNotificationLevel_ShipNeededRepair,
			ConfigurationNotifierResources.AnchorageRepairNotificationLevel_Preset
		});
		ToolTipText.SetToolTip(AnchorageRepairNotificationLevel, ConfigurationNotifierResources.AnchorageRepairNotificationLevelToolTip);

		Text = NotifyRes.Title;
	}

	private void GroupSound_DragEnter(object sender, DragEventArgs e)
	{

		if (e.Data?.GetDataPresent(DataFormats.FileDrop) is true)
		{
			e.Effect = DragDropEffects.Copy;
		}
		else
		{
			e.Effect = DragDropEffects.None;
		}

	}

	private void GroupSound_DragDrop(object sender, DragEventArgs e)
	{
		if (e.Data is null) return;
		SoundPath.Text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
	}

	private void GroupImage_DragEnter(object sender, DragEventArgs e)
	{

		if (e.Data?.GetDataPresent(DataFormats.FileDrop) is true)
		{
			e.Effect = DragDropEffects.Copy;
		}
		else
		{
			e.Effect = DragDropEffects.None;
		}

	}

	private void GroupImage_DragDrop(object sender, DragEventArgs e)
	{
		if (e.Data is null) return;
		ImagePath.Text = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
	}



	private void SoundPath_TextChanged(object sender, EventArgs e)
	{

		SoundChanged = true;
	}

	private void ImagePath_TextChanged(object sender, EventArgs e)
	{

		ImageChanged = true;
	}

	private void SoundPathSearch_Click(object sender, EventArgs e)
	{

		if (SoundPath.Text != "")
		{
			try
			{
				DialogOpenSound.InitialDirectory = System.IO.Path.GetDirectoryName(SoundPath.Text);

			}
			catch (Exception) 
			{
				// do not throw to avoid issues
			}
		}

		if (DialogOpenSound.ShowDialog(App.Current!.MainWindow!) == System.Windows.Forms.DialogResult.OK)
		{
			SoundPath.Text = DialogOpenSound.FileName;
		}

	}

	private void ImagePathSearch_Click(object sender, EventArgs e)
	{

		if (ImagePath.Text != "")
		{
			try
			{
				DialogOpenImage.InitialDirectory = System.IO.Path.GetDirectoryName(ImagePath.Text);

			}
			catch (Exception)
			{
				// do not throw to avoid issues
			}
		}

		if (DialogOpenImage.ShowDialog(App.Current!.MainWindow!) == System.Windows.Forms.DialogResult.OK)
		{
			ImagePath.Text = DialogOpenImage.FileName;
		}
	}


	private void ForeColorSelect_Click(object sender, EventArgs e)
	{

		DialogColor.Color = ForeColorPreview.ForeColor;
		if (DialogColor.ShowDialog(App.Current!.MainWindow!) == System.Windows.Forms.DialogResult.OK)
		{
			ForeColorPreview.ForeColor = DialogColor.Color;
		}
	}

	private void BackColorSelect_Click(object sender, EventArgs e)
	{

		DialogColor.Color = BackColorPreview.ForeColor;
		if (DialogColor.ShowDialog(App.Current!.MainWindow!) == System.Windows.Forms.DialogResult.OK)
		{
			BackColorPreview.ForeColor = DialogColor.Color;
		}
	}


	private void ForeColorPreview_ForeColorChanged(object sender, EventArgs e)
	{

		if (ForeColorPreview.ForeColor.GetBrightness() >= 0.5)
		{
			ForeColorPreview.BackColor = Color.Black;
		}
		else
		{
			ForeColorPreview.BackColor = Color.White;
		}
	}

	private void BackColorPreview_ForeColorChanged(object sender, EventArgs e)
	{

		if (BackColorPreview.ForeColor.GetBrightness() >= 0.5)
		{
			BackColorPreview.BackColor = Color.Black;
		}
		else
		{
			BackColorPreview.BackColor = Color.White;
		}
	}



	private void ButtonOK_Click(object sender, EventArgs e)
	{

		if (!SetConfiguration()) return;

		DialogResult = System.Windows.Forms.DialogResult.OK;
	}

	private void ButtonCancel_Click(object sender, EventArgs e)
	{

		DialogResult = System.Windows.Forms.DialogResult.Cancel;
	}


	private bool SetConfiguration()
	{
		if (SoundChanged && !Notifier.LoadSound(SoundPath.Text) && PlaysSound.Checked)
		{
			MessageBox.Show(NotifyRes.FailedLoadSound, ConfigurationNotifierResources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
			return false;
		}
		if (ImageChanged && !Notifier.DialogData.LoadImage(ImagePath.Text) && DrawsImage.Checked)
		{
			MessageBox.Show(NotifyRes.FailedLoadImage, ConfigurationNotifierResources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
			return false;
		}


		//set configuration
		Notifier.IsEnabled = IsEnabled.Checked;

		Notifier.PlaysSound = PlaysSound.Checked;
		Notifier.DialogData.DrawsImage = DrawsImage.Checked;
		Notifier.SoundVolume = (int)SoundVolume.Value;
		Notifier.LoopsSound = LoopsSound.Checked;

		Notifier.ShowsDialog = ShowsDialog.Checked;
		Notifier.DialogData.TopMost = TopMostFlag.Checked;
		Notifier.DialogData.Alignment = (NotifierDialogAlignment)Alignment.SelectedIndex;
		Notifier.DialogData.Location = new Point((int)LocationX.Value, (int)LocationY.Value);
		Notifier.DialogData.DrawsMessage = DrawsMessage.Checked;
		Notifier.DialogData.HasFormBorder = HasFormBorder.Checked;
		Notifier.AccelInterval = (int)(AccelInterval.Value * 1000);
		Notifier.DialogData.ClosingInterval = (int)(ClosingInterval.Value * 1000);

		int flag = 0;
		for (int i = 0; i < (int)NotifierDialogClickFlags.HighestBit; i++)
			flag |= (CloseList.GetItemChecked(i) ? 1 : 0) << i;
		Notifier.DialogData.ClickFlag = (NotifierDialogClickFlags)flag;

		Notifier.DialogData.CloseOnMouseMove = CloseList.GetItemChecked((int)NotifierDialogClickFlags.HighestBit);
		Notifier.DialogData.ForeColor = ForeColorPreview.ForeColor;
		Notifier.DialogData.BackColor = BackColorPreview.ForeColor;
		Notifier.DialogData.ShowWithActivation = ShowWithActivation.Checked;

		if (Notifier is NotifierDamage ndmg)
		{
			ndmg.NotifiesBefore = NotifiesBefore.Checked;
			ndmg.NotifiesNow = NotifiesNow.Checked;
			ndmg.NotifiesAfter = NotifiesAfter.Checked;
			ndmg.ContainsNotLockedShip = ContainsNotLockedShip.Checked;
			ndmg.ContainsSafeShip = ContainsSafeShip.Checked;
			ndmg.ContainsFlagship = ContainsFlagship.Checked;
			ndmg.LevelBorder = (int)LevelBorder.Value;
			ndmg.NotifiesAtEndpoint = NotifiesAtEndpoint.Checked;
		}

		if (Notifier is NotifierAnchorageRepair nanc)
		{
			nanc.NotificationLevel = AnchorageRepairNotificationLevel.SelectedIndex;
		}

		if (Notifier is NotifierBaseAirCorps nbac)
		{
			nbac.NotifiesNotSupplied = BaseAirCorps_NotSupplied.Checked;
			nbac.NotifiesTired = BaseAirCorps_Tired.Checked;
			nbac.NotifiesNotOrganized = BaseAirCorps_NotOrganized.Checked;
			nbac.NotifiesStandby = BaseAirCorps_Standby.Checked;
			nbac.NotifiesRetreat = BaseAirCorps_Retreat.Checked;
			nbac.NotifiesRest = BaseAirCorps_Rest.Checked;
			nbac.NotifiesNormalMap = BaseAirCorps_NormalMap.Checked;
			nbac.NotifiesEventMap = BaseAirCorps_EventMap.Checked;
			nbac.NotifiesSquadronRelocation = BaseAirCorps_SquadronRelocation.Checked;
			nbac.NotifiesEquipmentRelocation = BaseAirCorps_EquipmentRelocation.Checked;
		}

		if (Notifier is NotifierBattleEnd nba)
		{
			nba.Config.IdleTimerEnabled = BattleEnd_IdleTimerEnabled.Checked;
			nba.Config.IdleSeconds = (int)BattleEnd_IdleTime.Value;
		}

		return true;
	}


	private void ButtonTest_Click(object sender, EventArgs e)
	{

		if (!SetConfiguration()) return;

		if (Notifier.DialogData.Alignment == NotifierDialogAlignment.Custom)
		{
			Notifier.DialogData.Message = ConfigurationNotifierResources.TestNotificationCustomPositioning;
			Notifier.Notify((_sender, _e) =>
			{
				if (_sender is DialogNotifier dialog)
				{
					Notifier.DialogData.Location = dialog.Location;
					LocationX.Value = dialog.Location.X;
					LocationY.Value = dialog.Location.Y;
				}
			});
		}
		else
		{
			Notifier.DialogData.Message = ConfigurationNotifierResources.TestNotification;
			Notifier.Notify();
		}
	}

	private void SoundPathDirectorize_Click(object sender, EventArgs e)
	{
		if (!string.IsNullOrWhiteSpace(SoundPath.Text))
		{
			try
			{
				SoundPath.Text = System.IO.Path.GetDirectoryName(SoundPath.Text);
			}
			catch (Exception)
			{
				// *ぷちっ*
			}
		}
	}


}
