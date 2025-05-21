using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using BrowserLibCore;
using DynaJson;
using ElectronicObserver.Avalonia.Services;
using ElectronicObserver.Data.DiscordRPC;
using ElectronicObserver.Resource.Record;
using ElectronicObserver.Utility.Mathematics;
using ElectronicObserver.Utility.Storage;
using ElectronicObserver.Window.Control;
using ElectronicObserverTypes;

namespace ElectronicObserver.Utility;

public sealed class Configuration
{


	private static readonly Configuration instance = new Configuration();

	public static Configuration Instance => instance;


	private const string SaveFileName = @"Settings\Configuration.xml";


	public delegate void ConfigurationChangedEventHandler();
	public event ConfigurationChangedEventHandler ConfigurationChanged = delegate { };


	[DataContract(Name = "Configuration")]
	public sealed class ConfigurationData : DataStorage
	{

		public class ConfigPartBase
		{
			//reserved
		}


		/// <summary>
		/// 通信の設定を扱います。
		/// </summary>
		public class ConfigConnection : ConfigPartBase, IConfigurationConnection
		{

			/// <summary>
			/// ポート番号
			/// </summary>
			public ushort Port { get; set; }

			/// <summary>
			/// 通信内容を保存するか
			/// </summary>
			public bool SaveReceivedData { get; set; }

			/// <summary>
			/// 通信内容保存：保存先
			/// </summary>
			public string SaveDataPath { get; set; }

			/// <summary>
			/// 通信内容保存：Requestを保存するか
			/// </summary>
			public bool SaveRequest { get; set; }

			/// <summary>
			/// 通信内容保存：Responseを保存するか
			/// </summary>
			public bool SaveResponse { get; set; }

			/// <summary>
			/// 通信内容保存：その他ファイルを保存するか
			/// </summary>
			public bool SaveOtherFile { get; set; }

			/// <summary>
			/// 通信内容保存：バージョンを追加するか
			/// </summary>
			public bool ApplyVersion { get; set; }


			/// <summary>
			/// システムプロキシに登録するか
			/// </summary>
			public bool RegisterAsSystemProxy { get; set; }

			/// <summary>
			/// 上流プロキシを利用するか
			/// </summary>
			public bool UseUpstreamProxy { get; set; }

			/// <summary>
			/// 上流プロキシのポート番号
			/// </summary>
			public ushort UpstreamProxyPort { get; set; }

			/// <summary>
			/// 上流プロキシのアドレス
			/// </summary>
			public string UpstreamProxyAddress { get; set; }

			/// <summary>
			/// システムプロキシを利用するか
			/// </summary>
			public bool UseSystemProxy { get; set; }

			/// <summary>
			/// 下流プロキシ設定
			/// 空なら他の設定から自動生成する
			/// </summary>
			public string DownstreamProxy { get; set; }

			public ConfigConnection()
			{

				Port = 40620;
				SaveReceivedData = false;
				SaveDataPath = @"KCAPI";
				SaveRequest = false;
				SaveResponse = true;
				SaveOtherFile = false;
				ApplyVersion = false;
				RegisterAsSystemProxy = false;
				UseUpstreamProxy = false;
				UpstreamProxyPort = 0;
				UpstreamProxyAddress = "127.0.0.1";
				UseSystemProxy = false;
				DownstreamProxy = "";
			}

		}
		/// <summary>通信</summary>
		[DataMember]
		public ConfigConnection Connection { get; private set; }


		public class ConfigUI : ConfigPartBase
		{

			/// <summary>
			/// メインフォント
			/// </summary>
			public SerializableFont MainFont { get; set; }

			/// <summary>
			/// サブフォント
			/// </summary>
			public SerializableFont SubFont { get; set; }

			public string Culture { get; set; }

			/// <summary>
			/// Whether to use Japanese or English ship names
			/// </summary>
			public bool JapaneseShipName { get; set; }

			/// <summary>
			/// Whether to use Japanese or English ship names
			/// </summary>
			public bool JapaneseShipType { get; set; }

			/// <summary>
			/// Whether to use Japanese or English equipment names
			/// </summary>
			public bool JapaneseEquipmentName { get; set; }

			/// <summary>
			/// Whether to use Japanese or English equipment names
			/// </summary>
			public bool JapaneseEquipmentType { get; set; }

			/// <summary>
			/// Expeditions, sortie maps and quests
			/// </summary>
			public bool DisableOtherTranslations { get; set; }

			/// <summary>
			/// Use the real integer ID when true, translate to letter when false
			/// </summary>
			public bool UseOriginalNodeId { get; set; }

			// ColorMode
			public int ThemeMode { get; set; }

			// ThemeID
			public int ThemeID { get; set; }

			// 明石 ToolTip 每 HP 耗时最大显示数量
			public int MaxAkashiPerHP { get; set; }

			// 修理时间偏移值 ( 暂定 +30s, 待验证 )
			public int DockingUnitTimeOffset { get; set; }

			// 使用旧版熟练度图标
			public bool UseOldAircraftLevelIcons { get; set; }

			// 日志窗口自动换行
			public bool TextWrapInLogWindow { get; set; }

			// 日志窗口精简模式
			public bool CompactModeLogWindow { get; set; }

			// 日志窗口反向滚动 ( 新日志在顶端 )
			public bool InvertedLogWindow { get; set; }


			// 司令部各资源储量过低警告值
			// -1: 自然恢复上限 ( 桶无效 ) | 3000: 小于 3000
			public int HqResLowAlertFuel { get; set; }
			public int HqResLowAlertAmmo { get; set; }
			public int HqResLowAlertSteel { get; set; }
			public int HqResLowAlertBauxite { get; set; }
			public int HqResLowAlertBucket { get; set; }

			/// <summary>
			/// 舰队编成 - 新增排序列 ( Lv 序, 舰种序, NEW 序 )
			/// | true: 开启此功能 ( 默认 ) | false: 禁用此功能 ( 若过分拖慢可关闭 )
			/// </summary>
			public bool AllowSortIndexing { get; set; }

			[IgnoreDataMember]
			private bool _barColorMorphing;

			/// <summary>
			/// HPバーの色を滑らかに変化させるか
			/// </summary>
			public bool BarColorMorphing
			{
				get
				{
					SetBarColorScheme();
					return _barColorMorphing;
				}
				set
				{
					_barColorMorphing = value;
					SetBarColorScheme();
				}
			}

			public void SetBarColorScheme()
			{
				if (BarColorSchemes == null) return;
				if (!_barColorMorphing)
					BarColorScheme = new List<SerializableColor>(BarColorSchemes[0]);
				else
					BarColorScheme = new List<SerializableColor>(BarColorSchemes[1]);
			}

			/// <summary>
			/// HPバーのカラーリング
			/// </summary>
			public List<SerializableColor> BarColorScheme { get; private set; }

			#region - UI Colors -

			// 数值条 ( 耐久、燃料、弹药... ) 颜色
			[IgnoreDataMember]
			public List<SerializableColor>[] BarColorSchemes { get; set; }
			// 面板颜色
			[IgnoreDataMember]
			public Color ForeColor { get; set; }
			[IgnoreDataMember]
			public Color BackColor { get; set; }
			[IgnoreDataMember]
			public Color SubForeColor { get; set; }
			[IgnoreDataMember]
			public Color SubBackColor { get; set; }
			[IgnoreDataMember]
			public Pen SubBackColorPen { get; set; }
			// 状态栏颜色
			[IgnoreDataMember]
			public Color StatusBarForeColor { get; set; }
			[IgnoreDataMember]
			public Color StatusBarBackColor { get; set; }
			// 标签页颜色 ( DockPanelSuite )
			[IgnoreDataMember]
			public string[] DockPanelSuiteStyles { get; set; }
			// 基本颜色
			[IgnoreDataMember]
			public Color Color_Red { get; set; }
			[IgnoreDataMember]
			public Color Color_Orange { get; set; }
			[IgnoreDataMember]
			public Color Color_Yellow { get; set; }
			[IgnoreDataMember]
			public Color Color_Green { get; set; }
			[IgnoreDataMember]
			public Color Color_Cyan { get; set; }
			[IgnoreDataMember]
			public Color Color_Blue { get; set; }
			[IgnoreDataMember]
			public Color Color_Magenta { get; set; }
			[IgnoreDataMember]
			public Color Color_Violet { get; set; }

			// 视图 - 舰队
			[IgnoreDataMember] // 入渠计时器文字色
			public Color Fleet_ColorRepairTimerText { get; set; }
			[IgnoreDataMember] // 疲劳状态文字色
			public Color Fleet_ColorConditionText { get; set; }
			[IgnoreDataMember] // 严重疲劳
			public Color Fleet_ColorConditionVeryTired { get; set; }
			[IgnoreDataMember] // 中等疲劳
			public Color Fleet_ColorConditionTired { get; set; }
			[IgnoreDataMember] // 轻微疲劳
			public Color Fleet_ColorConditionLittleTired { get; set; }
			[IgnoreDataMember] // 战意高扬
			public Color Fleet_ColorConditionSparkle { get; set; }
			[IgnoreDataMember] // 装备改修值
			public Color Fleet_EquipmentLevelColor { get; set; }
			[IgnoreDataMember]
			public Color Fleet_RemodelReadyColor { get; set; }

			// 视图 - 舰队一览
			[IgnoreDataMember] // 大破 / 大破进击文字色
			public Color FleetOverview_ShipDamagedFG { get; set; }
			[IgnoreDataMember] // 大破 / 大破进击背景色
			public Color FleetOverview_ShipDamagedBG { get; set; }
			[IgnoreDataMember] // 远征返回文字色
			public Color FleetOverview_ExpeditionOverFG { get; set; }
			[IgnoreDataMember] // 远征返回背景色
			public Color FleetOverview_ExpeditionOverBG { get; set; }
			[IgnoreDataMember] // 疲劳恢复文字色
			public Color FleetOverview_TiredRecoveredFG { get; set; }
			[IgnoreDataMember] // 疲劳恢复背景色
			public Color FleetOverview_TiredRecoveredBG { get; set; }
			[IgnoreDataMember] // 未远征提醒文字色
			public Color FleetOverview_AlertNotInExpeditionFG { get; set; }
			[IgnoreDataMember] // 未远征提醒背景色
			public Color FleetOverview_AlertNotInExpeditionBG { get; set; }

			// 视图 - 入渠
			[IgnoreDataMember] // 修理完成文字色
			public Color Dock_RepairFinishedFG { get; set; }
			[IgnoreDataMember] // 修理完成背景色
			public Color Dock_RepairFinishedBG { get; set; }

			// 视图 - 工厂
			[IgnoreDataMember] // 建造完成文字色
			public Color Arsenal_BuildCompleteFG { get; set; }
			[IgnoreDataMember] // 建造完成背景色
			public Color Arsenal_BuildCompleteBG { get; set; }

			// 视图 - 司令部
			[IgnoreDataMember] // 资源超过自然恢复上限文字色
			public Color Headquarters_ResourceOverFG { get; set; }
			[IgnoreDataMember] // 资源超过自然恢复上限背景色
			public Color Headquarters_ResourceOverBG { get; set; }
			[IgnoreDataMember] // 剩余船位、装备位不满足活动图出击要求时闪烁文字色
			public Color Headquarters_ShipCountOverFG { get; set; }
			[IgnoreDataMember] // 剩余船位、装备位不满足活动图出击要求时闪烁背景色
			public Color Headquarters_ShipCountOverBG { get; set; }
			[IgnoreDataMember] // 资材达到 3,000 个时文字色
			public Color Headquarters_MaterialMaxFG { get; set; }
			[IgnoreDataMember] // 资材达到 3,000 个时背景色
			public Color Headquarters_MaterialMaxBG { get; set; }
			[IgnoreDataMember] // 家具币达到 200,000 个时文字色
			public Color Headquarters_CoinMaxFG { get; set; }
			[IgnoreDataMember] // 家具币达到 200,000 个时背景色
			public Color Headquarters_CoinMaxBG { get; set; }
			[IgnoreDataMember] // 资源储量低于警告值文字色
			public Color Headquarters_ResourceLowFG { get; set; }
			[IgnoreDataMember] // 资源储量低于警告值背景色
			public Color Headquarters_ResourceLowBG { get; set; }
			[IgnoreDataMember] // 资源储量达到 300,000 时文字色
			public Color Headquarters_ResourceMaxFG { get; set; }
			[IgnoreDataMember] // 资源储量达到 300,000 时背景色
			public Color Headquarters_ResourceMaxBG { get; set; }

