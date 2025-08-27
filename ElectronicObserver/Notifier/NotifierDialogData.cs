using System;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace ElectronicObserver.Notifier;

public class NotifierDialogData
{

	/// <summary>
	/// 通知用の画像
	/// </summary>
	public Bitmap Image { get; protected set; }

	/// <summary>
	/// 画像のパス
	/// </summary>
	public string ImagePath { get; private set; }

	/// <summary>
	/// 通知メッセージ
	/// </summary>
	public string Message { get; set; }

	/// <summary>
	/// 通知のタイトル
	/// </summary>
	public string Title { get; set; }


	/// <summary>
	/// 画像を描画するか
	/// </summary>
	public bool DrawsImage { get; set; }

	/// <summary>
	/// 通知メッセージを描画するか
	/// </summary>
	public bool DrawsMessage { get; set; }


	/// <summary>
	/// 自動で閉じるまでの時間(ミリ秒, 0=閉じない)
	/// </summary>
	public int ClosingInterval { get; set; }


	/// <summary>
	/// マウスポインタがフォーム上を動いたとき自動的に閉じる
	/// </summary>
	public bool CloseOnMouseMove { get; set; }

	/// <summary>
	/// 閉じるマウスボタンのフラグ
	/// </summary>
	public NotifierDialogClickFlags ClickFlag { get; set; }


	/// <summary>
	/// 通知ダイアログの出現位置設定
	/// </summary>
	public NotifierDialogAlignment Alignment { get; set; }

	/// <summary>
	/// 通知ダイアログの出現位置
	/// </summary>
	public Point Location { get; set; }

	/// <summary>
	/// 通知ダイアログに枠をつけるか
	/// </summary>
	public bool HasFormBorder { get; set; }

	/// <summary>
	/// 最前面に表示する
	/// </summary>
	public bool TopMost { get; set; }

	/// <summary>
	/// 表示時にアクティベートするか
	/// </summary>
	public bool ShowWithActivation { get; set; }

	/// <summary>
	/// 前景色
	/// </summary>
	public Color ForeColor { get; set; }

	/// <summary>
	/// 背景色
	/// </summary>
	public Color BackColor { get; set; }



	/// <summary>
	/// イベント発動時に全ウィンドウを閉じる
	/// </summary>
	public event EventHandler CloseAll = delegate { };




	public NotifierDialogData()
	{

		Image = null;

	}

	public NotifierDialogData(Utility.Configuration.ConfigurationData.ConfigNotifierBase config)
	{

		Image = null;
		ImagePath = "";
		if (config.DrawsImage && config.ImagePath != null && config.ImagePath != "")
			LoadImage(config.ImagePath);
		DrawsImage = config.DrawsImage;
		DrawsMessage = config.DrawsMessage;
		ClosingInterval = config.ClosingInterval;
		CloseOnMouseMove = config.CloseOnMouseMove;
		ClickFlag = config.ClickFlag;
		Alignment = config.Alignment;
		Location = config.Location;
		HasFormBorder = config.HasFormBorder;
		TopMost = config.TopMost;
		ShowWithActivation = config.ShowWithActivation;
		ForeColor = config.ForeColor;
		BackColor = config.BackColor;
	}


	public NotifierDialogData Clone()
	{
		return (NotifierDialogData)MemberwiseClone();
	}


	#region 通知画像

	/// <summary>
	/// 通知画像を読み込みます。
	/// </summary>
	/// <param name="path">画像ファイルへのパス。</param>
	/// <returns>成功すれば true 、失敗すれば false を返します。</returns>
	public bool LoadImage(string path)
	{

		try
		{

			DisposeImage();
			using (System.IO.FileStream stream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read))
			{
				Image = new Bitmap(stream);
				ImagePath = path;
			}

			return true;

		}
		catch (Exception ex)
		{

			Utility.ErrorReporter.SendErrorReport(ex, string.Format(NotifierRes.FailedToLoadImage, path));
			DisposeImage();

		}

