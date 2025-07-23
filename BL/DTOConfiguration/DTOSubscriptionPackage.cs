using System;
using System.Collections.Generic;
using Domains;
using BL.DTOConfiguration.Base;
namespace BL.DTOConfiguration;

public partial class DTOSubscriptionPackage : BaseDTO
{


    public string PackageName { get; set; } = null!;

    public int ShippimentCount { get; set; }

    public double NumberOfKiloMeters { get; set; }

    public double TotalWeight { get; set; }


}
