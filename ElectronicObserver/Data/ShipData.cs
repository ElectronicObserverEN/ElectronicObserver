﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Attacks;
using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.KancolleApi.Types.Models;
using ElectronicObserver.Utility.Data;

namespace ElectronicObserver.Data;

/// <summary>
/// 個別の艦娘データを保持します。
/// </summary>
public class ShipData : APIWrapper, IIdentifiable, IShipData
{



	/// <summary>
	/// 艦娘を一意に識別するID
	/// </summary>
	public int MasterID => (int)RawData.api_id;

	/// <summary>
	/// 並べ替えの順番
	/// </summary>
	public int SortID => (int)RawData.api_sortno;

	/// <summary>
	/// 艦船ID
	/// </summary>
	public int ShipID => (int)RawData.api_ship_id;

	/// <summary>
	/// レベル
	/// </summary>
	public int Level => (int)RawData.api_lv;

	/// <summary>
	/// 累積経験値
	/// </summary>
	public int ExpTotal => (int)RawData.api_exp[0];

	/// <summary>
	/// 次のレベルに達するために必要な経験値
	/// </summary>
	public int ExpNext => (int)RawData.api_exp[1];

	/// <inheritdoc />
	public double ExpNextPercentage => (ExpTable.ShipExp.ContainsKey(Level + 1) && Level != 99) switch
	{
		true => (ExpTable.ShipExp[Level].Next - ExpNext) / (double)ExpTable.ShipExp[Level].Next,
		_ => 0
	};


	/// <summary>
	/// 耐久現在値
	/// </summary>
	public int HPCurrent { get; set; }

	/// <summary>
	/// 耐久最大値
	/// </summary>
	public int HPMax => (int)RawData.api_maxhp;


	/// <summary>
	/// 速力
	/// </summary>
	public int Speed => RawData.api_soku() ? (int)RawData.api_soku : MasterShip.Speed;

	/// <summary>
	/// 射程
	/// </summary>
	public int Range => (int)RawData.api_leng;


	/// <summary>
	/// 装備スロット(ID)
	/// </summary>
	public IList<int> Slot { get; private set; }


	/// <summary>
	/// 装備スロット(マスターID)
	/// </summary>
	public IList<int> SlotMaster => Array.AsReadOnly(Slot.Select(id => KCDatabase.Instance.Equipments[id]?.EquipmentID ?? -1).ToArray());

	/// <summary>
	/// 装備スロット(装備データ)
	/// </summary>
	public IList<IEquipmentData?> SlotInstance => Array.AsReadOnly(Slot.Select(id => KCDatabase.Instance.Equipments[id]).Cast<IEquipmentData?>().ToArray());

	/// <summary>
	/// 装備スロット(装備マスターデータ)
	/// </summary>
	public IList<IEquipmentDataMaster?> SlotInstanceMaster => Array.AsReadOnly(Slot.Select(id => KCDatabase.Instance.Equipments[id]?.MasterEquipment).ToArray());


	/// <summary>
	/// 補強装備スロット(ID)
	/// 0=未開放, -1=装備なし 
	/// </summary>
	public int ExpansionSlot { get; private set; }

	/// <summary>
	/// 補強装備スロット(マスターID)
	/// </summary>
	public int ExpansionSlotMaster => ExpansionSlot == 0 ? 0 : (KCDatabase.Instance.Equipments[ExpansionSlot]?.EquipmentID ?? -1);

	/// <summary>
	/// 補強装備スロット(装備データ)
	/// </summary>
	public IEquipmentData? ExpansionSlotInstance => KCDatabase.Instance.Equipments[ExpansionSlot];

	/// <summary>
	/// 補強装備スロット(装備マスターデータ)
	/// </summary>
	public IEquipmentDataMaster? ExpansionSlotInstanceMaster => KCDatabase.Instance.Equipments[ExpansionSlot]?.MasterEquipment;


	/// <summary>
	/// 全てのスロット(ID)
	/// </summary>
	public IList<int> AllSlot => Array.AsReadOnly(Slot.Concat(new[] { ExpansionSlot }).ToArray());

	/// <summary>
	/// 全てのスロット(マスターID)
	/// </summary>
	public IList<int> AllSlotMaster => Array.AsReadOnly(AllSlot.Select(id => KCDatabase.Instance.Equipments[id]?.EquipmentID ?? -1).ToArray());

	/// <summary>
	/// 全てのスロット(マスターID)
	/// </summary>
	public IList<int> AllSlotMasterReplay => Array.AsReadOnly(AllSlot.Select(id => KCDatabase.Instance.Equipments[id]?.EquipmentID ?? 0).ToArray());

	/// <summary>
	/// 全てのスロット(装備データ)
	/// </summary>
	public IList<IEquipmentData?> AllSlotInstance => Array.AsReadOnly(AllSlot.Select(id => KCDatabase.Instance.Equipments[id]).Cast<IEquipmentData>().ToArray());

	/// <summary>
	/// 全てのスロット(装備マスターデータ)
	/// </summary>
	public IList<IEquipmentDataMaster?> AllSlotInstanceMaster => Array.AsReadOnly(AllSlot.Select(id => KCDatabase.Instance.Equipments[id]?.MasterEquipment).ToArray());



	private int[] _aircraft;
	/// <summary>
	/// 各スロットの航空機搭載量
	/// </summary>
	public IList<int> Aircraft => Array.AsReadOnly(_aircraft);


	/// <summary>
	/// 現在の航空機搭載量
	/// </summary>
	public int AircraftTotal => _aircraft.Sum(a => Math.Max(a, 0));


	/// <summary>
	/// 搭載燃料
	/// </summary>
	public int Fuel { get; set; }

	/// <summary>
	/// 搭載弾薬
	/// </summary>
	public int Ammo { get; set; }


	/// <summary>
	/// スロットのサイズ
	/// </summary>
	public int SlotSize => !RawData.api_slotnum() ? 0 : (int)RawData.api_slotnum;

	/// <summary>
	/// 入渠にかかる時間(ミリ秒)
	/// </summary>
	public int RepairTime => (int)RawData.api_ndock_time;

