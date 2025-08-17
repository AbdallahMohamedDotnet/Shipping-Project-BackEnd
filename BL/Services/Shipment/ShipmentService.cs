using AutoMapper;
using BL.Contract;
using BL.Contract.Shipment;
using BL.Contracts;
using BL.DTOConfiguration;
using DAL.Contracts;

using Domains;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class ShipmentService : BaseServices<TbShipment, DTOShipment>, IShipment
    {
        IUserReceiver userReceiver;
        IUserSender userSender;
        ITrackingNumberCreator trackingCreator;
        IRateCalculator rateCalculator;
        ITableRepository<TbShipment> repo;
        public ShipmentService(ITableRepository<TbShipment> repo, IMapper mapper,
             IUserService userService, IUserReceiver userReceiver,
             IUserSender userSender, ITrackingNumberCreator trackingCreator
            , IRateCalculator rateCalculator/*, IUnitOfWork uow*/) : base(repo, mapper, userService)
        {
            this.repo = repo;
            this.userReceiver = userReceiver;
            this.userSender = userSender;
            this.trackingCreator = trackingCreator;
            this.rateCalculator = rateCalculator;
        }
        public async Task Create(DTOShipment DTO)
        {
            // create tracking number
            DTO.TrackingNumber = trackingCreator.Create(DTO);
            // calculate date
            DTO.ShippingRate = rateCalculator.Calculate(DTO);
            // save sender
            if (DTO.SenderId == Guid.Empty)
            {
                Guid gSenderId = Guid.Empty;
                userSender.Add(DTO.UserSender, out gSenderId);
                DTO.SenderId = gSenderId;
            }
            // save receiver
            if (DTO.ReceiverId == Guid.Empty)
            {
                Guid gReciverId = Guid.Empty;
                userReceiver.Add(DTO.UserReceiver, out gReciverId);
                DTO.ReceiverId = gReciverId;
            }
            // save shipment
            this.Add(DTO);
        }
    }
}
