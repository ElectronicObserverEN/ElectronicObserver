using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using ElectronicObserver.Window;

namespace ElectronicObserver.Notifier;

public sealed class NotifierManager
{

	#region Singleton

	private static readonly NotifierManager instance = new NotifierManager();

	public static NotifierManager Instance => instance;

	#endregion

	private object _parentForm;


	public NotifierExpedition Expedition { get; private set; }
	public NotifierConstruction Construction { get; private set; }
	public NotifierRepair Repair { get; private set; }
	public NotifierCondition Condition { get; private set; }
	public NotifierDamage Damage { get; private set; }
	public NotifierAnchorageRepair AnchorageRepair { get; private set; }
	public NotifierHomePortSupply HomePortSupply { get; private set; }
	public NotifierBaseAirCorps BaseAirCorps { get; private set; }
	public NotifierBattleEnd BattleEnd { get; private set; }
	public NotifierRemodelLevel RemodelLevel { get; private set; }
	public NotifierBase TrainingPlan { get; private set; }

	private NotifierManager()
	{
	}


	public void Initialize(object parent)
	{

		_parentForm = parent;

		var c = Utility.Configuration.Config;

		Expedition = new NotifierExpedition(c.NotifierExpedition);
		Construction = new NotifierConstruction(c.NotifierConstruction);
		Repair = new NotifierRepair(c.NotifierRepair);
		Condition = new NotifierCondition(c.NotifierCondition);
		Damage = new NotifierDamage(c.NotifierDamage);
		AnchorageRepair = new NotifierAnchorageRepair(c.NotifierAnchorageRepair);
		HomePortSupply = new NotifierHomePortSupply(c.NotifierHomePortSupply);
		BaseAirCorps = new NotifierBaseAirCorps(c.NotifierBaseAirCorps);
		BattleEnd = new NotifierBattleEnd(c.NotifierBattleEnd);
		RemodelLevel = new NotifierRemodelLevel(c.NotifierRemodelLevel);
		TrainingPlan = new NotifierTrainingPlan(c.NotifierTrainingPlan);
	}

	public void ApplyToConfiguration()
	{

		var c = Utility.Configuration.Config;

		Expedition.ApplyToConfiguration(c.NotifierExpedition);
		Construction.ApplyToConfiguration(c.NotifierConstruction);
		Repair.ApplyToConfiguration(c.NotifierRepair);
		Condition.ApplyToConfiguration(c.NotifierCondition);
		Damage.ApplyToConfiguration(c.NotifierDamage);
		AnchorageRepair.ApplyToConfiguration(c.NotifierAnchorageRepair);
		HomePortSupply.ApplyToConfiguration(c.NotifierHomePortSupply);
		BaseAirCorps.ApplyToConfiguration(c.NotifierBaseAirCorps);
		BattleEnd.ApplyToConfiguration(c.NotifierBattleEnd);
		RemodelLevel.ApplyToConfiguration(c.NotifierRemodelLevel);
		TrainingPlan.ApplyToConfiguration(c.NotifierTrainingPlan);
	}

	public void ShowNotifier(ElectronicObserver.Window.Dialog.DialogNotifier form)
	{
		switch (_parentForm)
		{
			case FormMainWpf parentForm:
			{
				if (form.DialogData.Alignment == NotifierDialogAlignment.CustomRelative)
				{       //cloneしているから書き換えても問題ないはず
					if (parentForm.ViewModel.FormBrowserHost.WindowsFormsHost.Child is FormBrowserHost browserHost)
					{
						Point p = browserHost.PointToScreen(new Point(browserHost.ClientSize.Width / 2, browserHost.ClientSize.Height / 2));
						p.Offset(new Point(-form.Width / 2, -form.Height / 2));
						p.Offset(form.DialogData.Location);

						form.DialogData.Location = p;
					}
				}
				parentForm.Dispatcher.Invoke((MethodInvoker)form.Show);
				break;
			}
		}
	}

	public IEnumerable<NotifierBase> GetNotifiers()
	{
		yield return Expedition;
		yield return Construction;
		yield return Repair;
		yield return Condition;
		yield return Damage;
		yield return AnchorageRepair;
		yield return HomePortSupply;
		yield return BaseAirCorps;
		yield return BattleEnd;
		yield return RemodelLevel;
		yield return TrainingPlan;
	}

}