	/// <summary>
	/// Time needed to repair 1 HP
	/// </summary>
	public TimeSpan RepairTimeUnit => Calculator.CalculateDockingUnitTime(this);

	/// <summary>
	/// 入渠にかかる鋼材
	/// </summary>
	public int RepairSteel => (int)RawData.api_ndock_item[1];

	/// <summary>
	/// 入渠にかかる燃料
	/// </summary>
	public int RepairFuel => (int)RawData.api_ndock_item[0];

	/// <summary>
	/// コンディション
	/// </summary>
	public int Condition { get; internal set; }


	#region Parameters

	/********************************************************
	 * 強化値：近代化改修・レベルアップによって上昇した数値
	 * 総合値：装備込みでのパラメータ
	 * 基本値：装備なしでのパラメータ(初期値+強化値)
	 ********************************************************/

	private int[] _modernized;

	public int[] Kyouka => (int[])RawData.api_kyouka;
	/// <summary>
	/// 火力強化値
	/// </summary>
	public int FirepowerModernized => _modernized.Length >= 5 ? _modernized[0] : 0;

	/// <summary>
	/// 雷装強化値
	/// </summary>
	public int TorpedoModernized => _modernized.Length >= 5 ? _modernized[1] : 0;

	/// <summary>
	/// 対空強化値
	/// </summary>
	public int AAModernized => _modernized.Length >= 5 ? _modernized[2] : 0;

	/// <summary>
	/// 装甲強化値
	/// </summary>
	public int ArmorModernized => _modernized.Length >= 5 ? _modernized[3] : 0;

	/// <summary>
	/// 運強化値
	/// </summary>
	public int LuckModernized => _modernized.Length >= 5 ? _modernized[4] : 0;

	/// <summary>
	/// 耐久強化値
	/// </summary>
	public int HPMaxModernized => _modernized.Length >= 7 ? _modernized[5] : 0;

	/// <summary>
	/// 対潜強化値
	/// </summary>
	public int ASWModernized => _modernized.Length >= 7 ? _modernized[6] : 0;


	/// <summary>
	/// 火力改修残り
	/// </summary>
	public int FirepowerRemain => (MasterShip.FirepowerMax - MasterShip.FirepowerMin) - FirepowerModernized;

	/// <summary>
	/// 雷装改修残り
	/// </summary>
	public int TorpedoRemain => (MasterShip.TorpedoMax - MasterShip.TorpedoMin) - TorpedoModernized;

	/// <summary>
	/// 対空改修残り
	/// </summary>
	public int AARemain => (MasterShip.AAMax - MasterShip.AAMin) - AAModernized;

	/// <summary>
	/// 装甲改修残り
	/// </summary>
	public int ArmorRemain => (MasterShip.ArmorMax - MasterShip.ArmorMin) - ArmorModernized;

	/// <summary>
	/// 運改修残り
	/// </summary>
	public int LuckRemain => (MasterShip.LuckMax - MasterShip.LuckMin) - LuckModernized;

	/// <summary>
	/// 耐久改修残り
	/// </summary>
	public int HPMaxRemain => (IsMarried ? MasterShip.HPMaxMarriedModernizable : MasterShip.HPMaxModernizable) - HPMaxModernized;

	/// <summary>
	/// 対潜改修残り
	/// </summary>
	public int ASWRemain => ASWMax <= 0 ? 0 : MasterShip.ASWModernizable - ASWModernized;


	/// <summary>
	/// 火力総合値
	/// </summary>
	public int FirepowerTotal => (int)RawData.api_karyoku[0];

	/// <summary>
	/// 雷装総合値
	/// </summary>
	public int TorpedoTotal => (int)RawData.api_raisou[0];

	/// <summary>
	/// 対空総合値
	/// </summary>
	public int AATotal => (int)RawData.api_taiku[0];

	/// <summary>
	/// 装甲総合値
	/// </summary>
	public int ArmorTotal => (int)RawData.api_soukou[0];

	/// <summary>
	/// 回避総合値
	/// </summary>
	public int EvasionTotal => (int)RawData.api_kaihi[0];

	/// <summary>
	/// 対潜総合値
	/// </summary>
	public int ASWTotal => (int)RawData.api_taisen[0];

	/// <summary>
	/// 索敵総合値
	/// </summary>
	public int LOSTotal => (int)RawData.api_sakuteki[0];

	/// <summary>
	/// 運総合値
	/// </summary>
	public int LuckTotal => (int)RawData.api_lucky[0];

	/// <summary>
	/// 爆装総合値
	/// </summary>
	public int BomberTotal => AllSlotInstanceMaster.Sum(eq => eq?.Bomber ?? 0);

	/// <summary>
	/// 命中総合値
	/// </summary>
	public int AccuracyTotal => AllSlotInstanceMaster.Sum(eq => eq?.Accuracy ?? 0);


	/// <summary>
	/// 火力基本値
	/// </summary>
	public int FirepowerBase => MasterShip.FirepowerMin + FirepowerModernized;


	/// <summary>
	/// 雷装基本値
	/// </summary>
	public int TorpedoBase => MasterShip.TorpedoMin + TorpedoModernized;


	/// <summary>
	/// 対空基本値
	/// </summary>
	public int AABase => MasterShip.AAMin + AAModernized;


	/// <summary>
	/// 装甲基本値
	/// </summary>
	public int ArmorBase => MasterShip.ArmorMin + ArmorModernized;


	/// <summary>
	/// 回避基本値
	/// </summary>
	public int EvasionBase
	{
		get
		{
			if (MasterShip.Evasion?.IsDetermined ?? false)
				return MasterShip.Evasion.GetParameter(Level);

			// パラメータ上限下限が分かっていれば上ので確実に取れる
			// 不明な場合は以下で擬似計算（装備分を引くだけ）　装備シナジーによって上昇している場合誤差が発生します
			return EvasionTotal - AllSlotInstance.Sum(eq => eq?.MasterEquipment?.Evasion ?? 0);
		}
	}

