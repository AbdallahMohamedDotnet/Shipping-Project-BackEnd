using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BL.DTOConfiguration.Base;

namespace BL.DTOConfiguration;

public partial class DTOCountry : BaseDTO
{
    [Required(ErrorMessage = "Arabic country name is required")]
    [StringLength(200, ErrorMessage = "Arabic name cannot exceed 200 characters")]
    [Display(Name = "Arabic Name")]
    public string? CountryAname { get; set; }

    [Required(ErrorMessage = "English country name is required")]
    [StringLength(200, ErrorMessage = "English name cannot exceed 200 characters")]
    [Display(Name = "English Name")]
    public string? CountryEname { get; set; }
}
