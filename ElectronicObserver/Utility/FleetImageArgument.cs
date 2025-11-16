using System.Drawing;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
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
	public int[] FleetIDs { get; private set; } = [];

	/// <summary> 艦隊を横に並べる最大数 </summary>
	[DataMember]
	public int HorizontalFleetCount { get; set; } = 2;

	/// <summary> 艦船を横に並べる最大数 </summary>
	[DataMember]
	public int HorizontalShipCount { get; private set; } = 2;


	/// <summary> HP に応じて中破グラフィックを適用するか </summary>
	[DataMember]
	public bool ReflectDamageGraphic { get; private set; }

	/// <summary> Twitter の画像圧縮を回避する情報を埋め込むか </summary>
	[DataMember]
	public bool AvoidTwitterDeterioration { get; private set; } = true;

	[IgnoreDataMember]
	[JsonIgnore]
	private static string DefaultFontFamily => "Meiryo UI";

	/// <summary> タイトルのフォント </summary>
	[IgnoreDataMember]
	[JsonIgnore]
	public Font TitleFont { get; set; } = new(DefaultFontFamily, 32, FontStyle.Bold, GraphicsUnit.Pixel);

	/// <summary> 大きい文字のフォント(艦隊名など) </summary>
	[IgnoreDataMember]
	[JsonIgnore]
	public Font LargeFont { get; set; } = new(DefaultFontFamily, 24, FontStyle.Regular, GraphicsUnit.Pixel);

	/// <summary> 通常の文字のフォント(艦船・装備など) </summary>
	[IgnoreDataMember]
	[JsonIgnore]
	public Font MediumFont { get; set; } = new(DefaultFontFamily, 16, FontStyle.Regular, GraphicsUnit.Pixel);

	/// <summary> 小さな文字のフォント() </summary>
	[IgnoreDataMember]
	[JsonIgnore]
	public Font SmallFont { get; set; } = new(DefaultFontFamily, 12, FontStyle.Regular, GraphicsUnit.Pixel);

	/// <summary> 通常の英数字フォント(Lvなど) </summary>
	[IgnoreDataMember]
	[JsonIgnore]
	public Font MediumDigitFont { get; set; } = new(DefaultFontFamily, 16, FontStyle.Regular, GraphicsUnit.Pixel);

	/// <summary> 小さな英数字フォント(搭載機数など) </summary>
	[IgnoreDataMember]
	[JsonIgnore]
	public Font SmallDigitFont { get; set; } = new(DefaultFontFamily, 12, FontStyle.Regular, GraphicsUnit.Pixel);


	[DataMember]
	public SerializableFont SerializedTitleFont
	{
		get => TitleFont;
		set => TitleFont = value;
	}

	[DataMember]
	public SerializableFont SerializedLargeFont
	{
		get => LargeFont;
		set => LargeFont = value;
	}

	[DataMember]
	public SerializableFont SerializedMediumFont
	{
		get => MediumFont;
		set => MediumFont = value;
	}

	[DataMember]
	public SerializableFont SerializedSmallFont
	{
		get => SmallFont;
		set => SmallFont = value;
	}

	[DataMember]
	public SerializableFont SerializedMediumDigitFont
	{
		get => MediumDigitFont;
		set => MediumDigitFont = value;
	}

	[DataMember]
	public SerializableFont SerializedSmallDigitFont
	{
		get => SmallDigitFont;
		set => SmallDigitFont = value;
	}


	/// <summary> 背景画像ファイルへのパス(空白の場合描画されません) </summary>
	[DataMember]
	public string BackgroundImagePath { get; set; } = "";


	/// <summary> ユーザ指定のタイトル </summary>
	[DataMember]
	public string Title { get; set; } = "";

	/// <summary> ユーザ指定のコメント </summary>
	[DataMember]
	public string Comment { get; set; } = "";
}
