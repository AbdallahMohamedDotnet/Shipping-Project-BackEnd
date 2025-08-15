using System;
using System.Collections.Generic;
using BL.DTOConfiguration.Base;
namespace BL.DTOConfiguration;

public partial class DTOUserSender : BaseDTO
{


    public Guid UserId { get; set; }

    public string SenderName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public Guid CityId { get; set; }

    public string Address { get; set; } = null!;
    public string? PostalCode { get; set; }
    public string Contact { get; set; }
    public string? OtherAddress { get; set; }

}
