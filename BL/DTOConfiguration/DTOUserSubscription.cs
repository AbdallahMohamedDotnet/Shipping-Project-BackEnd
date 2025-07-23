using System;
using System.Collections.Generic;
using BL.DTOConfiguration.Base;
namespace BL.DTOConfiguration;

public partial class DTOUserSubscription : BaseDTO
{

    public Guid UserId { get; set; }

    public Guid PackageId { get; set; }

    public DateTime SubscriptionDate { get; set; }

}
