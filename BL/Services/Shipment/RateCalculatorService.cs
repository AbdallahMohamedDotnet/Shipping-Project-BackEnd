using BL.Contract.Shipment;
using BL.DTOConfiguration;
using BL.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Shipment
{
    public class RateCalculatorService : IRateCalculator
    {
        public decimal Calculate(DTOShipment DTO)
        {
            return 4545;
        }
    }
}
