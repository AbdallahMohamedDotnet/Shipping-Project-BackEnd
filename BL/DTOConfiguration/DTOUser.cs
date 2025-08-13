using BL.DTOConfiguration.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.DTOConfiguration
{
    public class DTOUser : BaseDTO
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
