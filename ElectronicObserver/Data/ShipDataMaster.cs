using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.Resource.Record;

namespace ElectronicObserver.Data;

/// <summary>
/// 艦船のマスターデータを保持します。
/// </summary>
public class ShipDataMaster : ResponseWrapper, IIdentifiable, IShipDataMaster
{

	/// <summary>
	/// 艦船ID
	/// </summary>
	public int ShipID => (int)RawData.api_id;

	/// <summary>
	/// 艦船ID
	/// </summary>
	public ShipId ShipId => (ShipId)RawData.api_id;

	/// <summary>
	/// 図鑑番号
	/// </summary>
	public int AlbumNo => !RawData.api_sortno() ? 0 : (int)RawData.api_sortno;

	/// <summary>
	/// 母港ソート順
	/// </summary>
	public int SortID => !RawData.api_sort_id() ? 0 : (int)RawData.api_sort_id;

	/// <summary>
	/// 名前
	/// </summary>
	public string Name => RawData.api_name;

	/// <summary>
	/// Localized name
	/// </summary>
	public string NameEN => KCDatabase.Instance.Translation.Ship.Name(RawData.api_name, ShipId);

	/// <summary>
	/// 読み
	/// </summary>
	public string NameReading => RawData.api_yomi() switch
	{
		true => RawData.api_yomi,
		_ => "",
	};
	public string NameReadingEN => Utility.Configuration.Config.UI.JapaneseShipName ? NameReading : NameEN;

	/// <summary>
	/// 艦種
	/// </summary>
	public ShipTypes ShipType => (ShipTypes)(int)RawData.api_stype;

	/// <summary>
	/// 艦型
	/// </summary>
	public int ShipClass => (int)RawData.api_ctype;

	/// <summary>
	/// 艦型
	/// </summary>
	public ShipClass ShipClassTyped => (ShipClass)ShipClass;


	/// <summary>
	/// 改装Lv.
	/// </summary>
	public int RemodelAfterLevel => !RawData.api_afterlv() ? 0 : (int)RawData.api_afterlv;

	/// <summary>
	/// 改装後の艦船ID
	/// 0=なし
	/// </summary>
	public int RemodelAfterShipID => !RawData.api_aftershipid() ? 0 : int.Parse((string)RawData.api_aftershipid);

	/// <summary>
	/// 改装後の艦船
	/// </summary>
	public IShipDataMaster? RemodelAfterShip => RemodelAfterShipID > 0 ? KCDatabase.Instance.MasterShips[RemodelAfterShipID] : null;


	/// <summary>
	/// 改装前の艦船ID
	/// 0=なし
	/// </summary>
	public int RemodelBeforeShipID { get; set; }

	/// <summary>
	/// 改装前の艦船
	/// </summary>
	public IShipDataMaster? RemodelBeforeShip => RemodelBeforeShipID > 0 ? KCDatabase.Instance.MasterShips[RemodelBeforeShipID] : null;


	/// <summary>
	/// 改装に必要な弾薬
	/// </summary>
	public int RemodelAmmo => !RawData.api_afterbull() ? 0 : (int)RawData.api_afterbull;

	/// <summary>
	/// 改装に必要な鋼材
	/// </summary>
	public int RemodelSteel => !RawData.api_afterfuel() ? 0 : (int)RawData.api_afterfuel;

	/// <summary>
	/// 改装に必要な 改装設計図 の枚数
	/// </summary>
	public int NeedBlueprint { get; set; }

	/// <summary>
	/// 改装に必要な 試製甲板カタパルト の個数
	/// </summary>
	public int NeedCatapult { get; set; }

	/// <summary>
	/// 改装に必要な 戦闘詳報 の枚数
	/// </summary>
	public int NeedActionReport { get; set; }

	/// <summary>
	/// 改装に必要な 新型航空兵装資材 の個数
	/// </summary>
	public int NeedAviationMaterial { get; set; }

	/// <summary>
	/// 改装に必要な 新型兵装資材 の個数
	/// </summary>
	public int NeedArmamentMaterial { get; set; }



