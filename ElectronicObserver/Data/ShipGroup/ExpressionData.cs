using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using ElectronicObserver.Core;
using ElectronicObserver.Utility.Mathematics;

namespace ElectronicObserver.Data.ShipGroup;

/// <summary>
/// 艦船フィルタの式データ
/// </summary>
[DataContract(Name = "ExpressionData")]
public class ExpressionData : ICloneable
{

	public enum ExpressionOperator
	{
		Equal,
		NotEqual,
		LessThan,
		LessEqual,
		GreaterThan,
		GreaterEqual,
		Contains,
		NotContains,
		BeginWith,
		NotBeginWith,
		EndWith,
		NotEndWith,
		ArrayContains,
		ArrayNotContains,
	}


	[DataMember]
	public string LeftOperand { get; set; }

	[DataMember]
	public ExpressionOperator Operator { get; set; }

	[DataMember]
	public object RightOperand { get; set; }


	[DataMember]
	public bool Enabled { get; set; }


	[IgnoreDataMember]
	private static readonly Regex regex_index = new Regex(@"\.(?<name>\w+)(\[(?<index>\d+?)\])?", RegexOptions.Compiled);

	[IgnoreDataMember]
	public static readonly Dictionary<string, string> LeftOperandNameTable = new Dictionary<string, string>() {
		{ ".MasterID", ExpressionDataRes.MasterID },
		{ ".ShipID", ExpressionDataRes.ShipID },
		{ ".MasterShip.NameWithClass",ExpressionDataRes.NameWithClass },
		{ ".MasterShip.ShipType",ExpressionDataRes.ShipType },
		{ ".Level", ExpressionDataRes.Level },
		{ ".ExpTotal", ExpressionDataRes.ExpTotal },
		{ ".ExpNext", ExpressionDataRes.ExpNext },
		{ ".ExpNextRemodel",ExpressionDataRes.ExpNextRemodel },
		{ ".HPCurrent", ExpressionDataRes.HPCurrent},
		{ ".HPMax", ExpressionDataRes.HPMax },
		{ ".HPRate", ExpressionDataRes.HPRate },
		{ ".Condition", ExpressionDataRes.Condition },
		{ ".AllSlotMaster", ExpressionDataRes.AllSlotMaster },
		{ ".SlotMaster[0]", ExpressionDataRes.SlotMaster1 },	//checkme: 要る?
		{ ".SlotMaster[1]", ExpressionDataRes.SlotMaster2 },
		{ ".SlotMaster[2]", ExpressionDataRes.SlotMaster3 },
		{ ".SlotMaster[3]", ExpressionDataRes.SlotMaster4 },
		{ ".SlotMaster[4]", ExpressionDataRes.SlotMaster5 },
		{ ".ExpansionSlotMaster", ExpressionDataRes.ExpansionSlotMaster },
		{ ".Aircraft[0]", ExpressionDataRes.AircraftSlot1 },
		{ ".Aircraft[1]", ExpressionDataRes.AircraftSlot2 },
		{ ".Aircraft[2]", ExpressionDataRes.AircraftSlot3},
		{ ".Aircraft[3]", ExpressionDataRes.AircraftSlot4 },
		{ ".Aircraft[4]",ExpressionDataRes.AircraftSlot5 },
		{ ".AircraftTotal", ExpressionDataRes.AircraftTotal },
		{ ".MasterShip.Aircraft[0]", ExpressionDataRes.MasterAircraft1 },
		{ ".MasterShip.Aircraft[1]", ExpressionDataRes.MasterAircraft2 },
		{ ".MasterShip.Aircraft[2]", ExpressionDataRes.MasterAircraft3 },
		{ ".MasterShip.Aircraft[3]", ExpressionDataRes.MasterAircraft4 },
		{ ".MasterShip.Aircraft[4]", ExpressionDataRes.MasterAircraft5 },
		{ ".MasterShip.AircraftTotal", ExpressionDataRes.MasterAircraftTotal },		//要る？
		{ ".AircraftRate[0]", ExpressionDataRes.AircraftRate1 },
		{ ".AircraftRate[1]", ExpressionDataRes.AircraftRate2},
		{ ".AircraftRate[2]", ExpressionDataRes.AircraftRate3 },
		{ ".AircraftRate[3]", ExpressionDataRes.AircraftRate4 },
		{ ".AircraftRate[4]", ExpressionDataRes.AircraftRate5 },
		{ ".AircraftTotalRate", ExpressionDataRes.AircraftRateTotal },
		{ ".Fuel", ExpressionDataRes.Fuel },
		{ ".Ammo", ExpressionDataRes.Ammo },
		{ ".FuelMax", ExpressionDataRes.FuelMax},
		{ ".AmmoMax", ExpressionDataRes.AmmoMax },
		{ ".FuelRate", ExpressionDataRes.FuelRate },
		{ ".AmmoRate",ExpressionDataRes.AmmoRate },
		{ ".SlotSize", ExpressionDataRes.SlotSize},
		{ ".RepairingDockID", ExpressionDataRes.RepairingDockID},
		{ ".RepairTime", ExpressionDataRes.RepairTime },
		{ ".RepairTimeUnit", ShipGroupResources.RepairTimeUnit },
		{ ".RepairSteel", ExpressionDataRes.RepairSteel },
		{ ".RepairFuel", ExpressionDataRes.RepairFuel },
		//強化値シリーズは省略
		{ ".FirepowerBase", ExpressionDataRes.FirePowerBase },
		{ ".TorpedoBase", ExpressionDataRes.TorpedoBase },
		{ ".AABase", ExpressionDataRes.AABase },
		{ ".ArmorBase", ExpressionDataRes.ArmorBase },
		{ ".EvasionBase", ExpressionDataRes.EvasionBase },
		{ ".ASWBase", ExpressionDataRes.ASWBase },
		{ ".LOSBase", ExpressionDataRes.LOSBase },
		{ ".LuckBase", ExpressionDataRes.LuckBase },
		{ ".FirepowerTotal", ExpressionDataRes.FirePowerTotal },
		{ ".TorpedoTotal", ExpressionDataRes.TorpedoTotal },
		{ ".AATotal", ExpressionDataRes.AATotal },
		{ ".ArmorTotal", ExpressionDataRes.ArmorTotal },
		{ ".EvasionTotal", ExpressionDataRes.EvasionTotal},
		{ ".ASWTotal", ExpressionDataRes.ASWTotal },
		{ ".LOSTotal", ExpressionDataRes.LOSTotal},
		{ ".LuckTotal", ExpressionDataRes.LuckTotal },
		{ ".BomberTotal", ExpressionDataRes.BomberTotal },
		{ ".FirepowerRemain", ExpressionDataRes.FirePowerRemain },
		{ ".TorpedoRemain", ExpressionDataRes.TorpedoRemain },
		{ ".AARemain", ExpressionDataRes.AARemain },
		{ ".ArmorRemain",ExpressionDataRes.ArmorRemain },
		{ ".LuckRemain",ExpressionDataRes.LuckRemain },
		{ ".Range", ExpressionDataRes.Range },		//現在の射程
		{ ".Speed",ExpressionDataRes.Speed },
		{ ".MasterShip.Speed", ExpressionDataRes.MasterSpeed },
		{ ".MasterShip.Rarity", ExpressionDataRes.MasterRarity },
		{ ".IsLocked", ExpressionDataRes.IsLocked },
		{ ".IsLockedByEquipment", ExpressionDataRes.IsLockedByEquipment },
		{ ".SallyArea", ExpressionDataRes.SallyArea },
		{ ".FleetWithIndex", ExpressionDataRes.FleetWithIndex },
		{ ".IsMarried", ExpressionDataRes.IsMarried },
		{ ".AirBattlePower", ExpressionDataRes.AirBattlePower },
		{ ".ShellingPower", ExpressionDataRes.ShellingPower},
		{ ".AircraftPower", ExpressionDataRes.AircraftPower },
		{ ".AntiSubmarinePower", ExpressionDataRes.ASWPower },
		{ ".TorpedoPower",ExpressionDataRes.TorpedoPower },
		{ ".NightBattlePower", ExpressionDataRes.NightBattlePower },
		{ ".MasterShip.AlbumNo", ExpressionDataRes.AlbumNo },
		{ ".MasterShip.NameReading", ExpressionDataRes.NameReading },
		{ ".MasterShip.RemodelBeforeShipID", ExpressionDataRes.RemodelBeforeShipID},
		{ ".MasterShip.RemodelAfterShipID", ExpressionDataRes.RemodelAfterShipID },
		//マスターのパラメータ系もおそらく意味がないので省略		
		{ ".MasterShip.EquippableCategories", ExpressionDataRes.EquippableCategories },
		{ ".MasterShip.SortID", ShipGroupResources.SortId },
	};

