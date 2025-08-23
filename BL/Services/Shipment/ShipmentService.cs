using AutoMapper;
using BL.Contract;
using BL.Contract.Shipment;
using BL.Contracts;
using BL.DTOConfiguration;
using DAL.Contracts;
using DAL.Models;
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
        IUserService _userService;
        ITableRepository<TbShipment> _repo;
        IMapper _mapper;
        IShippmentStatus _shipmentStatus;
        public ShipmentService(ITableRepository<TbShipment> repo, IMapper mapper,
             IUserService userService, IUserReceiver userReceiver,
             IUserSender userSender, ITrackingNumberCreator trackingCreator
            , IRateCalculator rateCalculator, IShippmentStatus shipmentStatus, IUnitOfWork uow) : base(uow, mapper, userService)
        {
            _uow = uow;
            _repo = repo;
            _mapper = mapper;
            _userReceiver = userReceiver;
            _userSender = userSender;
            _trackingCreator = trackingCreator;
            _rateCalculator = rateCalculator;
            _userService = userService;
            _shipmentStatus = shipmentStatus;
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
                var userId = _userService.GetLoggedInUser();
                if (dto.SenderId == Guid.Empty)
                {
                    Guid gSenderId = Guid.Empty;
                    dto.UserSender.UserId = userId;
                    _userSender.Add(dto.UserSender, out gSenderId);
                    dto.SenderId = gSenderId;
                }
                // save receiver
                if (dto.ReceiverId == Guid.Empty)
                {
                    Guid gReciverId = Guid.Empty;
                    dto.UserReceiver.UserId = userId;
                    _userReceiver.Add(dto.UserReceiver, out gReciverId);
                    dto.ReceiverId = gReciverId;
                }
                // save shipment
                Guid gShipmentId = Guid.Empty;
                this.Add(dto, out gShipmentId);
                // add shipment status
                DTOShippmentStatus status = new DTOShippmentStatus();
                status.ShippmentId = gShipmentId;
                status.CurrentState = (int)ShipmentStatusEnum.Created;
                _shipmentStatus.Add(status);
                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                throw new Exception();
            }
        }

        public async Task Edit(DTOShipment dto)
        {
            try
            {
                await _uow.BeginTransactionAsync();
                // calculate date
                dto.ShippingRate = _rateCalculator.Calculate(dto);
                // save sender
                dto.UserSender.Id = dto.SenderId;
                _userSender.Update(dto.UserSender);
                // save receiver
                dto.UserReceiver.Id = dto.ReceiverId;
                _userReceiver.Update(dto.UserReceiver);
                // save shipment
                this.Update(dto);
                await _uow.CommitAsync();
            }
            catch (Exception ex)
            {
                await _uow.RollbackAsync();
                throw new Exception();
            }
        }

        public async Task<List<DTOShipment>> GetShipments()
        {
            try
            {
                var userId = _userService.GetLoggedInUser();

                // Get the shipment entities using GetList with Where filter, then use Select for mapping
                var shipments = await Task.FromResult(_repo.GetList(a => a.CreatedBy == userId)
                    .Where(a => a.CurrentState > 0)
                    .Select(a => new DTOShipment
                    {
                        Id = a.Id,
                        ShipingDate = a.ShipingDate,
                        DelivryDate = a.DelivryDate,
                        SenderId = a.SenderId,
                        ReceiverId = a.ReceiverId,
                        ShippingTypeId = a.ShippingTypeId,
                        ShipingPackgingId = a.ShipingPackgingId,
                        Width = a.Width,
                        Height = a.Height,
                        Weight = a.Weight,
                        Length = a.Length,
                        PackageValue = a.PackageValue,
                        ShippingRate = a.ShippingRate,
                        PaymentMethodId = a.PaymentMethodId,
                        UserSubscriptionId = a.UserSubscriptionId,
                        TrackingNumber = a.TrackingNumber,
                        ReferenceId = a.ReferenceId,

                        UserSender = a.Sender != null ? new DTOUserSender
                        {
                            Id = a.Sender.Id,
                            SenderName = a.Sender.SenderName,
                            Email = a.Sender.Email,
                            Phone = a.Sender.Phone
                        } : null,
                        UserReceiver = a.Receiver != null ? new DTOUserReceiver
                        {
                            Id = a.Receiver.Id,
                            ReceiverName = a.Receiver.ReceiverName,
                            Email = a.Receiver.Email,
                            Phone = a.Receiver.Phone
                        } : null
                    })
                    .OrderByDescending(a => a.CreatedDate)
                    .ToList());

                return shipments;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting shipments", ex);
            }
        }

        public async Task<PagedResult<DTOShipment>> GetShipments(int pageNumber, int pageSize)
        {
            try
            {
                var userId = _userService.GetLoggedInUser();

                var result = await _repo.GetPagedList(
                    pageNumber: pageNumber,
                    pageSize: pageSize,
                    filter: a => a.CreatedBy == userId && a.CurrentState > 0,
                    selector: a => new DTOShipment
                    {
                        Id = a.Id,
                        ShipingDate = a.ShipingDate,
                        DelivryDate = a.DelivryDate,
                        SenderId = a.SenderId,
                        ReceiverId = a.ReceiverId,
                        ShippingTypeId = a.ShippingTypeId,
                        ShipingPackgingId = a.ShipingPackgingId,
                        Width = a.Width,
                        Height = a.Height,
                        Weight = a.Weight,
                        Length = a.Length,
                        PackageValue = a.PackageValue,
                        ShippingRate = a.ShippingRate,
                        PaymentMethodId = a.PaymentMethodId,
                        UserSubscriptionId = a.UserSubscriptionId,
                        TrackingNumber = a.TrackingNumber,
                        ReferenceId = a.ReferenceId,
                        UserSender = new DTOUserSender
                        {
                            Id = a.Sender.Id,
                            SenderName = a.Sender.SenderName,
                            Email = a.Sender.Email,
                            Phone = a.Sender.Phone
                        },
                        UserReceiver = new DTOUserReceiver
                        {
                            Id = a.Receiver.Id,
                            ReceiverName = a.Receiver.ReceiverName,
                            Email = a.Receiver.Email,
                            Phone = a.Receiver.Phone
                        }
                    },
                    orderBy: a => a.CreatedDate,
                    isDescending: true,
                    a => a.Sender, a => a.Receiver
                );

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting shipments", ex);
            }
        }

        public async Task<DTOShipment> GetShipment(Guid id)
        {
            try
            {
                var userId = _userService.GetLoggedInUser();

                // Get the shipment entity first using GetList (without selector)
                var shipmentEntity = await Task.FromResult(_repo.GetList(a => a.Id == id && a.CreatedBy == userId).FirstOrDefault());

                if (shipmentEntity == null)
                    return null;

                // Use LINQ to manually map the entity to DTO
                var shipment = new DTOShipment
                {
                    Id = shipmentEntity.Id,
                    ShipingDate = shipmentEntity.ShipingDate,
                    DelivryDate = shipmentEntity.DelivryDate,
                    SenderId = shipmentEntity.SenderId,
                    ReceiverId = shipmentEntity.ReceiverId,
                    ShippingTypeId = shipmentEntity.ShippingTypeId,
                    ShipingPackgingId = shipmentEntity.ShipingPackgingId,
                    Width = shipmentEntity.Width,
                    Height = shipmentEntity.Height,
                    Weight = shipmentEntity.Weight,
                    Length = shipmentEntity.Length,
                    PackageValue = shipmentEntity.PackageValue,
                    ShippingRate = shipmentEntity.ShippingRate,
                    PaymentMethodId = shipmentEntity.PaymentMethodId,
                    UserSubscriptionId = shipmentEntity.UserSubscriptionId,
                    TrackingNumber = shipmentEntity.TrackingNumber,
                    ReferenceId = shipmentEntity.ReferenceId,

                    UserSender = shipmentEntity.Sender != null ? new DTOUserSender
                    {
                        Id = shipmentEntity.Sender.Id,
                        SenderName = shipmentEntity.Sender.SenderName,
                        Email = shipmentEntity.Sender.Email,
                        Phone = shipmentEntity.Sender.Phone,
                        Address = shipmentEntity.Sender.Address,
                        Contact = shipmentEntity.Sender.Contact,
                        PostalCode = shipmentEntity.Sender.PostalCode,
                        OtherAddress = shipmentEntity.Sender.OtherAddress,
                        CityId = shipmentEntity.Sender.CityId,
                        CountryId = shipmentEntity.Sender.City?.CountryId ?? Guid.Empty
                    } : null,
                    UserReceiver = shipmentEntity.Receiver != null ? new DTOUserReceiver
                    {
                        Id = shipmentEntity.Receiver.Id,
                        ReceiverName = shipmentEntity.Receiver.ReceiverName,
                        Email = shipmentEntity.Receiver.Email,
                        Phone = shipmentEntity.Receiver.Phone,
                        Address = shipmentEntity.Receiver.Address,
                        Contact = shipmentEntity.Receiver.Contact,
                        PostalCode = shipmentEntity.Receiver.PostalCode,
                        OtherAddress = shipmentEntity.Receiver.OtherAddress,
                        CityId = shipmentEntity.Receiver.CityId
                    } : null
                };

                return shipment;
            }
            catch (Exception ex)
            {
                throw new Exception("Error while getting shipment", ex);
            }
        }


    }
}
