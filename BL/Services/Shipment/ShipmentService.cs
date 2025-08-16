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
        ITableRepository<TbShipment> _repo;
        public ShipmentService(ITableRepository<TbShipment> repo, IMapper mapper,
             IUserService userService, IUserReceiver userReceiver,
             IUserSender userSender, ITrackingNumberCreator trackingCreator
            , IRateCalculator rateCalculator/*, IUnitOfWork uow*/) : base(repo, mapper, userService)
        {
            this._repo = repo;
            this._userReceiver = userReceiver;
            this._userSender = userSender;
            this._trackingCreator = trackingCreator;
            this._rateCalculator = rateCalculator;
        }
        public async Task Create(DTOShipment DTO)
        {


        }
    }
}