	/// <summary>
	/// 対潜基本値
	/// </summary>
	public int ASWBase
	{
		get
		{
			if (MasterShip.ASW?.IsDetermined ?? false)
				return MasterShip.ASW.GetParameter(Level) + ASWModernized;

			return ASWTotal - AllSlotInstance.Sum(eq => eq?.MasterEquipment?.ASW ?? 0);
		}
	}

	/// <summary>
	/// 索敵基本値
	/// </summary>
	public int LOSBase
	{
		get
		{
			if (MasterShip.LOS?.IsDetermined ?? false)
				return MasterShip.LOS.GetParameter(Level);

			return LOSTotal - AllSlotInstance.Sum(eq => eq?.MasterEquipment?.LOS ?? 0);
		}
	}

	/// <summary>
	/// 運基本値
	/// </summary>
	public int LuckBase => MasterShip.LuckMin + LuckModernized;



	/// <summary>
	/// 回避最大値
	/// </summary>
	public int EvasionMax => (int)RawData.api_kaihi[1];

	/// <summary>
	/// 対潜最大値
	/// </summary>
	public int ASWMax => (int)RawData.api_taisen[1];

	/// <summary>
	/// 索敵最大値
	/// </summary>
	public int LOSMax => (int)RawData.api_sakuteki[1];

	/// <summary>
	/// Bonus items applied to that ship
	/// </summary>
	public List<SpecialEffectItem> SpecialEffectItems { get; private set; } = new();
	
	/// <summary>
	/// Bonus firepower from special items
	/// </summary>
	public int SpecialEffectItemFirepower => SpecialEffectItems.Sum(item => item.Firepower);

	/// <summary>
	/// Bonus torpedo from special items
	/// </summary>
	public int SpecialEffectItemTorpedo => SpecialEffectItems.Sum(item => item.Torpedo);

	/// <summary>
	/// Bonus armor from special items
	/// </summary>
	public int SpecialEffectItemArmor => SpecialEffectItems.Sum(item => item.Armor);

	/// <summary>
	/// Bonus evasion from special items
	/// </summary>
	public int SpecialEffectItemEvasion => SpecialEffectItems.Sum(item => item.Evasion);

	#endregion


	/// <summary>
	/// 保護ロックの有無
	/// </summary>
	public bool IsLocked => (int)RawData.api_locked != 0;

	/// <summary>
	/// 装備による保護ロックの有無
	/// </summary>
	public bool IsLockedByEquipment => (int)RawData.api_locked_equip != 0;


	/// <summary>
	/// 出撃海域
	/// 0 - no lock, > 0 - lock ID
	/// </summary>
	public int SallyArea => RawData.api_sally_area() ? (int)RawData.api_sally_area : 0;



	/// <summary>
	/// 艦船のマスターデータへの参照
	/// </summary>
	public IShipDataMaster MasterShip => KCDatabase.Instance.MasterShips[ShipID];


	/// <summary>
	/// 入渠中のドックID　非入渠時は-1
	/// </summary>
	public int RepairingDockID => KCDatabase.Instance.Docks.Values.FirstOrDefault(dock => dock.ShipID == MasterID)?.DockID ?? -1;


	/// <summary>
	/// 所属艦隊　-1=なし
	/// </summary>
	public int Fleet => KCDatabase.Instance.Fleet.Fleets.Values.FirstOrDefault(f => f.Members.Contains(MasterID))?.FleetID ?? -1;



	/// <summary>
	/// 所属艦隊及びその位置
	/// ex. 1-3 (位置も1から始まる)
	/// 所属していなければ 空文字列
	/// </summary>
	public string FleetWithIndex
	{
		get
		{
			FleetManager fm = KCDatabase.Instance.Fleet;
			foreach (var f in fm.Fleets.Values)
			{
				int index = f.Members.IndexOf(MasterID);
				if (index != -1)
				{
					return $"{f.FleetID}-{index + 1}";
				}
			}
			return "";
		}

	}


	/// <summary>
	/// ケッコン済みかどうか
	/// </summary>
	public bool IsMarried => Level > 99;


	/// <summary>
	/// 次の改装まで必要な経験値
	/// </summary>
	public int ExpNextRemodel
	{
		get
		{
			IShipDataMaster master = MasterShip;
			if (master.RemodelAfterShipID <= 0)
				return 0;
			return Math.Max(ExpTable.ShipExp[master.RemodelAfterLevel].Total - ExpTotal, 0);
		}
	}


	/// <summary>
	/// 艦名
	/// </summary>
	public string Name => MasterShip.NameEN;


	/// <summary>
	/// 艦名(レベルを含む)
	/// </summary>
	public string NameWithLevel => $"{MasterShip.NameEN} Lv. {Level}";


	/// <summary>
	/// HP/HPmax
	/// </summary>
	public double HPRate => HPMax > 0 ? (double)HPCurrent / HPMax : 0;

	public DamageState DamageState => this.GetDamageState();

	/// <summary>
	/// 最大搭載燃料
	/// </summary>
	public int FuelMax => MasterShip.Fuel;

	/// <summary>
	/// 最大搭載弾薬
	/// </summary>
	public int AmmoMax => MasterShip.Ammo;


	/// <summary>
	/// 燃料残量割合
	/// </summary>
	public double FuelRate => (double)Fuel / Math.Max(FuelMax, 1);

	/// <summary>
	/// 弾薬残量割合
	/// </summary>
	public double AmmoRate => (double)Ammo / Math.Max(AmmoMax, 1);


	/// <summary>
	/// 補給で消費する燃料
	/// </summary>
	public int SupplyFuel => (FuelMax - Fuel) == 0 ? 0 : Math.Max((int)Math.Floor((FuelMax - Fuel) * (IsMarried ? 0.85 : 1)), 1);

	/// <summary>
	/// 補給で消費する弾薬
	/// </summary>
	public int SupplyAmmo => (AmmoMax - Ammo) == 0 ? 0 : Math.Max((int)Math.Floor((AmmoMax - Ammo) * (IsMarried ? 0.85 : 1)), 1);


