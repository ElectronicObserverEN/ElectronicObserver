using System.Windows.Media;
using ElectronicObserver.Data;
using ElectronicObserver.Utility;

namespace ElectronicObserver.Window.Wpf.Quest;

public class QuestItemViewModel
{
	private QuestData Quest { get; }
	private int QuestIndex { get; }

	public int QuestId => Quest.QuestID;
	public string QuestCode => Quest.Code;
	public string QuestName => Quest.Name;
	public string QuestDescription => Quest.Description;

	public bool? QuestView_State { get; set; }
	public int StateSort => QuestView_State switch
	{
		null => 3,
		true => 2,
		false => 1,
	};
	public string QuestView_StateToolTip { get; set; } = "";

	public int QuestView_Type { get; set; }
	public string QuestView_TypeDisplay => Constants.GetQuestType(QuestView_Type);
	public string? QuestView_TypeToolTip { get; set; }

	public int QuestView_Category { get; set; }
	public string QuestView_CategoryDisplay => Constants.GetQuestCategory(QuestView_Category);
	public SolidColorBrush QuestView_CategoryBackground => QuestView_Category switch
	{
		// 編成
		1 => Configuration.Config.UI.Quest_Type1Color.ToBrush(),
		// 出撃
		2 => Configuration.Config.UI.Quest_Type2Color.ToBrush(),
		// 演習
		3 => Configuration.Config.UI.Quest_Type3Color.ToBrush(),
		// 遠征
		4 => Configuration.Config.UI.Quest_Type4Color.ToBrush(),
		// 補給/入渠
		5 => Configuration.Config.UI.Quest_Type5Color.ToBrush(),
		// 工廠
		6 => Configuration.Config.UI.Quest_Type6Color.ToBrush(),
		// 改装
		7 => Configuration.Config.UI.Quest_Type7Color.ToBrush(),
		// 出撃(2)
		8 => Configuration.Config.UI.Quest_Type2Color.ToBrush(),
		// 出撃(3)
		9 => Configuration.Config.UI.Quest_Type2Color.ToBrush(),
		// その他
		10 => Configuration.Config.UI.Quest_Type2Color.ToBrush(),
		11 => Configuration.Config.UI.Quest_Type6Color.ToBrush(),

		_ => System.Drawing.Color.Transparent.ToBrush(),
	};
	public SolidColorBrush QuestView_CategoryForeground => QuestView_Category switch
	{
		_ => Configuration.Config.UI.Quest_TypeFG.ToBrush()
	};
	public string? QuestView_CategoryToolTip { get; set; }

	public string? QuestView_Name { get; set; }
	public string? QuestView_NameToolTip { get; set; }

	public double QuestView_Progress { get; set; }
	public string? QuestView_ProgressText { get; set; }
	public string? QuestView_ProgressToolTip { get; set; }

	public SolidColorBrush ProgressBrush => QuestView_Progress switch
	{
		< 0.5 => Configuration.Config.UI.Quest_ColorProcessLT50.ToBrush(),
		< 0.8 => Configuration.Config.UI.Quest_ColorProcessLT80.ToBrush(),
		< 1.0 => Configuration.Config.UI.Quest_ColorProcessLT100.ToBrush(),
		_ => Configuration.Config.UI.Quest_ColorProcessDefault.ToBrush()
	};

	public SolidColorBrush Background { get; set; }

	public QuestItemViewModel()
	{

	}

	public QuestItemViewModel(QuestData quest, int questIndex)
	{
		Quest = quest;
		QuestIndex = questIndex;
	}
}
