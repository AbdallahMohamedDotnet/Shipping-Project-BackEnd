using System;
using System.Collections.Generic;
using Domains;
using BL.DTOConfiguration.Base;
namespace BL.DTOConfiguration;

public partial class DTOShippmentStatus  : BaseDTO
{

    public Guid? ShippmentId { get; set; }

    public int CurrentState { get; set; }

    public string? Notes { get; set; }

    public Guid CarrierId { get; set; }


}
