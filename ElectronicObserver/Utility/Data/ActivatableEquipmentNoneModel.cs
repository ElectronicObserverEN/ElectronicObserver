﻿using ElectronicObserver.Core.Types;

namespace ElectronicObserver.Utility.Data;

public record ActivatableEquipmentNoneModel : IActivatableEquipment
{
	public double ActivationRate => 1;

	public override string ToString() => AirControlSimulatorResources.None;
}