	/// <summary>
	/// 搭載機残量割合
	/// </summary>
	public IList<double> AircraftRate
	{
		get
		{
			double[] airs = new double[_aircraft.Length];
			var airmax = MasterShip.Aircraft;

			for (int i = 0; i < airs.Length; i++)
			{
				airs[i] = (double)_aircraft[i] / Math.Max(airmax[i], 1);
			}

			return Array.AsReadOnly(airs);
		}
	}

	/// <summary>
	/// 搭載機残量割合
	/// </summary>
	public double AircraftTotalRate => (double)AircraftTotal / Math.Max(MasterShip.AircraftTotal, 1);



	/// <summary>
	/// 補強装備スロットが使用可能か
	/// </summary>
	public bool IsExpansionSlotAvailable => ExpansionSlot != 0;
	/// <summary>
	/// FirePower Total for Expeditions
	/// </summary>
	public int ExpeditionFirepowerTotal { get; private set; }
	/// <summary>
	/// ASW Total For Expeditions
	/// </summary>
	public int ExpeditionASWTotal { get; private set; }
	/// <summary>
	/// LOS Total for Expeditions
	/// </summary>
	public int ExpeditionLOSTotal { get; private set; }
	/// <summary>
	/// AA Total for Expeditions
	/// </summary>
	public int ExpeditionAATotal { get; private set; }
	#region ダメージ威力計算

	/// <summary>
	/// 航空戦威力
	/// 本来スロットごとのものであるが、ここでは最大火力を採用する
	/// </summary>
	public int AirBattlePower => _airbattlePowers.Max();

	private int[] _airbattlePowers;
	/// <summary>
	/// 各スロットの航空戦威力
	/// </summary>
	public IList<int> AirBattlePowers => Array.AsReadOnly(_airbattlePowers);

	/// <summary>
	/// 砲撃威力
	/// </summary>
	public int ShellingPower { get; private set; }

	//todo: ShellingPower に統合予定
	/// <summary>
	/// 空撃威力
	/// </summary>
	public int AircraftPower { get; private set; }

	/// <summary>
	/// 対潜威力
	/// </summary>
	public int AntiSubmarinePower { get; private set; }

	/// <summary>
	/// 雷撃威力
	/// </summary>
	public int TorpedoPower { get; private set; }

	/// <summary>
	/// 夜戦威力
	/// </summary>
	public int NightBattlePower { get; private set; }



	/// <summary>
	/// 装備改修補正(砲撃戦)
	/// </summary>
	private double GetDayBattleEquipmentLevelBonus()
	{

		double basepower = 0;
		foreach (var slot in AllSlotInstance)
		{
			if (slot == null)
				continue;

			switch (slot.MasterEquipment.CategoryType)
			{
				case EquipmentTypes.MainGunSmall:
				case EquipmentTypes.MainGunMedium:
				case EquipmentTypes.APShell:
				case EquipmentTypes.AADirector:
				case EquipmentTypes.Searchlight:
				case EquipmentTypes.SearchlightLarge:
				case EquipmentTypes.AAGun:
				case EquipmentTypes.LandingCraft:
				case EquipmentTypes.SpecialAmphibiousTank:
					basepower += Math.Sqrt(slot.Level);
					break;

				case EquipmentTypes.MainGunLarge:
				case EquipmentTypes.MainGunLarge2:
					basepower += Math.Sqrt(slot.Level) * 1.5;
					break;

				case EquipmentTypes.SecondaryGun:
					switch (slot.EquipmentID)
					{
						case 10:        // 12.7cm連装高角砲
						case 66:        // 8cm高角砲
						case 220:       // 8cm高角砲改+増設機銃
						case 275:       // 10cm連装高角砲改+増設機銃
							basepower += 0.2 * slot.Level;
							break;

						case 12:        // 15.5cm三連装副砲
						case 234:       // 15.5cm三連装副砲改
							basepower += 0.3 * slot.Level;
							break;

						default:
							basepower += Math.Sqrt(slot.Level);
							break;
					}
					break;

				case EquipmentTypes.Sonar:
				case EquipmentTypes.SonarLarge:
					basepower += Math.Sqrt(slot.Level) * 0.75;
					break;

				case EquipmentTypes.DepthCharge:
					if (!slot.MasterEquipment.IsDepthCharge)
						basepower += Math.Sqrt(slot.Level) * 0.75;
					break;

			}
		}
		return basepower;
	}



	/// <summary>
	/// 装備改修補正(雷撃戦)
	/// </summary>
	private double GetTorpedoEquipmentLevelBonus()
	{
		double basepower = 0;
		foreach (var slot in AllSlotInstance)
		{
			if (slot == null)
				continue;

			switch (slot.MasterEquipment.CategoryType)
			{
				case EquipmentTypes.Torpedo:
				case EquipmentTypes.AAGun:
				case EquipmentTypes.SubmarineTorpedo:
					basepower += Math.Sqrt(slot.Level) * 1.2;
					break;
			}
		}
		return basepower;
	}

	/// <summary>
	/// 装備改修補正(対潜)
	/// </summary>
	private double GetAntiSubmarineEquipmentLevelBonus()
	{
		double basepower = 0;
		foreach (var slot in AllSlotInstance)
		{
			if (slot == null)
				continue;

			switch (slot.MasterEquipment.CategoryType)
			{
				case EquipmentTypes.DepthCharge:
				case EquipmentTypes.Sonar:
					basepower += Math.Sqrt(slot.Level);
					break;
			}
		}
		return basepower;
	}

