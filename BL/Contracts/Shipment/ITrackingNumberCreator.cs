using BL.DTOConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Contract.Shipment
{
    public interface ITrackingNumberCreator
    {
        public double Create(DTOShipment DTO);
    }
}