	#region Parameters

	/// <summary>
	/// 耐久初期値
	/// </summary>
	public int HPMin
	{
		get
		{
			if (RawData.api_taik())
			{
				return (int)RawData.api_taik[0];
			}
			else
			{
				return GetParameterElement()?.HPMin ?? 0;
			}
		}
	}

	/// <summary>
	/// 耐久最大値
	/// </summary>
	public int HPMax
	{
		get
		{
			if (RawData.api_taik())
			{
				return (int)RawData.api_taik[1];
			}
			else
			{
				return GetParameterElement()?.HPMax ?? 0;
			}
		}
	}

	/// <summary>
	/// 装甲初期値
	/// </summary>
	public int ArmorMin
	{
		get
		{
			if (RawData.api_souk())
			{
				return (int)RawData.api_souk[0];
			}
			else
			{
				return GetParameterElement()?.ArmorMin ?? 0;
			}
		}
	}

	/// <summary>
	/// 装甲最大値
	/// </summary>
	public int ArmorMax
	{
		get
		{
			if (RawData.api_souk())
			{
				return (int)RawData.api_souk[1];
			}
			else
			{
				return GetParameterElement()?.ArmorMax ?? 0;
			}
		}
	}

	/// <summary>
	/// 火力初期値
	/// </summary>
	public int FirepowerMin
	{
		get
		{
			if (RawData.api_houg())
			{
				return (int)RawData.api_houg[0];
			}
			else
			{
				return GetParameterElement()?.FirepowerMin ?? 0;
			}
		}
	}

	/// <summary>
	/// 火力最大値
	/// </summary>
	public int FirepowerMax
	{
		get
		{
			if (RawData.api_houg())
			{
				return (int)RawData.api_houg[1];
			}
			else
			{
				return GetParameterElement()?.FirepowerMax ?? 0;
			}
		}
	}

	/// <summary>
	/// 雷装初期値
	/// </summary>
	public int TorpedoMin
	{
		get
		{
			if (RawData.api_raig())
			{
				return (int)RawData.api_raig[0];
			}
			else
			{
				return GetParameterElement()?.TorpedoMin ?? 0;
			}
		}
	}

	/// <summary>
	/// 雷装最大値
	/// </summary>
	public int TorpedoMax
	{
		get
		{
			if (RawData.api_raig())
			{
				return (int)RawData.api_raig[1];
			}
			else
			{
				return GetParameterElement()?.TorpedoMax ?? 0;
			}
		}
	}

	/// <summary>
	/// 対空初期値
	/// </summary>
	public int AAMin
	{
		get
		{
			if (RawData.api_tyku())
			{
				return (int)RawData.api_tyku[0];
			}
			else
			{
				return GetParameterElement()?.AAMin ?? 0;
			}
		}
	}

	/// <summary>
	/// 対空最大値
	/// </summary>
	public int AAMax
	{
		get
		{
			if (RawData.api_tyku())
			{
				return (int)RawData.api_tyku[1];
			}
			else
			{
				return GetParameterElement()?.AAMax ?? 0;
			}
		}
	}


	/// <summary>
	/// 対潜
	/// </summary>
	public IParameter? ASW => GetParameterElement()?.ASW;

	/// <summary>
	/// 回避
	/// </summary>
	public IParameter? Evasion => GetParameterElement()?.Evasion;

	/// <summary>
	/// 索敵
	/// </summary>
	public IParameter? LOS => GetParameterElement()?.LOS;


	/// <summary>
	/// 運初期値
	/// </summary>
	public int LuckMin
	{
		get
		{
			if (RawData.api_luck())
			{
				return (int)RawData.api_luck[0];
			}
			else
			{
				return GetParameterElement()?.LuckMin ?? 0;
			}
		}
	}

	/// <summary>
	/// 運最大値
	/// </summary>
	public int LuckMax
	{
		get
		{
			if (RawData.api_luck())
			{
				return (int)RawData.api_luck[1];
			}
			else
			{
				return GetParameterElement()?.LuckMax ?? 0;
			}
		}
	}