	private static Dictionary<string, Type> ExpressionTypeTable = new Dictionary<string, Type>();


	[IgnoreDataMember]
	public static readonly Dictionary<ExpressionOperator, string> OperatorNameTable = new Dictionary<ExpressionOperator, string>() {
		{ ExpressionOperator.Equal, ExpressionDataRes.StrEquals},
		{ ExpressionOperator.NotEqual, ExpressionDataRes.StrNotEquals },
		{ ExpressionOperator.LessThan, ExpressionDataRes.StrLessThan },
		{ ExpressionOperator.LessEqual,ExpressionDataRes.StrLessEquals },
		{ ExpressionOperator.GreaterThan, ExpressionDataRes.StrMoreThan },
		{ ExpressionOperator.GreaterEqual, ExpressionDataRes.StrMoreEquals },
		{ ExpressionOperator.Contains, ExpressionDataRes.StrContains },
		{ ExpressionOperator.NotContains, ExpressionDataRes.StrNotContains },
		{ ExpressionOperator.BeginWith, ExpressionDataRes.StrBeginsWith },
		{ ExpressionOperator.NotBeginWith, ExpressionDataRes.StrNotBeginsWith},
		{ ExpressionOperator.EndWith, ExpressionDataRes.StrEndWith },
		{ ExpressionOperator.NotEndWith, ExpressionDataRes.StrNotEndWith },
		{ ExpressionOperator.ArrayContains, ExpressionDataRes.StrArrayContains },
		{ ExpressionOperator.ArrayNotContains, ExpressionDataRes.StrArrayNotContains },

	};



