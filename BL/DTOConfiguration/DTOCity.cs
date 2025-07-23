using System;
using System.Collections.Generic;
using BL.DTOConfiguration.Base;
namespace BL.DTOConfiguration;

public partial class DTOCity : BaseDTO
{

    public string? CityAname { get; set; }

    public string? CityEname { get; set; }

    public Guid CountryId { get; set; }


}
