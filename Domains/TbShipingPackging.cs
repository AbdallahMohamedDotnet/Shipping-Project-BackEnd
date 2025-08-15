using System;
using System.Collections.Generic;

namespace Domains;

public partial class TbShipingPackging : BaseTable
{
    public string? ShipingPackgingAname { get; set; }

    public string? ShipingPackgingEname { get; set; }

    public virtual ICollection<TbShipment> TbShippments { get; set; } = new List<TbShipment>();
}
