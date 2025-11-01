using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ElectronicObserver.Core.Services.Data;
using ElectronicObserver.Core.Types;
using ElectronicObserver.Core.Types.Data;
using ElectronicObserver.Core.Types.Extensions;
using ElectronicObserver.ViewModels.Translations;

namespace ElectronicObserver.Utility.Data;

public class TransportGaugeService(IKCDatabase db, FormFleetOverviewTranslationViewModel translations) : ITransportGaugeService
{
	private IKCDatabase KCDatabase { get; } = db;
	private FormFleetOverviewTranslationViewModel Translations { get; } = translations;

	public string GetCurrentEventLandingOperationToolTip(List<IFleetData> fleets)
	{
		// LastOrDefault is used here because in debug we add old event areas to KCDatabase. In release there's 0 or 1 event area.
		if (KCDatabase.MapArea.Values.LastOrDefault(area => area.IsEventArea) is not { } eventArea) return "";

		StringBuilder sb = new();

		foreach (TpGauge gauge in Enum.GetValues<TpGauge>().Where(gauge => gauge.GetGaugeAreaId() == eventArea.MapAreaID))
		{
			int tp = gauge.GetTp(fleets);

			sb.AppendLine($"E{gauge.GetGaugeMapId()}-{gauge.GetGaugeIndex()}: S {tp} / A {(int)(tp * 0.7)}");
		}

		if (sb.Length is 0) return "";

		return $"\n{Translations.LandingOperationTooltip}:\n{sb.ToString().TrimEnd()}";
	}
}