			// 视图 - 任务
			[IgnoreDataMember] // 任务类型文字色
			public Color Quest_TypeFG { get; set; }
			[IgnoreDataMember] // 编成
			public Color Quest_Type1Color { get; set; }
			[IgnoreDataMember] // 出击
			public Color Quest_Type2Color { get; set; }
			[IgnoreDataMember] // 演习
			public Color Quest_Type3Color { get; set; }
			[IgnoreDataMember] // 远征
			public Color Quest_Type4Color { get; set; }
			[IgnoreDataMember] // 补给、入渠
			public Color Quest_Type5Color { get; set; }
			[IgnoreDataMember] // 工厂
			public Color Quest_Type6Color { get; set; }
			[IgnoreDataMember] // 改装
			public Color Quest_Type7Color { get; set; }
			[IgnoreDataMember] // 进度 <50%
			public Color Quest_ColorProcessLT50 { get; set; }
			[IgnoreDataMember] // 进度 <80%
			public Color Quest_ColorProcessLT80 { get; set; }
			[IgnoreDataMember] // 进度 <100%
			public Color Quest_ColorProcessLT100 { get; set; }
			[IgnoreDataMember] // 进度 100%
			public Color Quest_ColorProcessDefault { get; set; }

			// 视图 - 罗盘
			[IgnoreDataMember] // 敌舰名 - elite
			public Color Compass_ShipNameColor2 { get; set; }
			[IgnoreDataMember] // 敌舰名 - flagship
			public Color Compass_ShipNameColor3 { get; set; }
			[IgnoreDataMember] // 敌舰名 - 鬼 / 改 flagship / 后期型
			public Color Compass_ShipNameColor4 { get; set; }
			[IgnoreDataMember] // 敌舰名 - 姫 / 后期型 elite
			public Color Compass_ShipNameColor5 { get; set; }
			[IgnoreDataMember] // 敌舰名 - 水鬼 / 后期型 flagship
			public Color Compass_ShipNameColor6 { get; set; }
			[IgnoreDataMember] // 敌舰名 - 水姫
			public Color Compass_ShipNameColor7 { get; set; }
			[IgnoreDataMember] // 敌舰名 - 壊
			public Color Compass_ShipNameColorDestroyed { get; set; }
			[IgnoreDataMember] // 事件类型 - 夜战
			public Color Compass_ColorTextEventKind3 { get; set; }
			[IgnoreDataMember] // 事件类型 - 航空战 / 长距离空袭战
			public Color Compass_ColorTextEventKind6 { get; set; }
			[IgnoreDataMember] // 事件类型 - 敌联合舰队
			public Color Compass_ColorTextEventKind5 { get; set; }
			[IgnoreDataMember] // 半透明背景色，当舰载机数量叠加到飞机图标上时背景填充的色块
			public Color Compass_ColoroverlayBrush { get; set; }
			// default: return Color.FromArgb(0xC0, 0xF0, 0xF0, 0xF0);

			// 视图 - 战斗：血条背景色、血条文字色
			[IgnoreDataMember] // MVP
			public Color Battle_ColorHPBarsMVP { get; set; }
			[IgnoreDataMember] // MVP 主文字色
			public Color Battle_ColorTextMVP { get; set; }
			[IgnoreDataMember] // MVP 副文字色
			public Color Battle_ColorTextMVP2 { get; set; }
			[IgnoreDataMember] // 已退避
			public Color Battle_ColorHPBarsEscaped { get; set; }
			[IgnoreDataMember] // 已退避主文字色
			public Color Battle_ColorTextEscaped { get; set; }
			[IgnoreDataMember] // 已退避副文字色
			public Color Battle_ColorTextEscaped2 { get; set; }
			[IgnoreDataMember] // 受损状态 BOSS
			public Color Battle_ColorHPBarsBossDamaged { get; set; }
			[IgnoreDataMember] // 受损状态 BOSS 主文字色
			public Color Battle_ColorTextBossDamaged { get; set; }
			[IgnoreDataMember] // 受损状态 BOSS 副文字色
			public Color Battle_ColorTextBossDamaged2 { get; set; }

			public bool RemoveBarShadow
			{
				get
				{
					switch (ThemeID)
					{
						case 0: return true;
						case 1: return true;
						default: return false;
					}
				}
			}

			#endregion

			/// <summary>
			/// 固定レイアウト(フォントに依存しないレイアウト)を利用するか
			/// </summary>
			public bool IsLayoutFixed;

			/// <summary>
			/// When enabled, you can search for fonts by writing text in the combobox.
			/// When text search is enabled, the combobox input can't be localized.
			/// </summary>
			public bool FontFamilyTextSearch { get; set; }

			public bool UseCustomBrowserFont { get; set; }

			public string? BrowserFontName { get; set; }

			/// <summary>
			/// When enabled, the browser font will be the same as the main font.
			/// </summary>
			public bool MatchMainFont { get; set; }

			public ConfigUI()
			{
				MainFont = new Font("Meiryo UI", 12, FontStyle.Regular, GraphicsUnit.Pixel);
				SubFont = new Font("Meiryo UI", 10, FontStyle.Regular, GraphicsUnit.Pixel);
				ThemeMode = 0;
				ThemeID = 0;
				MaxAkashiPerHP = 5;
				DockingUnitTimeOffset = 30;
				UseOldAircraftLevelIcons = true;
				TextWrapInLogWindow = false;
				CompactModeLogWindow = false;
				InvertedLogWindow = false;
				HqResLowAlertFuel = 0;
				HqResLowAlertAmmo = 0;
				HqResLowAlertSteel = 0;
				HqResLowAlertBauxite = 0;
				HqResLowAlertBucket = 0;
				AllowSortIndexing = true;
				BarColorMorphing = false;
				IsLayoutFixed = true;

				Culture = CultureInfo.CurrentCulture.Name switch
				{
					"ja-JP" => "ja-JP",
					_ => "en-US"
				};
				bool disableTranslations = CultureInfo.CurrentCulture.Name switch
				{
					"ja-JP" => true,
					_ => false
				};

				JapaneseShipName = disableTranslations;
				JapaneseShipType = disableTranslations;
				JapaneseEquipmentName = disableTranslations;
				JapaneseEquipmentType = disableTranslations;
				DisableOtherTranslations = disableTranslations;
				UseOriginalNodeId = false;
				FontFamilyTextSearch = false;
			}
		}
		/// <summary>UI</summary>
		[DataMember]
		public ConfigUI UI { get; private set; }


		/// <summary>
		/// ログの設定を扱います。
		/// </summary>
		public class ConfigLog : ConfigPartBase
		{

			/// <summary>
			/// ログのレベル
			/// </summary>
			public int LogLevel { get; set; }

			/// <summary>
			/// ログを保存するか
			/// </summary>
			public bool SaveLogFlag { get; set; }

			/// <summary>
			/// エラーレポートを保存するか
			/// </summary>
			public bool SaveErrorReport { get; set; }

			/// <summary>
			/// ファイル エンコーディングのID
			/// </summary>
			public int FileEncodingID { get; set; }

			/// <summary>
			/// ファイル エンコーディング
			/// </summary>
			[IgnoreDataMember]
			public Encoding FileEncoding
			{
				get
				{
					// needed for Shift-JIS
					Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
					switch (FileEncodingID)
					{
						case 0:
							return new System.Text.UTF8Encoding(false);
						case 1:
							return new System.Text.UTF8Encoding(true);
						case 2:
							return new System.Text.UnicodeEncoding(false, false);
						case 3:
							return new System.Text.UnicodeEncoding(false, true);
						case 4:
							return Encoding.GetEncoding(932);
						default:
							return new System.Text.UTF8Encoding(false);

					}
				}
			}

			/// <summary>
			/// ネタバレを許可するか
			/// </summary>
			public bool ShowSpoiler { get; set; }

			/// <summary>
			/// プレイ時間
			/// </summary>
			public double PlayTime { get; set; }

			/// <summary>
			/// これ以上の無通信時間があったときプレイ時間にカウントしない
			/// </summary>
			public double PlayTimeIgnoreInterval { get; set; }

			/// <summary>
			/// 戦闘ログを保存するか
			/// </summary>
			public bool SaveBattleLog { get; set; }

			/// <summary>
			/// ログを即時保存するか
			/// </summary>
			public bool SaveLogImmediately { get; set; }


			public ConfigLog()
			{
				LogLevel = 2;
				SaveLogFlag = true;
				SaveErrorReport = true;
				FileEncodingID = 4;
				ShowSpoiler = true;
				PlayTime = 0;
				PlayTimeIgnoreInterval = 10 * 60;
				SaveBattleLog = false;
				SaveLogImmediately = false;
			}

		}
		/// <summary>ログ</summary>
		[DataMember]
		public ConfigLog Log { get; private set; }


		/// <summary>
		/// 動作の設定を扱います。
		/// </summary>
		public class ConfigControl : ConfigPartBase
		{

			/// <summary>
			/// 疲労度ボーダー
			/// </summary>
			public int ConditionBorder { get; set; }

			/// <summary>
			/// レコードを自動保存するか
			/// 0=しない、1=1時間ごと、2=1日ごと, 3=即時
			/// </summary>
			public int RecordAutoSaving { get; set; }

			/// <summary>
			/// システムの音量設定を利用するか
			/// </summary>
			public bool UseSystemVolume { get; set; }

			/// <summary>
			/// 前回終了時の音量
			/// </summary>
			public float LastVolume { get; set; }

			/// <summary>
			/// 前回終了時にミュート状態だったか
			/// </summary>
			public bool LastIsMute { get; set; }

			/// <summary>
			/// 威力表示の基準となる交戦形態
			/// </summary>
			public int PowerEngagementForm { get; set; }

			/// <summary>
			/// 出撃札がない艦娘が出撃したときに警告ダイアログを表示するか
			/// </summary>
			public bool ShowSallyAreaAlertDialog { get; set; }

			/// <summary>
			/// 必要経験値計算：出撃当たりの経験値
			/// </summary>
			public int ExpCheckerExpUnit { get; set; }

			/// <summary>
			/// 遠征に失敗する可能性があるとき警告ダイアログを表示するか
			/// </summary>
			public bool ShowExpeditionAlertDialog { get; set; }

			/// <summary>
			/// Enable Discord RPC
			/// </summary>
			public bool EnableDiscordRPC { get; set; }

			/// <summary>
			/// Discord RPC message to display
			/// Use {{secretary}} to insert secretary name
			/// </summary>
			public string DiscordRPCMessage { get; set; }

			/// <summary>
			/// Set if the Discord rich presence shouuld show your number of first class medals
			/// </summary>
			public bool DiscordRPCShowFCM { get; set; }

			/// <summary>
			/// Set the application ID to use for the discord RPC
			/// </summary>
			public string DiscordRPCApplicationId { get; set; }

			/// <summary>
			/// Repository to use for updates
			/// </summary>
			public Uri UpdateRepoURL { get; set; }

			/// <summary>
			/// Here for backward compatibility
			/// Replaced by <see cref="RpcIconKind"/>
			/// </summary>
			public bool UseFlagshipIconForRPC { get; set; }

			/// <summary>
			/// What kind of icon RPC should use
			/// </summary>
			public RpcIconKind RpcIconKind { get; set; }

			public ShipId? ShipUsedForRpcIcon { get; set; }

			/// <summary>
			/// Here for backward compatibility
			/// Replaced by <see cref="ConfigDataSubmission.SubmitDataToTsunDb"/>
			/// </summary>
			public bool? SubmitDataToTsunDb { get; set; }

			public ConfigControl()
			{
				ConditionBorder = 40;
				RecordAutoSaving = 1;
				UseSystemVolume = true;
				LastVolume = 0.8f;
				LastIsMute = false;
				PowerEngagementForm = 1;
				ShowSallyAreaAlertDialog = true;
				ExpCheckerExpUnit = 2268;
				ShowExpeditionAlertDialog = true;
				EnableDiscordRPC = false;
				DiscordRPCMessage = "Headpatting {{secretary}}";
				DiscordRPCShowFCM = true;
				DiscordRPCApplicationId = "";
				UpdateRepoURL = new Uri("https://raw.githubusercontent.com/ElectronicObserverEN/Data/master/");
				UseFlagshipIconForRPC = false;
				RpcIconKind = RpcIconKind.Default;
				SubmitDataToTsunDb = null;
			}
		}
		/// <summary>動作</summary>
		[DataMember]
		public ConfigControl Control { get; private set; }


		/// <summary>
		/// デバッグの設定を扱います。
		/// </summary>
		public class ConfigDebug : ConfigPartBase
		{

