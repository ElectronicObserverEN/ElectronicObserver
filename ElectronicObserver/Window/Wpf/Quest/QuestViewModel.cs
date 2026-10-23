using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Forms;
using System.Windows.Media;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using DynaJson;
using ElectronicObserver.Avalonia.QuestTrackerManager2;
using ElectronicObserver.Core.Services;
using ElectronicObserver.Core.Types.Quests;
using ElectronicObserver.Data;
using ElectronicObserver.Data.Quest;
using ElectronicObserver.Observer;
using ElectronicObserver.Resource;
using ElectronicObserver.Utility;
using ElectronicObserver.ViewModels;
using ElectronicObserver.ViewModels.Translations;
using ElectronicObserver.Window.Dialog.QuestTrackerManager;

namespace ElectronicObserver.Window.Wpf.Quest;

public partial class QuestViewModel : AnchorableViewModel
{
	public FontFamily Font { get; set; }
	public double FontSize { get; set; }
	public SolidColorBrush FontBrush { get; set; }

	public FormQuestTranslationViewModel FormQuest { get; }
	private IClipboardService ClipboardService { get; }

	private ObservableCollection<QuestItemViewModel> Quests { get; set; } = new();
	public ICollectionView View { get; set; }
	public List<SortDescription> SortDescriptions { get; set; }

	public QuestItemViewModel? SelectedQuest { get; set; }

	public int HeaderMinSize { get; set; }
	public int RowMinSize { get; set; }

	public bool WebSearchEnabled => SelectedQuest is not null;
	public bool WikiSearchEnabled => SelectedQuest is not null;

	private string SearchKey => SelectedQuest switch
	{
		{ QuestName: { Length: > 25 } name } => name[..22] + "...",
		{ QuestName: { } name } => name,
		_ => ""
	};

	public string DuckDuckGoSearchText => SelectedQuest switch
	{
		{ QuestName: { } } => string.Format(FormQuest.LookUpSpecificQuestOnDuckDuckGo, SearchKey),
		_ => FormQuest.LookUpQuestOnDuckDuckGo
	};

	public string StartpageSearchText => SelectedQuest switch
	{
		{ QuestName: { } } => string.Format(FormQuest.LookUpSpecificQuestOnStartpage, SearchKey),
		_ => FormQuest.LookUpQuestOnStartpage
	};

	public string WikiSearchText => SelectedQuest switch
	{
		{ QuestName: { } } => string.Format(FormQuest.LookUpSpecificQuestOnWiki, SearchKey),
		_ => FormQuest.MenuMain_KcwikiQuest
	};

	private bool IsLoaded { get; set; }

	public bool MenuMain_ShowRunningOnly { get; set; }
	public bool MenuMain_ShowOnce { get; set; }
	public bool MenuMain_ShowDaily { get; set; }
	public bool MenuMain_ShowWeekly { get; set; }
	public bool MenuMain_ShowMonthly { get; set; }
	public bool MenuMain_ShowOther { get; set; }

	public bool ShowQuestCode { get; set; }

	public ColumnViewModel StateColumn { get; } = new();
	public ColumnViewModel TypeColumn { get; } = new();
	public ColumnViewModel CategoryColumn { get; } = new();
	public ColumnViewModel NameColumn { get; } = new();
	public ColumnViewModel ProgressColumn { get; } = new();
	public ColumnViewModel FleetCheckColumn { get; } = new();

	private List<ColumnViewModel> Columns { get; }

