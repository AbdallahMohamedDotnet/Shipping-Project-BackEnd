using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BL.DTOConfiguration.Base;

namespace BL.DTOConfiguration;

public partial class DTOShippingType : BaseDTO
{
    [Required(ErrorMessage = "Arabic shipping type name is required")]
    [StringLength(200, ErrorMessage = "Arabic name cannot exceed 200 characters")]
    [Display(Name = "Arabic Name")]
    public string? ShippingTypeAname { get; set; }

    [Required(ErrorMessage = "English shipping type name is required")]
    [StringLength(200, ErrorMessage = "English name cannot exceed 200 characters")]
    [Display(Name = "English Name")]
    public string? ShippingTypeEname { get; set; }

    [Required(ErrorMessage = "Shipping factor is required")]
    [Range(0.1, 10.0, ErrorMessage = "Shipping factor must be between 0.1 and 10.0")]
    [Display(Name = "Shipping Factor")]
    public double ShippingFactor { get; set; }
}