			/// <summary>
			/// デバッグメニューを有効にするか
			/// </summary>
			public bool EnableDebugMenu { get; set; }

			/// <summary>
			/// 起動時にAPIリストをロードするか
			/// </summary>
			public bool LoadAPIListOnLoad { get; set; }

			/// <summary>
			/// APIリストのパス
			/// </summary>
			public string APIListPath { get; set; }

			/// <summary>
			/// Electronic Observer API URL
			/// </summary>
			public string ElectronicObserverApiUrl { get; set; } = "";

			/// <summary>
			/// エラー発生時に警告音を鳴らすか
			/// </summary>
			public bool AlertOnError { get; set; }

			public ConfigDebug()
			{
				EnableDebugMenu = false;
				LoadAPIListOnLoad = false;
				APIListPath = "";
				AlertOnError = false;
			}
		}
		/// <summary>デバッグ</summary>
		[DataMember]
		public ConfigDebug Debug { get; private set; }


		/// <summary>
		/// 起動と終了の設定を扱います。
		/// </summary>
		public class ConfigLife : ConfigPartBase
		{

			/// <summary>
			/// 終了時に確認するか
			/// </summary>
			public bool ConfirmOnClosing { get; set; }

			/// <summary>
			/// 最前面に表示するか
			/// </summary>
			public bool TopMost { get; set; }

			/// <summary>
			/// レイアウトファイルのパス
			/// </summary>
			public string LayoutFilePath { get; set; }

			/// <summary>
			/// 更新情報を取得するか
			/// </summary>
			public bool CheckUpdateInformation { get; set; }

			/// <summary>
			/// ステータスバーを表示するか
			/// </summary>
			public bool ShowStatusBar { get; set; }

			/// <summary>
			/// 時計表示のフォーマット
			/// </summary>
			public int ClockFormat { get; set; }

			/// <summary>
			/// レイアウトをロックするか
			/// </summary>
			public bool LockLayout { get; set; }

			/// <summary>
			/// レイアウトロック中でもフロートウィンドウを閉じられるようにするか
			/// </summary>
			public bool CanCloseFloatWindowInLock { get; set; }

			public string? CsvExportPath { get; set; }

			public ConfigLife()
			{
				ConfirmOnClosing = true;
				TopMost = false;
				LayoutFilePath = @"Settings\WindowLayout.zip";
				CheckUpdateInformation = true;
				ShowStatusBar = true;
				ClockFormat = 0;
				LockLayout = false;
				CanCloseFloatWindowInLock = false;
			}
		}
		/// <summary>起動と終了</summary>
		[DataMember]
		public ConfigLife Life { get; private set; }


		/// <summary>
		/// [工廠]ウィンドウの設定を扱います。
		/// </summary>
		public class ConfigFormArsenal : ConfigPartBase
		{

			/// <summary>
			/// 艦名を表示するか
			/// </summary>
			public bool ShowShipName { get; set; }

			/// <summary>
			/// 完了時に点滅させるか
			/// </summary>
			public bool BlinkAtCompletion { get; set; }

			/// <summary>
			/// 艦名表示の最大幅
			/// </summary>
			public int MaxShipNameWidth { get; set; }

			public ConfigFormArsenal()
			{
				ShowShipName = true;
				BlinkAtCompletion = true;
				MaxShipNameWidth = 60;
			}
		}
		/// <summary>[工廠]ウィンドウ</summary>
		[DataMember]
		public ConfigFormArsenal FormArsenal { get; private set; }


		/// <summary>
		/// [入渠]ウィンドウの設定を扱います。
		/// </summary>
		public class ConfigFormDock : ConfigPartBase
		{

			/// <summary>
			/// 完了時に点滅させるか
			/// </summary>
			public bool BlinkAtCompletion { get; set; }

			/// <summary>
			/// 艦名表示の最大幅
			/// </summary>
			public int MaxShipNameWidth { get; set; }

			public ConfigFormDock()
			{
				BlinkAtCompletion = true;
				MaxShipNameWidth = 64;
			}
		}
		/// <summary>[入渠]ウィンドウ</summary>
		[DataMember]
		public ConfigFormDock FormDock { get; private set; }


		/// <summary>
		/// [司令部]ウィンドウの設定を扱います。
		/// </summary>
		public class ConfigFormHeadquarters : ConfigPartBase
		{

			/// <summary>
			/// 艦船/装備が満タンの時点滅するか
			/// </summary>
			public bool BlinkAtMaximum { get; set; }


			/// <summary>
			/// 項目の可視/不可視設定
			/// </summary>
			public SerializableList<bool> Visibility { get; set; }

			/// <summary>
			/// 任意アイテム表示のアイテムID
			/// </summary>
			public int DisplayUseItemID { get; set; }

			/// <summary>
			/// Workaround for WPF not properly calculating content size in WrapPanel
			/// </summary>
			public int WrappingOffset { get; set; }

			public ConfigFormHeadquarters()
			{
				BlinkAtMaximum = true;
				Visibility = null;      // フォーム側で設定します
				DisplayUseItemID = 68;  // 秋刀魚
			}
		}
		/// <summary>[司令部]ウィンドウ</summary>
		[DataMember]
		public ConfigFormHeadquarters FormHeadquarters { get; private set; }


		/// <summary>
		/// [艦隊]ウィンドウの設定を扱います。
		/// </summary>
		public class ConfigFormFleet : ConfigPartBase
		{

			/// <summary>
			/// 艦載機を表示するか
			/// </summary>
			public bool ShowAircraft { get; set; }

			/// <summary>
			/// 索敵式の計算方法
			/// </summary>
			public int SearchingAbilityMethod { get; set; }

			/// <summary>
			/// スクロール可能か
			/// </summary>
			public bool IsScrollable { get; set; }

			/// <summary>
			/// 艦名表示の幅を固定するか
			/// </summary>
			public bool FixShipNameWidth { get; set; }

			/// <summary>
			/// HPバーを短縮するか
			/// </summary>
			public bool ShortenHPBar { get; set; }

			/// <summary>
			/// next lv. を表示するか
			/// </summary>
			public bool ShowNextExp { get; set; }

			/// <summary>
			/// 装備の改修レベル・艦載機熟練度の表示フラグ
			/// </summary>
			public LevelVisibilityFlag EquipmentLevelVisibility { get; set; }

			/// <summary>
			/// 艦載機熟練度を数字で表示するフラグ
			/// </summary>
			public bool ShowAircraftLevelByNumber { get; set; }

			/// <summary>
			/// 制空戦力の計算方法
			/// </summary>
			public int AirSuperiorityMethod { get; set; }

			/// <summary>
			/// 泊地修理タイマを表示するか
			/// </summary>
			public bool ShowAnchorageRepairingTimer { get; set; }

			/// <summary>
			/// タイマー完了時に点滅させるか
			/// </summary>
			public bool BlinkAtCompletion { get; set; }

			/// <summary>
			/// 疲労度アイコンを表示するか
			/// </summary>
			public bool ShowConditionIcon { get; set; }

			/// <summary>
			/// 艦名表示幅固定時の幅
			/// </summary>
			public int FixedShipNameWidth { get; set; }

			/// <summary>
			/// 制空戦力を範囲表示するか
			/// </summary>
			public bool ShowAirSuperiorityRange { get; set; }

			/// <summary>
			/// 泊地修理によるHP回復を表示に反映するか
			/// </summary>
			public bool ReflectAnchorageRepairHealing { get; set; }

			/// <summary>
			/// 遠征艦隊が母港にいるとき強調表示
			/// </summary>
			public bool EmphasizesSubFleetInPort { get; set; }

			/// <summary>
			/// 大破時に点滅させる
			/// </summary>
			public bool BlinkAtDamaged { get; set; }

			/// <summary>
			/// 艦隊状態の表示方法
			/// </summary>
			public int FleetStateDisplayMode { get; set; }

			/// <summary>
			/// 出撃海域によって色分けするか
			/// </summary>
			public bool AppliesSallyAreaColor { get; set; }

			/// <summary>
			/// 出撃海域による色分けのテーブル
			/// </summary>
			public List<SerializableColor> SallyAreaColorScheme { get; set; }

			[IgnoreDataMember]
			internal readonly List<SerializableColor> DefaultSallyAreaColorScheme = new List<SerializableColor>()
			{
				SerializableColor.UIntToColor(0xfff0f0f0),
				SerializableColor.UIntToColor(0xffffdddd),
				SerializableColor.UIntToColor(0xffddffdd),
				SerializableColor.UIntToColor(0xffddddff),
				SerializableColor.UIntToColor(0xffffffcc),
				SerializableColor.UIntToColor(0xffccffff),
				SerializableColor.UIntToColor(0xffffccff),
				SerializableColor.UIntToColor(0xffffffff),
				SerializableColor.UIntToColor(0xffffead5),
				SerializableColor.UIntToColor(0xffe7c8c8),
				SerializableColor.UIntToColor(0xffe7e7b8),
				SerializableColor.UIntToColor(0xffc8e7c8),
				SerializableColor.UIntToColor(0xffb8e7e7),
				SerializableColor.UIntToColor(0xffc8c8e7),
				SerializableColor.UIntToColor(0xffe7b8e7),
			};

			public ConfigFormFleet()
			{
				ShowAircraft = true;
				SearchingAbilityMethod = 4;
				IsScrollable = true;
				FixShipNameWidth = false;
				ShortenHPBar = false;
				ShowNextExp = true;
				EquipmentLevelVisibility = LevelVisibilityFlag.Both;
				ShowAircraftLevelByNumber = false;
				AirSuperiorityMethod = 1;
				ShowAnchorageRepairingTimer = true;
				BlinkAtCompletion = true;
				ShowConditionIcon = true;
				FixedShipNameWidth = 40;
				ShowAirSuperiorityRange = false;
				ReflectAnchorageRepairHealing = true;
				EmphasizesSubFleetInPort = false;
				BlinkAtDamaged = true;
				FleetStateDisplayMode = 2;
				AppliesSallyAreaColor = false;
				SallyAreaColorScheme = DefaultSallyAreaColorScheme.ToList();
			}
		}
		/// <summary>[艦隊]ウィンドウ</summary>
		[DataMember]
		public ConfigFormFleet FormFleet { get; private set; }


		/// <summary>
		/// [任務]ウィンドウの設定を扱います。
		/// </summary>
		public class ConfigFormQuest : ConfigPartBase
		{

			/// <summary>
			/// 遂行中の任務のみ表示するか
			/// </summary>
			public bool ShowRunningOnly { get; set; }


			/// <summary>
			/// 単発を表示
			/// </summary>
			public bool ShowOnce { get; set; }

			/// <summary>
			/// デイリーを表示
			/// </summary>
			public bool ShowDaily { get; set; }

			/// <summary>
			/// ウィークリーを表示
			/// </summary>
			public bool ShowWeekly { get; set; }

			/// <summary>
			/// マンスリーを表示
			/// </summary>
			public bool ShowMonthly { get; set; }

			/// <summary>
			/// その他を表示
			/// </summary>
			public bool ShowOther { get; set; }

			/// <summary>
			/// 列の可視性
			/// </summary>
			public SerializableList<bool> ColumnFilter { get; set; }

			/// <summary>
			/// 列の幅
			/// </summary>
			public SerializableList<int> ColumnWidth { get; set; }

			public SerializableList<int>? ColumnSort { get; set; }

			public List<SortDescription> SortDescriptions { get; set; } = new();

			/// <summary>
			/// どの行をソートしていたか
			/// </summary>
			public int SortParameter { get; set; }

			/// <summary>
			/// 進捗を自動保存するか
			/// 0 = しない、1 = 一時間ごと、2 = 一日ごと
			/// </summary>
			public int ProgressAutoSaving { get; set; }

			public bool AllowUserToSortRows { get; set; }

			public bool ShowQuestCode { get; set; } = true;

			public int HeaderMinSize { get; set; } = 32;

			public int RowMinSize { get; set; } = 24;

			public ConfigFormQuest()
			{
				ShowRunningOnly = false;
				ShowOnce = true;
				ShowDaily = true;
				ShowWeekly = true;
				ShowMonthly = true;
				ShowOther = true;
				ColumnFilter = null;        //実際の初期化は FormQuest で行う
				ColumnWidth = null;         //上に同じ
				ColumnSort = null;          //上に同じ
				SortParameter = 3 << 1 | 0;
				ProgressAutoSaving = 1;
				AllowUserToSortRows = true;
			}
		}
		/// <summary>[任務]ウィンドウ</summary>
		[DataMember]
		public ConfigFormQuest FormQuest { get; private set; }