	public QuestViewModel() : base("Quest", "Quest", IconContent.FormQuest)
	{
		FormQuest = Ioc.Default.GetRequiredService<FormQuestTranslationViewModel>();
		ClipboardService = Ioc.Default.GetRequiredService<IClipboardService>();

		View = CollectionViewSource.GetDefaultView(Quests);

		Title = FormQuest.Title;
		FormQuest.PropertyChanged += (_, _) => Title = FormQuest.Title;
		KCDatabase.Instance.Quest.QuestUpdated += Updated;
		SubscribeToApis();

		Columns =
		[
			StateColumn,
			TypeColumn,
			CategoryColumn,
			NameColumn,
			ProgressColumn,
			FleetCheckColumn,
		];

		ConfigurationChanged();

		SystemEvents.SystemShuttingDown += SystemEvents_SystemShuttingDown;

		Configuration.Instance.ConfigurationChanged += ConfigurationChanged;
		IsLoaded = true;

		foreach (ColumnViewModel column in Columns)
		{
			column.PropertyChanged += ColumnVisibilityChanged;
			column.PropertyChanged += ColumnWidthChanged;
		}

		PropertyChanged += VisibleQuestsChanged;

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(ShowQuestCode)) return;

			Configuration.Config.FormQuest.ShowQuestCode = ShowQuestCode;