	/// <summary>
	/// 速力
	/// 0=陸上基地, 5=低速, 10=高速
	/// </summary>
	public int Speed => (int)RawData.api_soku;

	/// <summary>
	/// 射程
	/// </summary>
	public int Range
	{
		get
		{
			if (RawData.api_leng())
			{
				return (int)RawData.api_leng;
			}
			else
			{
				return GetParameterElement()?.Range ?? 0;
			}
		}
	}
	#endregion


	/// <summary>
	/// 装備スロットの数
	/// </summary>
	public int SlotSize => (int)RawData.api_slot_num;

	/// <summary>
	/// 各スロットの航空機搭載数
	/// </summary>
	public IList<int> Aircraft
	{
		get
		{
			if (RawData.api_maxeq())
			{
				return Array.AsReadOnly((int[])RawData.api_maxeq);
			}
			else
			{
				var p = GetParameterElement();
				if (p != null && p.Aircraft != null)
					return Array.AsReadOnly(p.Aircraft);
				else
					return Array.AsReadOnly(new[] { 0, 0, 0, 0, 0 });
			}
		}
	}

	/// <summary>
	/// 搭載
	/// </summary>
	public int AircraftTotal => Aircraft.Sum(a => Math.Max(a, 0));


	/// <summary>
	/// 初期装備のID
	/// </summary>
	public IList<int> DefaultSlot
	{
		get
		{
			var p = GetParameterElement();
			if (p != null && p.DefaultSlot != null)
				return Array.AsReadOnly(p.DefaultSlot);
			else
				return null;
		}
	}


	/// <summary>
	/// 特殊装備カテゴリ　指定がない場合は null
	/// </summary>
	public IEnumerable<int>? SpecialEquippableCategories { get; set; }

	/// <summary>
	/// 装備可能なカテゴリ
	/// </summary>
	public IEnumerable<int> EquippableCategories => SpecialEquippableCategories 
		?? KCDatabase.Instance.ShipTypes[(int)ShipType].EquippableCategories;

	public IEnumerable<EquipmentTypes> EquippableCategoriesTyped => EquippableCategories.Cast<EquipmentTypes>();

	/// <summary>
	/// 建造時間(分)
	/// </summary>
	public int BuildingTime => !RawData.api_buildtime() ? 0 : (int)RawData.api_buildtime;


	/// <summary>
	/// 解体資材
	/// </summary>
	public IList<int> Material => Array.AsReadOnly(!RawData.api_broken() ? new[] { 0, 0, 0, 0 } : (int[])RawData.api_broken);

	/// <summary>
	/// 近代化改修の素材にしたとき上昇するパラメータの量
	/// </summary>
	public IList<int> PowerUp => Array.AsReadOnly(!RawData.api_powup() ? new[] { 0, 0, 0, 0 } : (int[])RawData.api_powup);

	/// <summary>
	/// レアリティ
	/// </summary>
	public int Rarity => !RawData.api_backs() ? 0 : (int)RawData.api_backs;

	/// <summary>
	/// ドロップ/ログイン時のメッセージ
	/// </summary>
	public string MessageGet => GetParameterElement()?.MessageGet?.Replace("<br>", "\r\n") ?? "";

	/// <summary>
	/// 艦船名鑑でのメッセージ
	/// </summary>
	public string MessageAlbum => GetParameterElement()?.MessageAlbum?.Replace("<br>", "\r\n") ?? "";


	/// <summary>
	/// 搭載燃料
	/// </summary>
	public int Fuel => !RawData.api_fuel_max() ? 0 : (int)RawData.api_fuel_max;

	/// <summary>
	/// 搭載弾薬
	/// </summary>
	public int Ammo => !RawData.api_bull_max() ? 0 : (int)RawData.api_bull_max;


	/// <summary>
	/// ボイス再生フラグ
	/// </summary>
	public int VoiceFlag => !RawData.api_voicef() ? 0 : (int)RawData.api_voicef;