	/// <summary>
	/// 装備改修補正(夜戦)
	/// </summary>
	private double GetNightBattleEquipmentLevelBonus()
	{
		double basepower = 0;
		foreach (var slot in AllSlotInstance)
		{
			if (slot == null)
				continue;

			switch (slot.MasterEquipment.CategoryType)
			{
				case EquipmentTypes.MainGunSmall:
				case EquipmentTypes.MainGunMedium:
				case EquipmentTypes.MainGunLarge:
				case EquipmentTypes.Torpedo:
				case EquipmentTypes.APShell:
				case EquipmentTypes.LandingCraft:
				case EquipmentTypes.Searchlight:
				case EquipmentTypes.SubmarineTorpedo:
				case EquipmentTypes.AADirector:
				case EquipmentTypes.MainGunLarge2:
				case EquipmentTypes.SearchlightLarge:
				case EquipmentTypes.SpecialAmphibiousTank:
					basepower += Math.Sqrt(slot.Level);
					break;

				case EquipmentTypes.SecondaryGun:
					switch (slot.EquipmentID)
					{
						case 10:        // 12.7cm連装高角砲
						case 66:        // 8cm高角砲
						case 220:       // 8cm高角砲改+増設機銃
						case 275:       // 10cm連装高角砲改+増設機銃
							basepower += 0.2 * slot.Level;
							break;

						case 12:        // 15.5cm三連装副砲
						case 234:       // 15.5cm三連装副砲改
							basepower += 0.3 * slot.Level;
							break;

						default:
							basepower += Math.Sqrt(slot.Level);
							break;
					}
					break;
			}
		}
		return basepower;
	}

	/// <summary>
	/// 耐久値による攻撃力補正
	/// </summary>
	private double GetHPDamageBonus()
	{
		if (HPRate <= 0.25)
			return 0.4;
		else if (HPRate <= 0.5)
			return 0.7;
		else
			return 1.0;
	}

	/// <summary>
	/// 耐久値による攻撃力補正(雷撃戦)
	/// </summary>
	/// <returns></returns>
	private double GetTorpedoHPDamageBonus()
	{
		if (HPRate <= 0.25)
			return 0.0;
		else if (HPRate <= 0.5)
			return 0.8;
		else
			return 1.0;
	}

	/// <summary>
	/// 交戦形態による威力補正
	/// </summary>
	private double GetEngagementFormDamageRate(int form)
	{
		switch (form)
		{
			case 1:     // 同航戦
			default:
				return 1.0;
			case 2:     // 反航戦
				return 0.8;
			case 3:     // T字有利
				return 1.2;
			case 4:     // T字不利
				return 0.6;
		}
	}

	/// <summary>
	/// 残り弾薬量による威力補正
	/// <returns></returns>
	private double GetAmmoDamageRate()
	{
		return Math.Min(Math.Floor(AmmoRate * 100) / 50.0, 1.0);
	}

	/// <summary>
	/// 連合艦隊編成における砲撃戦火力補正
	/// </summary>
	private double GetCombinedFleetShellingDamageBonus()
	{
		int fleet = Fleet;
		if (fleet == -1 || fleet > 2)
			return 0;

		switch (KCDatabase.Instance.Fleet.CombinedFlag)
		{
			case FleetType.Carrier:
				if (fleet == 1)
					return +2;
				else
					return +10;

			case FleetType.Surface:
				if (fleet == 1)
					return +10;
				else
					return -5;

			case FleetType.Transport:
				if (fleet == 1)
					return -5;
				else
					return +10;

			default:
				return 0;
		}
	}

	/// <summary>
	/// 連合艦隊編成における雷撃戦火力補正
	/// </summary>
	private double GetCombinedFleetTorpedoDamageBonus()
	{
		int fleet = Fleet;
		if (fleet == -1 || fleet > 2)
			return 0;

		if (KCDatabase.Instance.Fleet.CombinedFlag == 0)
			return 0;

		return -5;
	}

	/// <summary>
	/// 軽巡軽量砲補正
	/// </summary>
	private double GetLightCruiserDamageBonus()
	{
		if (MasterShip.ShipType == ShipTypes.LightCruiser ||
			MasterShip.ShipType == ShipTypes.TorpedoCruiser ||
			MasterShip.ShipType == ShipTypes.TrainingCruiser)
		{

			int single = 0;
			int twin = 0;

			foreach (var slot in AllSlotMaster)
			{
				if (slot == -1) continue;

				switch (slot)
				{
					case 4:     // 14cm単装砲
					case 11:    // 15.2cm単装砲
						single++;
						break;
					case 65:    // 15.2cm連装砲
					case 119:   // 14cm連装砲
					case 139:   // 15.2cm連装砲改
						twin++;
						break;
				}
			}

			return Math.Sqrt(twin) * 2.0 + Math.Sqrt(single);
		}

		return 0;
	}

	/// <summary>
	/// イタリア重巡砲補正
	/// </summary>
	/// <returns></returns>
	private double GetItalianDamageBonus()
	{
		switch (ShipID)
		{
			case 448:       // Zara
			case 358:       // 改
			case 496:       // due
			case 449:       // Pola
			case 361:       // 改
				return Math.Sqrt(AllSlotMaster.Count(id => id == 162));     // √( 203mm/53 連装砲 装備数 )

			default:
				return 0;
		}
	}

	private double CapDamage(double damage, int max)
	{
		if (damage < max)
			return damage;
		else
			return max + Math.Sqrt(damage - max);
	}


	/// <summary>
	/// 航空戦での威力を求めます。
	/// </summary>
	/// <param name="slotIndex">スロットのインデックス。 0 起点です。</param>
	private int CalculateAirBattlePower(int slotIndex)
	{
		double basepower = 0;
		var slots = AllSlotInstance;

		var eq = SlotInstance[slotIndex];

		if (eq == null || _aircraft[slotIndex] == 0)
			return 0;

		switch (eq.MasterEquipment.CategoryType)
		{
			case EquipmentTypes.CarrierBasedBomber:
			case EquipmentTypes.SeaplaneBomber:
			case EquipmentTypes.JetBomber:              // 通常航空戦においては /√2 されるが、とりあえず考えない
				basepower = eq.MasterEquipment.Bomber * Math.Sqrt(_aircraft[slotIndex]) + 25;
				break;
			case EquipmentTypes.CarrierBasedTorpedo:
			case EquipmentTypes.JetTorpedo:
				// 150% 補正を引いたとする
				basepower = (eq.MasterEquipment.Torpedo * Math.Sqrt(_aircraft[slotIndex]) + 25) * 1.5;
				break;
			default:
				return 0;
		}

		//キャップ
		basepower = Math.Floor(CapDamage(basepower, 170));

		return (int)(basepower * GetAmmoDamageRate());
	}

