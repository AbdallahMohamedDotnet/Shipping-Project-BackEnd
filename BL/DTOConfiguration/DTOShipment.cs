using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Domains;
using BL.DTOConfiguration.Base;

namespace BL.DTOConfiguration;

public partial class DTOShipment : BaseDTO
{
    public DateTime ShipingDate { get; set; }
    public DateTime DelivryDate { get; set; }

    public Guid SenderId { get; set; }
    public DTOUserSender UserSender { get; set; }
    public Guid ReceiverId { get; set; }
    public DTOUserReceiver UserReceiver { get; set; }
    public Guid ShippingTypeId { get; set; }
    public Guid? ShipingPackgingId { get; set; }
    public Guid? CarrierId { get; set; }
    public double Width { get; set; }

    public double Height { get; set; }

    public double Weight { get; set; }

    public double Length { get; set; }

    public decimal PackageValue { get; set; }

    public decimal ShippingRate { get; set; }

    public Guid? PaymentMethodId { get; set; }

    public Guid? UserSubscriptionId { get; set; }

    public double? TrackingNumber { get; set; }

    public Guid? ReferenceId { get; set; }
}