	/// <summary>
	/// グラフィック設定データへの参照
	/// </summary>
	public IShipGraphicData GraphicData => KCDatabase.Instance.ShipGraphics[ShipID];

	/// <summary>
	/// リソースのファイル/フォルダ名
	/// </summary>
	public string ResourceName => GraphicData?.ResourceName ?? "";

	/// <summary>
	/// 画像リソースのバージョン
	/// </summary>
	public string ResourceGraphicVersion => GraphicData?.GraphicVersion ?? "";

	/// <summary>
	/// ボイスリソースのバージョン
	/// </summary>
	public string ResourceVoiceVersion => GraphicData?.VoiceVersion ?? "";

	/// <summary>
	/// 母港ボイスリソースのバージョン
	/// </summary>
	public string ResourcePortVoiceVersion => GraphicData?.PortVoiceVersion ?? "";



	/// <summary>
	/// 衣替え艦：ベースとなる艦船ID
	/// </summary>
	public int OriginalCostumeShipID => GetParameterElement()?.OriginalCostumeShipID ?? -1;



	//以下、自作計算プロパティ群

	public static readonly int HPModernizableLimit = 2;
	public static readonly int ASWModernizableLimit = 9;


	/// <summary>
	/// ケッコンカッコカリ後のHP
	/// </summary>
	public int HPMaxMarried
	{
		get
		{
			int incr;
			if (HPMin < 30) incr = 4;
			else if (HPMin < 40) incr = 5;
			else if (HPMin < 50) incr = 6;
			else if (HPMin < 70) incr = 7;
			else if (HPMin < 90) incr = 8;
			else incr = 9;

			return Math.Min(HPMin + incr, HPMax);
		}
	}

	/// <summary>
	/// HP改修可能値(未婚時)
	/// </summary>
	public int HPMaxModernizable => Math.Min(HPMax - HPMin, HPModernizableLimit);

	/// <summary>
	/// HP改修可能値(既婚時)
	/// </summary>
	public int HPMaxMarriedModernizable => Math.Min(HPMax - HPMaxMarried, HPModernizableLimit);

	/// <summary>
	/// 近代化改修後のHP(未婚時)
	/// </summary>
	public int HPMaxModernized => Math.Min(HPMin + HPMaxModernizable, HPMax);


	/// <summary>
	/// 近代化改修後のHP(既婚時)
	/// </summary>
	public int HPMaxMarriedModernized => Math.Min(HPMaxMarried + HPMaxModernizable, HPMax);



	/// <summary>
	/// 対潜改修可能値
	/// </summary>
	public int ASWModernizable => ASW == null || ASW.Maximum == 0 ? 0 : ASWModernizableLimit;


	/// <summary>
	/// 深海棲艦かどうか
	/// </summary>
	public bool IsAbyssalShip => ShipID > 1500;


	/// <summary>
	/// クラスも含めた艦名
	/// </summary>
	public string NameWithClass
	{
		get
		{
			if (!IsAbyssalShip || NameReading == "" || NameReading == "-")
				return NameEN;
			else
				return $"{NameEN} {NameReading}";
		}
	}

	/// <summary>
	/// 艦種インスタンス
	/// </summary>
	public IShipType ShipTypeInstance => KCDatabase.Instance.ShipTypes[(int)ShipType];

	/// <summary>
	/// 陸上基地かどうか
	/// </summary>
	public bool IsLandBase => Speed == 0;


	/// <summary>
	/// 図鑑に載っているか
	/// </summary>
	public bool IsListedInAlbum => 0 < AlbumNo && AlbumNo <= 420;


	/// <summary>
	/// 改装段階
	/// 初期 = 0, 改 = 1, 改二 = 2, ...
	/// </summary>
	public int RemodelTier
	{
		get
		{
			int tier = 0;
			IShipDataMaster ship = this;
			while (ship.RemodelBeforeShip != null)
			{
				tier++;
				ship = ship.RemodelBeforeShip;
			}

			return tier;
		}
	}