	/// <summary>
	/// 砲撃戦での砲撃威力を求めます。
	/// </summary>
	/// <param name="engagementForm">交戦形態。既定値は 1 (同航戦) です。</param>
	private int CalculateShellingPower(int engagementForm = 1)
	{
		var attackKind = Calculator.GetDayAttackKind(AllSlotMaster.ToArray(), ShipID, -1);
		if (attackKind == DayAttackKind.AirAttack || attackKind == DayAttackKind.CutinAirAttack)
			return 0;


		double basepower = FirepowerTotal + GetDayBattleEquipmentLevelBonus() + GetCombinedFleetShellingDamageBonus() + 5;

		basepower *= GetHPDamageBonus() * GetEngagementFormDamageRate(engagementForm);

		basepower += GetLightCruiserDamageBonus() + GetItalianDamageBonus();

		// キャップ
		basepower = Math.Floor(CapDamage(basepower, 220));

		// 弾着観測射撃
		switch (attackKind)
		{
			case DayAttackKind.DoubleShelling:
			case DayAttackKind.CutinMainRadar:
				basepower *= 1.2;
				break;
			case DayAttackKind.CutinMainSub:
				basepower *= 1.1;
				break;
			case DayAttackKind.CutinMainAP:
				basepower *= 1.3;
				break;
			case DayAttackKind.CutinMainMain:
				basepower *= 1.5;
				break;
			case DayAttackKind.ZuiunMultiAngle:
				basepower *= 1.35;
				break;
			case DayAttackKind.SeaAirMultiAngle:
				basepower *= 1.3;
				break;

		}

		return (int)(basepower * GetAmmoDamageRate());
	}
	
	/// <summary>
	/// 砲撃戦での空撃威力を求めます。
	/// </summary>
	/// <param name="engagementForm">交戦形態。既定値は 1 (同航戦) です。</param>
	private int CalculateAircraftPower(int engagementForm = 1)
	{
		var attackKind = Calculator.GetDayAttackKind(AllSlotMaster.ToArray(), ShipID, -1);
		if (attackKind != DayAttackKind.AirAttack && attackKind != DayAttackKind.CutinAirAttack)
			return 0;

		double basepower = Math.Floor((FirepowerTotal + TorpedoTotal + Math.Floor(BomberTotal * 1.3) + GetDayBattleEquipmentLevelBonus() + GetCombinedFleetShellingDamageBonus()) * 1.5) + 55;

		basepower *= GetHPDamageBonus() * GetEngagementFormDamageRate(engagementForm);

		// キャップ
		basepower = Math.Floor(CapDamage(basepower, 220));


		// 空母カットイン
		if (attackKind == DayAttackKind.CutinAirAttack)
		{
			var kind = Calculator.GetDayAirAttackCutinKind(SlotInstanceMaster);
			switch (kind)
			{
				case DayAirAttackCutinKind.FighterBomberAttacker:
					basepower *= 1.25;
					break;

				case DayAirAttackCutinKind.BomberBomberAttacker:
					basepower *= 1.20;
					break;

				case DayAirAttackCutinKind.BomberAttacker:
					basepower *= 1.15;
					break;
			}
		}

		return (int)(basepower * GetAmmoDamageRate());
	}

	/// <summary>
	/// 砲撃戦での対潜威力を求めます。
	/// </summary>
	/// <param name="engagementForm">交戦形態。既定値は 1 (同航戦) です。</param>
	private int CalculateAntiSubmarinePower(int engagementForm = 1)
	{
		if (!CanAttackSubmarine)
			return 0;

		double eqpower = 0;
		foreach (var slot in AllSlotInstance)
		{
			if (slot == null)
				continue;

			switch (slot.MasterEquipment.CategoryType)
			{
				case EquipmentTypes.CarrierBasedBomber:
				case EquipmentTypes.CarrierBasedTorpedo:
				case EquipmentTypes.SeaplaneBomber:
				case EquipmentTypes.Sonar:
				case EquipmentTypes.DepthCharge:
				case EquipmentTypes.Autogyro:
				case EquipmentTypes.ASPatrol:
				case EquipmentTypes.SonarLarge:
					eqpower += slot.MasterEquipment.ASW;
					break;
			}
		}

		double basepower = Math.Sqrt(ASWBase) * 2 + eqpower * 1.5 + GetAntiSubmarineEquipmentLevelBonus();
		if (Calculator.GetDayAttackKind(AllSlotMaster.ToArray(), ShipID, 126, false) == DayAttackKind.AirAttack)
		{       //126=伊168; 対潜攻撃が空撃なら
			basepower += 8;
		}
		else
		{   //爆雷攻撃なら
			basepower += 13;
		}


		basepower *= GetHPDamageBonus() * GetEngagementFormDamageRate(engagementForm);


		//対潜シナジー

		int depthChargeCount = 0;
		int depthChargeProjectorCount = 0;
		int otherDepthChargeCount = 0;
		int sonarCount = 0;         // ソナーと大型ソナーの合算
		int largeSonarCount = 0;

		foreach (var slot in AllSlotInstanceMaster)
		{
			if (slot == null)
				continue;

			switch (slot.CategoryType)
			{
				case EquipmentTypes.Sonar:
					sonarCount++;
					break;
				case EquipmentTypes.DepthCharge:
					if (slot.IsDepthCharge)
						depthChargeCount++;
					else if (slot.IsDepthChargeProjector)
						depthChargeProjectorCount++;
					else
						otherDepthChargeCount++;
					break;
				case EquipmentTypes.SonarLarge:
					largeSonarCount++;
					sonarCount++;
					break;
			}
		}

		double synergy = 1.0;
		if (sonarCount > 0 && depthChargeProjectorCount > 0 && depthChargeCount > 0)
			synergy = 1.4375;
		else if (sonarCount > 0 && (depthChargeCount + depthChargeProjectorCount + otherDepthChargeCount) > 0)
			synergy = 1.15;
		else if (depthChargeProjectorCount > 0 && depthChargeCount > 0)
			synergy = 1.1;

		basepower *= synergy;


		//キャップ
		basepower = Math.Floor(CapDamage(basepower, 170));

		return (int)(basepower * GetAmmoDamageRate());
	}

