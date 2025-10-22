using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Utility.Storage;
using ElectronicObserver.Window.Control;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels;

// todo all the code here aside from SetSlotList should be useless
public class EquipmentItemViewModel : ObservableObject
{
	public SerializableFont Font { get; set; }

	/// <summary>
	/// 艦載機非搭載スロットの文字色
	/// </summary>
	[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "170, 170, 170")]
	[Description("艦載機非搭載スロットの文字色を指定します。")]
	public System.Drawing.Color AircraftColorDisabled { get; set; }

	/// <summary>
	/// 艦載機全滅スロットの文字色
	/// </summary>
	[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "255, 0, 255")]
	[Description("艦載機全滅スロットの文字色を指定します。")]
	public System.Drawing.Color AircraftColorLost { get; set; }

	/// <summary>
	/// 艦載機被撃墜スロットの文字色
	/// </summary>
	[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "255, 0, 0")]
	[Description("艦載機被撃墜スロットの文字色を指定します。")]
	public System.Drawing.Color AircraftColorDamaged { get; set; }

	/// <summary>
	/// 艦載機満載スロットの文字色
	/// </summary>
	[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "0, 0, 0")]
	[Description("艦載機満載スロットの文字色を指定します。")]
	public System.Drawing.Color AircraftColorFull { get; set; }


	/// <summary>
	/// 改修レベルの色
	/// </summary>
	[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "0, 102, 102")]
	[Description("改修レベルの文字色を指定します。")]
	public System.Drawing.Color EquipmentLevelColor { get; set; }

	/// <summary>
	/// 艦載機熟練度の色 ( Lv. 1 ~ Lv. 3 )
	/// </summary>
	[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "102, 153, 238")]
	[Description("艦載機熟練度の文字色( Lv. 1 ~ 3 )を指定します。")]
	public System.Drawing.Color AircraftLevelColorLow { get; set; }

	/// <summary>
	/// 艦載機熟練度の色 ( Lv. 4 ~ Lv. 7 )
	/// </summary>
	[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "255, 170, 0")]
	[Description("艦載機熟練度の文字色( Lv. 4 ~ 7 )を指定します。")]
	public System.Drawing.Color AircraftLevelColorHigh { get; set; }

	/// <summary>
	/// 不正スロットの背景色
	/// </summary>
	[Browsable(true), Category("Appearance"), DefaultValue(typeof(Color), "64, 255, 0, 0")]
	[Description("不正スロットの文字色を指定します。")]
	public System.Drawing.Color InvalidSlotColor { get; set; }

	public SolidColorBrush InvalidSlotBrush => InvalidSlotColor.ToBrush();


	/// <summary>
	/// 艦載機搭載数を表示するか
	/// </summary>
	[Browsable(true), Category("Behavior"), DefaultValue(true)]
	[Description("艦載機搭載数を表示するかを指定します。")]
	public bool ShowAircraft { get; set; }

	public LevelVisibilityFlag LevelVisibility { get; set; }
	public bool ShowAircraftLevelByNumber { get; set; }
	public string? ToolTip { get; set; }

	public FontFamily FontFamily => new(Font.FontData.Name);
	public float FontSize => Font.FontData.ToSize();

	public List<ShipSlotViewModel> Slots { get; } = new()
	{
		new(),
		new(),
		new(),
		new(),
		new(),
		new(),
	};

	public EquipmentItemViewModel()
	{
		Font = Utility.Configuration.Config.UI.SubFont;

		AircraftColorDisabled = System.Drawing.Color.FromArgb(0xAA, 0xAA, 0xAA);
		AircraftColorLost = Utility.Configuration.Config.UI.Color_Magenta;
		AircraftColorDamaged = Utility.Configuration.Config.UI.Color_Red;
		AircraftColorFull = Utility.Configuration.Config.UI.ForeColor;

		EquipmentLevelColor = Utility.Configuration.Config.UI.Fleet_EquipmentLevelColor;
		AircraftLevelColorLow = System.Drawing.Color.FromArgb(0x66, 0x99, 0xEE);
		AircraftLevelColorHigh = System.Drawing.Color.FromArgb(0xFF, 0xAA, 0x00);

		InvalidSlotColor = System.Drawing.Color.FromArgb(0x40, 0xFF, 0x00, 0x00);

		ShowAircraft = true;
		SetShowAircraft();

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ShowAircraft)) return;

			SetShowAircraft();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(Font)) return;

			foreach (ShipSlotViewModel slot in Slots)
			{
				slot.Font = Font;
			}
		};
	}

	public void SetShowAircraft()
	{
		foreach (ShipSlotViewModel slot in Slots)
		{
			slot.ShowAircraft = ShowAircraft;
		}
	}

	public void SetSlotList(IShipData ship)
	{
		var slots = ship.AllSlotInstance
			.Zip(ship.Aircraft, (eq, s) => (Equipment: eq, CurrentAircraft: s))
			.Zip(ship.MasterShip.Aircraft, (slot, t) => (slot.Equipment, slot.CurrentAircraft, Size: t))
			.ToList();

		for (int i = 0; i < Slots.Count; i++)
		{
			if (i < ship.SlotSize)
			{
				Slots[i].Equipment = slots[i].Equipment;
				Slots[i].AircraftCurrent = slots[i].CurrentAircraft;
				Slots[i].AircraftMax = slots[i].Size;
				Slots[i].SlotVisibility = Visibility.Visible;
			}
			else
			{
				Slots[i].SlotVisibility = Visibility.Collapsed;
			}
		}

		if (ship.IsExpansionSlotAvailable)
		{
			Slots[^1].Equipment = ship.ExpansionSlotInstance;
			Slots[^1].SlotVisibility = Visibility.Visible;
		}
	}

	private void ShipStatusEquipment_Paint()
	{
		/*
		e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
		e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

		Rectangle basearea = new Rectangle(Padding.Left, Padding.Top, Width - Padding.Horizontal, Height - Padding.Vertical);
		//e.Graphics.DrawRectangle( Pens.Magenta, basearea.X, basearea.Y, basearea.Width - 1, basearea.Height - 1 );

		ImageList eqimages = ResourceManager.Instance.Equipments;

		TextFormatFlags textformatBottomRight = GetBaseTextFormat() | TextFormatFlags.Bottom | TextFormatFlags.Right;
		TextFormatFlags textformatTopLeft = GetBaseTextFormat() | TextFormatFlags.Top | TextFormatFlags.Left;
		TextFormatFlags textformatTopRight = GetBaseTextFormat() | TextFormatFlags.Top | TextFormatFlags.Right;


		LayoutParam.UpdateParameters(e.Graphics, basearea.Size, Font);

		*/
		for (int slotindex = 0; slotindex < Slots.Count; slotindex++)
		{
			ShipSlotViewModel slot = Slots[slotindex];
			/*

			Image image = null;

			var origin = new Point(basearea.X + LayoutParam.SlotUnitSize.Width * slotindex, basearea.Y);


			if (slotindex >= SlotSize && slot.EquipmentID != -1)
			{
				//invalid!
				e.Graphics.FillRectangle(_invalidSlotBrush, new Rectangle(origin, LayoutParam.SlotUnitSize));
			}


			if (slot.EquipmentID == -1)
			{
				if (slotindex < SlotSize)
				{
					//nothing
					image = eqimages.Images[(int)ResourceManager.EquipmentContent.Nothing];

				}
				else
				{
					//locked
					image = eqimages.Images[(int)ResourceManager.EquipmentContent.Locked];
				}

			}
			else
			{
				int imageID = slot.EquipmentIconID;
				if (imageID <= 0 || (int)ResourceManager.EquipmentContent.Locked <= imageID)
					imageID = (int)ResourceManager.EquipmentContent.Unknown;

				image = eqimages.Images[imageID];
			}


			Rectangle imagearea = new Rectangle(origin.X, origin.Y + (LayoutParam.SlotUnitSize.Height - LayoutParam.ImageSize.Height) / 2, LayoutParam.ImageSize.Width, LayoutParam.ImageSize.Height);
			if (image != null)
			{

				e.Graphics.DrawImage(image, imagearea);
			}

			*/

			System.Drawing.Color aircraftColor = AircraftColorDisabled;
			bool drawAircraftSlot = ShowAircraft;

			if (slot.AircraftMax == 0)
			{
				if (slot.Equipment?.MasterEquipment.IsAircraft ?? false)
				{
					aircraftColor = AircraftColorDisabled;
				}
				else
				{
					drawAircraftSlot = false;
				}

			}
			else if (slot.AircraftCurrent == 0)
			{
				aircraftColor = AircraftColorLost;

			}
			else if (slot.AircraftCurrent < slot.AircraftMax)
			{
				aircraftColor = AircraftColorDamaged;

			}
			else if (!(slot.Equipment?.MasterEquipment.IsAircraft ?? false))
			{
				aircraftColor = AircraftColorDisabled;

			}
			else
			{
				aircraftColor = AircraftColorFull;
			}

			slot.CurrentAircraftColor = aircraftColor;

			// 艦載機数描画
			if (drawAircraftSlot)
			{
				/*
				Rectangle textarea = new Rectangle(origin.X + LayoutParam.ImageSize.Width, origin.Y + LayoutParam.InfoAreaSize.Height * 3 / 4 - LayoutParam.Digit2Size.Height / 2,
					LayoutParam.InfoAreaSize.Width, LayoutParam.Digit2Size.Height);
				//e.Graphics.DrawRectangle( Pens.Cyan, textarea );

				if (slot.AircraftCurrent < 10)
				{
					//1桁なら画像に近づける

					textarea.Width -= LayoutParam.Digit2Size.Width / 2;

				}
				else if (slot.AircraftCurrent >= 100)
				{
					//3桁以上ならオーバーレイを入れる

					Size sz_realstr = TextRenderer.MeasureText(e.Graphics, slot.AircraftCurrent.ToString(), Font, new Size(int.MaxValue, int.MaxValue), textformatBottomRight);

					textarea.X -= sz_realstr.Width - textarea.Width;
					textarea.Width = sz_realstr.Width;

					e.Graphics.FillRectangle(_overlayBrush, textarea);
				}

				TextRenderer.DrawText(e.Graphics, slot.AircraftCurrent.ToString(), Font, textarea, aircraftColor, textformatBottomRight);
				*/
			}
			/*
			// 改修レベル描画
			if (slot.Level > 0)
			{

				if (LevelVisibility == LevelVisibilityFlag.LevelOnly ||
					LevelVisibility == LevelVisibilityFlag.Both ||
					LevelVisibility == LevelVisibilityFlag.AircraftLevelOverlay ||
					(LevelVisibility == LevelVisibilityFlag.LevelPriority && (!_onMouse || slot.AircraftLevel == 0)) ||
					(LevelVisibility == LevelVisibilityFlag.AircraftLevelPriority && (_onMouse || slot.AircraftLevel == 0)))
				{

					TextRenderer.DrawText(e.Graphics, slot.Level >= 10 ? "★" : "+" + slot.Level, Font,
						new Rectangle(origin.X + LayoutParam.ImageSize.Width, origin.Y + LayoutParam.InfoAreaSize.Height * 1 / 4 - LayoutParam.Digit2Size.Height / 2,
							LayoutParam.InfoAreaSize.Width, LayoutParam.Digit2Size.Height), EquipmentLevelColor, textformatTopRight);

				}

			}
			*/

			slot.EquipmentLevelColor = EquipmentLevelColor;

			/*
			// 艦載機熟練度描画
			if (slot.AircraftLevel > 0)
			{

				if (LevelVisibility == LevelVisibilityFlag.AircraftLevelOnly ||
					LevelVisibility == LevelVisibilityFlag.Both ||
					(LevelVisibility == LevelVisibilityFlag.AircraftLevelPriority && (!_onMouse || slot.Level == 0)) ||
					(LevelVisibility == LevelVisibilityFlag.LevelPriority && (_onMouse || slot.Level == 0)))
				{
					// 右上に描画

					if (ShowAircraftLevelByNumber)
					{
						var area = new Rectangle(origin.X + LayoutParam.ImageSize.Width, origin.Y + LayoutParam.InfoAreaSize.Height * 1 / 4 - LayoutParam.Digit2Size.Height / 2,
							LayoutParam.InfoAreaSize.Width, LayoutParam.Digit2Size.Height);
						TextRenderer.DrawText(e.Graphics, slot.AircraftLevel.ToString(), Font, area, GetAircraftLevelColor(slot.AircraftLevel), textformatTopRight);

					}
					else
					{
						var area = new Rectangle(origin.X + LayoutParam.ImageSize.Width, origin.Y,
							LayoutParam.ImageSize.Width, LayoutParam.ImageSize.Height);
						e.Graphics.DrawImage(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.AircraftLevelTop0 + slot.AircraftLevel], area);
					}


				}
				else if (LevelVisibility == LevelVisibilityFlag.AircraftLevelOverlay)
				{
					// 左上に描画

					if (ShowAircraftLevelByNumber)
					{
						var area = new Rectangle(origin.X, origin.Y, LayoutParam.Digit2Size.Width / 2, LayoutParam.Digit2Size.Height);
						e.Graphics.FillRectangle(_overlayBrush, area);
						TextRenderer.DrawText(e.Graphics, slot.AircraftLevel.ToString(), Font, area, GetAircraftLevelColor(slot.AircraftLevel), textformatTopLeft);

					}
					else
					{
						e.Graphics.FillRectangle(_overlayBrush, new Rectangle(origin.X, origin.Y, LayoutParam.ImageSize.Width, LayoutParam.ImageSize.Height / 2));
						e.Graphics.DrawImage(ResourceManager.Instance.Icons.Images[(int)ResourceManager.IconContent.AircraftLevelTop0 + slot.AircraftLevel], new Rectangle(origin, LayoutParam.ImageSize));
					}
				}

			}
			*/
		}
	}

}
