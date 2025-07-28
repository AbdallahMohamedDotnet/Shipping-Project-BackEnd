using BL.DTOConfiguration.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using AppResources;
namespace BL.DTOConfiguration;

public partial class DTOCity : BaseDTO
{

    [Required(ErrorMessageResourceName = "NameArRequired", ErrorMessageResourceType = typeof(Messages), AllowEmptyStrings = false)]
    [StringLength(100, MinimumLength = 3, ErrorMessageResourceName = "NameLenght", ErrorMessageResourceType = typeof(Messages))]
    public string? CityAname { get; set; }
    [Required(ErrorMessageResourceName = "NameArRequired", ErrorMessageResourceType = typeof(Messages), AllowEmptyStrings = false)]
    [StringLength(100, MinimumLength = 3, ErrorMessageResourceName = "NameLenght", ErrorMessageResourceType = typeof(Messages))]
    public string? CityEname { get; set; }

    public string? CountryAname { get; set; }

    public string? CountryEname { get; set; }

    [Required(ErrorMessageResourceName = "CountryRequired", ErrorMessageResourceType = typeof(Messages), AllowEmptyStrings = false)]
    public Guid CountryId { get; set; }


}
