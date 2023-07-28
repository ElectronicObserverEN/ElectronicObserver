using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using CommunityToolkit.Mvvm.ComponentModel;
using ElectronicObserver.Utility.Storage;
using ElectronicObserver.Window.Control;
using ElectronicObserverTypes;
using PropertyChanged;

namespace ElectronicObserver.Window.Wpf.Fleet.ViewModels;

public class ShipSlotViewModel : ObservableObject
{
	private IEquipmentData? _equipment;
	public Visibility SlotVisibility { get; set; }

	[DoNotNotify]
	public IEquipmentData? Equipment
	{
		get => _equipment;
		set
		{
			_equipment = value;

			Level = Equipment?.Level ?? 0;
			AircraftLevel = Equipment?.AircraftLevel ?? 0;
			NameWithLevel = Equipment?.NameWithLevel ?? "";
			EquipmentIconType = Equipment?.MasterEquipment.IconTypeTyped ?? EquipmentIconType.Nothing;
		}
	}

	public SerializableFont Font { get; set; }
	public bool ShowAircraft { get; set; } = true;

	public int AircraftCurrent { get; set; }
	public int AircraftMax { get; set; }

	public int Level { get; set; }
	public int AircraftLevel { get; set; }
	public string NameWithLevel { get; set; } = "";

	public string LevelString => Level switch
	{
		10 => "★",
		> 0 => $"+{Level}",
		_ => ""
	};

	public string CurrentAircraftString { get; set; } = "";

	public FontFamily FontFamily => new(Font.FontData.FontFamily.Name);
	public float FontSize => Font.FontData.ToSize();

	public System.Drawing.Color EquipmentLevelColor { get; set; }
	public SolidColorBrush EquipmentLevelBrush => EquipmentLevelColor.ToBrush();

	public System.Drawing.Color CurrentAircraftColor { get; set; }
	public SolidColorBrush CurrentAircraftBrush => CurrentAircraftColor.ToBrush();

	public EquipmentIconType EquipmentIconType { get; set; } = EquipmentIconType.Nothing;

	private bool NumericProficiency { get; set; }
	private LevelVisibilityFlag LevelVisibilityFlag { get; set; }

	public int AircraftProficiencyColumn => LevelVisibilityFlag switch
	{
		LevelVisibilityFlag.AircraftLevelOverlay => 0,
		_ => 1
	};

	public bool EquipmentLevelVisible => LevelVisibilityFlag switch
	{
		LevelVisibilityFlag.Invisible => false,
		LevelVisibilityFlag.AircraftLevelOnly => false,

		LevelVisibilityFlag.LevelOnly or
		LevelVisibilityFlag.LevelPriority or
		LevelVisibilityFlag.AircraftLevelOverlay or
		LevelVisibilityFlag.Both => true,

		_ when AircraftLevel is 0 => true,

		_ => false
	};

	public bool AircraftProficiencyVisible => LevelVisibilityFlag switch
	{
		LevelVisibilityFlag.Invisible => false,
		LevelVisibilityFlag.LevelOnly => false,

		LevelVisibilityFlag.AircraftLevelOnly or
		LevelVisibilityFlag.AircraftLevelPriority or
		LevelVisibilityFlag.AircraftLevelOverlay or
		LevelVisibilityFlag.Both => true,

		_ when LevelString is "" => true,

		_ => false
	};

	public bool EquipmentLevelHoverVisible => LevelVisibilityFlag switch
	{
		LevelVisibilityFlag.Invisible or
		LevelVisibilityFlag.AircraftLevelOnly => false,

		LevelVisibilityFlag.LevelPriority when AircraftLevel > 0 => false,
		LevelVisibilityFlag.AircraftLevelPriority when LevelString is "" => false,

		_ => true
	};

	public bool AircraftProficiencyHoverVisible => LevelVisibilityFlag switch
	{
		LevelVisibilityFlag.Invisible or
		LevelVisibilityFlag.LevelOnly => false,

		LevelVisibilityFlag.LevelPriority when AircraftLevel == 0 => false,
		LevelVisibilityFlag.AircraftLevelPriority when LevelString is not "" => false,

		_ => true
	};

	public SolidColorBrush ProficiencyBrush => AircraftLevel switch
	{
		> 3 => HighProficiencyBrush,
		_ => LowProficiencyBrush
	};

	public double ProficiencyScaleX => NumericProficiency switch
	{
		false => Font.FontData.ToSize() * 0.1,
		_ => 1
	};
	public double ProficiencyScaleY => NumericProficiency switch
	{
		false => Font.FontData.ToSize() * 0.04,
		_ => 1
	};

	public object? AircraftLevelContent => (AircraftLevel, NumericProficiency) switch
	{
		( > 0, true) => $"{AircraftLevel}",

