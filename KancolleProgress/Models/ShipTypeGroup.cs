using System.Collections.Generic;

namespace KancolleProgress.Models;

public record ShipTypeGroup(string Label, IEnumerable<ShipClassGroup> ClassGroups);