	public ExpressionData()
	{
		Enabled = true;
	}

	public ExpressionData(string left, ExpressionOperator ope, object right)
		: this()
	{
		LeftOperand = left;
		Operator = ope;
		RightOperand = right;
	}


	public Expression Compile(ParameterExpression paramex)
	{

		Expression memberex = null;
		Expression constex = Expression.Constant(RightOperand, RightOperand.GetType());

		{
			Match match = regex_index.Match(LeftOperand);
			if (match.Success)
			{

				do
				{

					if (memberex == null)
					{
						memberex = Expression.PropertyOrField(paramex, match.Groups["name"].Value);
					}
					else
					{
						memberex = Expression.PropertyOrField(memberex, match.Groups["name"].Value);
					}

					if (int.TryParse(match.Groups["index"].Value, out int index))
					{
						memberex = Expression.Property(memberex, "Item", Expression.Constant(index, typeof(int)));
					}

				} while ((match = match.NextMatch()).Success);

			}
			else
			{
				memberex = Expression.PropertyOrField(paramex, LeftOperand);
			}
		}

		if (memberex.Type.IsEnum)
			memberex = Expression.Convert(memberex, typeof(int));

		Expression condex;
		switch (Operator)
		{
			case ExpressionOperator.Equal:
				condex = Expression.Equal(memberex, constex);
				break;
			case ExpressionOperator.NotEqual:
				condex = Expression.NotEqual(memberex, constex);
				break;
			case ExpressionOperator.LessThan:
				condex = Expression.LessThan(memberex, constex);
				break;
			case ExpressionOperator.LessEqual:
				condex = Expression.LessThanOrEqual(memberex, constex);
				break;
			case ExpressionOperator.GreaterThan:
				condex = Expression.GreaterThan(memberex, constex);
				break;
			case ExpressionOperator.GreaterEqual:
				condex = Expression.GreaterThanOrEqual(memberex, constex);
				break;
			case ExpressionOperator.Contains:
				condex = Expression.Call(memberex, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), constex);
				break;
			case ExpressionOperator.NotContains:
				condex = Expression.Not(Expression.Call(memberex, typeof(string).GetMethod("Contains", new Type[] { typeof(string) }), constex));
				break;
			case ExpressionOperator.BeginWith:
				condex = Expression.Call(memberex, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), constex);
				break;
			case ExpressionOperator.NotBeginWith:
				condex = Expression.Not(Expression.Call(memberex, typeof(string).GetMethod("StartsWith", new Type[] { typeof(string) }), constex));
				break;
			case ExpressionOperator.EndWith:
				condex = Expression.Call(memberex, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), constex);
				break;
			case ExpressionOperator.NotEndWith:
				condex = Expression.Not(Expression.Call(memberex, typeof(string).GetMethod("EndsWith", new Type[] { typeof(string) }), constex));
				break;
			case ExpressionOperator.ArrayContains:  // returns Enumerable.Contains<>( memberex )
				condex = Expression.Call(typeof(Enumerable), "Contains", new Type[] { memberex.Type.GetElementType() ?? memberex.Type.GetGenericArguments().First() }, memberex, constex);
				break;
			case ExpressionOperator.ArrayNotContains:   // returns !Enumerable.Contains<>( memberex )
				condex = Expression.Not(Expression.Call(typeof(Enumerable), "Contains", new Type[] { memberex.Type.GetElementType() ?? memberex.Type.GetGenericArguments().First() }, memberex, constex));
				break;

			default:
				throw new NotImplementedException();
		}

		return condex;
	}



	public static Type GetLeftOperandType(string left)
	{

		if (ExpressionTypeTable.ContainsKey(left))
		{
			return ExpressionTypeTable[left];

		}
		else if (KCDatabase.Instance.Ships.Count > 0)
		{

			object obj = KCDatabase.Instance.Ships.Values.First();

			Match match = regex_index.Match(left);
			if (match.Success)
			{

				do
				{

					if (int.TryParse(match.Groups["index"].Value, out int index))
					{
						obj = ((dynamic)obj.GetType().InvokeMember(match.Groups["name"].Value, System.Reflection.BindingFlags.GetProperty, null, obj, null))[index];
					}
					else
					{
						object obj2 = obj.GetType().InvokeMember(match.Groups["name"].Value, System.Reflection.BindingFlags.GetProperty, null, obj, null);
						if (obj2 == null)
						{   //プロパティはあるけどnull
							var type = obj.GetType().GetProperty(match.Groups["name"].Value).GetType();
							ExpressionTypeTable.Add(left, type);
							return type;
						}
						else
						{
							obj = obj2;
						}
					}

				} while (obj != null && (match = match.NextMatch()).Success);


				if (obj != null)
				{
					ExpressionTypeTable.Add(left, obj.GetType());
					return obj.GetType();
				}
			}

		}

		return null;
	}

	public Type GetLeftOperandType()
	{
		return GetLeftOperandType(LeftOperand);
	}



	public override string ToString() => $"{LeftOperandToString()} は {RightOperandToString()} {OperatorToString()}";



	/// <summary>
	/// 左辺値の文字列表現を求めます。
	/// </summary>
	public string LeftOperandToString()
	{
		if (LeftOperandNameTable.ContainsKey(LeftOperand))
			return LeftOperandNameTable[LeftOperand];
		else
			return LeftOperand;
	}

	/// <summary>
	/// 演算子の文字列表現を求めます。
	/// </summary>
	public string OperatorToString()
	{
		return OperatorNameTable[Operator];
	}

	/// <summary>
	/// 右辺値の文字列表現を求めます。
	/// </summary>
	public string RightOperandToString()
	{

		if (LeftOperand == ".MasterID")
		{
			var ship = KCDatabase.Instance.Ships[(int)RightOperand];
			if (ship != null)
				return $"{ship.MasterID} ({ship.NameWithLevel})";
			else
				return $"{(int)RightOperand} (未在籍)";

		}
		else if (LeftOperand == ".ShipID")
		{
			var ship = KCDatabase.Instance.MasterShips[(int)RightOperand];
			if (ship != null)
				return $"{ship.ShipID} ({ship.NameWithClass})";
			else
				return $"{(int)RightOperand} (存在せず)";

		}
		else if (LeftOperand == ".MasterShip.ShipType")
		{
			var shiptype = KCDatabase.Instance.ShipTypes[(int)RightOperand];
			if (shiptype != null)
				return shiptype.NameEN;
			else
				return $"{(int)RightOperand} (未定義)";

		}
		else if (LeftOperand.Contains("SlotMaster"))
		{
			if ((int)RightOperand == -1)
			{
				return "(なし)";
			}
			else
			{
				var eq = KCDatabase.Instance.MasterEquipments[(int)RightOperand];
				if (eq != null)
					return eq.NameEN;
				else
					return $"{(int)RightOperand} (未定義)";
			}
		}
		else if (LeftOperand == ".MasterShip.EquippableCategories")
		{
			var cat = KCDatabase.Instance.EquipmentTypes[(int)RightOperand];
			if (cat != null)
				return cat.NameEN;
			else
				return $"{(int)RightOperand} (未定義)";

		}
		else if (LeftOperand.Contains("Rate") && RightOperand is double)
		{
			return ((double)RightOperand).ToString("P0");

		}
		else if (LeftOperand is ".RepairTime" or ".RepairTimeUnit")
		{
			return DateTimeHelper.ToTimeRemainString(DateTimeHelper.FromAPITimeSpan((int)RightOperand));

		}
		else if (LeftOperand == ".Range")
		{
			return Constants.GetRange((int)RightOperand);

		}
		else if (LeftOperand == ".Speed" || LeftOperand == ".MasterShip.Speed")
		{
			return Constants.GetSpeed((int)RightOperand);

		}
		else if (LeftOperand == ".MasterShip.Rarity")
		{
			return Constants.GetShipRarity((int)RightOperand);

		}
		else if (LeftOperand == ".MasterShip.AlbumNo")
		{
			var ship = KCDatabase.Instance.MasterShips.Values.FirstOrDefault(s => s.AlbumNo == (int)RightOperand);
			if (ship != null)
				return $"{(int)RightOperand} ({ship.NameWithClass})";
			else
				return $"{(int)RightOperand} (存在せず)";

		}
		else if (LeftOperand == ".MasterShip.RemodelAfterShipID")
		{

			if (((int)RightOperand) == 0)
				return "最終改装";

			var ship = KCDatabase.Instance.MasterShips[(int)RightOperand];
			if (ship != null)
				return $"{ship.ShipID} ({ship.NameWithClass})";
			else
				return $"{(int)RightOperand} (存在せず)";

		}
		else if (LeftOperand == ".MasterShip.RemodelBeforeShipID")
		{

			if (((int)RightOperand) == 0)
				return "未改装";

			var ship = KCDatabase.Instance.MasterShips[(int)RightOperand];
			if (ship != null)
				return $"{ship.ShipID} ({ship.NameWithClass})";
			else
				return $"{(int)RightOperand} (存在せず)";

		}
		else if (RightOperand is bool)
		{
			return ((bool)RightOperand) ? "○" : "×";

		}
		else
		{
			return RightOperand.ToString();

		}

	}


	public ExpressionData Clone()
	{
		var clone = MemberwiseClone();      //checkme: 右辺値に参照型を含む場合死ぬ
		return (ExpressionData)clone;
	}

	object ICloneable.Clone()
	{
		return Clone();
	}
}
