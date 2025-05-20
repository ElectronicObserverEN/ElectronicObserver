using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using ElectronicObserver.Utility.Storage;

namespace ElectronicObserver.Utility;

/// <summary>
/// FleetImageGenerator クラスのメソッドに与えるパラメータ群を保持します。
/// </summary>
[DataContract(Name = "FleetImageArgument")]
public class FleetImageArgument
{

	/// <summary> 対象となる艦隊IDのリスト </summary>
	[DataMember]
	public int[] FleetIDs;

	/// <summary> 艦隊を横に並べる最大数 </summary>
	[DataMember]
	public int HorizontalFleetCount;

	/// <summary> 艦船を横に並べる最大数 </summary>
	[DataMember]
	public int HorizontalShipCount;


	/// <summary> HP に応じて中破グラフィックを適用するか </summary>
	[DataMember]
	public bool ReflectDamageGraphic;

	/// <summary> Twitter の画像圧縮を回避する情報を埋め込むか </summary>
	[DataMember]
	public bool AvoidTwitterDeterioration;



	/// <summary> タイトルのフォント </summary>
	[IgnoreDataMember]
	public Font TitleFont;

	/// <summary> 大きい文字のフォント(艦隊名など) </summary>
	[IgnoreDataMember]
	public Font LargeFont;

	/// <summary> 通常の文字のフォント(艦船・装備など) </summary>
	[IgnoreDataMember]
	public Font MediumFont;

	/// <summary> 小さな文字のフォント() </summary>
	[IgnoreDataMember]
	public Font SmallFont;

	/// <summary> 通常の英数字フォント(Lvなど) </summary>
	[IgnoreDataMember]
	public Font MediumDigitFont;

	/// <summary> 小さな英数字フォント(搭載機数など) </summary>
	[IgnoreDataMember]
	public Font SmallDigitFont;


	[DataMember]
	public SerializableFont SerializedTitleFont
	{
		get { return TitleFont; }
		set { TitleFont = value; }
	}
	[DataMember]
	public SerializableFont SerializedLargeFont
	{
		get { return LargeFont; }
		set { LargeFont = value; }
	}
	[DataMember]
	public SerializableFont SerializedMediumFont
	{
		get { return MediumFont; }
		set { MediumFont = value; }
	}
	[DataMember]
	public SerializableFont SerializedSmallFont
	{
		get { return SmallFont; }
		set { SmallFont = value; }
	}
	[DataMember]
	public SerializableFont SerializedMediumDigitFont
	{
		get { return MediumDigitFont; }
		set { MediumDigitFont = value; }
	}
	[DataMember]
	public SerializableFont SerializedSmallDigitFont
	{
		get { return SmallDigitFont; }
		set { SmallDigitFont = value; }
	}


	/// <summary> 背景画像ファイルへのパス(空白の場合描画されません) </summary>
	[DataMember]
	public string BackgroundImagePath;


	/// <summary> ユーザ指定のタイトル </summary>
	[DataMember]
	public string Title;

	/// <summary> ユーザ指定のコメント </summary>
	[DataMember]
	public string Comment;



	public FleetImageArgument()
	{
		BackgroundImagePath = "";
		Title = "";
		Comment = "";
	}


	public static FleetImageArgument GetDefaultInstance()
	{
		var ret = new FleetImageArgument
		{
			FleetIDs = new int[0],
			HorizontalFleetCount = 2,
			HorizontalShipCount = 2,
			AvoidTwitterDeterioration = true,

			Fonts = GetDefaultFonts()
		};

		return ret;
	}

	public static readonly string DefaultFontFamily = "Meiryo UI";
	public static readonly float[] DefaultFontPixels = new float[] { 32, 24, 16, 12, 16, 12 };

	public static Font[] GetDefaultFonts()
	{
		var fonts = new Font[DefaultFontPixels.Length];
		for (int i = 0; i < fonts.Length; i++)
		{
			fonts[i] = new Font(DefaultFontFamily, DefaultFontPixels[i], i == 0 ? FontStyle.Bold : FontStyle.Regular, GraphicsUnit.Pixel);
		}
		return fonts;
	}

	public FleetImageArgument Clone()
	{

		var clone = (FleetImageArgument)MemberwiseClone();

		clone.FleetIDs = FleetIDs.ToArray();

		clone.Fonts = Fonts.Select(f => (Font)f?.Clone()).ToArray();

		return clone;
	}


	public void DisposeResources()
	{
		foreach (var font in Fonts)
		{
			if (font != null)
				font.Dispose();
		}
	}


	public Font[] Fonts
	{
		get
		{
			return new Font[] {
				TitleFont,
				LargeFont,
				MediumFont,
				SmallFont,
				MediumDigitFont,
				SmallDigitFont,
			};
		}
		set
		{
			TitleFont = value[0];
			LargeFont = value[1];
			MediumFont = value[2];
			SmallFont = value[3];
			MediumDigitFont = value[4];
			SmallDigitFont = value[5];
		}

	}

}
