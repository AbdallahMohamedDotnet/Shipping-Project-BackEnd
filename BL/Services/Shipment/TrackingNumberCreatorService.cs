using BL.Contract.Shipment;
using BL.DTOConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services.Shipment
{
    public class TrackingNumberCreatorService : ITrackingNumberCreator
    {
        public double Create(DTOShipment DTO)
        {
            return 545466;
        }
    }
}