	/// <summary>
	/// 雷撃戦での威力を求めます。
	/// </summary>
	/// <param name="engagementForm">交戦形態。既定値は 1 (同航戦) です。</param>
	private int CalculateTorpedoPower(int engagementForm = 1)
	{
		if (TorpedoBase == 0)
			return 0;       //雷撃不能艦は除外

		double basepower = TorpedoTotal + GetTorpedoEquipmentLevelBonus() + GetCombinedFleetTorpedoDamageBonus() + 5;

		basepower *= GetTorpedoHPDamageBonus() * GetEngagementFormDamageRate(engagementForm);

		//キャップ
		basepower = Math.Floor(CapDamage(basepower, 180));


		return (int)(basepower * GetAmmoDamageRate());
	}

	/// <summary>
	/// 夜戦での威力を求めます。
	/// </summary>
	private int CalculateNightBattlePower()
	{
		var kind = Calculator.GetNightAttackKind(AllSlotMaster.ToArray(), ShipID, -1);
		double basepower = 0;

		if (kind == NightAttackKind.CutinAirAttack)
		{
			var airs = SlotInstance.Zip(Aircraft, (eq, count) => new { eq, master = eq?.MasterEquipment, count }).Where(a => a.eq != null);

			basepower = FirepowerBase +
						airs.Where(p => p.master.IsNightAircraft)
							.Sum(p => p.master.Firepower + p.master.Torpedo + p.master.Bomber +
									  3 * p.count +
									  0.45 * (p.master.Firepower + p.master.Torpedo + p.master.Bomber + p.master.ASW) * Math.Sqrt(p.count) + Math.Sqrt(p.eq.Level)) +
						airs.Where(p => p.master.IsSwordfish || p.master.EquipmentID == 154 || p.master.EquipmentID == 320)   // 零戦62型(爆戦/岩井隊)、彗星一二型(三一号光電管爆弾搭載機)
							.Sum(p => p.master.Firepower + p.master.Torpedo + p.master.Bomber +
									  0.3 * (p.master.Firepower + p.master.Torpedo + p.master.Bomber + p.master.ASW) * Math.Sqrt(p.count) + Math.Sqrt(p.eq.Level));

		}
		else if (ShipID == 515 || ShipID == 393)
		{       // Ark Royal (改)
			basepower = FirepowerBase + SlotInstanceMaster.Where(eq => eq?.IsSwordfish ?? false).Sum(eq => eq.Firepower + eq.Torpedo);
		}
		else if (ShipID == 353 || ShipID == 432 || ShipID == 433)
		{       // Graf Zeppelin(改), Saratoga
			basepower = FirepowerBase + SlotInstanceMaster.Where(eq => eq?.IsSwordfish ?? false).Sum(eq => eq.Firepower + eq.Torpedo);
		}
		else
		{
			basepower = FirepowerTotal + TorpedoTotal + GetNightBattleEquipmentLevelBonus();
		}


		basepower *= GetHPDamageBonus();

		switch (kind)
		{
			case NightAttackKind.DoubleShelling:
				basepower *= 1.2;
				break;

			case NightAttackKind.CutinMainTorpedo:
				basepower *= 1.3;
				break;

			case NightAttackKind.CutinTorpedoTorpedo:
			{
				switch (Calculator.GetNightTorpedoCutinKind(AllSlotInstanceMaster, ShipID, -1))
				{
					case NightTorpedoCutinKind.LateModelTorpedoSubmarineEquipment:
						basepower *= 1.75;
						break;
					case NightTorpedoCutinKind.LateModelTorpedo2:
						basepower *= 1.6;
						break;
					default:
						basepower *= 1.5;
						break;
				}
			}
			break;

			case NightAttackKind.CutinMainSub:
				basepower *= 1.75;
				break;

			case NightAttackKind.CutinMainMain:
				basepower *= 2.0;
				break;

			case NightAttackKind.CutinAirAttack:
			{
				int nightFighter = SlotInstanceMaster.Count(eq => eq?.IsNightFighter ?? false);
				int nightAttacker = SlotInstanceMaster.Count(eq => eq?.IsNightAttacker ?? false);
				int nightBomber = SlotInstanceMaster.Count(eq => eq?.EquipmentID == 320);     // 彗星一二型(三一号光電管爆弾搭載機)

				if (nightFighter >= 2 && nightAttacker >= 1)
					basepower *= 1.25;
				else if (nightBomber >= 1 && nightFighter + nightAttacker >= 1)
					basepower *= 1.2;
				else if (nightFighter >= 1 && nightAttacker >= 1)
					basepower *= 1.2;
				else
					basepower *= 1.18;
			}
			break;

			case NightAttackKind.CutinTorpedoRadar:
			{
				double baseModifier = 1.3;
				int typeDmod2 = AllSlotInstanceMaster.Count(eq => eq?.EquipmentID == 267);  // 12.7cm連装砲D型改二
				int typeDmod3 = AllSlotInstanceMaster.Count(eq => eq?.EquipmentID == 366);  // 12.7cm連装砲D型改三
				var modifierTable = new double[] { 1, 1.25, 1.4 };

				baseModifier *= modifierTable[Math.Min(typeDmod2 + typeDmod3, modifierTable.Length - 1)] * (1 + typeDmod3 * 0.05);

				basepower *= baseModifier;
			}

			break;

			case NightAttackKind.CutinTorpedoPicket:
			{
				double baseModifier = 1.25;     // TODO: 処理の共通化
				int typeDmod2 = AllSlotInstanceMaster.Count(eq => eq?.EquipmentID == 267);  // 12.7cm連装砲D型改二
				int typeDmod3 = AllSlotInstanceMaster.Count(eq => eq?.EquipmentID == 366);  // 12.7cm連装砲D型改三
				var modifierTable = new double[] { 1, 1.25, 1.4 };

				baseModifier *= modifierTable[Math.Min(typeDmod2 + typeDmod3, modifierTable.Length - 1)] * (1 + typeDmod3 * 0.05);

				basepower *= baseModifier;
			}
			break;
		}

		basepower += GetLightCruiserDamageBonus() + GetItalianDamageBonus();

		//キャップ
		basepower = Math.Floor(CapDamage(basepower, 360));


		return (int)(basepower * GetAmmoDamageRate());
	}