			Updated();
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(HeaderMinSize)) return;

			Configuration.Config.FormQuest.HeaderMinSize = HeaderMinSize;
		};

		PropertyChanged += (sender, args) =>
		{
			if (args.PropertyName is not nameof(RowMinSize)) return;

			Configuration.Config.FormQuest.RowMinSize = RowMinSize;
		};
	}

	private void SubscribeToApis()
	{
		APIObserver o = APIObserver.Instance;

		o.ApiReqHensei_Change.RequestReceived += FleetChanged;
		o.ApiReqKousyou_DestroyShip.RequestReceived += FleetChanged;
		o.ApiReqKaisou_Remodeling.RequestReceived += FleetChanged;

		o.ApiPort_Port.ResponseReceived += FleetChanged;
		o.ApiGetMember_Ship2.ResponseReceived += FleetChanged;
		o.ApiGetMember_Ship3.ResponseReceived += FleetChanged;
		o.ApiReqKousyou_DestroyShip.ResponseReceived += FleetChanged;
		o.ApiReqHensei_PresetSelect.ResponseReceived += FleetChanged;
		o.ApiReqKaisou_SlotExchangeIndex.ResponseReceived += FleetChanged;
		o.ApiReqKaisou_SlotDeprive.ResponseReceived += FleetChanged;
		o.ApiReqKaisou_Marriage.ResponseReceived += FleetChanged;
	}

	private void FleetChanged(string apiname, dynamic data) => Updated();

	private void ColumnWidthChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(ColumnViewModel.Width)) return;

		QuestView_ColumnWidthChanged();
	}

	private void ColumnVisibilityChanged(object? sender, PropertyChangedEventArgs e)
	{
		if (e.PropertyName is not nameof(ColumnViewModel.Visible)) return;

		MenuMain_ColumnFilter_Click();
	}

	private void VisibleQuestsChanged(object? sender, PropertyChangedEventArgs e)
	{
		bool visibleQuestsChanged = e.PropertyName is
			nameof(MenuMain_ShowRunningOnly) or
			nameof(MenuMain_ShowOnce) or
			nameof(MenuMain_ShowDaily) or
			nameof(MenuMain_ShowWeekly) or
			nameof(MenuMain_ShowMonthly) or
			nameof(MenuMain_ShowOther);

		if (!visibleQuestsChanged) return;

		var c = Configuration.Config;

		c.FormQuest.ShowRunningOnly = MenuMain_ShowRunningOnly;
		c.FormQuest.ShowOnce = MenuMain_ShowOnce;
		c.FormQuest.ShowDaily = MenuMain_ShowDaily;
		c.FormQuest.ShowWeekly = MenuMain_ShowWeekly;
		c.FormQuest.ShowMonthly = MenuMain_ShowMonthly;
		c.FormQuest.ShowOther = MenuMain_ShowOther;

		Updated();
	}

	private void ConfigurationChanged()
	{
		var c = Configuration.Config;

		Font = new FontFamily(c.UI.MainFont.FontData.FontFamily.Name);
		FontSize = c.UI.MainFont.FontData.ToSize();
		FontBrush = c.UI.ForeColor.ToBrush();

		HeaderMinSize = c.FormQuest.HeaderMinSize;
		RowMinSize = c.FormQuest.RowMinSize;

		MenuMain_ShowRunningOnly = c.FormQuest.ShowRunningOnly;
		MenuMain_ShowOnce = c.FormQuest.ShowOnce;
		MenuMain_ShowDaily = c.FormQuest.ShowDaily;
		MenuMain_ShowWeekly = c.FormQuest.ShowWeekly;
		MenuMain_ShowMonthly = c.FormQuest.ShowMonthly;
		MenuMain_ShowOther = c.FormQuest.ShowOther;

		ShowQuestCode = c.FormQuest.ShowQuestCode;

		List<bool> columnFilters = (List<bool>)c.FormQuest.ColumnFilter;

		if (c.FormQuest.ColumnFilter == null || columnFilters.Count < Columns.Count)
		{
			c.FormQuest.ColumnFilter = columnFilters.Concat(Enumerable.Repeat(true, Columns.Count - columnFilters.Count)).ToList();
		}

		List<int> columnWidths = (List<int>)c.FormQuest.ColumnWidth;

		if (c.FormQuest.ColumnWidth == null || columnWidths.Count < Columns.Count)
		{
			c.FormQuest.ColumnWidth = columnWidths.Concat(Columns.Skip(Columns.Count - columnWidths.Count).Select(column => (int)column.Width.DisplayValue)).ToList();
		}

		List<int> columnSorts = (List<int>?)c.FormQuest.ColumnSort ?? [];

		if (c.FormQuest.ColumnSort == null || columnSorts.Count < Columns.Count)
		{
			c.FormQuest.ColumnSort = columnSorts.Concat(Columns.Skip(Columns.Count - columnSorts.Count).Select(column => column.SortDirection.ToSerializableValue())).ToList();
		}

		List<bool> list = c.FormQuest.ColumnFilter;
		List<int> widths = c.FormQuest.ColumnWidth;
		List<ListSortDirection?> sorts = [.. c.FormQuest.ColumnSort.List.Select(s => s.ToSortDirection())];

		foreach ((ColumnViewModel column, int i) in Columns.Select((column, i) => (column, i)))
		{
			column.Visible = list[i];
			column.Width = new DataGridLength(widths[i]);
			column.SortDirection = sorts[i];
		}

		SortDescriptions = c.FormQuest.SortDescriptions;

		Updated();
	}



	private void SystemEvents_SystemShuttingDown()
	{

		try
		{
			/*
			if (QuestView.SortedColumn != null)
				Utility.Configuration.Config.FormQuest.SortParameter = QuestView.SortedColumn.Index << 1 | (QuestView.SortOrder == SortOrder.Ascending ? 0 : 1);

			*/
			Configuration.Config.FormQuest.ColumnWidth = Columns.Select(c => (int)c.Width.DisplayValue).ToList();
			// ColumnSort only gives you a visual indication for sorting
			// SortDescriptions do the actual sorting
			Configuration.Config.FormQuest.ColumnSort = Columns.Select(c => c.SortDirection.ToSerializableValue()).ToList();
			Configuration.Config.FormQuest.SortDescriptions = SortDescriptions;
		}
		catch (Exception)
		{
			// *ぷちっ*
		}
	}


	private void Updated()
	{
		if (!KCDatabase.Instance.Quest.IsLoaded) return;

		Quests.Clear();

		var indexedQuests = KCDatabase.Instance.Quest.Quests.Values
			.OrderBy(q => q.ID)
			.Select((q, i) => (q, i));

		foreach ((QuestData q, int questIndex) in indexedQuests)
		{

			if (MenuMain_ShowRunningOnly && !(q.State is 2 or 3))
				continue;

			switch (q.Type)
			{
				case 1:
					if (!MenuMain_ShowDaily) continue;
					break;
				case 2:
					if (!MenuMain_ShowWeekly) continue;
					break;
				case 3:
					if (!MenuMain_ShowMonthly) continue;
					break;
				case 4:
				default:
					if (!MenuMain_ShowOnce) continue;
					break;
				case 5:
					if (q.QuestID == 211 || q.QuestID == 212)
					{
						// 空母3か輸送5
						if (!MenuMain_ShowDaily) continue;
					}
					else
					{
						if (!MenuMain_ShowOther) continue;
					}

					break;
			}


			QuestItemViewModel row = new(q, questIndex)
			{
				// Add support for tooltip-based page numbering
				Background = (MenuMain_ShowRunningOnly, questIndex / 5 % 2) switch
				{
					(false, 1) => Configuration.Config.UI.SubBackColor.ToBrush(),
					_ => Configuration.Config.UI.BackColor.ToBrush(),
				},

				QuestView_State = q.State switch
				{
					3 => null,
					_ => q.State == 2,
				},
				QuestView_StateToolTip = $"{QuestResources.Page} #{questIndex / 5 + 1}",

				QuestView_Type = q.LabelType switch
				{
					>= 100 => q.LabelType,
					_ => q.Type
				},
				QuestView_TypeToolTip = Constants.GetQuestCategory(q.Category),

				QuestView_Category = q.Category,
				QuestView_CategoryToolTip = Constants.GetQuestCategory(q.Category),

				QuestView_Name = ShowQuestCode switch
				{
					true => q.NameWithCode,
					_ => q.Name
				}
			};

			ProgressData? progress = KCDatabase.Instance.QuestProgress[q.QuestID];
			ITrackerViewModel? tracker =
				KCDatabase.Instance.QuestTrackerManager2.GetTrackerById(q.QuestID)
				?? KCDatabase.Instance.SystemQuestTrackerManagerV2.GetTrackerById(q.QuestID)
				?? KCDatabase.Instance.QuestTrackerManager.GetTrackerById(q.QuestID)
				?? KCDatabase.Instance.SystemQuestTrackerManager.GetTrackerById(q.QuestID);

			{
				row.QuestView_NameToolTip = $"{row.QuestView_Name} (ID: {q.QuestID})\r\n{q.Description}";

				string? clearCondition = tracker?.ClearCondition ?? progress?.GetClearCondition();

				if (!string.IsNullOrEmpty(clearCondition))
				{
					row.QuestView_NameToolTip += $"\r\n{clearCondition}";
				}

				if (!string.IsNullOrEmpty(tracker?.ConditionDisplay))
				{
					row.QuestView_NameToolTip += $"\r\n\r\n{tracker?.ConditionDisplay}";
				}

				StringBuilder toolTipExtra = new();

				if (q.Type != 1 && q.GetProgressResetType() is QuestResetType.Daily)
				{
					toolTipExtra.AppendLine();
					toolTipExtra.Append(FormQuest.QuestView_ProgressResetsDaily);
				}

				if (q.GetEndDateTime() is DateTime endTime)
				{
					toolTipExtra.AppendLine();
					toolTipExtra.AppendFormat(FormQuest.QuestView_EndsOn, endTime);
				}

				row.QuestView_NameToolTipExtra = toolTipExtra.ToString();

				if (row.QuestView_NameToolTipExtra?.Length > 0)
				{
					row.QuestView_NameToolTip += "\r\n";
				}
			}
			{
				string value;
				double tag;

				if (q.State == 3)
				{
					value = QuestResources.Complete;
					tag = 1.0;

				}
				else
				{
					if (tracker is { TaskCount: > 0 })
					{
						value = tracker.ProgressDisplay();
						tag = tracker.Progress();
					}
					else if (progress is not null)
					{
						value = progress.ToString();
						tag = progress.ProgressPercentage;
					}
					else
					{
						switch (q.Progress)
						{
							case 0:
								value = "-";
								tag = 0.0;
								break;
							case 1:
								value = GeneralRes.Over50;
								tag = 0.5;
								break;
							case 2:
								value = GeneralRes.Over80;
								tag = 0.8;
								break;
							default:
								value = "???";
								tag = 0.0;
								break;
						}
					}
				}

				row.QuestView_Progress = tag;
				row.QuestView_ProgressToolTip = value;
				row.QuestView_ProgressText = value
					.Replace("\r\n", " ")
					.Replace("\n\r", " ")
					.Replace("\n", " ")
					.Replace("\r", " ");
			}

			if (tracker is not null)
			{
				List<(FleetData Fleet, bool Matches)> fleetResults = KCDatabase.Instance.Fleet.Fleets.Values
					.OrderBy(f => f.FleetID)
					.Select(f => (Fleet: f, Matches: tracker.ConditionMet(f)))
					.ToList();

				row.MatchesCondition = fleetResults switch
				{
					[] => null,
					[{ Matches: true }, ..] => true,
					_ when fleetResults.All(r => !r.Matches) => false,
					_ => null,
				};

				row.MatchesConditionToolTip = string.Join("\r\n", fleetResults.Select(r => string.Format(
					r.Matches switch
					{
						true => QuestResources.FleetMatchesCondition,
						_ => QuestResources.FleetDoesNotMatchCondition
					},
					$"#{r.Fleet.FleetID} {r.Fleet.Name}")));
			}
			else
			{
				row.MatchesCondition = null;
				row.MatchesConditionToolTip = QuestResources.FleetConditionNotConfigured;
			}

			Quests.Add(row);
		}


		if (KCDatabase.Instance.Quest.Quests.Count < KCDatabase.Instance.Quest.Count)
		{
			// all quests get loaded at once now so this shouldn't matter anymore
			// int index = QuestView.Rows.Add();
			// QuestView.Rows[index].Cells[QuestView_State.Index].Value = null;
			// QuestView.Rows[index].Cells[QuestView_Name.Index].Value = string.Format(Translation.OtherQuests, (KCDatabase.Instance.Quest.Count - KCDatabase.Instance.Quest.Quests.Count));
		}

		if (KCDatabase.Instance.Quest.Quests.Count == 0)
		{
			// int index = QuestView.Rows.Add();
			// QuestView.Rows[index].Cells[QuestView_State.Index].Value = null;
			// QuestView.Rows[index].Cells[QuestView_Name.Index].Value = GeneralRes.AllComplete;
			Quests.Add(new()
			{
				QuestView_State = null,
				QuestView_Name = GeneralRes.AllComplete,
			});
		}

		//更新時にソートする
		// if (QuestView.SortedColumn != null)
		// 	QuestView.Sort(QuestView.SortedColumn, QuestView.SortOrder == SortOrder.Ascending ? ListSortDirection.Ascending : ListSortDirection.Descending);

		// Retain scroll position
		// if (QuestView.FirstDisplayedScrollingRowIndex > 0 && QuestView.Rows.Count > scrollPos)
		// 	QuestView.FirstDisplayedScrollingRowIndex = scrollPos;

		// QuestView.ResumeLayout();
		ICollectionView view = CollectionViewSource.GetDefaultView(Quests);

		foreach (SortDescription description in SortDescriptions)
		{
			view.SortDescriptions.Add(description);
		}

		View = view;
	}

	[RelayCommand]
	private void MenuMain_Initialize_Click()
	{
		if (MessageBox.Show(GeneralRes.InitializeQuestData, GeneralRes.InitQuestTitle,
				MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) !=
			DialogResult.Yes)
		{
			return;
		}

		KCDatabase.Instance.Quest.Clear();
		KCDatabase.Instance.QuestProgress.Clear();
		ClearQuestView();
	}


	private void ClearQuestView()
	{

		// QuestView.Rows.Clear();
		Quests.Clear();

		{
			// DataGridViewRow row = new DataGridViewRow();
			// row.CreateCells(QuestView);
			// row.SetValues(null, null, null, Translation.Unknown, null);
			// QuestView.Rows.Add(row);

			Quests.Add(new()
			{
				QuestView_Name = QuestResources.Unknown,
			});
		}

	}


	// todo: rename to SaveColumnVisibility
	private void MenuMain_ColumnFilter_Click()
	{
		Configuration.Config.FormQuest.ColumnFilter.List = Columns.Select(c => c.Visible).ToList();
	}

	private void QuestView_ColumnWidthChanged()
	{
		if (!IsLoaded) return;

		Configuration.Config.FormQuest.ColumnWidth = Columns.Select(c => (int)c.Width.DisplayValue).ToList();
	}

	[RelayCommand]
	private void ManuMain_QuestTitle_Click()
	{
		if (SelectedQuest is null) return;

		ClipboardService.SetTextAndLogErrors(SelectedQuest.QuestName);
	}

	[RelayCommand]
	private void ManuMain_QuestDescription_Click()
	{
		if (SelectedQuest is null) return;

		ClipboardService.SetTextAndLogErrors(SelectedQuest.QuestDescription.Replace(Environment.NewLine, ""));
	}

	[RelayCommand]
	private void ManuMain_QuestTranslate_Click()
	{
		bool needTranslation = false;
		dynamic json = new JsonObject();
		foreach (QuestData quest in KCDatabase.Instance.Quest.Quests.Values)
		{
			if (quest.Translated) continue;

			json[quest.ID.ToString()] = new
			{
				code = "",
				name_jp = quest.NameJP,
				name = "",
				desc_jp = quest.DescriptionJP.Replace("\r\n", "<br>"),
				desc = ""
			};
			needTranslation = true;
		}
		string serializedOutput = json.ToString();

		if (needTranslation == false)
		{
			MessageBox.Show(QuestResources.AllQuestsAreTranslated, QuestResources.Information, MessageBoxButtons.OK,
				MessageBoxIcon.Information);
			return;
		}

		ClipboardService.SetTextAndLogErrors(serializedOutput);
	}

	[RelayCommand]
	private void LookUpOnDuckDuckGo()
	{
		if (SelectedQuest is null) return;

		try
		{
			ProcessStartInfo psi = new ProcessStartInfo
			{
				FileName =
					@"https://www.duckduckgo.com/?q=" +
					Uri.EscapeDataString(SelectedQuest.QuestCode) +
					"+" +
					Uri.EscapeDataString(SelectedQuest.QuestName) +
					QuestResources.SearchEngineKancolleSpecifier,
				UseShellExecute = true
			};

			Process.Start(psi);
		}
		catch (Exception ex)
		{
			ErrorReporter.SendErrorReport(ex, QuestResources.FailedToSearchOnDuckDuckGo);
		}
	}

	[RelayCommand]
	private void LookUpOnStartpage()
	{
		if (SelectedQuest is null) return;

		try
		{
			ProcessStartInfo psi = new ProcessStartInfo
			{
				FileName =
					@"https://www.startpage.com/do/metasearch.pl?query=" +
					Uri.EscapeDataString(SelectedQuest.QuestCode) +
					"+" +
					Uri.EscapeDataString(SelectedQuest.QuestName) +
					QuestResources.SearchEngineKancolleSpecifier,
				UseShellExecute = true
			};

			Process.Start(psi);
		}
		catch (Exception ex)
		{
			ErrorReporter.SendErrorReport(ex, QuestResources.FailedToSearchOnStartpage);
		}
	}

	[RelayCommand]
	private void MenuMain_KcwikiQuest_Click()
	{
		if (SelectedQuest == null) return;

		try
		{
			string url = SelectedQuest.QuestCode switch
			{
				"" => @"https://www.duckduckgo.com/?q=" + Uri.EscapeDataString(SelectedQuest.QuestName) + "+" + Uri.EscapeDataString(QuestResources.SearchOnWikiQuery),
				_ => string.Format(QuestResources.SearchLinkWiki, Uri.EscapeDataString(SelectedQuest.QuestCode)),
			};

			ProcessStartInfo psi = new ProcessStartInfo
			{
				FileName = url,
				UseShellExecute = true
			};
			Process.Start(psi);
		}
		catch (Exception ex)
		{
			ErrorReporter.SendErrorReport(ex, QuestResources.FailedToOpenKancolleWiki);
		}
	}

	[RelayCommand]
	private void MenuProgress_Increment_Click()
	{
		if (SelectedQuest is null) return;

		int id = SelectedQuest.QuestId;

		var quest = KCDatabase.Instance.Quest[id];
		var progress = KCDatabase.Instance.QuestProgress[id];

		if (id == -1 || quest == null || progress == null) return;

		try
		{
			progress.Increment();
			Updated();

		}
		catch (Exception)
		{
			Logger.Add(3, string.Format(QuestResources.FailedToChangeQuestProgress, quest.Name));
			System.Media.SystemSounds.Hand.Play();
		}
	}

	[RelayCommand]
	private void MenuProgress_Decrement_Click()
	{
		if (SelectedQuest is null) return;

		int id = SelectedQuest.QuestId;

		var quest = KCDatabase.Instance.Quest[id];
		var progress = KCDatabase.Instance.QuestProgress[id];

		if (id == -1 || quest == null || progress == null) return;

		try
		{
			progress.Decrement();
			Updated();

		}
		catch (Exception)
		{
			Logger.Add(3, string.Format(QuestResources.FailedToChangeQuestProgress, quest.Name));
			System.Media.SystemSounds.Hand.Play();
		}
	}

	[RelayCommand]
	private void MenuProgress_Reset_Click()
	{
		if (SelectedQuest is null) return;

		int id = SelectedQuest.QuestId;

		QuestData? quest = KCDatabase.Instance.Quest[id];
		ProgressData? progress = KCDatabase.Instance.QuestProgress[id];

		if (id == -1 || (quest == null && progress == null)) return;

		if (MessageBox.Show(
				string.Format(QuestResources.QuestResetConfirmationText, (quest != null ? ($"『{quest.Name}』") : ($"ID: {id} "))),
				QuestResources.QuestResetConfirmationCaption,
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Exclamation,
				MessageBoxDefaultButton.Button1) is not DialogResult.Yes)
		{
			return;
		}

		if (quest != null)
		{
			KCDatabase.Instance.Quest.Quests.Remove(quest);
		}

		if (progress != null)
		{
			KCDatabase.Instance.QuestProgress.Progresses.Remove(progress);
		}

		Updated();
	}

	public bool ModifyOldVisible =>
#if DEBUG
		true;
#else
		false;
#endif

	[RelayCommand]
	private void ManageTrackerOld()
	{
		if (SelectedQuest is null) return;

		KCDatabase.Instance.QuestTrackerManager.ManageTracker(SelectedQuest.QuestId);

		new QuestTrackerManagerWindow(KCDatabase.Instance.QuestTrackerManager).Show(App.Current.MainWindow);
	}

	[RelayCommand]
	private void ManageTracker()
	{
		if (SelectedQuest is null) return;

		QuestTrackerManager2ViewModel viewModel = Ioc.Default.GetRequiredService<QuestTrackerManager2ViewModel>();
		(TrackerModelV2 Model, IEnumerable<int> Progress)? bundledTracker =
			KCDatabase.Instance.SystemQuestTrackerManagerV2.TryCopyTracker(SelectedQuest.QuestId);

		viewModel.ManageTracker(SelectedQuest.QuestId, bundledTracker?.Model, bundledTracker?.Progress);

		new QuestTrackerManager2Window(viewModel).Show(App.Current.MainWindow);
	}
}
