using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.DTOConfiguration.Base;
namespace BL.DTOConfiguration;


    public class DTORefreshToken : BaseDTO
    {
        public string Token { get; set; }

        public string UserId { get; set; }

        public DateTime Expires { get; set; }

        public int CurrentState { get; set; }
    }