	/// <summary>
	/// 威力系の計算をまとめて行い、プロパティを更新します。
	/// </summary>
	private void CalculatePowers()
	{

		int form = Utility.Configuration.Config.Control.PowerEngagementForm;

		_airbattlePowers = Slot.Select((_, i) => CalculateAirBattlePower(i)).ToArray();
		ShellingPower = CalculateShellingPower(form);
		AircraftPower = CalculateAircraftPower(form);
		AntiSubmarinePower = CalculateAntiSubmarinePower(form);
		TorpedoPower = CalculateTorpedoPower(form);
		NightBattlePower = CalculateNightBattlePower();
		ExpeditionFirepowerTotal = this.ExpeditionFirepowerTotal();
		ExpeditionASWTotal = this.ExpeditionAswTotal();
		ExpeditionLOSTotal = this.ExpeditionLosTotal();
		ExpeditionAATotal = this.ExpeditionAaTotal();
	}

	#endregion

	/// <summary>
	/// 対潜攻撃可能か
	/// </summary>
	public bool CanAttackSubmarine => this.CanAttackSubmarine();

	/// <summary>
	/// 開幕対潜攻撃可能か
	/// </summary>
	public bool CanOpeningASW => this.CanDoOpeningAsw();

	public bool CanNoSonarOpeningAsw => this.CanNoSonarOpeningAsw();

	/// <summary>
	/// 夜戦攻撃可能か
	/// </summary>
	public bool CanAttackAtNight
	{
		get
		{
			var master = MasterShip;

			if (HPRate <= 0.25)
				return false;

			if (master.FirepowerMin + master.TorpedoMin > 0)
				return true;

			// Ark Royal(改)
			if (master.ShipID == 515 || master.ShipID == 393)
			{
				if (AllSlotInstanceMaster.Any(eq => eq != null && eq.IsSwordfish))
					return true;
			}

			if (master.IsAircraftCarrier)
			{
				// 装甲空母ではなく、中破以上の被ダメージ
				if (master.ShipType != ShipTypes.ArmoredAircraftCarrier && HPRate <= 0.5)
					return false;

				// Saratoga Mk.II/赤城改二戊/加賀改二戊 は不要
				bool hasNightPersonnel = master.ShipID == 545 || master.ShipID == 599 || master.ShipID == 610 ||
										 AllSlotInstanceMaster.Any(eq => eq != null && eq.IsNightAviationPersonnel);

				bool hasNightAircraft = AllSlotInstanceMaster.Any(eq => eq != null && eq.IsNightAircraft);

				if (hasNightPersonnel && hasNightAircraft)
					return true;
			}

			return false;
		}
	}

	public bool CanBeTargeted { get; set; } = true;


	/// <summary>
	/// 発動可能なダメコンのID -1=なし, 42=要員, 43=女神
	/// </summary>
	public int DamageControlID
	{
		get
		{
			if (ExpansionSlotMaster == 42 || ExpansionSlotMaster == 43)
				return ExpansionSlotMaster;

			foreach (var eq in SlotMaster)
			{
				if (eq == 42 || eq == 43)
					return eq;
			}

			return -1;
		}
	}



	public int ID => MasterID;
	public override string ToString() => $"[{MasterID}] {NameWithLevel}";


	public override void LoadFromResponse(string apiname, dynamic data)
	{

		switch (apiname)
		{
			default:
				base.LoadFromResponse(apiname, (object)data);

				HPCurrent = (int)RawData.api_nowhp;
				Fuel = (int)RawData.api_fuel;
				Ammo = (int)RawData.api_bull;
				Condition = (int)RawData.api_cond;
				Slot = Array.AsReadOnly((int[])RawData.api_slot);
				ExpansionSlot = (int)RawData.api_slot_ex;
				_aircraft = (int[])RawData.api_onslot;
				_modernized = (int[])RawData.api_kyouka;

				if (data.api_sp_effect_items())
				{
					string json = data.api_sp_effect_items.ToString();

					SpecialEffectItems = JsonSerializer
						.Deserialize<List<ApiSpEffectItem>>(json)
						?.Select(i => new SpecialEffectItem
						{
							ApiKind = i.ApiKind,
							Firepower = i.ApiHoug,
							Torpedo = i.ApiRaig,
							Armor = i.ApiSouk,
							Evasion = i.ApiKaih,
						}).ToList() ?? new();
				}
				break;

			case "api_req_hokyu/charge":
				Fuel = (int)data.api_fuel;
				Ammo = (int)data.api_bull;
				_aircraft = (int[])data.api_onslot;
				break;
		}

		CalculatePowers();
	}


	public override void LoadFromRequest(string apiname, Dictionary<string, string> data)
	{
		base.LoadFromRequest(apiname, data);

		KCDatabase db = KCDatabase.Instance;

		switch (apiname)
		{
			case "api_req_kousyou/destroyship":
			{
				for (int i = 0; i < Slot.Count; i++)
				{
					if (Slot[i] == -1)
						continue;

					db.Equipments.Remove(Slot[i]);
				}
			}
			break;

			case "api_req_kaisou/open_exslot":
				ExpansionSlot = -1;
				break;
		}
	}


	/// <summary>
	/// 入渠完了時の処理を行います。
	/// </summary>
	internal void Repair()
	{

		HPCurrent = HPMax;
		Condition = Math.Max(Condition, 40);

		RawData.api_ndock_time = 0;
		RawData.api_ndock_item[0] = 0;
		RawData.api_ndock_item[1] = 0;

	}


}