		/// <summary>
		/// [艦船グループ]ウィンドウの設定を扱います。
		/// </summary>
		public class ConfigFormShipGroup : ConfigPartBase
		{

			/// <summary>
			/// 自動更新するか
			/// </summary>
			public bool AutoUpdate { get; set; }

			/// <summary>
			/// ステータスバーを表示するか
			/// </summary>
			public bool ShowStatusBar { get; set; }


			/// <summary>
			/// 艦名列のソート方法
			/// 0 = 図鑑番号順, 1 = あいうえお順
			/// </summary>
			public int ShipNameSortMethod { get; set; }

			public double GroupHeight { get; set; }
			public int ColumnHeaderHeight { get; set; } = 32;
			public int RowHeight { get; set; } = 32;

			public ConfigFormShipGroup()
			{
				AutoUpdate = true;
				ShowStatusBar = true;
				ShipNameSortMethod = 0;
			}
		}
		/// <summary>[艦船グループ]ウィンドウ</summary>
		[DataMember]
		public ConfigFormShipGroup FormShipGroup { get; private set; }


		/// <summary>
		/// [ブラウザ]ウィンドウの設定を扱います。
		/// </summary>
		public class ConfigFormBrowser : ConfigPartBase
		{
			public BrowserOption Browser { get; set; }

			/// <summary>
			/// ブラウザの拡大率 10-1000(%)
			/// </summary>
			public double ZoomRate { get; set; }

			/// <summary>
			/// ブラウザをウィンドウサイズに合わせる
			/// </summary>
			[DataMember]
			public bool ZoomFit { get; set; }

			/// <summary>
			/// ログインページのURL
			/// </summary>
			public string LogInPageURL { get; set; }

			/// <summary>
			/// ブラウザを有効にするか
			/// </summary>
			public bool IsEnabled { get; set; }

			/// <summary>
			/// スクリーンショットの保存先フォルダ
			/// </summary>
			public string ScreenShotPath { get; set; }

			/// <summary>
			/// スクリーンショットのフォーマット
			/// 1=jpeg, 2=png
			/// </summary>
			public int ScreenShotFormat { get; set; }

			/// <summary>
			/// スクリーンショットの保存モード
			/// 1=ファイル, 2=クリップボード, 3=両方
			/// </summary>
			public int ScreenShotSaveMode { get; set; }

			/// <summary>
			/// 適用するスタイルシート
			/// </summary>
			public string StyleSheet { get; set; }

			/// <summary>
			/// スクロール可能かどうか
			/// </summary>
			public bool IsScrollable { get; set; }

			/// <summary>
			/// スタイルシートを適用するか
			/// </summary>
			public bool AppliesStyleSheet { get; set; }

			/// <summary>
			/// DMMによるページ更新ダイアログを非表示にするか
			/// </summary>
			public bool IsDMMreloadDialogDestroyable { get; set; }

			/// <summary>
			/// Twitter の画像圧縮を回避するか
			/// </summary>
			public bool AvoidTwitterDeterioration { get; set; }

			/// <summary>
			/// ツールメニューの配置
			/// </summary>
			public DockStyle ToolMenuDockStyle { get; set; }

			/// <summary>
			/// ツールメニューの可視性
			/// </summary>
			public bool IsToolMenuVisible { get; set; }

			/// <summary>
			/// 再読み込み時に確認ダイアログを入れるか
			/// </summary>
			public bool ConfirmAtRefresh { get; set; }

			/// <summary>
			/// ハードウェアアクセラレーションを有効にするか
			/// </summary>
			public bool HardwareAccelerationEnabled { get; set; }

			/// <summary>
			/// 描画バッファを保持するか
			/// </summary>
			public bool PreserveDrawingBuffer { get; set; }

			/// <summary>
			/// カラープロファイルを sRGB に固定するか
			/// </summary>
			public bool ForceColorProfile { get; set; }

			/// <summary>
			/// ブラウザのログを保存するか
			/// </summary>
			public bool SavesBrowserLog { get; set; }

			/// <summary>
			/// Bypass foreigner block
			/// </summary>
			public bool UseGadgetRedirect { get; set; }

			/// <summary>
			///  Gadget Bypass server options
			/// </summary>
			public GadgetServerOptions GadgetBypassServer { get; set; }

			public string GadgetBypassServerCustom { get; set; }

			/// <summary>
			/// Rename WebView2 vulkan files so it can't use the vulkan software rendering implementation
			/// This fixes performance on older CPUs
			/// </summary>
			public bool UseVulkanWorkaround { get; set; }

			public float Volume { get; set; }

			public bool IsMute { get; set; }

			public bool IsBrowserContextMenuEnabled { get; set; }

			public bool UseHttps { get; set; } = true;

			public ConfigFormBrowser()
			{
				Browser = BrowserOption.CefSharp;
				ZoomRate = 1;
				ZoomFit = false;
				LogInPageURL = @"http://www.dmm.com/netgame/social/-/gadgets/=/app_id=854854/";
				IsEnabled = true;
				ScreenShotPath = "ScreenShot";
				ScreenShotFormat = 2;
				ScreenShotSaveMode = 1;
				StyleSheet = "\r\nbody {\r\n	margin:0;\r\n	overflow:hidden\r\n}\r\n\r\n#game_frame {\r\n	position:fixed;\r\n	left:50%;\r\n	top:-16px;\r\n	margin-left:-450px;\r\n	z-index:1\r\n}\r\n";
				IsScrollable = false;
				AppliesStyleSheet = true;
				IsDMMreloadDialogDestroyable = false;
				AvoidTwitterDeterioration = true;
				ToolMenuDockStyle = DockStyle.Top;
				IsToolMenuVisible = true;
				ConfirmAtRefresh = true;
				HardwareAccelerationEnabled = true;
				PreserveDrawingBuffer = true;
				ForceColorProfile = false;
				SavesBrowserLog = false;
				UseGadgetRedirect = CultureInfo.CurrentCulture.Name switch
				{
					"ja-JP" => false,
					_ => true
				};
				UseVulkanWorkaround = false;
				Volume = 100;
				IsMute = false;
				GadgetBypassServer = GadgetServerOptions.EO;
				GadgetBypassServerCustom = "";
				IsBrowserContextMenuEnabled = true;
			}
		}
		/// <summary>[ブラウザ]ウィンドウ</summary>
		[DataMember]
		public ConfigFormBrowser FormBrowser { get; private set; }


		/// <summary>
		/// [羅針盤]ウィンドウの設定を扱います。
		/// </summary>
		public class ConfigFormCompass : ConfigPartBase
		{

			/// <summary>
			/// 一度に表示する敵艦隊候補数
			/// </summary>
			public int CandidateDisplayCount { get; set; }

			/// <summary>
			/// スクロール可能か
			/// </summary>
			public bool IsScrollable { get; set; }

			/// <summary>
			/// 艦名表示の最大幅
			/// </summary>
			public int MaxShipNameWidth { get; set; }

			/// <summary>
			/// By default, only the compositions matching the preview from map screen will be shown. <br></br>
			/// If you enable this setting, the preview will be ignored and all compositions will be shown.
			/// </summary>
			public bool DisplayAllEnemyCompositions { get; set; }

			public ConfigFormCompass()
			{
				CandidateDisplayCount = 4;
				IsScrollable = false;
				MaxShipNameWidth = 60;
				DisplayAllEnemyCompositions = false;
			}
		}
		/// <summary>[羅針盤]ウィンドウ</summary>
		[DataMember]
		public ConfigFormCompass FormCompass { get; private set; }


		/// <summary>
		/// [JSON]ウィンドウの設定を扱います。
		/// </summary>
		public class ConfigFormJson : ConfigPartBase
		{

			/// <summary>
			/// 自動更新するか
			/// </summary>
			public bool AutoUpdate { get; set; }

			/// <summary>
			/// TreeView を更新するか
			/// </summary>
			public bool UpdatesTree { get; set; }

			/// <summary>
			/// 自動更新時のフィルタ
			/// </summary>
			public string AutoUpdateFilter { get; set; }


			public ConfigFormJson()
			{
				AutoUpdate = false;
				UpdatesTree = true;
				AutoUpdateFilter = "";
			}
		}
		/// <summary>[JSON]ウィンドウ</summary>
		[DataMember]
		public ConfigFormJson FormJson { get; private set; }


		/// <summary>
		/// [戦闘]ウィンドウの設定を扱います。
		/// </summary>
		public class ConfigFormBattle : ConfigPartBase
		{

			/// <summary>
			/// スクロール可能か
			/// </summary>
			public bool IsScrollable { get; set; }

			/// <summary>
			/// 戦闘中は表示を隠し、戦闘後のみ表示する
			/// </summary>
			public bool HideDuringBattle { get; set; }

			/// <summary>
			/// HP バーを表示するか
			/// </summary>
			public bool ShowHPBar { get; set; }

			/// <summary>
			/// HP バーに艦種を表示するか
			/// </summary>
			public bool ShowShipTypeInHPBar { get; set; }

			/// <summary>
			/// 7隻目を主力艦隊と同じ行に表示するか
			/// </summary>
			public bool Display7thAsSingleLine { get; set; }

			/// <summary>
			/// Hide ship type from the left side of the HP bar
			/// </summary>
			public bool CompactMode { get; set; }


			public ConfigFormBattle()
			{
				IsScrollable = false;
				HideDuringBattle = false;
				ShowHPBar = true;
				ShowShipTypeInHPBar = false;
				Display7thAsSingleLine = true;
				CompactMode = false;
			}
		}

		/// <summary>
		/// [戦闘]ウィンドウ
		/// </summary>
		[DataMember]
		public ConfigFormBattle FormBattle { get; private set; }


		/// <summary>
		/// [基地航空隊]ウィンドウの設定を扱います。
		/// </summary>
		public class ConfigFormBaseAirCorps : ConfigPartBase
		{

			/// <summary>
			/// イベント海域のもののみ表示するか
			/// </summary>
			public bool ShowEventMapOnly { get; set; }

			public ConfigFormBaseAirCorps()
			{
				ShowEventMapOnly = false;
			}
		}

		/// <summary>
		/// [基地航空隊]ウィンドウ
		/// </summary>
		[DataMember]
		public ConfigFormBaseAirCorps FormBaseAirCorps { get; private set; }

		/// <summary>
		/// Ship training configuration
		/// </summary>
		public class ConfigFormShipTraining : ConfigPartBase
		{

			/// <summary>
			/// Allow multiple plan for the same ship ?
			/// </summary>
			public bool AllowMultiplePlanPerShip { get; set; }


			public ConfigFormShipTraining()
			{
				AllowMultiplePlanPerShip = false;
			}
		}

		/// <summary>
		/// Ship training configuration
		/// </summary>
		[DataMember]
		public ConfigFormShipTraining FormShipTraining { get; private set; }

		/// <summary>
		/// 各[通知]ウィンドウの設定を扱います。
		/// </summary>
		public class ConfigNotifierBase : ConfigPartBase
		{

			public bool IsEnabled { get; set; }

			public bool IsSilenced { get; set; }

			public bool ShowsDialog { get; set; }

			public string ImagePath { get; set; }

			public bool DrawsImage { get; set; }

			public string SoundPath { get; set; }

			public bool PlaysSound { get; set; }

			public int SoundVolume { get; set; }

			public bool LoopsSound { get; set; }

			public bool DrawsMessage { get; set; }

			public int ClosingInterval { get; set; }

			public int AccelInterval { get; set; }

			public bool CloseOnMouseMove { get; set; }

			public Notifier.NotifierDialogClickFlags ClickFlag { get; set; }

			public Notifier.NotifierDialogAlignment Alignment { get; set; }

			public Point Location { get; set; }

			public bool HasFormBorder { get; set; }

			public bool TopMost { get; set; }

			public bool ShowWithActivation { get; set; }

			public SerializableColor ForeColor { get; set; }

			public SerializableColor BackColor { get; set; }


			public ConfigNotifierBase()
			{
				IsEnabled = true;
				IsSilenced = false;
				ShowsDialog = true;
				ImagePath = "";
				DrawsImage = false;
				SoundPath = "";
				PlaysSound = false;
				SoundVolume = 100;
				LoopsSound = false;
				DrawsMessage = true;
				ClosingInterval = 10000;
				AccelInterval = 0;
				CloseOnMouseMove = false;
				ClickFlag = Notifier.NotifierDialogClickFlags.Left;
				Alignment = Notifier.NotifierDialogAlignment.BottomRight;
				Location = new Point(0, 0);
				HasFormBorder = true;
				TopMost = true;
				ShowWithActivation = true;
				ForeColor = SystemColors.ControlText;
				BackColor = SystemColors.Control;
			}

		}


