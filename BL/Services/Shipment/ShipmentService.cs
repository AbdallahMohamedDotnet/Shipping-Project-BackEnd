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
        IUserReceiver _userReceiver;
        IUserSender _userSender;
        ITrackingNumberCreator _trackingCreator;
        IRateCalculator _rateCalculator;
        IUnitOfWork _uow;
        public ShipmentService(ITableRepository<TbShipment> repo, IMapper mapper,
             IUserService userService, IUserReceiver userReceiver,
             IUserSender userSender, ITrackingNumberCreator trackingCreator
            , IRateCalculator rateCalculator, IUnitOfWork uow) : base(uow, mapper, userService)
        {
            _uow = uow;
            _userReceiver = userReceiver;
            _userSender = userSender;
            _trackingCreator = trackingCreator;
            _rateCalculator = rateCalculator;
        }

        public async Task Create(DTOShipment dto)
        {
            try
            {
                await _uow.BeginTransactionAsync();
                // create tracking number
                dto.TrackingNumber = _trackingCreator.Create(dto);
                // calculate date
                dto.ShippingRate = _rateCalculator.Calculate(dto);
                // save sender
                if (dto.SenderId == Guid.Empty)
                {
                    Guid gSenderId = Guid.Empty;
                    _userSender.Add(dto.UserSender, out gSenderId);
                    dto.SenderId = gSenderId;
                }
                // save receiver
                if (dto.ReceiverId == Guid.Empty)
                {
                    Guid gReciverId = Guid.Empty;
                    _userReceiver.Add(dto.UserReceiver, out gReciverId);
                    dto.ReceiverId = gReciverId;
                }
                // save shipment
                this.Add(dto);
                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
            }
        }
    }
}