		return false;
	}

	/// <summary>
	/// 通知画像を破棄します。
	/// </summary>
	public void DisposeImage()
	{
		if (Image != null)
		{
			Image.Dispose();
			Image = null;
		}
		ImagePath = "";
	}

	#endregion


	public void ApplyToConfiguration(Utility.Configuration.ConfigurationData.ConfigNotifierBase config)
	{

		config.ImagePath = ImagePath;
		config.DrawsImage = DrawsImage;
		config.DrawsMessage = DrawsMessage;
		config.ClosingInterval = ClosingInterval;
		config.CloseOnMouseMove = CloseOnMouseMove;
		config.ClickFlag = ClickFlag;
		config.Alignment = Alignment;
		config.Location = Location;
		config.HasFormBorder = HasFormBorder;
		config.TopMost = TopMost;
		config.ShowWithActivation = ShowWithActivation;
		config.ForeColor = ForeColor;
		config.BackColor = BackColor;

	}


	public void OnCloseAll()
	{
		CloseAll(this, new EventArgs());
	}

}


/// <summary>
/// 通知ダイアログの出現位置を表します。
/// </summary>
public enum NotifierDialogAlignment
{

	/// <summary>未設定です。</summary>
	[Display(ResourceType = typeof(NotifyRes), Name = "AlignUnset")]
	NotSet,

	/// <summary>左上に配置されます。</summary>
	[Display(ResourceType = typeof(NotifyRes), Name = "AlignTopLeft")]
	TopLeft,

	/// <summary>上中央に配置されます。</summary>
	[Display(ResourceType = typeof(NotifyRes), Name = "AlignTop")]
	TopCenter,

	/// <summary>右上に配置されます。</summary>
	[Display(ResourceType = typeof(NotifyRes), Name = "AlignTopRight")]
	TopRight,

	/// <summary>左中央に配置されます。</summary>
	[Display(ResourceType = typeof(NotifyRes), Name = "AlignLeft")]
	MiddleLeft,

	/// <summary>中央に配置されます。</summary>
	[Display(ResourceType = typeof(NotifyRes), Name = "AlignCenter")]
	MiddleCenter,

	/// <summary>右中央に配置されます。</summary>
	[Display(ResourceType = typeof(NotifyRes), Name = "AlignRight")]
	MiddleRight,

	/// <summary>左下に配置されます。</summary>
	[Display(ResourceType = typeof(NotifyRes), Name = "AlignBottomLeft")]
	BottomLeft,

	/// <summary>下中央に配置されます。</summary>
	[Display(ResourceType = typeof(NotifyRes), Name = "AlignBottom")]
	BottomCenter,

	/// <summary>右下に配置されます。</summary>
	[Display(ResourceType = typeof(NotifyRes), Name = "AlignBottomRight")]
	BottomRight,

	/// <summary>ユーザーが設定した座標に配置されます。</summary>
	[Display(ResourceType = typeof(NotifyRes), Name = "AlignManualAbs")]
	Custom,

	/// <summary>ユーザーが設定した座標に配置されます(ブラウザウィンドウの中心を原点とする相対座標)。</summary>
	[Display(ResourceType = typeof(NotifyRes), Name = "AlignManualRel")]
	CustomRelative,
}


/// <summary>
/// 通知ダイアログのクリック種別を示します。
/// </summary>
[Flags]
public enum NotifierDialogClickFlags
{

	None = 0x0,
	[Display(ResourceType = typeof(NotifyRes), Name = "LeftClick")]
	Left = 0x1,
	[Display(ResourceType = typeof(NotifyRes), Name = "LeftDoubleClick")]
	LeftDouble = 0x2,
	[Display(ResourceType = typeof(NotifyRes), Name = "RightClick")]
	Right = 0x4,
	[Display(ResourceType = typeof(NotifyRes), Name = "RightDoubleClick")]
	RightDouble = 0x8,
	[Display(ResourceType = typeof(NotifyRes), Name = "MiddleClick")]
	Middle = 0x10,
	[Display(ResourceType = typeof(NotifyRes), Name = "MiddleDoubleClick")]
	MiddleDouble = 0x20,
	[Display(ResourceType = typeof(NotifyRes), Name = "MouseOver")]
	MouseOver = 0x40,

	/// <summary>最高位ビット</summary>
	HighestBit = 6,
}