		/// <summary>
		/// [大破進撃通知]の設定を扱います。
		/// </summary>
		public class ConfigNotifierDamage : ConfigNotifierBase
		{

			public bool NotifiesBefore { get; set; }
			public bool NotifiesNow { get; set; }
			public bool NotifiesAfter { get; set; }
			public int LevelBorder { get; set; }
			public bool ContainsNotLockedShip { get; set; }
			public bool ContainsSafeShip { get; set; }
			public bool ContainsFlagship { get; set; }
			public bool NotifiesAtEndpoint { get; set; }
			public ConfigNotifierDamage()
				: base()
			{
				NotifiesBefore = false;
				NotifiesNow = true;
				NotifiesAfter = true;
				LevelBorder = 1;
				ContainsNotLockedShip = true;
				ContainsSafeShip = true;
				ContainsFlagship = true;
				NotifiesAtEndpoint = false;
			}
		}


		/// <summary>
		/// [泊地修理通知]の設定を扱います。
		/// </summary>
		public class ConfigNotifierAnchorageRepair : ConfigNotifierBase
		{

			public int NotificationLevel { get; set; }

			public ConfigNotifierAnchorageRepair()
				: base()
			{
				NotificationLevel = 2;
			}
		}

		/// <summary>
		/// [基地航空隊通知]の設定を扱います。
		/// </summary>
		public class ConfigNotifierBaseAirCorps : ConfigNotifierBase
		{
			/// <summary>
			/// 未補給時に通知する
			/// </summary>
			public bool NotifiesNotSupplied { get; set; }

			/// <summary>
			/// 疲労時に通知する
			/// </summary>
			public bool NotifiesTired { get; set; }

			/// <summary>
			/// 編成されていないときに通知する
			/// </summary>
			public bool NotifiesNotOrganized { get; set; }


			/// <summary>
			/// 待機のとき通知する
			/// </summary>
			public bool NotifiesStandby { get; set; }

			/// <summary>
			/// 退避の時通知する
			/// </summary>
			public bool NotifiesRetreat { get; set; }

			/// <summary>
			/// 休息の時通知する
			/// </summary>
			public bool NotifiesRest { get; set; }


			/// <summary>
			/// 通常海域で通知する
			/// </summary>
			public bool NotifiesNormalMap { get; set; }

			/// <summary>
			/// イベント海域で通知する
			/// </summary>
			public bool NotifiesEventMap { get; set; }


			/// <summary>
			/// 基地枠の配置転換完了時に通知する
			/// </summary>
			public bool NotifiesSquadronRelocation { get; set; }

			/// <summary>
			/// 装備の配置転換完了時に通知する
			/// </summary>
			public bool NotifiesEquipmentRelocation { get; set; }


			public ConfigNotifierBaseAirCorps()
				: base()
			{
				NotifiesNotSupplied = true;
				NotifiesTired = false;
				NotifiesNotOrganized = false;

				NotifiesStandby = false;
				NotifiesRetreat = true;
				NotifiesRest = true;

				NotifiesNormalMap = false;
				NotifiesEventMap = true;

				NotifiesSquadronRelocation = true;
				NotifiesEquipmentRelocation = false;
			}
		}

		public class ConfigNotifierBattleEnd : ConfigNotifierBase
		{
			public bool IdleTimerEnabled { get; set; }
			public int IdleSeconds { get; set; } = 300;

			public ConfigNotifierBattleEnd()
			{
				IsEnabled = false;
			}
		}


		/// <summary>[遠征帰投通知]</summary>
		[DataMember]
		public ConfigNotifierBase NotifierExpedition { get; private set; }

		/// <summary>[建造完了通知]</summary>
		[DataMember]
		public ConfigNotifierBase NotifierConstruction { get; private set; }

		/// <summary>[入渠完了通知]</summary>
		[DataMember]
		public ConfigNotifierBase NotifierRepair { get; private set; }

		/// <summary>[疲労回復通知]</summary>
		[DataMember]
		public ConfigNotifierBase NotifierCondition { get; private set; }

		/// <summary>[大破進撃通知]</summary>
		[DataMember]
		public ConfigNotifierDamage NotifierDamage { get; private set; }

		/// <summary>[泊地修理通知]</summary>
		[DataMember]
		public ConfigNotifierAnchorageRepair NotifierAnchorageRepair { get; private set; }

		/// <summary>[基地航空隊通知]</summary>
		[DataMember]
		public ConfigNotifierBaseAirCorps NotifierBaseAirCorps { get; private set; }

		[DataMember]
		public ConfigNotifierBattleEnd NotifierBattleEnd { get; private set; }

		[DataMember]
		public ConfigNotifierBase NotifierRemodelLevel { get; private set; }

		[DataMember]
		public ConfigNotifierBase NotifierTrainingPlan { get; private set; }

		/// <summary>
		/// SyncBGMPlayer の設定を扱います。
		/// </summary>
		public class ConfigBGMPlayer : ConfigPartBase
		{

			public bool Enabled { get; set; }
			public List<SyncBGMPlayer.SoundHandle> Handles { get; set; }
			public bool SyncBrowserMute { get; set; }

			public ConfigBGMPlayer()
				: base()
			{
				// 初期値定義は SyncBGMPlayer 内でも
				Enabled = false;
				Handles = new List<SyncBGMPlayer.SoundHandle>();
				foreach (SyncBGMPlayer.SoundHandleID id in Enum.GetValues(typeof(SyncBGMPlayer.SoundHandleID)))
					Handles.Add(new SyncBGMPlayer.SoundHandle(id));
				SyncBrowserMute = false;
			}
		}
		[DataMember]
		public ConfigBGMPlayer BGMPlayer { get; private set; }


		/// <summary>
		/// 編成画像出力の設定を扱います。
		/// </summary>
		public class ConfigFleetImageGenerator : ConfigPartBase
		{

			public FleetImageArgument Argument { get; set; }
			public int ImageType { get; set; }
			public int OutputType { get; set; }
			public bool OpenImageAfterOutput { get; set; }
			public string LastOutputPath { get; set; }
			public bool DisableOverwritePrompt { get; set; }
			public bool AutoSetFileNameToDate { get; set; }
			public bool SyncronizeTitleAndFileName { get; set; }
			public int MaxEquipmentNameWidth { get; set; }
			public bool DownloadMissingShipImage { get; set; }
			public string ImageSaveLocation { get; set; }
			public bool QuickConfigAccess { get; set; }
			public bool UseCustomTheme { get; set; }
			public string ForegroundColor { get; set; }
			public string BackgroundColor { get; set; }
			public TpGauge TankTpGauge { get; set; } = 0;

			public ConfigFleetImageGenerator()
				: base()
			{
				Argument = new();
				ImageType = 0;
				OutputType = 0;
				OpenImageAfterOutput = false;
				LastOutputPath = "";
				DisableOverwritePrompt = false;
				AutoSetFileNameToDate = false;
				SyncronizeTitleAndFileName = false;
				MaxEquipmentNameWidth = 200;
				DownloadMissingShipImage = true;
				ImageSaveLocation = "FleetImageGenerator";
				QuickConfigAccess = false;
				UseCustomTheme = false;
				ForegroundColor = "#FFFFFFFF";
				BackgroundColor = "#FF000000";
			}
		}

		[DataMember]
		public ConfigFleetImageGenerator FleetImageGenerator { get; private set; }

		[DataMember]
		public ConfigDataSubmission DataSubmission { get; private set; }

		public class ConfigWhitecap : ConfigPartBase
		{

			public bool ShowInTaskbar { get; set; }
			public bool TopMost { get; set; }
			public int BoardWidth { get; set; }
			public int BoardHeight { get; set; }
			public int ZoomRate { get; set; }
			public int UpdateInterval { get; set; }
			public int ColorTheme { get; set; }
			public int BirthRule { get; set; }
			public int AliveRule { get; set; }

			public ConfigWhitecap()
				: base()
			{
				ShowInTaskbar = true;
				TopMost = false;
				BoardWidth = 200;
				BoardHeight = 150;
				ZoomRate = 2;
				UpdateInterval = 100;
				ColorTheme = 0;
				BirthRule = (1 << 3);
				AliveRule = (1 << 2) | (1 << 3);
			}
		}
		[DataMember]
		public ConfigWhitecap Whitecap { get; private set; }



		[DataMember]
		public string Version
		{
			get { return SoftwareInformation.VersionEnglish; }
			set { } //readonly
		}


		[DataMember]
		public string VersionUpdateTime { get; set; }



		public ConfigurationData()
		{
			Initialize();
		}

		public override void Initialize()
		{
			Connection = new ConfigConnection();
			UI = new ConfigUI();
			Log = new ConfigLog();
			Control = new ConfigControl();
			Debug = new ConfigDebug();
			Life = new ConfigLife();

			FormArsenal = new ConfigFormArsenal();
			FormDock = new ConfigFormDock();
			FormFleet = new ConfigFormFleet();
			FormHeadquarters = new ConfigFormHeadquarters();
			FormQuest = new ConfigFormQuest();
			FormShipGroup = new ConfigFormShipGroup();
			FormBattle = new ConfigFormBattle();
			FormBrowser = new ConfigFormBrowser();
			FormCompass = new ConfigFormCompass();
			FormJson = new ConfigFormJson();
			FormBaseAirCorps = new ConfigFormBaseAirCorps();
			FormShipTraining = new();

			NotifierExpedition = new ConfigNotifierBase();
			NotifierConstruction = new ConfigNotifierBase();
			NotifierRepair = new ConfigNotifierBase();
			NotifierCondition = new ConfigNotifierBase();
			NotifierDamage = new ConfigNotifierDamage();
			NotifierAnchorageRepair = new ConfigNotifierAnchorageRepair();
			NotifierBaseAirCorps = new ConfigNotifierBaseAirCorps();
			NotifierBattleEnd = new ConfigNotifierBattleEnd();
			NotifierRemodelLevel = new ConfigNotifierBase();
			NotifierTrainingPlan = new ConfigNotifierBase();

			BGMPlayer = new ConfigBGMPlayer();
			FleetImageGenerator = new ConfigFleetImageGenerator();
			DataSubmission = new ConfigDataSubmission();
			Whitecap = new ConfigWhitecap();

			VersionUpdateTime = DateTimeHelper.TimeToCSVString(SoftwareInformation.UpdateTime);
		}
	}
	private static ConfigurationData _config;

	public static ConfigurationData Config => _config;



	private Configuration()
		: base()
	{

		_config = new ConfigurationData();
	}


	internal void OnConfigurationChanged()
	{
		ConfigurationChanged();
	}


