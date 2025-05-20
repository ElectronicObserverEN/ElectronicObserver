using System.Drawing;
using System.Linq;
using System.Runtime.Serialization;
using ElectronicObserver.Utility.Storage;

namespace ElectronicObserver.Utility;

/// <summary>
/// FleetImageGenerator �N���X�̃��\�b�h�ɗ^����p�����[�^�Q��ێ����܂��B
/// </summary>
[DataContract(Name = "FleetImageArgument")]
public class FleetImageArgument
{

	/// <summary> �ΏۂƂȂ�͑�ID�̃��X�g </summary>
	[DataMember]
	public int[] FleetIDs;

	/// <summary> �͑������ɕ��ׂ�ő吔 </summary>
	[DataMember]
	public int HorizontalFleetCount;

	/// <summary> �͑D�����ɕ��ׂ�ő吔 </summary>
	[DataMember]
	public int HorizontalShipCount;


	/// <summary> HP �ɉ����Ē��j�O���t�B�b�N��K�p���邩 </summary>
	[DataMember]
	public bool ReflectDamageGraphic;

	/// <summary> Twitter �̉摜���k�����������𖄂ߍ��ނ� </summary>
	[DataMember]
	public bool AvoidTwitterDeterioration;



	/// <summary> �^�C�g���̃t�H���g </summary>
	[IgnoreDataMember]
	public Font TitleFont;

	/// <summary> �傫�������̃t�H���g(�͑����Ȃ�) </summary>
	[IgnoreDataMember]
	public Font LargeFont;

	/// <summary> �ʏ�̕����̃t�H���g(�͑D�E�����Ȃ�) </summary>
	[IgnoreDataMember]
	public Font MediumFont;

	/// <summary> �����ȕ����̃t�H���g() </summary>
	[IgnoreDataMember]
	public Font SmallFont;

	/// <summary> �ʏ�̉p�����t�H���g(Lv�Ȃ�) </summary>
	[IgnoreDataMember]
	public Font MediumDigitFont;

	/// <summary> �����ȉp�����t�H���g(���ڋ@���Ȃ�) </summary>
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


	/// <summary> �w�i�摜�t�@�C���ւ̃p�X(�󔒂̏ꍇ�`�悳��܂���) </summary>
	[DataMember]
	public string BackgroundImagePath;


	/// <summary> ���[�U�w��̃^�C�g�� </summary>
	[DataMember]
	public string Title;

	/// <summary> ���[�U�w��̃R�����g </summary>
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
