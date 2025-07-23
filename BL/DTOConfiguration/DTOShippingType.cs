using System;
using System.Collections.Generic;
using BL.DTOConfiguration.Base;
namespace BL.DTOConfiguration;
public partial class DTOShippingType  : BaseDTO
{
    public string? ShippingTypeAname { get; set; }

    public string? ShippingTypeEname { get; set; }

    public double ShippingFactor { get; set; }


}