	public void Load()
	{
		var temp = (ConfigurationData)_config.Load(SaveFileName);

		if (temp != null)
		{
			// hack: set defaults for players that have a configuration before language was added
			if (temp.UI.Culture == null)
			{
				temp.UI.Culture = CultureInfo.CurrentCulture.Name switch
				{
					"ja-JP" => "ja-JP",
					_ => "en-US"
				};

				temp.FormBrowser.UseGadgetRedirect = CultureInfo.CurrentCulture.Name switch
				{
					"ja-JP" => false,
					_ => true
				};

				bool disableTranslations = CultureInfo.CurrentCulture.Name switch
				{
					"ja-JP" => true,
					_ => false
				};

				temp.UI.JapaneseShipName = disableTranslations;
				temp.UI.JapaneseShipType = disableTranslations;
				temp.UI.JapaneseEquipmentName = disableTranslations;
				temp.UI.JapaneseEquipmentType = disableTranslations;
				temp.UI.DisableOtherTranslations = disableTranslations;
			}

			_config = temp;
			CheckUpdate();
			OnConfigurationChanged();
		}
		else
		{
			System.Windows.Window tempWindow = new() { Visibility = System.Windows.Visibility.Hidden };
			tempWindow.Show();

			MessageBox.Show(String.Format(Resources.FirstTimeDialog, SoftwareInformation.SoftwareNameEnglish),
				Resources.FirstTimeTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		ApplyTheme();
	}

	private dynamic ThemeStyle;

	public void ApplyTheme()
	{
		dynamic json;
		if (Config.UI.ThemeMode != 2)
		{
			string theme = Theme.GetTheme(Config.UI.ThemeMode);
			json = JsonObject.Parse(theme);
		}
		else
		{
			try
			{
				string s = String.Empty;
				StringBuilder sb = new StringBuilder();
				using (StreamReader sr = File.OpenText(@"Settings\ColorScheme.json"))
				{
					while ((s = sr.ReadLine()) != null)
					{
						s = Regex.Replace(s, @"\/\/.*?$", string.Empty);
						if (!String.IsNullOrWhiteSpace(s)) sb.Append(s);
					}
				}
				json = JsonObject.Parse(sb.ToString());
			}
			catch (FileNotFoundException)
			{
				Logger.Add(3, @"Settings\ColorScheme.json not found.");
				json = JsonObject.Parse(Theme.GetTheme(0));
			}
			catch
			{
				Logger.Add(3, @"Failed to read Settings\ColorScheme.json.");
				json = JsonObject.Parse(Theme.GetTheme(0));
			}
		}

		int themeId = Config.UI.ThemeID;
		if (!json.IsDefined(themeId))
		{
			themeId = Config.UI.ThemeID = 0;
			Logger.Add(2, "Failed to find selected ThemeID");
		}

		ThemeStyle = json[themeId];
		Logger.Add(1, "Color theme loaded: " + ThemeStyle["name"]);
		// 定义基本颜色
		Config.UI.Color_Red = ThemeColor("basicColors", "red");
		Config.UI.Color_Orange = ThemeColor("basicColors", "orange");
		Config.UI.Color_Yellow = ThemeColor("basicColors", "yellow");
		Config.UI.Color_Green = ThemeColor("basicColors", "green");
		Config.UI.Color_Cyan = ThemeColor("basicColors", "cyan");
		Config.UI.Color_Blue = ThemeColor("basicColors", "blue");
		Config.UI.Color_Magenta = ThemeColor("basicColors", "magenta");
		Config.UI.Color_Violet = ThemeColor("basicColors", "violet");
		// 定义面板颜色
		Config.UI.ForeColor = ThemeColor("panelColors", "foreground");
		Config.UI.BackColor = ThemeColor("panelColors", "background");
		Config.UI.SubForeColor = ThemeColor("panelColors", "foreground2");
		Config.UI.SubBackColor = ThemeColor("panelColors", "background2");
		Config.UI.SubBackColorPen = new Pen(Config.UI.SubBackColor);
		// 状态栏颜色
		Config.UI.StatusBarForeColor = ThemeColor("panelColors", "statusBarFG");
		Config.UI.StatusBarBackColor = ThemeColor("panelColors", "statusBarBG");
		// 定义 UI (DockPanelSuite) 颜色
		Config.UI.DockPanelSuiteStyles = new string[] {
			ThemePanelColorHex("skin", "panelSplitter"),
			ThemePanelColorHex("skin", "docTabBarFG"),
			ThemePanelColorHex("skin", "docTabBarBG"),
			ThemePanelColorHex("skin", "docTabActiveFG"),
			ThemePanelColorHex("skin", "docTabActiveBG"),
			ThemePanelColorHex("skin", "docTabActiveLostFocusFG"),
			ThemePanelColorHex("skin", "docTabActiveLostFocusBG"),
			ThemePanelColorHex("skin", "docTabInactiveHoverFG"),
			ThemePanelColorHex("skin", "docTabInactiveHoverBG"),
			ThemePanelColorHex("skin", "docBtnActiveHoverFG"),
			ThemePanelColorHex("skin", "docBtnActiveHoverBG"),
			ThemePanelColorHex("skin", "docBtnActiveLostFocusHoverFG"),
			ThemePanelColorHex("skin", "docBtnActiveLostFocusHoverBG"),
			ThemePanelColorHex("skin", "docBtnInactiveHoverFG"),
			ThemePanelColorHex("skin", "docBtnInactiveHoverBG"),
			ThemePanelColorHex("skin", "toolTabBarFG"),
			ThemePanelColorHex("skin", "toolTabBarBG"),
			ThemePanelColorHex("skin", "toolTabActive"),
			ThemePanelColorHex("skin", "toolTitleActiveFG"),
			ThemePanelColorHex("skin", "toolTitleActiveBG"),
			ThemePanelColorHex("skin", "toolTitleLostFocusFG"),
			ThemePanelColorHex("skin", "toolTitleLostFocusBG"),
			ThemePanelColorHex("skin", "toolTitleDotActive"),
			ThemePanelColorHex("skin", "toolTitleDotLostFocus"),
			ThemePanelColorHex("skin", "autoHideTabBarFG"),
			ThemePanelColorHex("skin", "autoHideTabBarBG"),
			ThemePanelColorHex("skin", "autoHideTabActive"),
			ThemePanelColorHex("skin", "autoHideTabInactive")
		};
		// 定义数值条颜色
		Config.UI.BarColorSchemes = new List<SerializableColor>[] {
			new List<SerializableColor>() {
				ThemeBarColor(0, 0),
				ThemeBarColor(0, 1),
				ThemeBarColor(0, 2),
				ThemeBarColor(0, 3),
				ThemeBarColor(0, 4),
				ThemeBarColor(0, 5),
				ThemeBarColor(0, 6),
				ThemeBarColor(0, 7),
				ThemeBarColor(0, 8),
				ThemeBarColor(0, 9),
				ThemeBarColor(0, 10),
				ThemeBarColor(0, 11)
			},
			new List<SerializableColor>() {
				ThemeBarColor(1, 0),
				ThemeBarColor(1, 1),
				ThemeBarColor(1, 2),
				ThemeBarColor(1, 3),
				ThemeBarColor(1, 4),
				ThemeBarColor(1, 5),
				ThemeBarColor(1, 6),
				ThemeBarColor(1, 7),
				ThemeBarColor(1, 8),
				ThemeBarColor(1, 9),
				ThemeBarColor(1, 10),
				ThemeBarColor(1, 11)
			}
		};
		Config.UI.SetBarColorScheme();
		// 设定各面板颜色
		Config.UI.Fleet_ColorRepairTimerText = ThemePanelColor("fleet", "repairTimerText");
		Config.UI.Fleet_ColorConditionText = ThemePanelColor("fleet", "conditionText");
		Config.UI.Fleet_ColorConditionVeryTired = ThemePanelColor("fleet", "conditionVeryTired");
		Config.UI.Fleet_ColorConditionTired = ThemePanelColor("fleet", "conditionTired");
		Config.UI.Fleet_ColorConditionLittleTired = ThemePanelColor("fleet", "conditionLittleTired");
		Config.UI.Fleet_ColorConditionSparkle = ThemePanelColor("fleet", "conditionSparkle");
		Config.UI.Fleet_EquipmentLevelColor = ThemePanelColor("fleet", "equipmentLevel");
		Config.UI.Fleet_RemodelReadyColor = ThemePanelColor("fleet", "remodelReady");
		Config.UI.FleetOverview_ShipDamagedFG = ThemePanelColor("fleetOverview", "shipDamagedFG");
		Config.UI.FleetOverview_ShipDamagedBG = ThemePanelColor("fleetOverview", "shipDamagedBG");
		Config.UI.FleetOverview_ExpeditionOverFG = ThemePanelColor("fleetOverview", "expeditionOverFG");
		Config.UI.FleetOverview_ExpeditionOverBG = ThemePanelColor("fleetOverview", "expeditionOverBG");
		Config.UI.FleetOverview_TiredRecoveredFG = ThemePanelColor("fleetOverview", "tiredRecoveredFG");
		Config.UI.FleetOverview_TiredRecoveredBG = ThemePanelColor("fleetOverview", "tiredRecoveredBG");
		Config.UI.FleetOverview_AlertNotInExpeditionFG = ThemePanelColor("fleetOverview", "alertNotInExpeditionFG");
		Config.UI.FleetOverview_AlertNotInExpeditionBG = ThemePanelColor("fleetOverview", "alertNotInExpeditionBG");
		Config.UI.Dock_RepairFinishedFG = ThemePanelColor("dock", "repairFinishedFG");
		Config.UI.Dock_RepairFinishedBG = ThemePanelColor("dock", "repairFinishedBG");
		Config.UI.Arsenal_BuildCompleteFG = ThemePanelColor("arsenal", "buildCompleteFG");
		Config.UI.Arsenal_BuildCompleteBG = ThemePanelColor("arsenal", "buildCompleteBG");
		Config.UI.Headquarters_ResourceOverFG = ThemePanelColor("hq", "resOverFG");
		Config.UI.Headquarters_ResourceOverBG = ThemePanelColor("hq", "resOverBG");
		Config.UI.Headquarters_ShipCountOverFG = ThemePanelColor("hq", "shipOverFG");
		Config.UI.Headquarters_ShipCountOverBG = ThemePanelColor("hq", "shipOverBG");
		Config.UI.Headquarters_MaterialMaxFG = ThemePanelColor("hq", "materialMaxFG");
		Config.UI.Headquarters_MaterialMaxBG = ThemePanelColor("hq", "materialMaxBG");
		Config.UI.Headquarters_CoinMaxFG = ThemePanelColor("hq", "coinMaxFG");
		Config.UI.Headquarters_CoinMaxBG = ThemePanelColor("hq", "coinMaxBG");
		Config.UI.Headquarters_ResourceLowFG = ThemePanelColor("hq", "resLowFG");
		Config.UI.Headquarters_ResourceLowBG = ThemePanelColor("hq", "resLowBG");
		Config.UI.Headquarters_ResourceMaxFG = ThemePanelColor("hq", "resMaxFG");
		Config.UI.Headquarters_ResourceMaxBG = ThemePanelColor("hq", "resMaxBG");
		Config.UI.Quest_TypeFG = ThemePanelColor("quest", "typeFG");
		Config.UI.Quest_Type1Color = ThemePanelColor("quest", "typeHensei");
		Config.UI.Quest_Type2Color = ThemePanelColor("quest", "typeShutsugeki");
		Config.UI.Quest_Type3Color = ThemePanelColor("quest", "typeEnshu");
		Config.UI.Quest_Type4Color = ThemePanelColor("quest", "typeEnsei");
		Config.UI.Quest_Type5Color = ThemePanelColor("quest", "typeHokyu");
		Config.UI.Quest_Type6Color = ThemePanelColor("quest", "typeKojo");
		Config.UI.Quest_Type7Color = ThemePanelColor("quest", "typeKaiso");
		Config.UI.Quest_ColorProcessLT50 = ThemePanelColor("quest", "processLT50");
		Config.UI.Quest_ColorProcessLT80 = ThemePanelColor("quest", "processLT80");
		Config.UI.Quest_ColorProcessLT100 = ThemePanelColor("quest", "processLT100");
		Config.UI.Quest_ColorProcessDefault = ThemePanelColor("quest", "processDefault");
		Config.UI.Compass_ShipNameColor2 = ThemePanelColor("compass", "shipClass2");
		Config.UI.Compass_ShipNameColor3 = ThemePanelColor("compass", "shipClass3");
		Config.UI.Compass_ShipNameColor4 = ThemePanelColor("compass", "shipClass4");
		Config.UI.Compass_ShipNameColor5 = ThemePanelColor("compass", "shipClass5");
		Config.UI.Compass_ShipNameColor6 = ThemePanelColor("compass", "shipClass6");
		Config.UI.Compass_ShipNameColor7 = ThemePanelColor("compass", "shipClass7");
		Config.UI.Compass_ShipNameColorDestroyed = ThemePanelColor("compass", "shipDestroyed");
		Config.UI.Compass_ColorTextEventKind3 = ThemePanelColor("compass", "eventKind3");
		Config.UI.Compass_ColorTextEventKind6 = ThemePanelColor("compass", "eventKind6");
		Config.UI.Compass_ColorTextEventKind5 = ThemePanelColor("compass", "eventKind5");
		Config.UI.Compass_ColoroverlayBrush = ThemePanelColor("compass", "overlayBrush");
		Config.UI.Battle_ColorHPBarsMVP = ThemePanelColor("battle", "barMVP");
		Config.UI.Battle_ColorTextMVP = ThemePanelColor("battle", "textMVP");
		Config.UI.Battle_ColorTextMVP2 = ThemePanelColor("battle", "textMVP2");
		Config.UI.Battle_ColorHPBarsEscaped = ThemePanelColor("battle", "barEscaped");
		Config.UI.Battle_ColorTextEscaped = ThemePanelColor("battle", "textEscaped");
		Config.UI.Battle_ColorTextEscaped2 = ThemePanelColor("battle", "textEscaped2");
		Config.UI.Battle_ColorHPBarsBossDamaged = ThemePanelColor("battle", "barBossDamaged");
		Config.UI.Battle_ColorTextBossDamaged = ThemePanelColor("battle", "textBossDamaged");
		Config.UI.Battle_ColorTextBossDamaged2 = ThemePanelColor("battle", "textBossDamaged2");
	}

	private Color ThemeColor(string type, string name)
	{
		if (ThemeStyle.IsDefined(type) && ThemeStyle[type].IsDefined(name))
		{
			return ColorTranslator.FromHtml(ThemeStyle[type][name]);
		}
		else
		{
			switch (type + "_" + name)
			{
				case "basicColors_red":
					return Color.Red;
				case "basicColors_orange":
					return Color.Orange;
				case "basicColors_yellow":
					return Color.Yellow;
				case "basicColors_green":
					return Color.Green;
				case "basicColors.cyan":
					return Color.Cyan;
				case "basicColors.blue":
					return Color.Blue;
				case "basicColors.magenta":
					return Color.Magenta;
				case "basicColors.violet":
					return Color.Violet;
				case "panelColors_foreground":
					return SystemColors.ControlText;
				case "panelColors_background":
					return SystemColors.Control;
				case "panelColors_foreground2":
					return SystemColors.ControlText;
				case "panelColors_background2":
					return SystemColors.ControlLight;
				case "panelColors_statusBarFG":
					return Config.UI.SubForeColor;
				case "panelColors_statusBarBG":
					return Config.UI.SubBackColor;
				default:
					return Color.Magenta;
			}
		}
	}

	private Color ThemePanelColor(string form, string name)
	{
		if (ThemeStyle.IsDefined("panelColors") && ThemeStyle["panelColors"].IsDefined(form) && ThemeStyle["panelColors"][form].IsDefined(name))
		{
			return ColorTranslator.FromHtml(ThemeStyle["panelColors"][form][name]);
		}
		else
		{
			return (form + "_" + name) switch
			{
				// 视图 - 舰队
				"fleet_conditionVeryTired" => Config.UI.Color_Red,
				"fleet_conditionTired" => Config.UI.Color_Orange,
				"fleet_conditionLittleTired" => Config.UI.Color_Yellow,
				"fleet_conditionSparkle" => Config.UI.Color_Blue,
				"fleet_equipmentLevel" => Config.UI.Color_Cyan,
				"fleet_remodelReady" => Color.FromArgb(0x90EE90),
				// 视图 - 司令部
				// 视图 - 任务
				"quest_typeHensei" => Config.UI.Color_Green,
				"quest_typeShutsugeki" => Config.UI.Color_Red,
				"quest_typeEnshu" => Config.UI.Color_Green,
				"quest_typeEnsei" => Config.UI.Color_Cyan,
				"quest_typeHokyu" => Config.UI.Color_Yellow,
				"quest_typeKojo" => Config.UI.Color_Orange,
				"quest_typeKaiso" => Config.UI.Color_Violet,
				"quest_processLT50" => Config.UI.Color_Orange,
				"quest_processLT80" => Config.UI.Color_Green,
				"quest_processLT100" => Config.UI.Color_Cyan,
				"quest_processDefault" => Config.UI.Color_Blue,
				// 视图 - 罗盘
				"compass_shipClass2" => Config.UI.Color_Red,
				"compass_shipClass3" => Config.UI.Color_Orange,
				"compass_shipClass4" => Config.UI.Color_Blue,
				"compass_shipClass5" => Config.UI.Color_Magenta,
				"compass_shipClass6" => Config.UI.Color_Yellow,
				"compass_eventKind3" => Config.UI.Color_Violet,
				"compass_eventKind6" => Config.UI.Color_Green,
				"compass_eventKind5" => Config.UI.Color_Red,
				// %75 透明度背景色
				"compass_overlayBrush" => Color.FromArgb(0xC0, Config.UI.BackColor),
				// 视图 - 战斗
				"battle_barMVP" => Config.UI.Color_Blue,
				"battle_textMVP" => Config.UI.BackColor,
				"battle_textMVP2" => Config.UI.SubBackColor,
				"battle_barEscaped" => Config.UI.SubBackColor,
				"battle_textEscaped" => Config.UI.ForeColor,
				"battle_textEscaped2" => Config.UI.SubForeColor,
				"battle_barBossDamaged" => Config.UI.Color_Orange,
				"battle_textBossDamaged" => Config.UI.BackColor,
				"battle_textBossDamaged2" => Config.UI.SubBackColor,
				// 未定义颜色
				_ => Color.Magenta,
			};
		}
	}

	private String ThemeColorHex(string type, string name)
	{
		if (ThemeStyle.IsDefined(type) && ThemeStyle[type].IsDefined(name))
		{
			return ThemeStyle[type][name];
		}
		else
		{
			switch (type + "_" + name)
			{
				case "panelColors_tabActiveFG":
					return ThemeColorHex("panelColors", "foreground2");
				case "panelColors_tabActiveBG":
					return ThemeColorHex("panelColors", "background2");
				case "panelColors_tabLostFocusFG":
					return ThemeColorHex("panelColors", "foreground2");
				case "panelColors_tabLostFocusBG":
					return ThemeColorHex("panelColors", "background2");
				case "panelColors_tabHoverFG":
					return ThemeColorHex("panelColors", "foreground2");
				case "panelColors_tabHoverBG":
					return ThemeColorHex("panelColors", "background2");
				default:
					var c = ThemeColor(type, name);
					return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
			}
		}
	}

	private String ThemePanelColorHex(string form, string name)
	{
		if (ThemeStyle.IsDefined("panelColors") && ThemeStyle["panelColors"].IsDefined(form) && ThemeStyle["panelColors"][form].IsDefined(name))
		{
			return ThemeStyle["panelColors"][form][name];
		}
		else
		{
			switch (form + "_" + name)
			{
				// 面板分割线
				case "skin_panelSplitter":
					return ThemeColorHex("panelColors", "background2");
				case "skin_docTabBarFG":
					return ThemeColorHex("panelColors", "foreground2");
				case "skin_docTabBarBG":
					return ThemeColorHex("panelColors", "background");
				case "skin_docTabActiveFG":
					return ThemeColorHex("panelColors", "foreground");
				case "skin_docTabActiveBG":
					return ThemeColorHex("panelColors", "background2");
				case "skin_docTabActiveLostFocusFG":
					return ThemeColorHex("panelColors", "foreground");
				case "skin_docTabActiveLostFocusBG":
					return ThemeColorHex("panelColors", "background2");
				case "skin_docTabInactiveHoverFG":
					return ThemeColorHex("panelColors", "foreground");
				case "skin_docTabInactiveHoverBG":
					return ThemeColorHex("panelColors", "background2");
				case "skin_docBtnActiveHoverFG":
					return ThemeColorHex("panelColors", "foreground");
				case "skin_docBtnActiveHoverBG":
					return ThemeColorHex("panelColors", "background2");
				case "skin_docBtnActiveLostFocusHoverFG":
					return ThemeColorHex("panelColors", "foreground");
				case "skin_docBtnActiveLostFocusHoverBG":
					return ThemeColorHex("panelColors", "background2");
				case "skin_docBtnInactiveHoverFG":
					return ThemeColorHex("panelColors", "foreground");
				case "skin_docBtnInactiveHoverBG":
					return ThemeColorHex("panelColors", "background2");
				case "skin_toolTabBarFG":
					return ThemeColorHex("panelColors", "foreground2");
				case "skin_toolTabBarBG":
					return ThemeColorHex("panelColors", "background");
				case "skin_toolTabActive":
					return ThemeColorHex("panelColors", "foreground");
				case "skin_toolTitleActiveFG":
					return ThemeColorHex("panelColors", "foreground");
				case "skin_toolTitleActiveBG":
					return ThemeColorHex("panelColors", "background2");
				case "skin_toolTitleLostFocusFG":
					return ThemeColorHex("panelColors", "foreground2");
				case "skin_toolTitleLostFocusBG":
					return ThemeColorHex("panelColors", "background");
				case "skin_toolTitleDotActive":
					return ThemeColorHex("panelColors", "background");
				case "skin_toolTitleDotLostFocus":
					return ThemeColorHex("panelColors", "background2");
				case "skin_autoHideTabBarFG":
					return ThemeColorHex("panelColors", "background2");
				case "skin_autoHideTabBarBG":
					return ThemeColorHex("panelColors", "background");
				case "skin_autoHideTabActive":
					return ThemeColorHex("panelColors", "foreground");
				case "skin_autoHideTabInactive":
					return ThemeColorHex("panelColors", "foreground2");
				default:
					var c = ThemePanelColor(form, name);
					return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
			}
		}
	}

	private Color ThemeBarColor(int type, int index)
	{
		if (ThemeStyle.IsDefined("barColors") && ThemeStyle["barColors"].IsDefined(type) && ThemeStyle["barColors"][type].IsDefined(11))
		{
			return ColorTranslator.FromHtml(ThemeStyle["barColors"][type][index]);
		}
		else
		{
			switch (type + "_" + index)
			{
				case "0_0":
					return Config.UI.Color_Red;
				case "0_1":
					return Config.UI.Color_Red;
				case "0_2":
					return Config.UI.Color_Orange;
				case "0_3":
					return Config.UI.Color_Orange;
				case "0_4":
					return Config.UI.Color_Yellow;
				case "0_5":
					return Config.UI.Color_Yellow;
				case "0_6":
					return Config.UI.Color_Green;
				case "0_7":
					return Config.UI.Color_Green;
				case "0_8":
					return Config.UI.Color_Blue;
				case "0_9":
					return Config.UI.Color_Magenta;
				case "0_10":
					return Config.UI.Color_Magenta;
				case "0_11":
					return Config.UI.SubBackColor;
				case "1_0":
					return ThemeBarColor(0, 0);
				case "1_1":
					return ThemeBarColor(0, 1);
				case "1_2":
					return ThemeBarColor(0, 2);
				case "1_3":
					return ThemeBarColor(0, 3);
				case "1_4":
					return ThemeBarColor(0, 4);
				case "1_5":
					return ThemeBarColor(0, 5);
				case "1_6":
					return ThemeBarColor(0, 6);
				case "1_7":
					return ThemeBarColor(0, 7);
				case "1_8":
					return ThemeBarColor(0, 8);
				case "1_9":
					return ThemeBarColor(0, 9);
				case "1_10":
					return ThemeBarColor(0, 10);
				case "1_11":
					return ThemeBarColor(0, 11);
				default:
					return Color.Magenta;
			}
		}
	}

	public void Save()
	{
		_config.Save(SaveFileName);
	}



	private void CheckUpdate()
	{
		DateTime dt = Config.VersionUpdateTime == null ? new DateTime(0) : DateTimeHelper.CSVStringToTime(Config.VersionUpdateTime);


		// version 2.5.5.1 or earlier
		if (dt <= DateTimeHelper.CSVStringToTime("2017/03/30 00:00:00"))
		{

			if (MessageBox.Show("Due to recent KanColle changes, Electronic Observer needs to convert current records data.\r\nStart the conversion?\r\n(You may encounter unexpected error if you skip this step)", "Version Update Confirmation(～2.5.5.1)",
				MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
			{

				// 敵編成レコードの敵編成ID再計算とドロップレコードの敵編成ID振りなおし
				// ~ver. 2.8.2 更新で内部処理が変わったので要確認
				try
				{
					var enemyFleetRecord = new EnemyFleetRecord();
					var convertPair = new Dictionary<ulong, ulong>();

					enemyFleetRecord.Load(RecordManager.Instance.MasterPath);

					foreach (var record in enemyFleetRecord.Record.Values)
					{
						ulong key = record.FleetID;
						for (int i = 0; i < record.FleetMember.Length; i++)
						{
							int id = record.FleetMember[i];
							record.FleetMember[i] = 500 < id && id < 1000 ? id + 1000 : id;
						}
						convertPair.Add(key, record.FleetID);
					}

					enemyFleetRecord.SaveAll(RecordManager.Instance.MasterPath);

					var shipDropRecord = new ShipDropRecord();
					shipDropRecord.Load(RecordManager.Instance.MasterPath);

					foreach (var record in shipDropRecord.Record)
					{
						if (convertPair.ContainsKey(record.EnemyFleetID))
							record.EnemyFleetID = convertPair[record.EnemyFleetID];
					}

					shipDropRecord.SaveAll(RecordManager.Instance.MasterPath);

				}
				catch (Exception ex)
				{
					ErrorReporter.SendErrorReport(ex, "CheckUpdate: Failed to reset ShipDropRecord ID.");
				}


				// パラメータレコードの移動と破損データのダウンロード
				try
				{

					var currentRecord = new ShipParameterRecord();
					currentRecord.Load(RecordManager.Instance.MasterPath);

					foreach (var record in currentRecord.Record.Values)
					{
						if (500 < record.ShipID && record.ShipID <= 1000)
						{
							record.ShipID += 1000;
						}
					}

					string defaultRecordPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
					while (Directory.Exists(defaultRecordPath))
						defaultRecordPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());

					Directory.CreateDirectory(defaultRecordPath);

					Resource.ResourceManager.CopyDocumentFromArchive("Record/" + currentRecord.FileName, Path.Combine(defaultRecordPath, currentRecord.FileName));

					var defaultRecord = new ShipParameterRecord();
					defaultRecord.Load(defaultRecordPath);
					var changed = new List<int>();

					foreach (var pair in defaultRecord.Record.Keys.GroupJoin(currentRecord.Record.Keys, i => i, i => i, (id, list) => new { id, list }))
					{
						if (defaultRecord[pair.id].HPMin > 0 && (pair.list == null || defaultRecord[pair.id].SaveLine() != currentRecord[pair.id].SaveLine()))
							changed.Add(pair.id);
					}

					foreach (var id in changed)
					{
						if (currentRecord[id] == null)
							currentRecord.Update(new ShipParameterRecord.ShipParameterElement());
						currentRecord[id].LoadLine(defaultRecord.Record[id].SaveLine());
					}

					currentRecord.SaveAll(RecordManager.Instance.MasterPath);

					Directory.Delete(defaultRecordPath, true);


				}
				catch (Exception ex)
				{
					ErrorReporter.SendErrorReport(ex, "Failed to reorganize ShipParameterRecord.");
				}

			}


		}


		// version 2.6.2 or earlier
		if (dt <= DateTimeHelper.CSVStringToTime("2017/05/07 23:00:00"))
		{

			// 開発レコードを重複記録してしまう不具合があったため、重複行の削除を行う

			try
			{

				var dev = new DevelopmentRecord();
				string path = RecordManager.Instance.MasterPath + "\\" + dev.FileName;


				string backupPath = RecordManager.Instance.MasterPath + "\\Backup_" + DateTimeHelper.GetTimeStamp();
				Directory.CreateDirectory(backupPath);
				File.Copy(path, backupPath + "\\" + dev.FileName);


				if (File.Exists(path))
				{

					var lines = new List<string>();
					using (StreamReader sr = new StreamReader(path, Utility.Configuration.Config.Log.FileEncoding))
					{
						sr.ReadLine();      // skip header row
						while (!sr.EndOfStream)
							lines.Add(sr.ReadLine());
					}

					int beforeCount = lines.Count;
					lines = lines.Distinct().ToList();
					int afterCount = lines.Count;

					using (StreamWriter sw = new StreamWriter(path, false, Utility.Configuration.Config.Log.FileEncoding))
					{
						sw.WriteLine(dev.RecordHeader);
						foreach (var line in lines)
						{
							sw.WriteLine(line);
						}
					}

					Utility.Logger.Add(2, "<= ver. 2.6.2 開発レコード重複不具合対応: 正常に完了しました。 " + (beforeCount - afterCount) + " 件の重複を削除しました。");

				}

			}
			catch (Exception ex)
			{
				ErrorReporter.SendErrorReport(ex, "<= ver. 2.6.2 開発レコード重複不具合対応: 失敗しました。");
			}
		}


		// version 2.8.2 or earlier
		if (dt <= DateTimeHelper.CSVStringToTime("2017/10/17 20:30:00"))
			Update282_ConvertRecord();

		if (dt <= DateTimeHelper.CSVStringToTime("2018/02/11 23:00:00"))
			Update307_ConvertRecord();

		if (dt <= DateTimeHelper.CSVStringToTime("2018/08/17 23:00:00"))
			Update312_RemoveObsoleteRegistry();

		if (dt <= DateTimeHelper.CSVStringToTime("2020/06/07 23:00:00"))
			Update460_AddSallyAreaColorScheme();

		if (dt <= DateTimeHelper.CSVStringToTime("2024/11/24 20:00:00"))
			Update538_ChangeTsunDbConfig();

		if (dt <= DateTimeHelper.CSVStringToTime("2025/03/22 11:00:00"))
			Update5_3_12_ChangeRpcIconProperty();

		Config.VersionUpdateTime = DateTimeHelper.TimeToCSVString(SoftwareInformation.UpdateTime);
	}


	private void Update282_ConvertRecord()
	{
		// 敵編成レコード：ハッシュ計算が変わり、項目が増えたため引き継ぎ不能、バックアップを取っておく
		// ドロップ記録レコード：〃　編成IDを 0x0 で初期化する


		// for retry
		do
		{
			try
			{
				var fleet = new EnemyFleetRecord();
				string fleetPath = RecordManager.Instance.MasterPath + "\\" + fleet.FileName;

				var drop = new ShipDropRecord();
				string dropPath = RecordManager.Instance.MasterPath + "\\" + drop.FileName;


				string backupDirectoryPath = RecordManager.Instance.MasterPath + "\\Backup_" + DateTimeHelper.GetTimeStamp();


				Directory.CreateDirectory(backupDirectoryPath);


				// enemy fleet record
				if (File.Exists(fleetPath))
				{
					bool isNewVersion;
					try
					{
						using (var reader = new StreamReader(fleetPath, Utility.Configuration.Config.Log.FileEncoding))
							isNewVersion = reader.ReadLine() == fleet.RecordHeader;
					}
					catch (Exception)
					{
						isNewVersion = false;
					}


					if (!isNewVersion)
					{
						File.Move(fleetPath, backupDirectoryPath + "\\" + fleet.FileName);
					}
					else
					{
						Utility.Logger.Add(1, "~2.8.2 Record conversion: Enemy Fleet Record is already in the latest format. Skipped.");
					}
				}


				// copy default record
				if (!File.Exists(fleetPath))
					Resource.ResourceManager.CopyDocumentFromArchive("Record/" + fleet.FileName, fleetPath);


				// drop record
				if (File.Exists(dropPath))
				{
					bool isNewVersion;
					try
					{
						using (var reader = new StreamReader(dropPath, Utility.Configuration.Config.Log.FileEncoding))
						{
							reader.ReadLine();
							isNewVersion = reader.ReadLine().Split(",".ToCharArray())[12].Length == 16;
						}
					}
					catch (Exception)
					{
						isNewVersion = false;
					}


					if (!isNewVersion)
					{
						File.Copy(dropPath, backupDirectoryPath + "\\" + drop.FileName);


						drop.Load(RecordManager.Instance.MasterPath);
						foreach (var r in drop.Record)
							r.EnemyFleetID = 0;

						drop.SaveAll(RecordManager.Instance.MasterPath);
					}
					else
					{
						Utility.Logger.Add(1, "~2.8.2 Record conversion: Drop Record already in the latest format. Skipped.");
					}
				}


				// 何もバックアップしなくてよかった時
				if (!Directory.EnumerateFiles(backupDirectoryPath).Any())
					Directory.Delete(backupDirectoryPath);


				Utility.Logger.Add(2, "~2.8.2 Record conversion: completed successfully.");

			}
			catch (Exception ex)
			{
				Utility.ErrorReporter.SendErrorReport(ex, "~2.8.2 Record conversion: failed.");

				if (MessageBox.Show($"An error occurred during record conversion to the latest version.\r\n\r\n{ex.Message}\r\n\r\nDo you want to try again?\r\n(Selecting 'No' may result in loss of recorded data.)",
						"~2.8.2 Record conversion:" + ex.GetType().Name, MessageBoxButtons.YesNo, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1)
					== DialogResult.Yes)
					continue;
				else
					break;
			}
		} while (false);


	}


	/// <summary>
	/// 難易度設定がずれたため、ハッシュ値が変更された
	/// それに対する対応・移行処理を行う
	/// </summary>
	private void Update307_ConvertRecord()
	{
		try
		{

			var fleet = new EnemyFleetRecord();
			string fleetPath = RecordManager.Instance.MasterPath + "\\" + fleet.FileName;

			var drop = new ShipDropRecord();
			string dropPath = RecordManager.Instance.MasterPath + "\\" + drop.FileName;


			string backupDirectoryPath = RecordManager.Instance.MasterPath + "\\Backup_" + DateTimeHelper.GetTimeStamp();



			Directory.CreateDirectory(backupDirectoryPath);
			File.Copy(fleetPath, backupDirectoryPath + "\\" + fleet.FileName);
			File.Copy(dropPath, backupDirectoryPath + "\\" + drop.FileName);


			var hashremap = new Dictionary<ulong, ulong>();

			using (var reader = new StreamReader(RecordManager.Instance.MasterPath + "\\" + fleet.FileName, Config.Log.FileEncoding))
			{
				reader.ReadLine();      // skip header

				while (!reader.EndOfStream)
				{
					string line = reader.ReadLine();

					var data = new EnemyFleetRecord.EnemyFleetElement(line);

					ulong oldhash = Convert.ToUInt64(line.Substring(0, 16), 16);
					ulong newhash = data.FleetID;

					if (oldhash != newhash)
					{
						hashremap.Add(oldhash, newhash);
						int diff = data.Difficulty;

						switch (diff)
						{
							case 2: diff = 1; break;        // 1(丁)が誤って "丙" と記録されている → ロードすると 2 になるので、1 に再設定
							case 3: diff = 2; break;
							case 4: diff = 3; break;
							case -1: diff = 4; break;       // 4(甲)は "不明" → ロードすると -1 になるので、 4 に
						}

						data = new EnemyFleetRecord.EnemyFleetElement(data.FleetName, data.MapAreaID, data.MapInfoID, data.CellID, diff, data.Formation, data.FleetMember, data.FleetMemberLevel, data.ExpShip);
					}

					// 敵連合艦隊データのフォーマットが誤っていた([6] == -1になって、[7]から随伴艦隊が始まる)ので、捨てなければならない :(
					bool rotten = data.FleetMember[6] == -1 && data.FleetMember[7] != -1;

					if (!rotten)
						fleet.Record.Add(data.FleetID, data);

				}
			}

			fleet.SaveAll(RecordManager.Instance.MasterPath);


			drop.Load(RecordManager.Instance.MasterPath);

			foreach (var d in drop.Record)
			{
				if (hashremap.ContainsKey(d.EnemyFleetID))
				{
					d.EnemyFleetID = hashremap[d.EnemyFleetID];

					int diff = d.Difficulty;
					switch (diff)
					{
						case 2: diff = 1; break;
						case 3: diff = 2; break;
						case 4: diff = 3; break;
						case -1: diff = 4; break;
					}

					d.Difficulty = diff;
				}
			}

			drop.SaveAll(RecordManager.Instance.MasterPath);


			Utility.Logger.Add(2, "<= ver. 3.0.7 changes to records due to difficulty level changes: Operation completed successfully.");

		}
		catch (Exception ex)
		{
			ErrorReporter.SendErrorReport(ex, "<= ver. 3.0.7 changes to records due to difficulty level changes: Operation failed.");
		}

	}


	private void Update312_RemoveObsoleteRegistry()
	{
		// ;)
		Config.FormBrowser.ZoomRate = 1;


		string RegistryPathMaster = @"Software\Microsoft\Internet Explorer\Main\FeatureControl\";
		string RegistryPathBrowserVersion = @"FEATURE_BROWSER_EMULATION\";
		string RegistryPathGPURendering = @"FEATURE_GPU_RENDERING\";


		try
		{
			using (var reg = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegistryPathMaster + RegistryPathBrowserVersion, true))
				reg.DeleteValue(Window.FormBrowserHost.BrowserExeName);

			using (var reg = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RegistryPathMaster + RegistryPathGPURendering, true))
				reg.DeleteValue(Window.FormBrowserHost.BrowserExeName);

		}
		catch (Exception ex)
		{
			Utility.ErrorReporter.SendErrorReport(ex, "<= ver. 3.1.2 移行処理: 古いレジストリ値の削除に失敗しました。");
		}
	}

	private void Update460_AddSallyAreaColorScheme()
	{
		if (Config.FormFleet.SallyAreaColorScheme.SequenceEqual(Config.FormFleet.DefaultSallyAreaColorScheme.Take(8)))
		{
			Config.FormFleet.SallyAreaColorScheme = Config.FormFleet.DefaultSallyAreaColorScheme.ToList();
			Utility.Logger.Add(1, "<= ver. 4.6.0 移行処理: カラースキームの追加が完了しました。");
		}
	}

	private void Update538_ChangeTsunDbConfig()
	{
		Config.DataSubmission.SubmitDataToTsunDb = Config.Control.SubmitDataToTsunDb is true;
	}

	private void Update5_3_12_ChangeRpcIconProperty()
	{
		Config.Control.RpcIconKind = Config.Control.UseFlagshipIconForRPC ? RpcIconKind.Secretary : RpcIconKind.Default;
	}
}
