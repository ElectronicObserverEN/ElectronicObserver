using MessagePack;

namespace BrowserLibCore;

[MessagePackObject]
public class BrowserConfiguration
{
	/// <summary>
	/// ブラウザの拡大率 10-1000(%)
	/// </summary>
	[Key(0)]
	public double ZoomRate { get; set; }

	/// <summary>
	/// ブラウザをウィンドウサイズに合わせる
	/// </summary>
	[Key(1)]
	public bool ZoomFit { get; set; }

	/// <summary>
	/// ログインページのURL
	/// </summary>
	[Key(2)]
	public string LogInPageURL { get; set; }

	/// <summary>
	/// ブラウザを有効にするか
	/// </summary>
	[Key(3)]
	public bool IsEnabled { get; set; }

	/// <summary>
	/// スクリーンショットの保存先フォルダ
	/// </summary>
	[Key(4)]
	public string ScreenShotPath { get; set; }

	/// <summary>
	/// スクリーンショットのフォーマット
	/// 1=jpeg, 2=png
	/// </summary>
	[Key(5)]
	public int ScreenShotFormat { get; set; }

	/// <summary>
	/// スクリーンショットの保存モード
	/// </summary>
	[Key(6)]
	public int ScreenShotSaveMode { get; set; }

	/// <summary>
	/// 適用するスタイルシート
	/// </summary>
	[Key(7)]
	public string StyleSheet { get; set; }

	/// <summary>
	/// スクロール可能かどうか
	/// </summary>
	[Key(8)]
	public bool IsScrollable { get; set; }

	/// <summary>
	/// スタイルシートを適用するか
	/// </summary>
	[Key(9)]
	public bool AppliesStyleSheet { get; set; }

	/// <summary>
	/// DMMによるページ更新ダイアログを表示するか
	/// </summary>
	[Key(10)]
	public bool IsDMMreloadDialogDestroyable { get; set; }

	/// <summary>
	/// スクリーンショットにおいて、Twitter の画像圧縮を回避するか
	/// </summary>
	[Key(11)]
	public bool AvoidTwitterDeterioration { get; set; }

	/// <summary>
	/// ツールメニューの配置
	/// </summary>
	[Key(12)]
	public int ToolMenuDockStyle { get; set; }

	/// <summary>
	/// ツールメニューの可視性
	/// </summary>
	[Key(13)]
	public bool IsToolMenuVisible { get; set; }

	/// <summary>
	/// 再読み込み時に確認ダイアログを入れるか
	/// </summary>
	[Key(14)]
	public bool ConfirmAtRefresh { get; set; }

	/// <summary>
	/// 現在の音量
	/// </summary>
	[Key(15)]
	public float Volume { get; set; }

	/// <summary>
	/// ミュートかどうか
	/// </summary>
	[Key(16)]
	public bool IsMute { get; set; }

	[Key(17)]
	public int BackColor { get; set; }

	/// <summary>
	/// ハードウェアアクセラレーションを有効にするか
	/// </summary>
	[Key(18)]
	public bool HardwareAccelerationEnabled { get; set; }

	/// <summary>
	/// 描画バッファを保持するか
	/// </summary>
	[Key(19)]
	public bool PreserveDrawingBuffer { get; set; }

	/// <summary>
	/// カラープロファイルを sRGB 固定にするか
	/// </summary>
	[Key(20)]
	public bool ForceColorProfile { get; set; }

	/// <summary>
	/// ブラウザのログを保存するか
	/// </summary>
	[Key(21)]
	public bool SavesBrowserLog { get; set; }

	/// <summary>
	/// デバッグメニューを有効にするか
	/// </summary>
	[Key(22)]
	public bool EnableDebugMenu { get; set; }

	[Key(23)]
	public bool UseVulkanWorkaround { get; set; }

	[Key(24)]
	public bool IsBrowserContextMenuEnabled { get; set; }

	[Key(25)]
	public string MainFont { get; set; } = null!;

	[Key(26)]
	public bool UseCustomBrowserFont { get; set; }

	[Key(27)]
	public string? BrowserFont { get; set; }

	[Key(28)]
	public bool MatchMainFont { get; set; }

	[Key(29)]
	public ScreenshotMode ScreenshotMode { get; set; }
}