	/// <summary>
	/// 改装段階
	/// 初期 = 0, 改 = 1, 改二 = 2, ...
	/// </summary>
	public RemodelTier RemodelTierTyped => (RemodelTier)RemodelTier;

	/// <summary>
	/// 艦種名
	/// </summary>
	public string ShipTypeName => KCDatabase.Instance.ShipTypes[(int)ShipType].NameEN;


	/// <summary>
	/// 潜水艦系か (潜水艦/潜水空母)
	/// </summary>
	public bool IsSubmarine => ShipType == ShipTypes.Submarine || ShipType == ShipTypes.SubmarineAircraftCarrier;

	/// <summary>
	/// 空母系か (軽空母/正規空母/装甲空母)
	/// </summary>
	public bool IsAircraftCarrier => ShipType == ShipTypes.LightAircraftCarrier || ShipType == ShipTypes.AircraftCarrier || ShipType == ShipTypes.ArmoredAircraftCarrier;

	/// <summary>
	/// Regular Carrier (CV/CVB)
	/// </summary>
	public bool IsRegularCarrier => ShipType == ShipTypes.AircraftCarrier || ShipType == ShipTypes.ArmoredAircraftCarrier;

	/// <summary>
	/// 護衛空母か
	/// </summary>
	public bool IsEscortAircraftCarrier => ShipType == ShipTypes.LightAircraftCarrier && ASW.Minimum > 0;

	public bool IsPt => this.IsPt();


	/// <summary>
	/// 自身のパラメータレコードを取得します。
	/// </summary>
	/// <returns></returns>
	private ShipParameterRecord.ShipParameterElement? GetParameterElement()
	{
		return RecordManager.Instance.ShipParameter[ShipID];
	}



	private static readonly Color[] ShipNameColors = new Color[] {
		Utility.Configuration.Config.UI.ForeColor,
		Utility.Configuration.Config.UI.Compass_ShipNameColor2,
		Utility.Configuration.Config.UI.Compass_ShipNameColor3,
		Utility.Configuration.Config.UI.Compass_ShipNameColor4,
		Utility.Configuration.Config.UI.Compass_ShipNameColor5,
		Utility.Configuration.Config.UI.Compass_ShipNameColor6,
		Utility.Configuration.Config.UI.Compass_ShipNameColor7
	};

	public Color GetShipNameColor()
	{

		if (!IsAbyssalShip)
		{
			return SystemColors.ControlText;
		}

		bool isLateModel = Name.Contains("後期型") || Name.Contains("Late Type");
		bool isRemodeled = Name.Contains("改") || Name.Contains("Kai");
		bool isDestroyed = Name.Contains("-壊") || Name.Contains("Damaged");
		bool isDemon = Name.Contains("鬼") || Name.Contains("Demon");
		bool isPrincess = Name.Contains("姫") || Name.Contains("Princess");
		bool isWaterDemon = Name.Contains("水鬼") || Name.Contains("Water Demon");
		bool isWaterPrincess = Name.Contains("水姫") || Name.Contains("Water Princess");
		bool isElite = NameReading == "elite";
		bool isFlagship = NameReading == "flagship";


		if (isDestroyed)
			return Utility.Configuration.Config.UI.Compass_ShipNameColorDestroyed;

		else if (isWaterPrincess)
			return ShipNameColors[6];
		else if (isWaterDemon)
			return ShipNameColors[5];
		else if (isPrincess)
			return ShipNameColors[4];
		else if (isDemon)
			return ShipNameColors[3];
		else
		{

			int tier;

			if (isFlagship)
				tier = 2;
			else if (isElite)
				tier = 1;
			else
				tier = 0;

			if (isLateModel || isRemodeled)
				tier += 3;

			return ShipNameColors[tier];
		}
	}


	public ShipDataMaster()
	{
		RemodelBeforeShipID = 0;
	}

	public IShipDataMaster BaseShip() => ShipDataExtensions.BaseShip(this);

	public int ID => ShipID;


	public override string ToString() => $"[{ShipID}] {NameWithClass}";


}
