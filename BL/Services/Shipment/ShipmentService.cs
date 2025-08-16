using AutoMapper;
using BL.Contract;
using BL.Contract.Shipment;
using BL.Contracts;
using BL.DTOConfiguration;
using DAL.Contracts;
using DAL.Repositories;
using Domains;
//using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Services
{
    public class ShipmentService : BaseService<TbShipment, DTOShipment>, IShipment
    {
        IUserReceiver _userReceiver;
        IUserSender _userSender;
        ITrackingNumberCreator _trackingCreator;
        IRateCalculator _rateCalculator;
       // IUnitOfWork _uow;
        public ShipmentService(ITableRepository<TbShipment> repo,IMapper mapper,
             IUserService userService, IUserReceiver userReceiver,
             IUserSender userSender, ITrackingNumberCreator trackingCreator
            , IRateCalculator rateCalculator,/*IUnitOfWork uow*/) : base(/*uow,*/ mapper, userService)
        {
            //_uow = uow;
            _userReceiver = userReceiver;
            _userSender = userSender;
            _trackingCreator = trackingCreator;
            _rateCalculator = rateCalculator;
        }

        public async Task Create(DTOShipment DTO )
        {
            try
            {
                await _uow.BeginTransactionAsync();
                // create tracking number
                DTO.TrackingNumber = _trackingCreator.Create(DTO);
                // calculate date
                DTO.ShippingRate = _rateCalculator.Calculate(DTO);
                // save sender
                if (DTO.SenderId == Guid.Empty)
                {
                    Guid gSenderId = Guid.Empty;
                    _userSender.Add(DTO.UserSender, out gSenderId);
                    DTO.SenderId = gSenderId;
                }
                // save receiver
                if (DTO.ReceiverId == Guid.Empty)
                {
                    Guid gReciverId = Guid.Empty;
                    _userReceiver.Add(DTO.UserReceiver, out gReciverId);
                    DTO.ReceiverId = gReciverId;
                }
                // save shipment
                this.Add(DTO);
                await _uow.CommitAsync();
            }
            catch(Exception ex)
            {
                await _uow.RollbackAsync();
            }
        }
    }
}