		(0, _) => null,
		(1, _) => new Path { Data = Geometry.Parse(Proficiency1Path), Fill = LowProficiencyBrush },
		(2, _) => new Path { Data = Geometry.Parse(Proficiency2Path), Fill = LowProficiencyBrush },
		(3, _) => new Path { Data = Geometry.Parse(Proficiency3Path), Fill = LowProficiencyBrush },
		(4, _) => new Path { Data = Geometry.Parse(Proficiency4Path), Fill = HighProficiencyBrush },
		(5, _) => new Path { Data = Geometry.Parse(Proficiency5Path), Fill = HighProficiencyBrush },
		(6, _) => new Path { Data = Geometry.Parse(Proficiency6Path), Fill = HighProficiencyBrush },
		(7, _) => new Path { Data = Geometry.Parse(Proficiency7Path), Fill = HighProficiencyBrush },

		_ => null
	};

	// copied from ING
	// https://github.com/amatukaze/ing/blob/master/src/Core/Desktop/Sakuno.ING.Views.Desktop.Common/Controls/AerialProficiencyPresenter.cs
	private const string Proficiency1Path = "M0,0 L0,20 2,20 2,0";
	private const string Proficiency2Path = "M0,0 L0,20 2,20 2,0 M4,0 L4,20 6,20 6,0";
	private const string Proficiency3Path = "M0,0 L0,20 2,20 2,0 M4,0 L4,20 6,20 6,0 M8,0 L8,20 10,20 10,0";
	private const string Proficiency4Path = "M0,0 L5,20 7,20 2,0";
	private const string Proficiency5Path = "M0,0 L5,20 7,20 2,0 M5,0 10,20 12,20 7,0";
	private const string Proficiency6Path = "M0,0 L5,20 7,20 2,0 M5,0 10,20 12,20 7,0 M10,0 15,20 17,20 12,0 ";
	private const string Proficiency7Path = "M0,0 L5,10 0,20 2,20 7,10 2,0 M5,0 L10,10 5,20 7,20 12,10 7,0";

	private static SolidColorBrush LowProficiencyBrush { get; } = new(Color.FromRgb(102, 153, 238));
	private static SolidColorBrush HighProficiencyBrush { get; } = new(Color.FromRgb(255, 170, 0));

	public ShipSlotViewModel()
	{
		Font = Utility.Configuration.Config.UI.SubFont;

		AircraftColorDisabled = System.Drawing.Color.FromArgb(0xAA, 0xAA, 0xAA);
		AircraftColorLost = Utility.Configuration.Config.UI.Color_Magenta;
		AircraftColorDamaged = Utility.Configuration.Config.UI.Color_Red;
		AircraftColorFull = Utility.Configuration.Config.UI.ForeColor;

		EquipmentLevelColor = Utility.Configuration.Config.UI.Fleet_EquipmentLevelColor;
		/*
		AircraftLevelColorLow = System.Drawing.Color.FromArgb(0x66, 0x99, 0xEE);
		AircraftLevelColorHigh = System.Drawing.Color.FromArgb(0xFF, 0xAA, 0x00);

		InvalidSlotColor = System.Drawing.Color.FromArgb(0x40, 0xFF, 0x00, 0x00);
		*/

		ConfigurationChanged();

		PropertyChanged += AircraftChange;
		Utility.Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
	}

	private void ConfigurationChanged()
	{
		NumericProficiency = Utility.Configuration.Config.FormFleet.ShowAircraftLevelByNumber;
		LevelVisibilityFlag = Utility.Configuration.Config.FormFleet.EquipmentLevelVisibility;
	}

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

	private void AircraftChange(object sender, PropertyChangedEventArgs e)
	{
		// when EquipmentIconType changes the plane count needs to be redrawn
		// if you equip a plane on a 0 slot, the 0 must be displayed
		// if you equip a non-plane on a 0 slot, the 0 must not be displayed
		// if you don't redraw when switching plane <-> non-plane, the 0 will be displayed incorrectly
		if (e.PropertyName is not (nameof(AircraftCurrent) or nameof(AircraftMax) or nameof(ShowAircraft)
			or nameof(EquipmentIconType)))
		{
			return;
		}

		System.Drawing.Color aircraftColor = AircraftColorDisabled;
		bool drawAircraftSlot = ShowAircraft;

		if (AircraftMax == 0)
		{
			if (Equipment?.MasterEquipment.IsAircraft ?? false)
			{
				aircraftColor = AircraftColorDisabled;
			}
			else
			{
				drawAircraftSlot = false;
			}

		}
		else if (AircraftCurrent == 0)
		{
			aircraftColor = AircraftColorLost;

		}
		else if (AircraftCurrent < AircraftMax)
		{
			aircraftColor = AircraftColorDamaged;

		}
		else if (!(Equipment?.MasterEquipment.IsAircraft ?? false))
		{
			aircraftColor = AircraftColorDisabled;

		}
		else
		{
			aircraftColor = AircraftColorFull;
		}

		CurrentAircraftColor = aircraftColor;

		CurrentAircraftString = drawAircraftSlot switch
		{
			true => $"{AircraftCurrent}",
			_ => ""
		};
	}
}
